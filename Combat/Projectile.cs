using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask layers;
    [SerializeField] float damage;
    [SerializeField] DamageType type;
    [SerializeField] float speed;
    [SerializeField] Transform source;
    DamageData dd;
    Vector3 velocity;
    Vector3 oldPos;
    Vector3 newPos;

    Vector3 grav;

    bool hasHit = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 60f);
        //SetProjectile(5f, DamageType.Pierce, transform.rotation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasHit == true) return;
        newPos = oldPos + velocity;
        velocity += grav;


        Debug.DrawRay(transform.position, velocity, Color.red, 12f);

        float dist = Vector3.Distance(newPos, oldPos);
        RaycastHit hit;

        if (Physics.Raycast(oldPos, transform.forward, out hit, dist, layers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 12f);
            Debug.Log("Hit: "+hit.transform.name);
            if(hit.collider.GetComponent<Health>())
            {
                hit.collider.GetComponent<Health>().Damage(dd);
                if(hit.collider.GetComponent<Health>() is BodyPartGO)
                {
                    hit.collider.GetComponent<BodyPartGO>().GetAttacked(source);
                }
            }
            if(hit.collider.GetComponent<BaseCharacter>())
            {
                hit.collider.GetComponent<BaseCharacter>().GetAttacked(source);
            }

            HitTarget(hit.transform, hit.point);
            hasHit = true;
            //Destroy(gameObject);
        }

        transform.LookAt(newPos);
        transform.position = newPos;
        oldPos = newPos;
    }

    public void HitTarget(Transform hit, Vector3 pos)
    {
        velocity = Vector3.zero;
        newPos = pos;
        transform.position = pos;
        grav = Vector3.zero;
        transform.parent = hit;
    }


    public void SetProjectile(Transform source, float damage, DamageType type)
    {
        dd = new DamageData(source, damage, type, damage);

        this.source = source;
        oldPos = transform.position;
        velocity = transform.forward * speed * Time.fixedDeltaTime;
        grav = Physics.gravity * Time.fixedDeltaTime * .1f;
    }


    public void SetProjectile(Transform source, float damage, DamageType type, Quaternion direction)
    {
        dd = new DamageData(source, damage, type, damage);

        transform.rotation = direction;

        this.source = source;
        oldPos = transform.position;
        velocity = transform.forward * speed * Time.fixedDeltaTime;
        grav = Physics.gravity * Time.fixedDeltaTime * .1f;
    }



    public void SetProjectile(Transform _source, float _damage, DamageType _type, Vector3 direction, float _speed)
    {
        dd = new DamageData(_source, _damage, _type, _damage);

        transform.forward = direction.normalized;

        this.source = _source;
        oldPos = transform.position;
        velocity = transform.forward * _speed * Time.fixedDeltaTime;
        grav = Physics.gravity * Time.fixedDeltaTime * .1f;
    }

}
