using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Create New/Item/Basic Item", order = 0)]
    public class Item : ScriptableObject
    {
        public Texture2D itemIcon;
        public GameObject model;
        public ItemCategory category = ItemCategory.Junk;
        public string itemName;
        public int value;
        [TextArea]
        public string itemDescription;
        public float weight = 1f;

        public virtual void UseItem()
        {
            //use behavior
        }

    }

    public enum ItemCategory
    {
        Weapon,
        Wearable,
        Consumable,
        Magic,
        Book,
        Quest,
        Junk
    }
}
