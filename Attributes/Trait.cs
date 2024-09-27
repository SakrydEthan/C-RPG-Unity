using System;
using UnityEngine;


namespace Assets.Scripts.Attributes
{
    [CreateAssetMenu(fileName = "Trait", menuName = "Create New/Trait")]
    public class Trait : ScriptableObject
    {
        [SerializeField] string traitName;
        [SerializeField] SkillBonus[] skills;
        [SerializeField] AttributeBonus[] attributes = new AttributeBonus[(int)Attribute.UNASSIGNED];
        [SerializeField] string description = "";

        public SkillBonus[] GetSkills()
        {
            return skills;
        }

        public AttributeBonus[] GetAttributes()
        {
            return attributes;
        }

        public string GetTraitName()
        {
            return traitName;
        }

        public string GetDescription()
        {
            return description;
        }
    }
}