using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Health : MonoBehaviour
    {
        public Faction faction = Faction.Unaligned;
        public float maxHealth = 25f;
        public float health = 25f;
        public float[] resistance = { 0f, 0f, 0f, 0f, 0f };

        public virtual void Damage(float damage)
        {
            health -= damage;
            if (health < 0) Die();
        }

        public virtual void Damage(DamageData damage)
        {
            float resist = 1f + (resistance[(int)damage.type] / AttributesCalculator.ARMORDMGRED);
            health -= damage.amount/resist;
            if (health < 0) Die();
        }

        public virtual void Die()
        {
            //if (GetComponent<BaseCharacter>()) GetComponent<BaseCharacter>().GetKillExp();
            Destroy(gameObject);
        }
    }
}
