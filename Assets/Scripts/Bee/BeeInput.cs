/*
 * (c) copyright 2023 John Klima
 * Free for non-commercial use
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeInput : MonoBehaviour
{

    BeeMovement moveBee = null;

    // Start is called before the first frame update
    void Start()
    {
        //give me my movement component
        moveBee = transform.GetComponent<BeeMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (moveBee.lockMovement())  //an old skool way, without param override it is a getter
            return;                  //no point being here if movement is locked (usually collision)


        float inForward = 0;
        float inSide = 0;
        float inUp;

        //mouse first
        inUp = Input.mouseScrollDelta.y;        //Elevation (helicopter)
        if(Input.GetMouseButton(0))
            inForward =  1.0f;

        if (Input.GetMouseButton(1))
            inForward = -1.0f;

        //now check keyboard
        if (inForward == 0)
            inForward = Input.GetAxis("Vertical");  //W S fly forward backward
        if(inSide == 0)
            inSide = Input.GetAxis("Horizontal");   //A D strafe
        
        //mouse controls the forward view in an orbit manner, so WS moves in that direction

        //poke into custom BeeMovement physics
        //(critical to understand, this is on the CAMERA, not the Bee geometry)
        moveBee.applyThrust(transform.right * inSide );
        moveBee.applyThrust(transform.forward * inForward );
        moveBee.applyThrust(transform.up * inUp * 5 ); 
        //a bit more thrust works best with scroll wheel as input

    }
}
