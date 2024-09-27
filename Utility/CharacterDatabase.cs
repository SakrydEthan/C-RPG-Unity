using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Create New/Character Database", order = 0)]
public class CharacterDatabase : ScriptableObject
{
    public List<SceneEntry> entries;

    public void AddEntry(SceneEntry entry)
    {
        foreach (SceneEntry scnEntry in entries) {
            if(scnEntry.scene == entry.scene) entries.Remove(scnEntry);
            Debug.Log("Removed duplicate scene entry for " + entry.scene);
            break;
        }

        entries.Add(entry);
    }
}

[System.Serializable]
public class CharacterDatabaseEntry
{
    public string id;
    public string name;
    public bool isPersistent;

    public CharacterDatabaseEntry(string id, string name, bool isPersistent)
    {
        this.id = id;
        this.name = name;
        this.isPersistent = isPersistent;
    }
}

[System.Serializable]
public class SceneEntry
{
    public string scene;
    public CharacterDatabaseEntry[] characters;

    public SceneEntry(string scene, CharacterDatabaseEntry[] characters)
    {
        this.scene = scene;
        this.characters = characters;
    }
}
