using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScript1 : MonoBehaviour
{
    public GameObject door;
    private GameObject switch1;
    private GameObject switch2;
    private GameObject switch3;
    private GameObject button1;
    private GameObject button2;
    private GameObject button3;
    private bool doorOpen = false;


    [SerializeField]
    private bool switch1Pressed = false;
    [SerializeField]
    private bool switch2Pressed = false;
    [SerializeField]
    private bool switch3Pressed = false;

    private float yPos;
    private float button1_yPos;
    private float button2_yPos;
    private float button3_yPos;

    // Start is called before the first frame update
    void Start()
    {
        //Get Objects
        switch1 = GameObject.Find("Switch 1");
        switch2 = GameObject.Find("Switch 2");
        switch3 = GameObject.Find("Switch 3");
        //Get the button as a child of the switch
        button1 = switch1.transform.GetChild(1).gameObject; 
        button2 = switch2.transform.GetChild(1).gameObject; 
        button3 = switch3.transform.GetChild(1).gameObject; 
        //Get the default positions
        yPos = door.transform.position.y;
        button1_yPos = button1.transform.position.y;
        button2_yPos = button2.transform.position.y;
        button3_yPos = button3.transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        //Get status from buttons
        switch1Pressed = switch1.GetComponent<ButtonScript>().switchPressed;
        switch2Pressed = switch2.GetComponent<ButtonScript>().switchPressed;
        switch3Pressed = switch3.GetComponent<ButtonScript>().switchPressed;
        //Animate the buttons
        if(switch1Pressed && button1.transform.position.y > button1_yPos - 30f)
        {
            button1.transform.position = button1.transform.position + new Vector3(0f, -0.2f, 0f);
        }
        if(switch2Pressed && button2.transform.position.y > button2_yPos - 30f)
        {
            button2.transform.position = button2.transform.position + new Vector3(0f, -0.2f, 0f);
        }
        if(switch3Pressed && button3.transform.position.y > button3_yPos - 30f)
        {
            button3.transform.position = button3.transform.position + new Vector3(0f, -0.2f, 0f);
        }
        //If all three switches are activated
        if(switch1Pressed && switch2Pressed && switch3Pressed)
        {
            //Play puzzle completion sound when the puzzle is completed
            if(!doorOpen)
            {
                FindObjectOfType<AudioManagerScript>().Play("DoorSound");
                doorOpen = true;
            }
            //Open door
            if (door.transform.position.y < yPos + 30f )
            {
                door.transform.position = door.transform.position + new Vector3(0f, 0.3f, 0f);
            }
        }
    }
}
