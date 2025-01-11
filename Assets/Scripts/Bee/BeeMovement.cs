/*
 * (c) copyright 2023 John Klima
 * Free for non-commercial use
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour {

    public float torqueScale = 100.0f;  //gameplay balance
    public float thrustScale = 100.0f;
    public float jumpScale = 100.0f;
    public float impulseScale = 100.0f;
    public float bounceScale = 100.0f;


    private float scaleSpeed = 100.0f;


    //gravity in meters per second per second
    public float GRAVITY_CONSTANT = -9.8f;                      // -- for earth,  -1.6 for moon 
    public float FORCE_CONSTANT = 100.0f;
    public float TORQUE_CONSTANT = 10.0f;

    //reduce velocity over time (friction???)
    public float decayXZ = 0.5f;
    public float decayY = 0.99f;     //bigger number is LESS decay, 1.0 = none

    public Vector3 velocity = new Vector3(0, 0, 0);                       //current direction and speed of movement
    [SerializeField] Vector3 acceleration = new Vector3(0, 0, 0);         //movement controlled by player movement force and gravity
    [SerializeField] Vector3 finalForce = new Vector3(0, 0, 0);           //final force to be applied this frame

    [SerializeField] Vector3 jump = new Vector3(0, 0, 0);

    [SerializeField] Vector3 bounce = new Vector3(0, 0, 0);
    [SerializeField] Vector3 thrust = new Vector3(0, 0, 0);               //player applied thrust vector
    [SerializeField] Vector3 impulse = new Vector3(0, 0, 0);
    [SerializeField] Vector3 torque = new Vector3(0, 0, 0);

    //public bool friction = false;

    public float mass = 1.0f;
    public float maxVelocity = 30.0f;
    public float energy = 10000.0f;
    
    Vector3 prevPosition;       //last known good position
    Quaternion prevRotation;    //last known good rotation

    bool lockMove = false;      //lock the camera (usually for collision)

    // Use this for initialization
    void Start() 
    {
        //might as well update these so they are non-zero to start 
        prevPosition = transform.position;
        prevRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if( lockMove == false)
            handleMovement();

        prevPosition = transform.position; //buffer last position for collisions (separate class perhaps?)


    }
    
    public void lockMovement(bool lockmove)
    {
        lockMove = lockmove;    
    }
    public bool lockMovement()
    {
        return lockMove;
    }


    public void applyJump(float force)
    {

        //one shot jump as impulse
        jump.y += force * jumpScale * FORCE_CONSTANT;   
         
    }

    public void applyThrust(Vector3 force)
    {

        //one frame thrust, continuous if key is held 
        thrust += force * thrustScale * FORCE_CONSTANT;

        //maybe antigrav here? otherwise jetforce must be > GRAVITY_CONSTANT on the Y axis
    }

    public void applyImpulse(Vector3 force)
    {
        impulse += force * impulseScale * FORCE_CONSTANT;
    }

    public void applyBouce(Vector3 force)
    {
        bounce += force * bounceScale * FORCE_CONSTANT;
    }

    public void applyTorque(Vector3 force)
    {
        torque += force * torqueScale * TORQUE_CONSTANT;
    }
    void handleMovement()
    {
        float dt = Time.deltaTime;

        //cheating physics on torque for now
        //transform.Rotate(torque * dt);

        //reset final force to the initial force of gravity
        finalForce.Set(0, GRAVITY_CONSTANT * mass, 0);
        finalForce += thrust;
        
        acceleration = finalForce / mass;        

        velocity += acceleration * dt;
        
        //jump is a oneshot impulse on Y axis
        velocity += jump;

        //as is impulse (explosion)
        velocity += impulse;

        //add bounce also an impulse really
        velocity += bounce;

        //resets
        thrust = Vector3.zero;
        jump = Vector3.zero;
        bounce = Vector3.zero;
        impulse = Vector3.zero;
        torque = Vector3.zero;

        //clamp velocity (terminal velocity)
        smoothClampVelocity(maxVelocity); //meters per second max

        //move the object
        transform.position += velocity * dt;

    }
    private void FixedUpdate()
    {
        //On my machine constant 0.02
        //On yours?

        //decayXZ force on X,Z axis
        Vector3 velo = velocity;
        velo.x *= decayXZ;
        velo.z *= decayXZ;
        velo.y *= decayY;

        velocity = velo;
    }
    private void smoothClampVelocity(float max)
    {
        //GENERAL RULE OF VELOCITY : don't let them go too fast!!!        
        float maxSpeedSquared = max * max;
        float velMagSquared = velocity.magnitude * velocity.magnitude;
        if (velMagSquared > maxSpeedSquared)
        {
            velocity *= (max / velocity.magnitude);
        }

    }

}
