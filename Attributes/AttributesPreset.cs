using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributesPreset", menuName = "Create New/Attributes/Preset")]
public class AttributesPreset : ScriptableObject
{
    [SerializeField] int[] weaponSkills = new int[4];

    public int[] GetWeaponSkills() { return weaponSkills; }
}
