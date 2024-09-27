using System;
using Assets.Scripts.Items;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Attributes;

namespace Assets.Scripts.Saving
{
    public class PlayerSave : Save
    {
        public string scene;
        public Vector3 pos;
        public Vector3 rot;

        public float hp;
        public float stamina;
        public float mana;

        public int level;
        public int xp;
        public int skillPoints;
        public int[] skills;
        public int[] attributes;
        public Trait[] traits;

        public ItemWeapon activeWeapon;
        public ItemEquipable[] equipment;
        public InventorySave inventory;

        /*
         * TODO: add different save classes, e.g. inventory save class saves space
         * LocationSave: scene, player position and look rotation
         * StatsSave: traits, skills, attributes, assignable points player has available
         * EquipmentSave: what the player has equipped, e.g. their active weapon and armor, rings, effects
        */

        public PlayerSave(string _scene, Vector3 _pos, Vector3 _rot, 
            float _hp, float _stamina, float _mana, int _level, int _xp, int _sPs, int _aPs, int[] _skills, int[] _attributes, Trait[] _traits, 
            ItemWeapon _activeWeapon, ItemEquipable[] _equipment, List<Item> _items, 
            int _gold, float _totalWeight)
        {
            scene = _scene;
            pos = _pos;
            rot = _rot;

            hp = _hp;
            stamina = _stamina;
            mana = _mana;

            level = _level;
            xp = _xp;
            skillPoints = _sPs;
            skills = _skills;
            attributes = _attributes;
            traits = _traits;

            activeWeapon = _activeWeapon;
            equipment = _equipment;
            inventory = new InventorySave(_items, _gold, _totalWeight);
        }
    }
}