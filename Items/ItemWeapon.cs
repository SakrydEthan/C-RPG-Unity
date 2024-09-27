using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create New/Item/Weapon", order = 1)]
public class ItemWeapon : Item
{
    public Skill skill = Skill.Sharp;
    public float damage = 2f;
    public DamageType damageType = DamageType.Slash;
    public bool isRightHanded = true;
    public bool isTwoHanded = false;
    public bool canBlock = true;
    public Vector3 offset;
    public Vector3 rotation = new Vector3(0f, -90f, 0f);
    public AnimatorOverrideController overrideController = null;

    public AudioClip hitSound;

    public override void UseItem()
    {
        //equip weapon
        PlayerInstanceController.EquipWeapon(this);
    }
}
