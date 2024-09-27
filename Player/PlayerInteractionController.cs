using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI;
using CMF;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{

    public Transform cam;
    public Transform camcon;
    public float interactDist = 6f;
    public bool isFrozen = false;

    PlayerUIController uiCon;

    float camSpeed;
    float moveSpeed;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        camSpeed = camcon.GetComponent<CameraController>().cameraSpeed;
        moveSpeed = GetComponent<AdvancedWalkerController>().movementSpeed;
        cam = Camera.main.transform;
        uiCon = GetComponent<PlayerUIController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (UIController.ToggleInventory()) FreezePlayer();
            else UnfreezePlayer();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            SaveController.instance.SaveGame();
        }

        if(Input.GetButtonDown("Pause"))
        {
            GameStateController.TogglePause();
        }

        if(Input.GetButtonDown("ToggleStats"))
        {
            if(UIController.ToggleSkills()) FreezePlayer();
            else UnfreezePlayer();
        }

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        uiCon.ClearInteractText();

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, interactDist, layerMask))
        {
            if (hit.collider.GetComponent<IInteractive>() != null)
            {
                uiCon.UpdateInteractText(hit.collider.GetComponent<IInteractive>().GetInteractText());
                if (Input.GetButtonDown("Interact"))
                {
                    hit.collider.GetComponent<IInteractive>().Interact();
                }
            }
        }
    }

    public void TogglePlayerFreeze()
    {
        if (isFrozen) UnfreezePlayer();
        else FreezePlayer();
    }

    public void FreezePlayer()
    {
        Debug.Log("freeze player was called");
        isFrozen = true;
        CameraController cc = camcon.GetComponent<CameraController>();
        //camSpeed = cc.cameraSpeed;
        cc.cameraSpeed = 0;
        if (GetComponent<SimpleWalkerController>() != null)
        {
            SimpleWalkerController sw = GetComponent<SimpleWalkerController>();
            //moveSpeed = sw.movementSpeed;
            sw.movementSpeed = 0;
        }
        if(GetComponent<AdvancedWalkerController>() != null)
        {
            AdvancedWalkerController aw = GetComponent<AdvancedWalkerController>();
            //moveSpeed = aw.movementSpeed;
            aw.movementSpeed = 0;
        }
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnfreezePlayer()
    {
        Debug.Log("unfreeze player was called");
        isFrozen = false;
        CameraController cc = camcon.GetComponent<CameraController>();
        cc.cameraSpeed = camSpeed;
        if (GetComponent<SimpleWalkerController>() != null)
        {
            SimpleWalkerController sw = GetComponent<SimpleWalkerController>();
            sw.movementSpeed = moveSpeed;
        }
        if (GetComponent<AdvancedWalkerController>() != null)
        {
            AdvancedWalkerController aw = GetComponent<AdvancedWalkerController>();
            aw.movementSpeed = moveSpeed;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }
}
