using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntryPoint : MonoBehaviour
{

    public int id = 0;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerInstanceController.instance.SetPlayerPositionLoad(transform.position, Quaternion.identity.eulerAngles);
        LevelController.CheckEntryPoint(id, transform);
    }
}
