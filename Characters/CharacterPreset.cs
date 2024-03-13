using Assets.Scripts.Items;
using System;
using UnityEngine;

namespace Assets.Scripts.Character
{

    [CreateAssetMenu(fileName = "CharacterPreset", menuName = "Create New/Character/Preset")]
    public class CharacterPreset : ScriptableObject
    {

        public Faction faction;
        [Tooltip("Shield, Blunt, Sharp, Ranged")]
        public int[] skills = new int[4];
        public int level = 1;

        public float maxHP = 20f;

        public bool isAggressive = true;

        public ItemWeapon weapon;
        public ItemWeapon[] weapons;
        public ItemWeaponCollection collection;

        [Tooltip("Slash, Blunt, Pierce, Magic, Poison")]
        public float[] resistances = new float[5];

        public Item[] inventory;

        public ItemWeapon GetWeapon()
        {
            if(weapon != null) return weapon;
            int rand = UnityEngine.Random.Range(0, weapons.Length);
            return weapons[rand];
            //return collection.GetRandomWeapon();
        }
    }
}