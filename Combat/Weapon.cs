using Assets.Scripts.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    public BaseCharacter character;
    public Health health;
    public ItemWeapon weapon;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public PlayerCombatController player;
    protected Faction faction;
    public Transform attacker;

    public GameObject projectile;

    public AudioSource source;
    public AudioClip hitSound;

    public float damage;
    [SerializeField] protected bool hasHit = false;

    protected DamageType type;
    protected bool isAI = true;

    //set linesegments to archery skill
    public int lineSegments = 63;
    public LineRenderer lineRenderer;
    [SerializeField] protected AttributesController attributes;


    void Start()
    {
        if(health == null) health = GetComponentInParent<Health>();

        if ((character = GetComponentInParent<BaseCharacter>()) == null)
        {
            isAI = false;
            player = GetComponentInParent<PlayerCombatController>();
        }

        if (GetComponent<Collider>())
        {
            Collider collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = hitSound;
        faction = health.faction;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        if(lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = lineSegments;
        lineRenderer.endColor = Color.red;
        lineRenderer.startColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(weapon != null && isAttacking)
        {
            if(weapon is ItemRanged)
            {
                DrawProjectileTrajectory();
            }
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
                }
            }
        }
        if(isBlocking)
        {
            if(other.GetComponent<Weapon>())
            {
                Weapon weapon = other.GetComponent<Weapon>();
                weapon.GetBlocked();
                attributes.DamageStamina(weapon.damage);
            }
        }
    }

    public virtual void SetItemWeapon(ItemWeapon weapon, AttributesController attributes) 
    { 
        this.weapon = weapon; 
        type = weapon.damageType; 
        if(weapon.hitSound != null)
        {
            hitSound = weapon.hitSound;
        }
        this.attributes = attributes;

        if(weapon is ItemRanged)
        {
            projectile = ((ItemRanged)weapon).projectile;
        }
    }

    public DamageData GetDamage()
    {
        float blunt = 0f;
        if (player != null) blunt = player.attributes.GetSkill(Skill.Blunt);
        float stagger = (weapon.weight * AttributesCalculator.WEPWGTSTGRFCTR) + (blunt / AttributesCalculator.BLUNTSKILLSTAGGERDIVISOR);
        if(isAI) return new DamageData(character.transform, damage, type, stagger);
        else return new DamageData(player.transform, damage, type, stagger);
    }

    public void SetAimSegments(int segments)
    {
        lineSegments = segments;
    }

    public void ShootWeapon()
    {
        lineRenderer.enabled = false;
        Quaternion shootDir = new Quaternion(transform.forward.x, transform.forward.y, -transform.forward.z, 0f);

        GameObject go = Instantiate(projectile, transform.position, shootDir);

        go.GetComponent<Projectile>().SetProjectile(player.transform, weapon.damage, type, -transform.forward, ((ItemRanged)weapon).projectileSpeed);
    }

    public void ShootWeaponAt(Vector3 position)
    {
        Vector3 direction = position - transform.position;

        GameObject go = Instantiate(projectile, transform.position, Quaternion.identity);

        go.GetComponent<Projectile>().SetProjectile(character.transform, weapon.damage, type, direction, ((ItemRanged)weapon).projectileSpeed);
    }



    public void SetWeaponDamage(float multiplier)
    {
        damage = weapon.damage*multiplier;
    }

    public void SetDamageBySkill(int skill)
    {
        damage = weapon.damage * AttributesCalculator.CalculateDamageMultiplier(skill);
    }

    public Skill GetWeaponSkill()
    {
        return weapon.skill;
    }

    public void GetBlocked()
    {
        character.GetBlocked();
        isAttacking = false;
    }

    public void StartAttack() 
    { 
        isAttacking = true; 
        hasHit = false; 
        
        if(weapon is ItemRanged && character == null)
        {
            lineRenderer.enabled = true;
        }
    }
    public void FinishAttack() { isAttacking = false;  }

    public void DrawProjectileTrajectory()
    {
        float speed = ((ItemRanged)weapon).projectileSpeed;

        Vector3 velocity = transform.forward * speed * Time.fixedDeltaTime * -1f;
        Vector3 oldPos = transform.position;
        Vector3 newPos;

        Vector3 grav = Physics.gravity * Time.fixedDeltaTime * .1f;


        for (int i = 0; i < lineSegments; i++)
        {
            newPos = oldPos + velocity;
            velocity += grav;


            Debug.DrawRay(oldPos, velocity, Color.red, Time.fixedDeltaTime);
            lineRenderer.SetPosition(i, newPos);

            oldPos = newPos;
        }
    }
}
