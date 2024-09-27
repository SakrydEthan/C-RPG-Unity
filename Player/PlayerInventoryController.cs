using Assets.Scripts;
using Assets.Scripts.Items;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour, ISaveable
{
    public static PlayerInventoryController instance = null;
    public List<Item> items;
    //public List<ItemWeapon> weapons;
    public int gold = 200;
    public float totalWeight = 0;

    //PlayerInventorySave save = null;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            Debug.Log("set inventory instance from script on "+transform.name);
            //UIInventory.instance.UpdateItemText(items[0]);
            instance = this;
            SaveController.instance.playerSaveDelegate += Save;
            //Load();
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddItem(Item item)
    {
        totalWeight += item.weight;
        //if (item is ItemWeapon) weapons.Add((ItemWeapon)item);
        //else 
        items.Add(item);
    }

    public void AddItemsByArray(Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            AddItem(items[i]);
        }
    }

    public void BuyItem(Item item, int cost)
    {
        if (cost > gold) return; //don't buy item if it's too expensive

        AddItem(item);
        gold -= cost;
    }

    public bool TryBuyItem(Item item, int cost)
    {
        if (cost > gold) return false;
        return true;
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }

    public bool HasGold(int amount)
    {
        return gold >= amount;
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }

    public void SpendGold(int amount)
    { 
        gold -= amount; 
    }

    public void AddGold(int amount)
    {
        gold += amount; 
    }


    public void Save()
    {
        //ES3.Save("LISTFUCKINGRETARD", items, Application.dataPath + SaveController.instance.saveName);
        //Debug.Log("saved inventory: " + save.items);
        /*string plitems = "";
        foreach (Item item in items)
        {
            plitems += item.itemName + ", ";
        }
        for(int i=0; i<items.Count; i++)
        {
            plitems+= items[i].itemName + ", ";
        }
        Debug.Log("saved player inventory with items " + plitems);*/


        /* made before moving player save info to centralised file
        Debug.Log("saved player inventory");
        //save = new PlayerInventorySave(items, weapons, gold, totalWeight);
        save = new PlayerInventorySave(items, gold, totalWeight);
        ES3.Save("PLAYERINVENTORY", save, Application.dataPath + SaveController.instance.saveName, ES3Settings.defaultSettings);
        */
    }


    //TODO update to include player keychain
    public void Load()
    {
        /*
        if (!SaveController.CheckForSave()) return;
        Debug.Log("loading player inventory");
        save = (PlayerInventorySave)ES3.Load("PLAYERINVENTORY", Application.dataPath + SaveController.instance.saveName);
        items.Clear();
        //weapons.Clear();
        foreach(Item item in save.s_items)
        {
            items.Add(item);
        }
        foreach (ItemWeapon item in save.s_weapons)
        {
            items.Add(item);
        }
        gold = save.s_gold;
        totalWeight = save.s_totalWeight;
        */
    }
}

/*

[System.Serializable]
public class PlayerInventorySave
{
    public List<Item> s_items;
    public List<ItemWeapon> s_weapons;
    public int s_gold;
    public float s_totalWeight;

    public PlayerInventorySave() { }
    public PlayerInventorySave(List<Item> _items, int _gold, float _totalWeight)
    {
        s_items = new List<Item>();
        s_weapons = new List<ItemWeapon>();

        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] is ItemWeapon) s_weapons.Add((ItemWeapon)_items[i]);
            else s_items.Add(_items[i]);
        }
        s_gold = _gold;
        s_totalWeight = _totalWeight;
    }
}

*/
