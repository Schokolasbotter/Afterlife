using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLookingScript : MonoBehaviour
{
    //Components
    private Transform playerBody;
    private PlayerInputActions playerInputActions;

    //Variables
    public float mouseSensitivity = 100f;
    float xRotation = 0f;

    void Awake()
    {
        //Manage Components
        playerBody = GetComponent<Transform>().parent;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.lookAround.performed += LookAround_performed;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide Cursor during Gameplay
    }

    private void LookAround_performed(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>(); //Read Value from InputSystem       

        if(playerBody != null)
        {
            //Camera Movement
            //X Axis
            playerBody.Rotate(Vector3.up * mouseInput.x * mouseSensitivity * Time.deltaTime); // Rotate Whole Player Object
            // Y Axis
            xRotation -= (mouseInput.y * mouseSensitivity * Time.deltaTime); //calculate the ammount of rotation on the X Axis ( Up and down)
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to give upper and lower boundaries        
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate Camera by that amount
        }
    }

    public void disableMouse()
    {
        playerInputActions.Player.Disable();
    }

    public void setSensibility(float newSensibility)
    {
        mouseSensitivity = newSensibility;
    }
}
