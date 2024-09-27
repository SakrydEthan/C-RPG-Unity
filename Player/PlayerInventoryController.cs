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
            instance = this;
            SaveController.instance.playerSaveDelegate += Save;
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
    }


    //TODO update to include player keychain
    public void Load()
    {
    }
}