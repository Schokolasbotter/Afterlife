using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject checkpoint;

    public Vector3 checkpointPosition;

    private void OnTriggerEnter(Collider other)
    {
        //Set checkpoint position when player reaches it
        if (other.tag == "Player")
        {    
            checkpointPosition = checkpoint.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
        }
    }
}
