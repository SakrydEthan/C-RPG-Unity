using Assets.Scripts.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShield : Health
{
    PlayerAttributesController player;

    public override void Damage(DamageData damage)
    {
        //base.Damage(damage);
        //drain players stamina
    }
}
