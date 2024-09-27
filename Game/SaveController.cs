using Assets.Scripts;
using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Saving;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Opsive.UltimateCharacterController.Character.Abilities.Items;
using Opsive.UltimateCharacterController.Inventory;

public class SaveController : MonoBehaviour
{
    public static SaveController instance;

    [Tooltip("The prefab the player plays the game with")]
    public GameObject playerGO;
    GameObject instantiatedPlayerGO;

    public bool levelLoadedFromSave = false;
    public bool isPlayerInstantiated = false;

    public string savedLevel;
    public string saveName = "/test.txt";

    public delegate void SaveDelegate();
    public SaveDelegate objectSaveDelegate;
    public SaveDelegate playerSaveDelegate;


    PlayerSave _save = null;

    [SerializeReference] Dictionary<string, Save> saveDictionary;

    //the dictionary for placed characters, tacked on bcs i just wanna finish this fn project
    [SerializeReference] Dictionary<string, BaseCharacterSave> characterSaveDictionary;

    bool loadedSave = false;
    //public Vector3 startPosition = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        //LoadPlayerSaveData();
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (CheckForSave())
            {
                LoadPlayerSaveData();
            }

            //build the save dictionary
            saveDictionary = new Dictionary<string, Save>();

            //check for save
            loadedSave = ES3.FileExists(Application.dataPath + instance.saveName);
            if (!loadedSave) return;

