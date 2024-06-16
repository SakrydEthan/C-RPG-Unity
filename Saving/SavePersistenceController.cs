using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePersistenceController : MonoBehaviour
{

    public static SavePersistenceController instance;

    [SerializeReference] public List<Save> saveObjects;

    [SerializeReference] public Dictionary<string, Save> saveDictionary;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("save persistence con start");
        if(instance == null)
        {
            Debug.Log("set spc instance");
            instance = this;
            saveObjects = new List<Save>();
            saveDictionary = new Dictionary<string, Save>();
        }
    }

    /// <summary>
    /// Clear the persistent save dictionary. Erases data of npcs that was stored in persistence but not saved.
    /// </summary>
    public static void ClearPersistentDictionary()
    {
        instance.saveDictionary.Clear();
    }

    public void AddSaveObject(Save save)
    {
        if (saveDictionary.ContainsKey(save.ID))
        {
            Debug.Log("replacing save data for " + save.ID);
            saveDictionary.Remove(save.ID);
            saveDictionary.Add(save.ID, save);
        }
        else
        {
            saveDictionary.Add(save.ID, save);
        }
    }

    public static void SavePersistentObjects()
    {
        foreach(Save save in instance.saveObjects)
        {
            Debug.Log("saving: " + save.ID);
            ES3.Save(save.ID, save, Application.dataPath + SaveController.instance.saveName);
        }
        foreach(Save save in instance.saveDictionary.Values)
        {
            Debug.Log("(dictionary) saving: "+save.ID);
            ES3.Save(save.ID, save, Application.dataPath + SaveController.instance.saveName);
        }
    }

    public static Save? GetSaveObjectByID(string id)
    {
        if(instance.saveDictionary.ContainsKey(id))
        {
            return instance.saveDictionary[id];
        }
        //Debug.Log("Could not find persistent save data for: " + id);
        return null;
    }



    /*
     //old method for loading character data, using the list
    public static Save? GetSaveObjectByID(string id)
    {
        foreach (Save save in instance.saveObjects)
        {
            if (save.ID == id) return save;
        }

        Debug.Log("Could not find persistent save data for: " + id);
        return null;
    }
    */
}
