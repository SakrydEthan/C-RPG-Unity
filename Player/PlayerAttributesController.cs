using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Saving;
using Assets.Scripts.Attributes;
using UnityEngine;
using Assets.Scripts.Combat;

public class PlayerAttributesController : AttributesController
{
    public static PlayerAttributesController instance;
    public PlayerCombatController combatCon;
    public PlayerLevelController levelCon;
    public PlayerUIController uiCon;

    public List<Trait> traits;

    public int[] skillBonuses = new int[(int)Skill.UNASSIGNED];

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        combatCon = GetComponent<PlayerCombatController>();
        levelCon = GetComponent<PlayerLevelController>();
        uiCon = GetComponent<PlayerUIController>();
        health = GetComponent<PlayerHealthController>();
        fixedHPRegen = hpRegen * Time.fixedDeltaTime;
        fixedStmRegen = stmRegen * Time.fixedDeltaTime;
        fixedManaRegen = manaRegen * Time.fixedDeltaTime;
        //LoadAttributes();
        //CreateFactors();
    }

    void FixedUpdate()
    {
        //increase hp, stamina, and mana by their respective regen amounts
        stamina = Mathf.Clamp(stamina + fixedStmRegen, 0, maxStamina);
        uiCon.UpdateStamina(stamina, maxStamina);
    }

    public void LoadSave(PlayerSave save)
    {
        for (int i = 0; i < (int)Skill.UNASSIGNED; i++) {
            skills[i] = save.skills[i];
        }
        for (int i = 0; i < (int)Attribute.UNASSIGNED; i++)
        {
            attributes[i] = save.attributes[i];
        }
    }

    public override void CreateFactors()
    {
        base.CreateFactors();
        Debug.Log("creating player's factors");

        GetComponent<Health>().SetHP(maxHP);
        GetComponent<Health>().SetMaxHP(maxHP);

        Debug.Log($"Setting HP bar to {hp}, {maxHP}");
        uiCon.UpdateHealth(hp, maxHP);
        Debug.Log($"Setting Stamina bar to {stamina}, {maxStamina}");
        uiCon.UpdateStamina(stamina, maxStamina);
        Debug.Log($"Setting Mana bar to {mana}, {maxMana}");
        uiCon.UpdateMana(mana, maxMana);

        Debug.Log("updating ui to show player factors");
        if(uiCon == null) uiCon = GetComponent<PlayerUIController>();

        Debug.Log("max hp is: " + maxHP);
    }

    public void LoadSavedFactors(PlayerSave save)
    {
        Debug.Log($"Loading player's saved hp: {save.hp}, stamina: {save.stamina}, and mana: {save.mana}");
        SetHealth(save.hp);
        SetStamina(save.stamina);
        SetMana(save.mana);
    }

    public void IncreaseSkill(int amount, Skill skill)
    {
        if (levelCon.skillIncreasesAvailable <= 0) return;
        levelCon.skillIncreasesAvailable--;
        skills[(int)skill] += amount;
        combatCon.UpdateWeaponDamage();
    }

    public void IncreaseAttribute(int amount, Attribute attribute)
    {
        if (levelCon.attributeIncreasesAvailable <= 0) return;
        levelCon.attributeIncreasesAvailable--;
        attributes[(int)attribute] += amount;
        //update mana, stamina, and hp values and update UI to reflect changes
        maxHP = GetMaxHealth();
        maxMana = GetMaxMana();
        maxStamina = GetMaxStamina();
        UpdateStaminaRegen();
    }

    public void AddSkillBonus(SkillBonus bonus)
    {
        skillBonuses[(int)bonus.skill] += bonus.amount;
        if(combatCon != null) combatCon.UpdateWeaponDamage();
    }
    public void RemoveSkillBonus(SkillBonus bonus)
    {
        skillBonuses[(int)bonus.skill] -= bonus.amount;
        if (combatCon != null) combatCon.UpdateWeaponDamage();
    }

    public override int GetSkill(Skill skill)
    {
        return skills[(int)skill] + skillBonuses[(int)skill];
    }

    public bool CanIncreaseSkill()
    {
        return levelCon.skillIncreasesAvailable > 0;
    }

    public bool CanIncreaseAttribute()
    {
        return levelCon.attributeIncreasesAvailable > 0;
    }

    public void LoadAttributes()
    {
        if(SaveController.CheckForSave())
        {
            //load attributes and traits from save
            PlayerSave save = SaveController.GetPlayerSave();
            skills = save.skills;
            foreach(Trait trait in save.traits)
            {
                traits.Add(trait);
            }
        }
        else
        {
            traits = NewGameController.instance.playerTraits;
            skills = new int[(int)Skill.UNASSIGNED];
            foreach (Trait trait in traits)
            {
                foreach(SkillBonus bonus in trait.GetSkills())
                {
                    skills[(int)bonus.skill] += bonus.amount;
                }
                foreach (AttributeBonus bonus in trait.GetAttributes())
                {
                    attributes[(int)bonus.attribute] += bonus.amount;
                }
            }
        }
    }

    public void LoadSavedAttributes(PlayerSave save)
    {
        if (SaveController.CheckForSave())
        {
            skills = save.skills;
            attributes = save.attributes;
            foreach (Trait trait in save.traits)
            {
                traits.Add(trait);
            }
        }
        UpdateStaminaRegen();
    }

    public void GetStartingTraits()
    {
        traits = NewGameController.instance.playerTraits;
        foreach (Trait trait in traits)
        {
            foreach (SkillBonus bonus in trait.GetSkills())
            {
                skills[(int)bonus.skill] += bonus.amount;
            }
            foreach (AttributeBonus bonus in trait.GetAttributes())
            {
                attributes[(int)bonus.attribute] += bonus.amount;
            }
        }
        UpdateStaminaRegen();
    }

    public void ApplyStartingTrait(Trait trait)
    {
        AttributeBonus[] attBonuses = trait.GetAttributes();
        SkillBonus[] skillBonuses = trait.GetSkills();

        foreach (AttributeBonus bonus in attBonuses)
        {
            attributes[(int)bonus.attribute] += bonus.amount;
        }
        foreach (SkillBonus bonus in skillBonuses)
        {
            skills[(int)bonus.skill] += bonus.amount;
        }
        UpdateStaminaRegen();

        UpdateMaxFactors();
        hp = maxHP;
        stamina = maxStamina;
        mana = maxMana;

        //health.SetMaxHP(maxHP);
        //health.SetHP(maxHP);
    }

    public void ApplyTraits()
    {

    }

    public void UpdateMaxFactors()
    {
        maxHP = GetMaxHealth();
        maxStamina = GetMaxStamina();
        maxMana = GetMaxMana();
    }

    public override void DamageHealth(float amount)
    {
        base.DamageHealth(amount);
        uiCon.UpdateHealth(health.hitpoints, amount);

    }

    public override void DamageStamina(float amount)
    {
        base.DamageStamina(amount);
        uiCon.UpdateStamina(stamina, maxStamina);
    }

    public override void DamageMana(float amount)
    {
        base.DamageMana(amount);
        uiCon.UpdateMana(mana, maxMana);
    }

    public override void SetHealth(float hp)
    {
        base.SetHealth(hp);
        uiCon.UpdateHealth(hp, maxHP);
    }

    public virtual void AddHealth(float amount)
    {
        hp = Mathf.Clamp(hp+amount, 0f, maxHP);
        health.SetHP(hp);
        uiCon.UpdateHealth(health.hitpoints, maxHP);
    }

    public virtual void AddStamina(float amount)
    {
        stamina = Mathf.Clamp(stamina + amount, 0f, maxStamina);
        uiCon.UpdateStamina(stamina, maxStamina);
    }

    public virtual void AddMana(float amount)
    {
        mana = Mathf.Clamp(mana + amount, 0f, maxMana);
        uiCon.UpdateMana(mana, maxMana);
    }

    public override void SetStamina(float stamina)
    {
        base.SetStamina(stamina);
        uiCon.UpdateStamina(stamina, maxStamina);
    }

    public override void SetMana(float mana)
    {
        base.SetMana(mana);
        uiCon.UpdateMana(mana, maxMana);
    }
}
