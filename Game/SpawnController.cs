using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    public static SpawnController instance;

    public List<SpawnedCharacter> spawnedCharacters;

    bool levelLoaded = false;
    bool hasLoadedSpawned = false;

    public Dictionary<string, SpawnedSave> persistentDictionary = new Dictionary<string, SpawnedSave>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;

            PersistentSave save = SaveController.LoadPersistentCharacters();
            persistentDictionary = new Dictionary<string, SpawnedSave>();
            if (save != null) {
                foreach (SpawnedSave character in save.characters)
                {
                    Debug.Log($"Adding character {character.charID} to persistent dictionary");
                    persistentDictionary.Add(character.charID, character);
                }
            }
        }
    }

    public static bool CheckinPersistentCharacter(SpawnedCharacter character)
    {
        instance.spawnedCharacters.Add(character);
        //SpawnedSave save;

        if (instance.persistentDictionary.ContainsKey(character.GetID()))
        {
            Debug.Log("character is in persistent dictionary");
            return instance.persistentDictionary[character.GetID()].isAlive;
        }
        else
        {
            Debug.Log("character is NOT in persistent dictionary");
            return true;
        }
    }

    public static void AddSpawnedCharacter(SpawnedCharacter character)
    {
        instance.spawnedCharacters.Add(character);
        instance.levelLoaded = true;

    }

    public static void SaveSpawnedCharacters()
    {
        for (int i = 0; i < instance.spawnedCharacters.Count; i++)
        {
            //instance.spawnedCharacters[i].
            SpawnedSave save = new SpawnedSave(instance.spawnedCharacters[i].CheckAlive(), instance.spawnedCharacters[i].GetID());
        }
    }

    public static void LoadSpawnedCharacters()
    {

    }

    public static void LoadSpawns()
    {
        if(!instance.hasLoadedSpawned)
        {
            instance.hasLoadedSpawned = true;
            LoadSpawnedCharacters();
        }
    }
}
