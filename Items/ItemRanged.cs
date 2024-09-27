using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create New/Item/Ranged Weapon", order = 1)]
public class ItemRanged : ItemWeapon
{
    public AnimatorOverrideController animOver;
    public GameObject projectile;

    public float projectileSpeed = 50f;
}
