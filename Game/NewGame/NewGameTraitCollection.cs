using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using Assets.Scripts.Attributes;

public class NewGameTraitCollection : MonoBehaviour
{
    [SerializeField] bool isComplete = false;
    [SerializeField] bool choseClass = false;
    [SerializeField] int traitsAllowed = 1;
    [SerializeField] int traitsSelected = 0;

    public List<Trait> traits;

    public StartingClass startingClass;

    // Use this for initialization
    void Start()
    {
        NewGameController.instance.traitCollections.Add(this);

        traits = new List<Trait>();
    }

    public void AddTrait(Trait trait)
    {

        if(traitsAllowed == 1)
        {
            if(traitsSelected == 0)
            {
                traitsSelected++;
                traits.Add(trait);
                isComplete = true;
            }
            else
            {
                traits.Clear();
                traits.Add(trait);
            }
        }


        #region old
        /*
        if(traitsAllowed == 1)
        {
            
            if(traitsSelected == 1)
            {
                if (trait = traits[0]) return;
                NewGameController.RemoveTrait(traits[0]);
                traits.RemoveAt(0);

                NewGameController.AddTrait(trait);
                traits.Add(trait);
            }
            if (traitsSelected == 0)
            {
                traitsSelected = 1;

                NewGameController.AddTrait(trait);
                traits.Add(trait);
            }
        
        }
        else if(traitsSelected < traitsAllowed)
        {
            traitsSelected++;
            NewGameController.AddTrait(trait);
            traits.Add(trait);
            if(traitsSelected == traitsAllowed) isComplete = true;
        }
        */
        #endregion old
    }

    public void RemoveTrait(Trait trait)
    {
        if (!traits.Contains(trait)) return;
        NewGameController.RemoveTrait(trait);
        if(traitsSelected > 0) traitsSelected--;
    }

    public bool CheckComplete() { return isComplete && choseClass; }

    public void SetStartingClass(StartingClass startingClass)
    {
        this.startingClass = startingClass;
        choseClass = true;
    }
}
