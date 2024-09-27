using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using TMPro;
using UnityEngine;


namespace Assets.Scripts.UI
{
    public class InventoryButton : MonoBehaviour
    {

        [SerializeField] Item item;

        public void SetItem(Item _item)
        {
            item = _item;
            GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
        }

        //public void ShowItemDescription() => UIInventory.instance.UpdateItemText(item);
        public void UpdateActiveItem() => UIInventory.instance.UpdateActiveItem(item);
    }
}