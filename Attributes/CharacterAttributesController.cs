using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Saving;

public class CharacterAttributesController : AttributesController
{
    private void Start()
    {
        fixedHPRegen = hpRegen * Time.fixedDeltaTime;
        fixedStmRegen = stmRegen * Time.fixedDeltaTime;
        fixedManaRegen = manaRegen * Time.fixedDeltaTime;

        CreateFactors();
    }

    public override void CreateFactors()
    {
        base.CreateFactors();

        //check if character has save
        Save? save = SaveController.GetSaveObjectByID(GetComponent<BaseCharacter>().id);
        //no save for this character
        if(save == null)
        {
            hp = maxHP;
            stamina = maxStamina;
            mana = maxMana;
        }
        else
        {
            if (save is BaseCharacterSave)
            {
                BaseCharacterSave based = (BaseCharacterSave)save;
                hp = based.Health;
                //player shouldn't be able to save during combat, so just assume the enemy regains all their
                //stamina and mana by the time they are reengaged
                stamina = maxStamina;
                mana = maxMana;
            }
            else Debug.LogWarning("a character did not hash to a character save");
        }
    }
}
