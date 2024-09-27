using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Attributes;
public class NewGameTraitButton : MonoBehaviour
{
    public NewGameTraitCollection traitCollection;
    public Trait trait;

    bool isSelected = false;


    public void Start()
    {
        traitCollection = GetComponentInParent<NewGameTraitCollection>();
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        if (text != null) text.text = trait.GetTraitName();
    }

    public void AddTrait()
    {
        traitCollection.AddTrait(trait);
        NewGameController.UpdateDescriptionText(trait);

        /*(
        if (!isSelected)
        {
            NewGameController.UpdateDescriptionText(trait);
            traitCollection.AddTrait(trait);
            isSelected = true;
        }
        else
        {
            NewGameController.ClearDescriptionText();
            isSelected = false;
            traitCollection.RemoveTrait(trait);
        }
        */
    }
}
