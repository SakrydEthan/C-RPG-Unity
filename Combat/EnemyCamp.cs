using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamp : MonoBehaviour
{
    
    public List<Enemy> enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AlertCamp()
    {

    }

    public void AlertCamp(Collider collider)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].AttackTarget(collider.transform);
        }
    }


    public void AlertCamp(Transform transform)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].AttackTarget(transform);
        }
    }
}
