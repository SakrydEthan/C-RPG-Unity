using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewGameStartingClassButton : MonoBehaviour
{
    public NewGameTraitCollection traitCollection;
    public StartingClass startingClass;
    NewGameItemCollection itemCollection;

    bool isSelected = false;


    public void Start()
    {
        traitCollection = GetComponentInParent<NewGameTraitCollection>();
        itemCollection = GetComponentInParent<NewGameItemCollection>();
    }

    public void SetClass(StartingClass startingClass)
    {
        this.startingClass = startingClass;
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        if (text != null) text.text = startingClass.GetTraitName();
    }

    public void SetStartingClassButton()
    {
        NewGameController.SetStartingClass(startingClass);
        NewGameController.UpdateDescriptionText(startingClass);
    }
}