/*
 * (c) copyright 2023 John Klima
 * Free for non-commercial use
 */

/*
 * the purpose of this script is to move the bee geom around in its local coordinate 
 * system, reacting to collisions, pushing some of that information back into the
 * bee camera, and to create a pleasing, non-reptitious response to camera control.
 * The camera is what moves, the Bee geometry is a child of the camera, and can move
 * in it's own local space, yet it is still a collision detector, so it can bounce around
 * without jarring the camera.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAnimator : MonoBehaviour
{

    public Vector2 pitchMinMax = new Vector2(-10, 10);
    public Vector2 yawMinMax   = new Vector2(-10, 10);
    public Vector2 rollMinMax  = new Vector2(-10, 10);

    public float rotationSmoothTime = .12f;
    
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    [SerializeField] float pitch = 0;
    [SerializeField] float yaw = 0;
    [SerializeField] float roll = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentRotation = transform.localRotation.eulerAngles;   
    }

    // Update is called once per frame
    void Update()
    {
        /* 
         * lets just start by a simple, constricted, pitch yaw roll, based on what the camera
         * is doing. The Bee geom is a child of camera, in this situation, it is the camera
         * that moves, not the Bee. This is to just get the Bee to angle around a little
         * Nothing much happeneing yet, still need to feed a pitch/yaw/roll so everything
         * below is actually zero at the moment. 
         */
        
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        yaw = Mathf.Clamp(yaw, yawMinMax.x, yawMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;


    }
}
