using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{    
    public Transform player;

    // Update is called once per frame
    void LateUpdate()
    {
        //Making the minimap follow the player
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        //Making the minimap rotate with the player
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}