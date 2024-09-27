using Assets.Scripts.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesController : MonoBehaviour
{
    [SerializeField] protected Health health;
    [SerializeField] protected int[] skills = new int[(int)Skill.UNASSIGNED];
    [SerializeField] protected int[] attributes = new int[(int)Attribute.UNASSIGNED];

    [SerializeField] protected float maxHP;
    [SerializeField] protected float maxStamina;
    [SerializeField] protected float maxMana;

    [SerializeField] protected float hp;
    [SerializeField] protected float stamina;
    [SerializeField] protected float mana;

    [SerializeField] protected float hpRegen = 0f;
    [SerializeField] protected float stmRegen = 0f;
    [SerializeField] protected float manaRegen = 0f;

    protected float fixedHPRegen = 0f;
    protected float fixedStmRegen = 0f;
    protected float fixedManaRegen = 0f;

    private void Start()
    {
        //fixedHPRegen = hpRegen * Time.fixedDeltaTime;
        //fixedStmRegen = stmRegen * Time.fixedDeltaTime;
        //fixedManaRegen = manaRegen * Time.fixedDeltaTime;
    }

    public virtual void CreateFactors()
    {
        maxHP = GetMaxHealth();
        maxStamina = GetMaxStamina();
        maxMana = GetMaxMana();

        stmRegen = AttributesCalculator.STAMINAREGENPERAGILITY;
        fixedStmRegen = stmRegen * Time.fixedDeltaTime;
    }

    #region Stats
    public virtual int GetSkill(Skill skill) {
        return skills[(int)skill];
    }

    public virtual int[] GetSkills() {
        return skills;
    }

    public virtual void SetSkill(Skill skill, int level) {
        skills[(int)skill] = level;
    }

    public virtual int GetAttribute(Attribute attribute) {
        return attributes[(int)attribute];
    }

    public virtual int[] GetAttributes()
    {
        return attributes;
    }

    public virtual void SetAttribute(Attribute attribute, int level)
    {
        attributes[(int)attribute] = level;
    }

    public virtual bool CheckSkillLevel(Skill skill, int level)
    {
        return skills[(int)skill] >= level;
    }

    public virtual bool CheckAttributeLevel(Attribute attribute, int level)
    {
        return attributes[(int)attribute] >= level;
    }
    #endregion

    #region Regenables
    public virtual float GetStamina() { return stamina; }
    public virtual float GetMana() { return mana; }
    public virtual float GetMaxHealth()
    {
        return attributes[(int)Attribute.Strength] * AttributesCalculator.HPPERSTRENGTH;
    }

    public virtual float GetMaxStamina()
    {
        return attributes[(int)Attribute.Agility] * AttributesCalculator.STMPERAGILITY;
    }

    public virtual float GetMaxMana()
    {
        return attributes[(int)Attribute.Intelligence] * AttributesCalculator.MANPERINTELLIGENCE;
    }

    public virtual void SetHealth(float hp) { this.hp = hp; GetComponent<Health>().SetHP(hp); }

    public virtual void SetStamina(float stamina) { this.stamina = stamina; }

    public virtual void SetMana(float mana) { this.mana = mana; }

    public virtual void DamageHealth(float amount)
    {
        if(GetComponent<Health>())
        {
            GetComponent<Health>().Damage(amount);
        }
    }

    public virtual void DamageStamina(float amount)
    {
        float newStamina = stamina - amount;
        if(newStamina < 0)
        {
            stamina = 0;
            DamageHealth(-newStamina);
        }
        else stamina = newStamina;
    }

    public virtual void DamageMana(float amount)
    {
        mana -= amount;
    }

    public virtual bool CheckHealth(float amount)
    {
        return health.GetHP() > amount;
    }

    public virtual bool CheckStamina(float amount)
    {
        return stamina > amount;
    }

    public virtual bool CheckMana(float amount)
    {
        return mana > amount;
    }

    public virtual void UpdateHealthRegen(float regen)
    {

    }

    public virtual void UpdateStaminaRegen()
    {
        stmRegen = AttributesCalculator.BASESTAMINAREGEN + (AttributesCalculator.STAMINAREGENPERAGILITY * attributes[(int)Attribute.Agility]);
        fixedStmRegen = stmRegen * Time.fixedDeltaTime;
    }
    #endregion

    #region Damage
    public virtual float GetDamageMultiplier(Skill skill)
    {
        return AttributesCalculator.CalculateDamageMultiplier(skills[(int)skill]);
    }

    public virtual float GetDamageMultiplier(ItemWeapon weapon)
    {
        return AttributesCalculator.CalculateDamageMultiplier(skills[(int)weapon.skill]);
    }

    public virtual float GetWeaponDamage(ItemWeapon weapon)
    {
        return weapon.damage * GetDamageMultiplier(weapon);
    }
    #endregion
}


