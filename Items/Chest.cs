using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractive
{

    public bool isChanged = false;
    public List<Item> items;


    public string GetInteractText()
    {
        return "Open Chest";
    }

    public void Interact()
    {
        UILoot.LootCharacter(items);
    }


}
