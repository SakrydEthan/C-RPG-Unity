using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttributeButton : MonoBehaviour
{

    [SerializeField] Attribute attribute;
    [SerializeField] TextMeshProUGUI attributeName;
    [SerializeField] TextMeshProUGUI attributeValue;
    public void SetAttribute(Attribute attribute) { 
        this.attribute = attribute; 
        attributeName.text = StatNames.GetAttributeName(attribute);
        UpdateValue(); 
    }

    public void IncreaseAttribute()
    {
        if (!PlayerAttributesController.instance.CanIncreaseAttribute()) return;
        PlayerAttributesController.instance.IncreaseAttribute(1, attribute);
        UISkills.instance.UpdateValues();
    }

    public void UpdateValue()
    {
        attributeValue.text = PlayerInstanceController.instance.attributes.GetAttribute(attribute).ToString();
    }
}
