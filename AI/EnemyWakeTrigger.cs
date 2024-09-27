using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWakeTrigger : MonoBehaviour
{
    public Enemy numberOne;


    // Start is called before the first frame update
    void Start()
    {
        numberOne = GetComponentInParent<Enemy>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            numberOne.WakeEnemy(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            numberOne.isAwake = false;
            numberOne.ReturnToPos();
            //numberOne.target = other.transform;
        }
    }
}
