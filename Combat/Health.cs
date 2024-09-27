using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Health : MonoBehaviour
    {
        public Faction faction = Faction.Unaligned;
        public float maxHitpoints = 25f;
        public float hitpoints = 25f;
        public float[] resistance = { 0f, 0f, 0f, 0f, 0f };

        public virtual void Damage(float damage)
        {
            hitpoints -= damage;
            if (hitpoints < 0) Die();
        }

        public virtual void Damage(DamageData damage)
        {
            float resist = 1f + (resistance[(int)damage.type] / AttributesCalculator.ARMORDMGRED);
            hitpoints -= damage.amount/resist;
            if (hitpoints < 0) Die();
        }

        public virtual void SetHP(float hp)
        {
            hitpoints = hp;
        }

        public virtual void SetMaxHP(float maxHP)
        {
            maxHitpoints = maxHP;
        }

        public virtual void AddHP(float amount)
        {
            hitpoints = Mathf.Clamp(hitpoints+amount, 0f, maxHitpoints);
        }

        public virtual void Die()
        {
            //if (GetComponent<BaseCharacter>()) GetComponent<BaseCharacter>().GetKillExp();
            Destroy(gameObject);
        }

        public virtual float GetHP() { return hitpoints; }
    }
}
