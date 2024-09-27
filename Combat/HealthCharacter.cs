using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class HealthCharacter : Health
    {
        public float staggerDamage = 1f;
        public BaseCharacter character;

        public AudioSource source;
        public AudioClip hitSound;

        [SerializeField] float sharpStaggerLight = 0.1f;
        [SerializeField] float sharpStaggerMed = 0.25f;
        [SerializeField] float sharpStaggerHvy = 0.4f;

        [SerializeField] float bluntStaggerLight = 1f;
        [SerializeField] float bluntStaggerMed = 2f;
        [SerializeField] float bluntStaggerHvy = 6f;

        // Start is called before the first frame update
        void Start()
        {
            character = GetComponent<BaseCharacter>();
            source = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Damage(float damage)
        {
            base.Damage(damage);
            source.clip = hitSound;
            source.Play();
            if (damage > staggerDamage)
            {
                //stagger the character, to prevent them from attacking
                character.GetStaggered();
            }
            if (hitpoints < 0) Die();
        }

        public override void Damage(DamageData damage)
        {
            if (!character.isAlive) return;
            source.clip = hitSound;
            source.Play();
            if (character is Enemy) ((Enemy)character).WakeEnemy(PlayerInstanceController.instance.transform);
            float resist = 1f + (resistance[(int)damage.type] / AttributesCalculator.ARMORDMGRED);
            hitpoints -= damage.amount / resist;

            //old method of staggering
            //if(damage.stagger > staggerDamage) character.GetStaggered();
            
            //new method, split into sharp and blunt (blunt should be better at staggering at the cost of dps)
            if(damage.type == DamageType.Slash)
            {
                float staggerFactor = (damage.amount / resist) / maxHitpoints;

                if (staggerFactor > sharpStaggerHvy) character.GetStaggeredHeavy();
                else if (staggerFactor > sharpStaggerMed) character.GetStaggeredMedium();
                else if(staggerFactor > sharpStaggerLight) character.GetStaggeredLight();
            }
            if(damage.type == DamageType.Blunt)
            {
                float afterBluntReduction = damage.stagger - resistance[(int)DamageType.Blunt];
                Debug.Log("Blunt stagger number: " + damage.stagger+", after reduction: "+afterBluntReduction);

                if (afterBluntReduction > bluntStaggerHvy)
                {
                    Debug.Log(character.name + " was staggered (heavy)");
                    character.GetStaggeredHeavy();
                }
                else if (afterBluntReduction > bluntStaggerMed)
                {
                    Debug.Log(character.name + " was staggered (medium)");
                    character.GetStaggeredMedium();
                }
                else if (afterBluntReduction > bluntStaggerLight)
                {
                    Debug.Log(character.name + " was staggered (light)");
                    character.GetStaggeredLight();
                }
            }


            if (hitpoints < 0) Die();
        }

        public override void Die()
        {
            character.GetKilled();
        }
    }
}
