using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootPositioner : MonoBehaviour
{

    public Vector3 position;
    public Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localPosition = position;
        transform.localRotation.SetEulerAngles(Mathf.Deg2Rad*rotation);
    }
}
