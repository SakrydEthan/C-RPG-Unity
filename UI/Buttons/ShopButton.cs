using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine;
using TMPro;

public class ShopButton : MonoBehaviour
{

    [SerializeField] Item item;

    public void SetItem(Item _item)
    {
        item = _item;
        GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
    }

    public void ShowItem() => UIShop.instance.UpdateActiveItem(item);
}
