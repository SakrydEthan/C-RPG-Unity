using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShopButton : MonoBehaviour
{

    public void Interact()
    {
        UIShop.instance.OpenShop();
    }
}
