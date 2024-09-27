using System.Collections;
using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Saving
{
    [System.Serializable]
    public class MerchantSave : BaseCharacterSave
    {

        public InventorySave ShopItems;

        public MerchantSave() { }
        public MerchantSave(string id, Vector3 _position, bool _isAlive, float _health, List<Item> inventory, List<Item> shopItems, int gold)
        {
            ID = id;
            Position = _position;
            IsAlive = _isAlive;
            Health = _health;
            Inventory = new InventorySave(inventory);
            ShopItems = new InventorySave(shopItems, gold);
        }
    }
}