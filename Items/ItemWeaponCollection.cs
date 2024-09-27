using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create New/Combat/Weapon Collection", order = 1)]
public class ItemWeaponCollection : ScriptableObject
{
    [SerializeField] ItemWeapon[] weapons;

    public ItemWeapon GetRandomWeapon()
    {
        int rand = Random.Range(0, weapons.Length);
        return weapons[rand];
    }
}
