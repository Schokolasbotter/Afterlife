using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ButtonScript : MonoBehaviour
{
    //Variables
    public bool switchPressed = false;

    //On Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // If player touches button
        {
            if (!switchPressed)
            {
                FindObjectOfType<AudioManagerScript>().Play("ButtonSound");
                switchPressed = true; //Set button to pressed
            }        
            
        }
    }
}
