using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName ="Consumable", menuName = "Create New/Item/Consumable", order = 0)]
    public class ItemConsumable : Item
    {

        public float amount = 5;

        public override void UseItem()
        {
            PlayerInstanceController.UseConsumable(this);
        }
    }
}