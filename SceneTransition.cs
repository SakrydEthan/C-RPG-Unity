using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour, IInteractive
{

    public string targetScene;
    public int entryPoint;


    public string GetInteractText()
    {
        return "Enter " + targetScene;
    }

    public void Interact()
    {
        LevelController.SetEntryPoint(entryPoint);
        LevelController.LoadLevelByName(targetScene);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
