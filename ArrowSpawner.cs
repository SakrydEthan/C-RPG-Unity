using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{

    public GameObject projectile;
    public float shootTimer = 5f;
    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ShootArrow();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(timer > shootTimer)
        {
            ShootArrow();
            timer = 0f;
        }
    }

    public void ShootArrow()
    {
        GameObject go = Instantiate(projectile, transform);
        //go.GetComponent<Projectile>().SetProjectile(1f, DamageType.Slash, transform.rotation);
    }
}
