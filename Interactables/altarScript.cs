using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altarScript : MonoBehaviour
{
    public GameObject totem1;
    public GameObject totem2;
    public GameObject totem3;

    private bool totem1Pressed = false;
    private bool totem2Pressed = false;
    private bool totem3Pressed = false;

    private bool totemEnterable = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get Status of Totems
        totem1Pressed = totem1.GetComponent<totemScript>().isOn;
        totem2Pressed = totem2.GetComponent<totemScript>().isOn;
        totem3Pressed = totem3.GetComponent<totemScript>().isOn;

        if (totem1Pressed && totem2Pressed && totem3Pressed)
        {
            //Activate Totem Enterable
            totemEnterable = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (totemEnterable)
        {
            //Trigger Endgame on Player Script
            other.GetComponent<PlayerMovementScript>().triggerEndgame();
        }
    }
}
