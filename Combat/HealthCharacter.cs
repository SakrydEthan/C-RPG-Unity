using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class HealthCharacter : Health
    {
        public float staggerDamage = 1f;
        public BaseCharacter character;

        // Start is called before the first frame update
        void Start()
        {
            character = GetComponent<BaseCharacter>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Damage(float damage)
        {
            base.Damage(damage);
            if (damage > staggerDamage)
            {
                //stagger the character, to prevent them from attacking
                character.GetStaggered();
            }
            if (health < 0) Die();
        }

        public override void Damage(DamageData damage)
        {
            if (character is Enemy) ((Enemy)character).WakeEnemy(PlayerInstanceController.instance.transform);
            float resist = 1f + (resistance[(int)damage.type] / AttributesCalculator.ARMORDMGRED);
            health -= damage.amount / resist;
            if(damage.stagger > staggerDamage) character.GetStaggered();
            if (health < 0) Die();
        }

        public override void Die()
        {
            character.GetKilled();
        }
    }
}
