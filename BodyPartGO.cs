using Assets.Scripts.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartGO : Health, IInteractive
{

    [SerializeField] BaseCharacter character;
    [SerializeField] Health health;
    [SerializeField] BodyPart part;
    public float dmgMult = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInParent<BaseCharacter>();
        health = character.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Damage(float damage)
    {
        Debug.Log("Hit "+PartToString(part)+" for "+damage.ToString()+" dmg");
        health.Damage(damage*dmgMult);
    }

    public override void Damage(DamageData damage)
    {
        Debug.Log("Hit " + PartToString(part) + " for " + damage.amount.ToString() + " dmg");
        //health.Damage(damage.amount*dmgMult);
        health.Damage(damage);
    }

    public void GetAttacked(Transform source)
    {
        character.GetAttacked(source);
    }

    public void Interact()
    {
        character.Interact();
    }

    public string GetInteractText()
    {
        return character.GetInteractText();
    }

    string PartToString(BodyPart bp)
    {
        switch (bp)
        {
            case BodyPart.Leg:
                return "Leg";
            case BodyPart.Chest:
                return "Chest";
            case BodyPart.Arm:
                return "Arm";
            case BodyPart.Head:
                return "Head";
        }
        return "Na";
    }
}

public enum BodyPart
{
    Leg,
    Chest,
    Arm,
    Head
}
