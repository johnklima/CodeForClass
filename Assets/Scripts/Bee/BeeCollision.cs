/*
 * (c) copyright 2023 John Klima
 * Free for non-commercial use
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeCollision : MonoBehaviour
{
    [Header("Bee Movement")]
    //could be acquired through the hierarchy, but that might change
    public BeeMovement movement;
    public BeeCamera camera;

    [Header("Bee Camera Independence")]
    //these are critical for how much freedom the bee has from the camera    
    public float physicsDecay = 0.95f;
    public float restRate = 1.0f;

    //regular physX object
    Rigidbody body;

    //am I???
    bool inCollision = false;

    //what is neutral, from design time
    [SerializeField] Quaternion initialRotation;
    [SerializeField] Vector3 initialPosition;


    // Start is called before the first frame update
    void Start()
    {
        //get my body
        body = GetComponent<Rigidbody>();

        //get start transform values
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    

    }

    // Update is called once per frame
    void Update()
    {
        //only thing we need to do here, is slerp back to our original position
        //as child of camera. hmmmmmm.... I hope that works, but I'm always doing this
        //if in collision or not?? Sort of.

        if(inCollision == false)
        {
            float restTime = Time.deltaTime * restRate;

            //slerp back to it's rest state
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, restTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, restTime);
            
        }
        else //continue to inform camera we are colliding
        {
            //reduce mouse sensitivity, but don't kill it
            camera.SetMouseSensitivity(1.0f);
        }

        //decay it's physics velocity in any case? Yes.
        body.velocity *= physicsDecay;

    }

    private void OnCollisionEnter(Collision collision)
    {
        //no point being here if it isn't an Obstacle
        if (collision.collider.tag != "Obstacle")
            return;

        //help me Jesus...
        Debug.Log("Bee collided with " + collision.collider.name);
            
        //when we collide, transfer what is essentially the camera velocity
        //into the bee body, and then let the unity physics engine handle a bounce
        body.velocity = movement.velocity;

        inCollision = true;

        //if I am colliding I will not influence movement
        movement.lockMovement(true);
        //reduce mouse sensitivity, but don't kill it
        camera.SetMouseSensitivity(1.0f);
        camera.DeCouple();
        

    }
    private void OnCollisionStay(Collision collision)
    {
        //frustrate any restitution processes
        inCollision = true;

        //if I am colliding I will not influence movement
        movement.lockMovement(true);
        //reduce mouse sensitivity, but don't kill it
        camera.SetMouseSensitivity(1.0f);
        //camera.DeCouple();

    }
    private void OnCollisionExit(Collision collision)
    {
        //all good, my body is out of trouble
        inCollision = false;
        movement.lockMovement(false);
        camera.Couple();
    }

}
