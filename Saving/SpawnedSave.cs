using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnedSave : Save
{
    public bool isAlive;
    public string charID;

    public SpawnedSave(bool isAlive, string id)
    {
        this.isAlive = isAlive;
        charID = id;
    }
}
