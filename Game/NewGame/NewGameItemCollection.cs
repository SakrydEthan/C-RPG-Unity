using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameItemCollection : MonoBehaviour
{
    [SerializeField] bool isComplete = false;
    [SerializeField] int traitsAllowed = 1;
    [SerializeField] int traitsSelected = 0;

    public List<Item> items;
    public List<Item> classItems;

    // Use this for initialization
    void Start()
    {
        NewGameController.instance.itemCollections.Add(this);

        items = new List<Item>();
    }

    public void AddItem(Item item)
    {

        if (traitsAllowed == 1)
        {
            if (traitsSelected == 0)
            {
                traitsSelected++;
                items.Add(item);
                isComplete = true;
            }
            else
            {
                items.Clear();
                items.Add(item);
            }
        }
    }

    public void AddItems(Item[] _items)
    {

        if (traitsAllowed == 1)
        {
            if (traitsSelected == 0)
            {
                traitsSelected++;
                for(int i = 0; i < _items.Length; i++)
                {
                    items.Add(_items[i]);
                }
                isComplete = true;
            }
            else
            {
                items.Clear();
                for (int i = 0; i < _items.Length; i++)
                {
                    items.Add(_items[i]);
                }
            }
        }
    }

    public void RemoveItem(Item item)
    {
        if (!items.Contains(item)) return;
        NewGameController.RemoveItem(item);
        if (traitsSelected > 0) traitsSelected--;
    }

    public void RemoveItems(Item[] _items)
    {
        for(int i=0; i<_items.Length; i++)
        {
            if (items.Contains(_items[i])) NewGameController.RemoveItem(items[i]);
        }
    }

    public void SetClassItems(Item[] _items)
    {
        classItems = new List<Item>();
        for (int i = 0; i < _items.Length; i++)
        {
            classItems.Add(_items[i]);
        }
    }

    public bool CheckComplete() { return isComplete; }
}
