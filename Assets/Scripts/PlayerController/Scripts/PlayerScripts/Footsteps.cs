using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Footsteps : MonoBehaviour
{
    
    public PlayerController controller;                 //set this in editor!!!
    public GameObject [] footsteps = new GameObject[4]; //set this in editor!!!
    public float hitDistance = 10;
    public GameObject theFootstep;                      //set this in editor!!!

    public float footTimer;

    public float sneakInterval = 1.0f;
    public float sneakSpeed = 2.0f;

    public float walkInterval = 0.5f;
    public float walkSpeed = 6.0f;

    public float runInterval = 0.25f;
    public float runSpeed = 10.0f;

    private float volume = 1.0f;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //only do this if moving
        if (controller.isMovingLateral)
        {


            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            //layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, layerMask))
            {
                if (hit.distance < hitDistance)                {
                    
                    if (hit.transform.tag == "grass")          //grass
                    {
                        ChangeSound(0);
                    }
                    else if (hit.transform.tag == "dirt")    //dirt 
                    {
                        ChangeSound(1);
                    }
                    else if (hit.transform.tag == "rock")    //rock
                    {
                        ChangeSound(2);
                    }
                    else if (hit.transform.tag == "water")   //water
                    {
                        ChangeSound(3);
                    }

                    Debug.Log(hit.transform.tag);

                    
                 
                }

            }
            else
            {
                ChangeSound(-1);
            }

        }
        else
        {
            ChangeSound(-1);

        }
        
        


    }
    void ChangeSound(int index)
    {
        //neg one means no sound
        if(index == -1 )
        {
            theFootstep.SetActive(false); //set curent to off            
            footTimer = Time.time;        //reset timer    
            return;                       //exit
        }

        //if the current is not the new sound, or it is not playing
        if (theFootstep != footsteps[index] || theFootstep.activeInHierarchy == false)
        {
            //turn off the old sound if it is on
            theFootstep.SetActive(false);
            //set foot timer
            footTimer = Time.time;
        }

        //turn on this sound
        footsteps[index].SetActive(true);
        //set current sound to this sound
        theFootstep = footsteps[index];

        //work on this, find a better way to reference
        theFootstep.GetComponent<StudioEventEmitter>().EventInstance.setVolume(volume);


        //now check move speed for on/off toggle (I could also ask the animator what is playing)
        if (controller.currentSpeed < sneakSpeed)
        {
            //sneak time
            if(Time.time - footTimer > sneakInterval )
            {
                ChangeSound(-1);  //turn off the sound, it will play next frame
                volume = 0.25f;
            }
        }
        else if (controller.currentSpeed < walkSpeed)
        {
            //walk time
            if (Time.time - footTimer > walkInterval)
            {
                ChangeSound(-1);
                volume = 1.0f;
            }
        }
        else if (controller.currentSpeed < runSpeed)
        {
            //run time
            if (Time.time - footTimer > runInterval)
            {
                ChangeSound(-1);
                volume = 1.0f;
            }
        }

    }
    IEnumerator Countdown(float time)
    {
        Debug.Log("countdown");        
        yield return new WaitForSeconds(time);
       
    }
}
