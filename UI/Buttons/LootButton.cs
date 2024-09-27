using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootButton : MonoBehaviour
{
    public Item item;

    public void SetItem(Item _item)
    {
        item = _item;
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = _item.itemName;
    }

    public void LootItem()
    {
        PlayerInstanceController.instance.inventory.AddItem(item);
        Destroy(gameObject);
    }

}
