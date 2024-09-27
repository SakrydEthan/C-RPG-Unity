

using UnityEngine;
using Assets.Scripts.Attributes;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Create New/Item/Equipable", order = 0)]
    public class ItemEquipable : Item
    {
        [SerializeField] EquipSlot slot = EquipSlot.Body;

        [Tooltip("Slash, Blunt, Pierce, Magic, Poison")]
        public float[] resistance = new float[5];
        public SkillBonus[] bonuses;


        public override void UseItem()
        {
            PlayerEquipmentController.instance.EquipItem(this);
        }

        public EquipSlot GetSlot() { return slot; }
    }
}