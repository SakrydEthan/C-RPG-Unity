using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyArcher : Enemy
{

    public float projSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartBehavior();
    }

    public override void StartBehavior()
    {
        //Debug.Log("enemy archer start behavior");
        base.StartBehavior();
        //target = PlayerInstanceController.instance.transform;
        if(itemWeapon is ItemRanged)
        {
            projSpeed = ((ItemRanged)itemWeapon).projectileSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();
    }

    public override void UpdateBehavior()
    {
        if (!isAwake || !isAlive) return;
        if (target == null) return;

        float dist = Vector3.Distance(target.position, transform.position);
        if (dist > attackRange)
        {
            //SetMoving();
            if (isAttacking)
            {
                agent.velocity = Vector3.zero;
                Vector3 lookAt = LeadTargetPos(target);
                Vector3 transformLookAt = new Vector3(lookAt.x, transform.position.y, lookAt.z);
                transform.LookAt(transformLookAt);
                if(weapon != null) weapon.transform.LookAt(-lookAt);
            }
            if (!isAttacking)
            {
                //agent.enabled = true;
                agent.isStopped = false;
                agent.SetDestination(target.position);
                SetMoving();
            }
        }
        else
        {
            SetAttacking();
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.SetDestination(transform.position);
            transform.LookAt(GetLookAt(target.position));
            //agent.enabled = false;
        }

        //Vector3 lead = LeadTargetPos(target);
        //Debug.DrawLine(weapon.transform.position, lead, Color.red, 1f);
    }

    public override void ShootProjectile()
    {
        weapon.ShootWeaponAt(LeadTargetPos(target));
    }

    public Vector3 LeadTargetPos(Transform _transform)
    {
        Vector3 targetVel = _transform.GetComponent<Rigidbody>().velocity;
        float dist = Vector3.Distance(_transform.position, transform.position);
        float travTime = dist / projSpeed;
        Vector3 height = (Physics.gravity * travTime * travTime * -1f) + (2 * Vector3.up);
        //Debug.Log(height.y);
        return _transform.position + height + (targetVel*travTime);
    }
}
