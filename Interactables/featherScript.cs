using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class featherScript : MonoBehaviour
{
    public Vector3 rotation;
    public float rotationSpeed;
    public int energyAmount = 40;
    void Update()
    {
        transform.Rotate(rotation * rotationSpeed * Time.deltaTime);
    }
    public int restoreEnergy()
    {
        FindObjectOfType<AudioManagerScript>().Play("FeatherSound");
        return energyAmount;  
    }
}
