using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class totemScript : MonoBehaviour
{
    public bool isOn = false;
    public GameObject[] lights;
    void Start()
    {
        foreach(GameObject light in lights){
            light.SetActive(false);
        }
    }

    public void switchOn()
    {
        if (!isOn)
        {
            isOn = true;
            FindObjectOfType<AudioManagerScript>().Play("TotemSound");
            foreach(GameObject light in lights){
                light.SetActive(true);
            }
        }
    }

    public bool getStatus()
    {
        return isOn;
    }
}

