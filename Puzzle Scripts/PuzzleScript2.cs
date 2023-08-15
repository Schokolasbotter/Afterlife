using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScript2 : MonoBehaviour
{
    public GameObject blueDoor;
    private GameObject blueSwitch1;
    private GameObject blueSwitch2;
    private GameObject blueSwitch3;
    private GameObject blueButton1;
    private GameObject blueButton2;
    private GameObject blueButton3;
    private bool blueDoorOpen = false;


    [SerializeField]
    private bool blueSwitch1Pressed = false;
    [SerializeField]
    private bool blueSwitch2Pressed = false;
    [SerializeField]
    private bool blueSwitch3Pressed = false;

    private float yPos;
    private float blueButton1_yPos;
    private float blueButton2_yPos;
    private float blueButton3_yPos;

    // Start is called before the first frame update
    void Start()
    {
        //Get Objects
        blueSwitch1 = GameObject.Find("BlueSwitch1");
        blueSwitch2 = GameObject.Find("BlueSwitch2");
        blueSwitch3 = GameObject.Find("BlueSwitch3");
        //Get the button as a child of the switch
        blueButton1 = blueSwitch1.transform.GetChild(1).gameObject; 
        blueButton2 = blueSwitch2.transform.GetChild(1).gameObject; 
        blueButton3 = blueSwitch3.transform.GetChild(1).gameObject; 
        //Get the default positions
        yPos = blueDoor.transform.position.y;
        blueButton1_yPos = blueButton1.transform.position.y;
        blueButton2_yPos = blueButton2.transform.position.y;
        blueButton3_yPos = blueButton3.transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        //Get status from buttons
        blueSwitch1Pressed = blueSwitch1.GetComponent<ButtonScript>().switchPressed;
        blueSwitch2Pressed = blueSwitch2.GetComponent<ButtonScript>().switchPressed;
        blueSwitch3Pressed = blueSwitch3.GetComponent<ButtonScript>().switchPressed;
        //Animate the buttons
        if(blueSwitch1Pressed && blueButton1.transform.position.y > blueButton1_yPos - 30f)
        {
            blueButton1.transform.position = blueButton1.transform.position + new Vector3(0f, -0.2f, 0f);
        }
        if(blueSwitch2Pressed && blueButton2.transform.position.y > blueButton2_yPos - 30f)
        {
            blueButton2.transform.position = blueButton2.transform.position + new Vector3(0f, -0.2f, 0f);
        }
        if(blueSwitch3Pressed && blueButton3.transform.position.y > blueButton3_yPos - 30f)
        {
            blueButton3.transform.position = blueButton3.transform.position + new Vector3(0f, -0.2f, 0f);
        }
        //If all three switches are activated
        if(blueSwitch1Pressed && blueSwitch2Pressed && blueSwitch3Pressed)
        {
            //Play puzzle completion sound when the puzzle is completed
            if(!blueDoorOpen)
            {
                FindObjectOfType<AudioManagerScript>().Play("DoorSound");
                blueDoorOpen = true;
            }
            //Open door
            if (blueDoor.transform.position.y < yPos + 30f )
            {
                blueDoor.transform.position = blueDoor.transform.position + new Vector3(0f, 0.3f, 0f);
            }
        }
    }
}
