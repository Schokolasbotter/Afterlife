using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textDisplay : MonoBehaviour
{
    public interactable interactable;
    public GameObject interactablePanel;
    public Text title;
    public Text description;
    public Text power;
    public Text note;
    public bool enablesGliding = false;
    public bool enablesLaunch = false;
    public bool enablesDash = false;

    public void openInteractableDisplay()
    {
        title.text = interactable.title;
        description.text = interactable.description;
        power.text = interactable.power;
        note.text = interactable.note;

        interactablePanel.SetActive(true);
    }
    
    public void closeInteractableDisplay()
    {
        interactablePanel.SetActive(false);
    }

    public bool getStatus()
    {
        if (interactablePanel.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

