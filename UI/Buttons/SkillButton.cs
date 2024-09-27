using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillButton : MonoBehaviour
{

    [SerializeField] Skill skill;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillValue;
    public void SetSkill(Skill skill) { 
        this.skill = skill; 
        skillName.text = StatNames.GetSkillName(skill);
        UpdateValue(); 
    }

    public void IncreaseSkill()
    {
        if(!PlayerAttributesController.instance.CanIncreaseSkill()) return;
        PlayerAttributesController.instance.IncreaseSkill(1, skill);
        UISkills.instance.UpdateValues();
    }

    public void UpdateValue()
    {
        skillValue.text = PlayerInstanceController.instance.attributes.GetSkill(skill).ToString();
    }
}
