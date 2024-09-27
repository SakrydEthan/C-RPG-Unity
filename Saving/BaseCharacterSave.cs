using System;
using UnityEngine;
using Assets.Scripts.Saving;
using Assets.Scripts.Items;
using System.Collections.Generic;

namespace Assets.Scripts.Saving
{
    [System.Serializable]
    public class BaseCharacterSave : Save
    {
        /*
        public Vector3 Position { get; protected set; }
        public bool IsAlive { get; protected set; }
        public float Health { get; protected set; }
        public InventorySave Inventory { get; protected set; }
        */

        public string charName;
        public Vector3 Position;
        public bool IsAlive;
        public float Health;
        public InventorySave Inventory;

        public BaseCharacterSave()
        {
        }

        public BaseCharacterSave(string id, bool isAlive)
        {
            ID = id;
            IsAlive = isAlive;
        }

        public BaseCharacterSave(string id, string _charName, Vector3 _position, bool _isAlive, float _health, List<Item> inventory)
        {
            ID = id;
            charName = _charName;
            Position = _position;
            IsAlive = _isAlive;
            Health = _health;
            Inventory = new InventorySave(inventory);
        }
    }
}