using Assets.Scripts.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, ICombatant
{

    Transform cam;
    public float hitDist = 10f;
    public float damage = 0f;

    public PlayerAttributesController attributes;
    public PlayerInteractionController interaction;
    public Animator animator;

    public Transform rightHand;
    public Transform leftHand;

    public ItemWeapon rightItemWeapon;
    public Weapon rightWeapon;
    GameObject rightGO;

    public ItemWeapon leftItemWeapon;
    public Weapon leftWeapon;
    GameObject leftGO;

    public RuntimeAnimatorController baseAnimator;

    bool hasReleasedAttack = false;
    bool isBusy = false;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main.transform;
        attributes = GetComponent<PlayerAttributesController>();
        interaction = GetComponent<PlayerInteractionController>();
        //baseAnimator = GetComponent<Animator>();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rightWeapon == null) return;
        if (Input.GetButtonDown("Attack"))
        {
            if (interaction.isFrozen) return;
            if (attributes.CheckStamina(rightItemWeapon.weight))
            {
                attributes.DamageStamina(rightItemWeapon.weight);
                animator.SetTrigger("attack");
            }
        }
        if (Input.GetButton("Attack")) Attack();
        if (Input.GetButtonUp("Attack"))
        {
            animator.speed = 1f;
            animator.ResetTrigger("attack");
            hasReleasedAttack = true;
        }
        if (Input.GetButton("Block"))
        {
            if (interaction.isFrozen) return;
            if (!animator.GetBool("isBlocking"))
            {
                animator.SetBool("isBlocking", true);
            }
            if (leftItemWeapon != null)
            {
                if (leftItemWeapon is ItemShield) leftWeapon.isBlocking = true;
                else rightWeapon.isBlocking = true;
            }
            else rightWeapon.isBlocking = true;
        }
        else
        {
            if (animator.GetBool("isBlocking"))
            {
                animator.SetBool("isBlocking", false);
            }
            if (leftItemWeapon != null) {
                if (leftItemWeapon is ItemShield) leftWeapon.isBlocking = false;
                else rightWeapon.isBlocking = false;
            }
            else rightWeapon.isBlocking = false;
        }
    }

    public void SetBaseAnimator()
    {
        baseAnimator = animator.runtimeAnimatorController;
    }

    /// <summary>
    /// Equips a weapon from its item SO. Adds weapon script to its gameobject
    /// </summary>
    /// <param name="_weapon"></param>
    public void EquipWeapon(ItemWeapon _weapon)
    {


        if (_weapon.isRightHanded)
        {
            UnequipRightWeapon();
            if(leftItemWeapon != null)
            {
                if (leftItemWeapon.isTwoHanded || _weapon.isTwoHanded) UnequipLeftWeapon();
            }

            rightItemWeapon = _weapon;
            rightGO = CreateWeaponGO(_weapon, rightHand);
            rightWeapon = CreateWeapon(_weapon, rightGO);
        }
        else
        {
            UnequipLeftWeapon();
            if (rightItemWeapon != null)
            {
                if (rightItemWeapon.isTwoHanded || _weapon.isTwoHanded) UnequipRightWeapon();
            }

            leftItemWeapon = _weapon;
            leftGO = CreateWeaponGO(_weapon, leftHand);
            leftWeapon = CreateWeapon(_weapon, leftGO);
        }

        bool hasLeftWeapon = leftItemWeapon != null;
        bool hasRightWeapon = rightItemWeapon != null;

        if (hasLeftWeapon && leftItemWeapon.overrideController != null) animator.runtimeAnimatorController = leftItemWeapon.overrideController;
        else if (hasRightWeapon && rightItemWeapon.overrideController != null) animator.runtimeAnimatorController = rightItemWeapon.overrideController;
        else { animator.runtimeAnimatorController = baseAnimator; }
    }

    public GameObject CreateWeaponGO(ItemWeapon _weapon, Transform parent)
    {
        GameObject weaponGO = Instantiate(_weapon.model, parent);
        weaponGO.transform.localPosition = _weapon.offset;
        weaponGO.transform.Rotate(_weapon.rotation, Space.Self); 

        return weaponGO;
    }

    public Weapon CreateWeapon(ItemWeapon itemWeapon, GameObject weaponGO)
    {
        Weapon weapon;
        if (itemWeapon is ItemShield) weapon = weaponGO.AddComponent<WeaponShield>();
        else if (itemWeapon is ItemRanged) weapon = weaponGO.AddComponent<Weapon>(); //TODO: replace with weaponranged
        else weapon = weaponGO.AddComponent<Weapon>();

        weapon.SetItemWeapon(itemWeapon, attributes);
        weapon.attacker = gameObject.transform;
        weapon.damage = attributes.GetWeaponDamage(itemWeapon);
        weapon.SetAimSegments(attributes.GetSkill(Skill.Ranged));
        return weapon;
    }



    public void UnequipRightWeapon()
    {
        if(rightItemWeapon == null) return;
        rightItemWeapon = null;
        rightWeapon = null;
        Destroy(rightGO);
    }

    public void UnequipLeftWeapon()
    {
        if (leftItemWeapon == null) return;
        leftItemWeapon = null;
        leftWeapon = null;
        Destroy(leftGO);
    }

    //if player stats change, update the weapon to reflect the changes
    public void UpdateWeaponDamage()
    {
        if (rightWeapon == null) return;
        int wepAttr = attributes.GetSkill(rightWeapon.GetWeaponSkill());
        rightWeapon.SetDamageBySkill(wepAttr);
    }

    public float GetWeaponDamage(ItemWeapon weapon)
    {
        return weapon.damage * attributes.GetDamageMultiplier(weapon.skill);
    }

    public void Attack()
    {
        if (PlayerInstanceController.CheckPlayerFrozen()) return;
        Debug.Log("player attacking");

        if (rightWeapon.isAttacking && !hasReleasedAttack) animator.speed = 0f;
    }

    public void AttackStart()
    {
        if (rightWeapon == null) return;
        rightWeapon.StartAttack();
        hasReleasedAttack = false;
    }
    public void AttackEnd()
    {
        if (rightWeapon == null) return;
        rightWeapon.FinishAttack();
        animator.ResetTrigger("attack");
    }
    public void ShootProjectile()
    {
        rightWeapon.ShootWeapon();
    }

    public void SetBusy()
    {
        isBusy = true;
    }

    public void UnsetBusy()
    {
        isBusy = false;
    }
    //public void
}
