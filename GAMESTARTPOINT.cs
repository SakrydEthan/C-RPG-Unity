using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAMESTARTPOINT : MonoBehaviour
{
    public static GAMESTARTPOINT instance;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SaveController.instance == null)
        {
            //PlayerInstanceController.instance.SetPlayerPositionLoad(transform.position, transform.rotation.eulerAngles);
            //Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerInstanceController.instance != null)
        {
            if(!SaveController.CheckForSave()) PlayerInstanceController.instance.SetPlayerPositionLoad(transform.position, transform.rotation.eulerAngles);
            Destroy(gameObject);
        }   
    }
}
