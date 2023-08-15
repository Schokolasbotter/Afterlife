using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour{

    //Components
    private CharacterController controller;
    private PlayerInputActions playerInputActions;
    public Transform groundcheck;

    //Variables
    public float speed = 12f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;
    public Vector2 launch = new Vector2(10f, 10f);
    public Vector2 dash = new Vector2(10f,10f);
    public bool canGlide = false;
    public float gravityDivider = 5f;
    public float energy = 100f;
    public float dashCost = 30f;
    public float launchCost = 40f;
    public float startRegeneration = 3f;
    public float energyRegeneration = 0.001f;
    public float interactableRadius = 5f;
    public bool nearInteractable = false;
    public bool isInteracting = false;

    private Vector3 velocity;
    private bool isGrounded;  
    private GameObject interactableObject;
    public bool hasGliding = false;
    public bool hasLaunch = false;
    public bool hasDash = false;

    private Vector3 resetPlayerPosition;
    private GameObject checkpoint;

    private bool isDead = false;

    public bool endgameStarted = false;
    private float footStepTimer = 0.5f;
    
    private void Awake()
    {
        //Manage Components
        controller = GetComponent<CharacterController>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        groundcheck = transform.Find("GroundCheck");
        checkpoint = GameObject.Find("CheckPoint");
    }    

    private void Update()
    {
        //If endgame started continue to trigger
        if (endgameStarted)
        {
            triggerEndgame();
        }

        //Update checkpoint position
        resetPlayerPosition = checkpoint.GetComponent<CheckPointScript>().checkpointPosition;
        //Check if grounded
        //groundcheck = transform.Find("GroundCheck");
        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);
        
        //Reset Gravity if grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            velocity.x = 0f;
            velocity.z = 0f;
            canGlide = false;
        }

        //Player Movement
        Vector2 movementInput = playerInputActions.Player.move.ReadValue<Vector2>(); //Get Input Values
        Vector3 movementVector = transform.right * movementInput.x + transform.forward * movementInput.y; // Create Movement Vector
        controller.Move(movementVector * speed * Time.deltaTime); // Move Character through Character Controller
        //Footstep Sounds
        if(isGrounded && (movementVector.x!=0f || movementVector.y != 0f))
        {
            footStepTimer -= Time.deltaTime;
            if(footStepTimer <= 0)
            {
                //Play Sound
                footStepTimer = 0.5f;
                FindObjectOfType<AudioManagerScript>().Play("FootStep");
            }
        }

        //Player Jump
        if(playerInputActions.Player.jump.triggered && isGrounded)
        {
            FindObjectOfType<AudioManagerScript>().Play("JumpSound");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        //Player Launch
        if(playerInputActions.Player.launch.triggered && (energy-launchCost) >= 0 && hasLaunch){  //If Spacebar is pressed and you are not gliding and you have enough energy
            canGlide = false;
            Vector3 launchVector = transform.up * launch.y + transform.forward * launch.x; // Create Dash Vector
            velocity = launchVector; // Accelerate in direction of dashVector
            energy -= launchCost;
            FindObjectOfType<AudioManagerScript>().Play("HighJumpSound");
        }

        //Player Dash
        if (playerInputActions.Player.dash.triggered && (energy - dashCost) >= 0 && hasDash)
        {  //If Spacebar is pressed and you are not gliding and you have enough energy
            canGlide = false;
            Vector3 dashVector = transform.up * dash.y + transform.forward * dash.x; // Create Dash Vector
            velocity = dashVector; // Accelerate in direction of dashVector
            energy -= dashCost;
            FindObjectOfType<AudioManagerScript>().Play("DashSound");
        }

        //Activate Gliding
        if (playerInputActions.Player.jump.triggered && !isGrounded && hasGliding)
        {
            //Switch gliding on and off
            if (canGlide)
            {
                canGlide = false;
            }
            else if (!canGlide)
            {
                velocity.y = 0f;
                canGlide = true;
                FindObjectOfType<AudioManagerScript>().Play("StartGlideSound");
                FindObjectOfType<AudioManagerScript>().Play("GlideSound");
            }            
        }
        //Gravity
        if(!canGlide)
        {
            //regular gravity
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            //gliding gravity
            velocity.y += gravity / gravityDivider * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);

        //Interactables
        int interactableCounter = 0;
        //get all colliders overlapping with player in radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactableRadius);
        //Go trough array to check every Collider hit
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.tag == "Interactable") //If it is an interactable
            {
                interactableCounter++; // Count it
                nearInteractable = true; //Activate Button Prompt                
                if (playerInputActions.Player.interact.triggered) //When button pressed call UI
                {
                    
                    if (hitCollider.gameObject.layer != LayerMask.NameToLayer("Totems"))
                    {
                        //Close UI if open
                        if (hitCollider.GetComponent<textDisplay>().getStatus())
                        {
                            isInteracting = false;
                            hitCollider.GetComponent<textDisplay>().closeInteractableDisplay();
                            //Debug.Log(hitCollider.GetComponent<textDisplay>().getStatus());
                        }
                        else
                        {
                            FindObjectOfType<AudioManagerScript>().Play("WorkbenchSound");
                            hitCollider.GetComponent<textDisplay>().openInteractableDisplay();
                            isInteracting = true; //Deactivates Button Prompt
                            interactableObject = hitCollider.gameObject; //Remember object

                            //Enable Powers if true
                            if (hitCollider.GetComponent<textDisplay>().enablesGliding)
                            {
                                hasGliding = true;
                            }
                            if (hitCollider.GetComponent<textDisplay>().enablesLaunch)
                            {
                                hasLaunch = true;
                            }
                            if (hitCollider.GetComponent<textDisplay>().enablesDash)
                            {
                                hasDash = true;
                            }
                        }                        
                    }
                    else
                    {
                        hitCollider.GetComponent<totemScript>().switchOn();
                    }                    
                }
            }           
        } 
        if(interactableCounter == 0) //When no interactable arround 
        {
            if (interactableObject) //if we interacted with an object 
            {
                isInteracting = false;
                interactableObject.GetComponent<textDisplay>().closeInteractableDisplay(); // Disable Interactable UI
            }
            nearInteractable = false; //Disable Button Prompt
        }
        //If the player dies, go back to checkpoint
        if(isDead)
        {
            transform.position = resetPlayerPosition;
            canGlide = false;
            isDead = false;
        }
    }
    //Wait before respawning feathers
    IEnumerator Respawn(Collider collision, int time)
    {
        yield return new WaitForSeconds(time);
        collision.gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        //If the player touches water, go to checkpoint
        if(other.tag == "Water")
        {
            isDead = true;
            FindObjectOfType<AudioManagerScript>().Play("WaterSound");
        }
        if(other.tag == "Feather")
        {
            energy += other.GetComponent<featherScript>().restoreEnergy();
            //Collecting a feather won't give the player more than 100 energy total
            other.gameObject.SetActive(false);
            if(energy > 100f)
            {
                energy = 100f;
            }
            //Wait 10 seconds
            StartCoroutine(Respawn(other, 10));
        }
    }

    //Endgame script
    public void triggerEndgame()
    {
        playerInputActions.Player.Disable();
        FindObjectOfType<AudioManagerScript>().changeVolume("BackgroundMusic", -0.01f);
        endgameStarted = true;
    }

    //Manual Reset from Pause Menu
    public void manualReset()
    {
        isDead = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactableRadius);
    }
}
