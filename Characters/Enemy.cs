using Assets.Scripts.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseCharacter
{
    public EnemyCamp camp;
    public Vector3 startPos = Vector3.zero;

    Vector3 attackPos = Vector3.zero;
    Vector3 oldPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        StartBehavior();
    }

    public override void StartBehavior()
    {
        //Debug.Log("enemy start behavior");
        base.StartBehavior();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        attributes = GetComponent<AttributesController>();
        health = GetComponent<HealthCharacter>();
        camp = GetComponentInParent<EnemyCamp>();
        if(camp != null) camp.enemies.Add(this);


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //if (player != null) target = player.transform;

        startPos = transform.position;
        Load();


        SetPreset();

        //gameObject.SetActive(false);
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
                if (oldPos != Vector3.zero)
                {
                    float dis = Vector3.Distance(oldPos, transform.position);
                }
                oldPos = transform.position;
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
    }

    private void LateUpdate()
    {
        if (isAttacking) transform.position = attackPos;
    }

    public void WakeEnemy(Collider collider)
    {
        if (camp != null) camp.AlertCamp(collider);

        isAwake = true;
        target = collider.transform;
    }

    public void WakeEnemy(Transform transform)
    {
        if (camp != null) camp.AlertCamp(transform);

        isAwake = true;
        target = transform;
    }

    public void AttackTarget(Transform _target)
    {
        target = _target;
        isAwake = true;
    }

    public override void AttackStart()
    {
        base.AttackStart();
        attackPos = transform.position;
    }

    public void SetMoving()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isIdle", false);
        animator.SetBool("isMoving", true);
    }

    public void SetAttacking()
    {
        animator.SetBool("isAttacking", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isMoving", false);
    }

    public void SetIdle()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isIdle", true);
        animator.SetBool("isMoving", false);
    }

    public void ReturnToPos()
    {
        agent.SetDestination(startPos);
    }


    public Vector3 GetLookAt(Vector3 pos)
    {
        Vector3 lookpos = new Vector3(pos.x, transform.position.y, pos.z);
        return lookpos;
    }

    /// <summary>
    /// Set an enemy to active when near player
    /// </summary>
    public void ActivateEnemy()
    {

    }
}
