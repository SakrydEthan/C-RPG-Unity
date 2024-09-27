using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "ShopInteraction", menuName = "Create New/Dialog/Shop", order = 1)]
public class ShopInteraction : MonoBehaviour
{

    public void Interaction()
    { 
        UIShop.instance.OpenShop();
    }

}
