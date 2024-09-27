using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Create New/Item/Shield", order = 1)]
public class ItemShield : ItemWeapon
{
    public float blockFactor = 1f;
    public float blockAmount = 7f;
}
