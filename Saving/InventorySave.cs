using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Saving
{
    [System.Serializable]
    public class InventorySave : Save
    {
        public List<Item> s_items;
        public List<ItemWeapon> s_weapons;
        public List<ItemRanged> s_ranged;
        public List<ItemEquipable> s_equipable;
        public List<ItemConsumable> s_consumables;
        public int s_gold;
        public float s_totalWeight;

        public InventorySave() { }

        public InventorySave(List<Item> _items)
        {

            s_items = new List<Item>();
            s_weapons = new List<ItemWeapon>();
            s_ranged = new List<ItemRanged>();
            s_equipable = new List<ItemEquipable>();
            s_consumables = new List<ItemConsumable>();

            SortItems(_items);

            s_gold = -1;
        }

        public InventorySave(List<Item> _items, int _gold)
        {

            s_items = new List<Item>();
            s_weapons = new List<ItemWeapon>();
            s_ranged = new List<ItemRanged>();
            s_equipable = new List<ItemEquipable>();
            s_consumables = new List<ItemConsumable>();

            SortItems(_items);

            s_gold = _gold;
        }

        public InventorySave(List<Item> _items, int _gold, float _totalWeight)
        {
            s_items = new List<Item>();
            s_weapons = new List<ItemWeapon>();
            s_ranged = new List<ItemRanged>();
            s_equipable = new List<ItemEquipable>();
            s_consumables = new List<ItemConsumable>();

            SortItems(_items);

            s_gold = _gold;
            s_totalWeight = _totalWeight;
        }

        public void SortItems(List<Item> _items)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] is ItemRanged) s_weapons.Add((ItemRanged)_items[i]);
                else if (_items[i] is ItemWeapon) s_weapons.Add((ItemWeapon)_items[i]);
                else if (_items[i] is ItemEquipable) s_equipable.Add((ItemEquipable)_items[i]);
                else if (_items[i] is ItemConsumable) s_consumables.Add((ItemConsumable)_items[i]);
                else s_items.Add(_items[i]);
            }

        }

        public List<Item> GetAllItems()
        {
            List<Item> items = new List<Item>();
            foreach (Item item in s_items) items.Add(item);
            foreach (ItemWeapon item in s_weapons)
            {
                //Debug.Log("adding weapon " + item.itemName + " to list");
                items.Add(item);
            }
            foreach (ItemEquipable item in s_equipable)
            {
                //Debug.Log("adding weapon " + item.itemName + " to list");
                items.Add(item);
            }
            foreach (ItemConsumable item in s_consumables)
            {
                //Debug.Log("adding weapon " + item.itemName + " to list");
                items.Add(item);
            }

            return items;
        }
    }
}