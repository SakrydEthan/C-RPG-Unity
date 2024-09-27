using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameItemButton : MonoBehaviour
{
    public Item[] items;
    NewGameItemCollection itemCollection;


    // Start is called before the first frame update
    void Start()
    {
        itemCollection = GetComponentInParent<NewGameItemCollection>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        itemCollection.AddItems(items);
    }
}
