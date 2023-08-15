using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableScript : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (playerInputActions.Player.interact.triggered)
        {
            //Activate Script
            print(other);
            //other.GetComponent<textDisplay>().openInteractableDispay();
        }
    }
}
