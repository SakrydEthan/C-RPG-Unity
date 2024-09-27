using System;
using UnityEngine;
using Assets.Scripts.Items;

[Serializable]
public class QuestItemObjective : QuestObjective
{
    [SerializeField] Item[] items;

    public override bool CheckCompleted()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (!PlayerInventoryController.instance.HasItem(items[i])) return false;
        }

        return true;
    }

    public Item[] GetItems() { return items; }
}
