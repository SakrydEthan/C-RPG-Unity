using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;


namespace Assets.Scripts.Combat
{
    public class WeaponShield : Weapon
    {
        [SerializeField] protected float blockFactor = 1f;

        public override void SetItemWeapon(ItemWeapon weapon, AttributesController attributes)
        {
            if (weapon is not ItemShield) Debug.LogWarning(weapon.name + " was assigned weaponshield, but not itemshield");
            else
            {
                blockFactor = ((ItemShield)weapon).blockFactor;
            }
            this.attributes = attributes;

            this.weapon = weapon;
            type = weapon.damageType;
            if (weapon.hitSound != null)
            {
                hitSound = weapon.hitSound;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isAttacking)
            {
                if (other.GetComponent<Health>())
                {
                    if (other.GetComponent<Health>() != health)
                    {
                        if (hasHit || other.GetComponent<Health>().faction == faction) return;
                        hasHit = true;
                        DamageData dd = GetDamage();
                        other.GetComponent<Health>().Damage(dd);
                        source.Play();
                        //Debug.Log("Weapon hit " + other.name);
                    }
                }
            }
            if (isBlocking)
            {
                if (other.GetComponent<Weapon>())
                {
                    Weapon weapon = other.GetComponent<Weapon>();
                    weapon.GetBlocked();
                    attributes.DamageStamina(damage / blockFactor);
                }
            }
        }
    }
}
