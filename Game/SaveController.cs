using Assets.Scripts;
using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Saving;
using UnityEngine;

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

    public void InstantiatePlayer()
    {
        //Debug.Log("instantiating player");
        if (isPlayerInstantiated == true)
        {
            Debug.LogWarning("player already instantiated");
            return;
        }

        isPlayerInstantiated = true;

        if (!CheckForSave()) Instantiate(playerGO);
        else
        {
            instantiatedPlayerGO = null;
            Debug.Log("INSTANTIATING PLAYER");
            instantiatedPlayerGO = Instantiate(playerGO, _save.pos, Quaternion.identity);
            DontDestroyOnLoad(instantiatedPlayerGO);
            //_player.GetComponent<PlayerInstanceController>().SetPlayerPositionLoad(_save.pos, _save.rot);
        }
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

        LoadPlayerSaveData();
        levelLoadedFromSave = true;

        savedLevel = _save.scene;
        LevelController.LoadLevelByName(savedLevel);

        Invoke("ClearLoadSaveFlag", 1f);
    }

    public void ClearLoadSaveFlag()
    {
        levelLoadedFromSave = false;
    }

    public void SaveGame()
    {
        Debug.Log("Saving game");
        SavePersistenceController.SavePersistentObjects();

        PlayerSave playerSave = PlayerInstanceController.CreatePlayerSave();
        QuestInfoSave questSave = QuestController.GetQuestSave();

        ES3.Save("PLAYERSAVE", playerSave, Application.dataPath + instance.saveName);
        ES3.Save("QUESTSAVE", questSave, Application.dataPath + instance.saveName);

        //save quest progress

        /*  OLD METHOD
        //save the name of the active scene
        //save: scene name, player location, player rotation, and player items
        //game objects that save data should be subscribed to the save delegate
        //call the delegate that the objects are subscribed to
        Debug.Log("saved game data at: "+Application.dataPath+instance.saveName);

        //save player info
        savedLevel = LevelController.GetActiveScene();
        Vector3 pos = PlayerInstanceController.instance.transform.position;
        Vector3 rot = PlayerInstanceController.instance.GetRotation();
        PlayerSave save = new PlayerSave(savedLevel, pos, rot);
        ES3.Save("PLAYERSAVE", save, Application.dataPath + instance.saveName);

        //prevent save delegate from breaking script if it has no subscribers
        if (objectSaveDelegate != null) objectSaveDelegate();
        if (playerSaveDelegate != null) playerSaveDelegate();
        */
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
    #region SaveReturners


    public static PlayerSave GetPlayerSave()
    {
        if (!CheckForSave()) return null;
        PlayerSave save = (PlayerSave)ES3.Load("PLAYERSAVE", Application.dataPath + instance.saveName);
        return save;
    }

    public static QuestInfoSave GetQuestSave()
    {
        if (!CheckForSave()) return null;
        return (QuestInfoSave)instance.saveDictionary["QUESTSAVE"];
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
