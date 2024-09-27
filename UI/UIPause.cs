using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : UISection
{
    public static UIPause instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void OpenPauseMenu()
    {
        instance.panel.SetActive(true);
    }

    public static void ClosePauseMenu()
    {
        instance.panel.SetActive(false);
    }

}