            string[] IDs = ES3.GetKeys(Application.dataPath + instance.saveName);
            foreach (string id in IDs)
            {
                Save save = (Save)ES3.Load(id, Application.dataPath + instance.saveName);
                saveDictionary.Add(id, save);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Loading
    public static void LoadPlayerSave()
    {//method for applying players saved data
        PlayerSave playerSave = GetPlayerSave();

        PlayerInstanceController.instance.SetPlayerPositionLoad(playerSave.pos, playerSave.rot);
        //PlayerInstanceController.instance.attributes.LoadSavedAttributes(playerSave);
        //attributes.CreateFactors();

        //level
        PlayerInstanceController.instance.level.StartBehavior(playerSave.xp, playerSave.level, playerSave.skillPoints);
        
        //attributes
        for (int i = 0; i < playerSave.skills.Length; i++)
        {
            PlayerInstanceController.instance.attributes.SetSkill((Skill)i, playerSave.skills[i]);
        }
        for (int i = 0; i < playerSave.attributes.Length; i++)
        {
            PlayerInstanceController.instance.attributes.SetAttribute((Attribute)i, playerSave.attributes[i]);
        }

        //equipment
        for (int i = 0; i < playerSave.equipment.Length; i++)
        {
            PlayerInstanceController.instance.equipment.EquipItem(playerSave.equipment[i]);
        }


        //PlayerInstanceController.instance.attributes.GetMaxHealth();

        PlayerInstanceController.instance.attributes.SetHealth(playerSave.hp);
        PlayerInstanceController.instance.attributes.SetStamina(playerSave.stamina);
        PlayerInstanceController.instance.attributes.SetMana(playerSave.mana);

        PlayerInstanceController.instance.attributes.CreateFactors();

        //health
        Debug.Log($"Player health: {playerSave.hp}");
        float maxhp = PlayerInstanceController.instance.attributes.GetMaxHealth();
        PlayerInstanceController.instance.health.SetMaxHP(maxhp);
        PlayerInstanceController.instance.health.SetHP(playerSave.hp);

        if (playerSave.activeWeapon != null) PlayerInstanceController.instance.combat.EquipWeapon(playerSave.activeWeapon);

        //inventory
        PlayerInstanceController.instance.inventory.items = playerSave.inventory.GetAllItems();
        PlayerInstanceController.instance.inventory.gold = playerSave.inventory.s_gold;
        PlayerInstanceController.instance.inventory.totalWeight = playerSave.inventory.s_totalWeight;


    }

    public static void ReloadLevel()
    {
        if (CheckForSave() == false) Application.Quit(); //no save to load, quit game
        Debug.Log("destroying player go");
        Destroy(instance.instantiatedPlayerGO);
        instance.ClearSaveDelegate();
        //clear the persistent dictionary since it contains data after the save we're loading
        SavePersistenceController.ClearPersistentDictionary();

        instance.isPlayerInstantiated = false;

        LevelController.ReloadLevel(instance.savedLevel);

        instance.Invoke("ClearLoadSaveFlag", 1f);
        //instance.InstantiatePlayer();
        //instance.Invoke("InstantiatePlayer", 3f);
    }


    public void LoadPlayerSaveData()
    {
        if (CheckForSave() == false) return;
        _save = GetPlayerSave();
        //PlayerInstanceController.instance.LoadPlayerSave(_save);
    }

    public void LoadLevelFromSave()
    {
        //get string name of level player saved in, then load it
        //get the position and rotation the player saved at and set them
        //add the items the player had when they saved to their inventory

        if (!CheckForSave()) return;
        Debug.Log("Loading level from save");
        LoadPlayerSaveData();
        QuestController.LoadQuestSave((QuestInfoSave)saveDictionary["QUESTSAVE"]);
        levelLoadedFromSave = true;

        savedLevel = _save.scene;
        LevelController.LoadLevelByName(savedLevel);

        Invoke("ClearLoadSaveFlag", 1f);
    }

    public void LoadGame()
    {
        if (!CheckForSave()) return;
        LoadPlayerSaveData();
    }

    public static PersistentSave LoadPersistentCharacters()
    {
        if (ES3.KeyExists("PERSISTENTSAVE", Application.dataPath + instance.saveName))
        {
            return (PersistentSave)ES3.Load("PERSISTENTSAVE", Application.dataPath + instance.saveName);
        }
        else return null;
    }
    #endregion Loading

    #region Player
    public GameObject InstantiatePlayer()
    {
        //Debug.Log("instantiating player");
        if (isPlayerInstantiated == true)
        {
            Debug.LogWarning("player already instantiated");
            return null;
        }

        isPlayerInstantiated = true;

        if (!CheckForSave())
        {
            return Instantiate(playerGO);
        }
        else
        {
            instantiatedPlayerGO = null;
            Debug.Log("INSTANTIATING PLAYER");
            instantiatedPlayerGO = Instantiate(playerGO, _save.pos, Quaternion.identity);
            DontDestroyOnLoad(instantiatedPlayerGO);
            //assign all of the players stats and items here


            return instantiatedPlayerGO;
        }

    }


    public static PlayerSave GetPlayerSave()
    {
        if (!CheckForSave()) return null;
        PlayerSave save = (PlayerSave)ES3.Load("PLAYERSAVE", Application.dataPath + instance.saveName);
        return save;
    }

    #endregion Player




    #region Saving
    public void ClearLoadSaveFlag()
    {
        levelLoadedFromSave = false;
    }

    public void SaveGame()
    {
        Debug.Log("Saving game at "+ Application.dataPath + instance.saveName);
        objectSaveDelegate();
        SavePersistenceController.SavePersistentObjects();

        PlayerSave playerSave = PlayerInstanceController.CreatePlayerSave();
        QuestInfoSave questSave = QuestController.GetQuestSave();

        ES3.Save("PLAYERSAVE", playerSave, Application.dataPath + instance.saveName);
        ES3.Save("QUESTSAVE", questSave, Application.dataPath + instance.saveName);
        SavePersistentCharacters(SpawnController.instance.spawnedCharacters);
    }

    public static void SavePersistentCharacters(List<SpawnedCharacter> characters)
    {
        if (characters.Count < 1) return;
        SpawnedSave[] spawnedSaves = new SpawnedSave[characters.Count];
        for (int i = 0; i < characters.Count; i++)
        {
            spawnedSaves[i] = characters[i].GetSave();
            Debug.Log(spawnedSaves[i].ID);
        }
        PersistentSave pSave = new PersistentSave(spawnedSaves);
        Debug.Log(pSave.ID);
        ES3.Save("PERSISTENTSAVE", pSave, Application.dataPath + instance.saveName);
    }

    public static void SavePlacedCharacters()
    {
        List<BaseCharacterSave> charSave = new List<BaseCharacterSave>();
        foreach(BaseCharacterSave save in instance.characterSaveDictionary.Values){
            charSave.Add(save);
        }
        PlacedSave pSave = new PlacedSave(charSave.ToArray());
        Debug.Log(pSave.ID);
        ES3.Save("PLACEDSAVE", pSave, Application.dataPath + instance.saveName);
    }


    public static bool CheckForSave()
    {
        return ES3.FileExists(Application.dataPath + SaveController.instance.saveName);
        //return false;
    }

    public void ClearSaveDelegate()
    {
        objectSaveDelegate = null;
    }




    #endregion Saving
    #region SaveReturners



    public static QuestInfoSave GetQuestSave()
    {
        if (!CheckForSave()) return null;
        //return (QuestInfoSave)instance.saveDictionary["QUESTSAVE"];
        return (QuestInfoSave)ES3.Load("QUESTSAVE", Application.dataPath + instance.saveName);
    }

    #endregion
    public static Save? GetSaveObjectByName(string name)
    {
        if (!ES3.KeyExists(name, Application.dataPath + instance.saveName)) return null;
        return (Save)ES3.Load(name, Application.dataPath + instance.saveName);
    }


    public static Save? GetSaveObjectByID(string id)
    {
        if(instance.saveDictionary.ContainsKey(id))
        {
            return instance.saveDictionary[id];
        }
        return null;
    }

}

/*
internal class PlayerSave
{
    public string scene;
    public Vector3 pos;
    public Vector3 rot;

    public PlayerSave(string _scene, Vector3 _pos, Vector3 _rot)
    {
        scene = _scene;
        pos = _pos;
        rot = _rot;
    }
}*/
