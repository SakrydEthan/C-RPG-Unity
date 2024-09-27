using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedCharacter : MonoBehaviour
{
    BaseCharacter character;

    public void Start()
    {
        character = GetComponent<BaseCharacter>();
        if (!SpawnController.CheckinPersistentCharacter(this))
        {
            gameObject.SetActive(false);
        }
    }

    public SpawnedSave GetSave()
    {
        BaseCharacter character = GetComponent<BaseCharacter>();
        SpawnedSave save = new SpawnedSave(character.isAlive, character.id);
        Debug.Log("Saved character: " + character.id);
        return save;
    }

    public void LoadSave()
    {
        
    }

    public string GetID() { return character.id; }

    public bool CheckAlive() {  return character.isAlive; }
}
