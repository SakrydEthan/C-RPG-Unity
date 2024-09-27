using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{
    [SerializeField] GameObject Player;

    // Start is called before the first frame update
    void Awake()
    {
        if(SaveController.instance != null)
        {
            gameObject.tag = "Untagged";
            Player.tag = "Untagged";
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
