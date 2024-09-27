using Assets.Scripts.Attributes;
using Assets.Scripts.Items;
using System;
using UnityEngine;

namespace Assets.Scripts.Character
{

    [CreateAssetMenu(fileName = "CharacterPreset", menuName = "Create New/Character/Preset")]
    public class CharacterPreset : ScriptableObject
    {

        [SerializeField] Faction faction;
        [Tooltip("How the character will distribute skill points per level.\nShield, Blunt, Sharp, Ranged, Stealth, Alchemy, Speech")]
        [SerializeField] float[] skills = new float[(int)Skill.UNASSIGNED];
        [Tooltip("How the character will distribute attribute points per level.\nStrength, Agility, Intelligence")]
        [SerializeField] float[] attributes = new float[(int)Attribute.UNASSIGNED];
        [SerializeField] int level = 1;

        [SerializeField] Trait[] traits;
        [SerializeField] bool isAggressive = true;

        public ItemWeapon weapon;
        public ItemWeapon[] weapons;
        public ItemWeaponCollection collection;

        //[SerializeField] ItemCollection

        [Tooltip("Slash, Blunt, Pierce, Magic, Poison")]
        [SerializeField] float[] baseResistances = new float[5];

        [SerializeField] Item[] inventory;

        public ItemWeapon GetWeapon()
        {
            if(weapon != null) return weapon;
            else return null;
            //int rand = UnityEngine.Random.Range(0, weapons.Length);
            //return weapons[rand];
            //return collection.GetRandomWeapon();
        }

        public int[] GetSkills()
        {
            //normalize skill distribution
            float sum = 0;
            for(int i = 0; i < skills.Length; i++)
            {
                sum += skills[i];
            }

            //multiply skill by level
            int[] finalSkills = new int[(int)Skill.UNASSIGNED];
            for(int i = 0; i < finalSkills.Length; i++)
            {
                //finalSkills[i] = (int)(skills[i]*level/sum);
                float temp = (skills[i] * level * 10f) / sum;
                finalSkills[i] = (int)temp;
            }
            //add trait skills
            for (int i = 0; i < traits.Length; i++)
            {
                SkillBonus[] bonuses = traits[i].GetSkills();
                for(int j=0; j < bonuses.Length; j++)
                {
                    finalSkills[(int)(bonuses[j].skill)] += bonuses[j].amount;
                }
            }
            return finalSkills;
        }

        public int[] GetAttributes()
        {
            //normalize attribute distribution
            float sum = 0;
            for (int i = 0; i < attributes.Length; i++)
            {
                sum += attributes[i];
            }

            //multiply attributes by level
            int[] finalAttributes = new int[(int)Attribute.UNASSIGNED];
            for (int i = 0; i < finalAttributes.Length; i++)
            {
                finalAttributes[i] = (int)(attributes[i] * level / sum);
            }
            //add trait attributes
            for (int i = 0; i < traits.Length; i++)
            {
                AttributeBonus[] bonuses = traits[i].GetAttributes();
                for (int j = 0; j < bonuses.Length; j++)
                {
                    finalAttributes[(int)(bonuses[j].attribute)] += bonuses[j].amount;
                }
            }
            return finalAttributes;
        }

        public bool CheckAggressive() { return isAggressive; }
    }
}