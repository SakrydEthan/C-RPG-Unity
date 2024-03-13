using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Trait", menuName = "Create New/Trait")]
public class Trait : ScriptableObject
{
    [SerializeField] string traitName;
    [SerializeField] SkillBonus[] skills;
    [SerializeField] string description = "";

    public SkillBonus[] GetSkills()
    {
        return skills;
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