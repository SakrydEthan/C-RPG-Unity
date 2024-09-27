using Assets.Scripts.Items;
using System;
using UnityEngine;


namespace Assets.Scripts.Attributes
{
    [CreateAssetMenu(fileName = "StarterClass", menuName = "Create New/Starting Class")]
    public class StartingClass : Trait
    {
        [SerializeField] Item[] startingItems;

        public Item[] GetStartingItems() { return startingItems; }
    }
}