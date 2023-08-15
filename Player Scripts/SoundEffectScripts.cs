using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectScripts : MonoBehaviour
{
    public AudioSource audioSource;
    private Transform audioPosition;
    private Animator anim; //aminate
    [HideInInspector]
    public Vector3 moveY = Vector3.zero; //the speed of moving

    private Transform groundCheckPos;
    private bool isGrounded = true;//check landing
    private bool isOnJumping = false; //in jumping(not landed)

    private float groundDistance = 0.4f;

    [SerializeField]
    private LayerMask groundMask;

    public AudioClip[] stepClips = new AudioClip[3]; 

    [SerializeField]
    private AudioSource stepSource; 

    enum AudioType  
    {
        Step = 0,
        Jump = 1,
        Land = 2
        
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        //groundCheckPos = transform.Find("Root_LocPos"); //groundCheckPos is a detection for char landed
        
    }

    void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundCheckPos.position,
            groundDistance, groundMask);

        if (isGrounded && isOnJumping) 
        {
            stepSource.PlayOneShot(stepClips[(int)AudioType.Land]);
            Debug.Log("Landed");
        }

        if (isGrounded && moveY.y < 0) 
        {
            //Debug.Log("isGrounded");
            moveY.y = -2f;
            isOnJumping = false;
        }
        else if (!isGrounded)
        {
            if (!anim.IsInTransition(0))
            {
                anim.SetBool("isJumping", false);
            }
        }

       
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isOnJumping = true;
            
            if (!anim.IsInTransition(0))
            {
                anim.SetBool("isJumping", true);
                stepSource.PlayOneShot(stepClips[(int)AudioType.Jump]);
            }

        }
        
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        if (isMoving(_xMov, _zMov)) 
        {
            if (!stepSource.isPlaying && !isOnJumping)
            {
                stepSource.PlayOneShot(stepClips[(int)AudioType.Step]);
            }

        }
        
    }
    
    bool isMoving(float horz, float vert)
    {
        return horz != 0 || vert != 0;
    }
}



//
//                       _oo0oo_
//                      o8888888o
//                      88" . "88
//                      (| -_- |)
//                      0\  =  /0
//                    ___/`---'\___
//                  .' \\|     |// '.
//                 / \\|||  :  |||// \
//                / _||||| -:- |||||- \
//               |   | \\\  -  /// |   |
//               | \_|  ''\---/''  |_/ |
//               \  .-\__  '-'  ___/-. /
//             ___'. .'  /--.--\  `. .'___
//          ."" '<  `.___\_<|>_/___.' >' "".
//         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//         \  \ `_.   \_ __\ /__ _/   .-` /  /
//     =====`-.____`.___ \_____/___.-`___.-'=====
//                       `=---='
//                please no bug there
