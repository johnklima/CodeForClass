using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	
    [Header("Character Attributes")]
	public float walkSpeed = 2;
	public float runSpeed = 6;
	public float gravity = -12;
	public float jumpHeight = 1;
	[Range(0, 1)]
	public float airControlPercent = 1;

    [Header("Smoothen's The Turn Rotation")]
	[Range(0, 0.2f)]
	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

    [Header("Extra Control (Usually Fine at 0 Though)")]
	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	public float currentSpeed;
	float velocityY;

	Transform cameraT;
	CharacterController controller;

    [Header("Character Movement Check")]
    public bool isMoving;
    public bool isMovingLateral;

    [Header("Camera Setting")]
    public bool bUseCameraControlRotation; // makes it so the rotation of the capsule follows the camera, Turning it off will make it so you can rotate with your camera without your character turning too.

    [Header("PlayerAnimator")]
	public PlayerAnimator playerAnimator;

    void Start()
	{
		cameraT = Camera.main.transform; // Camera initial transform cache
		controller = GetComponent<CharacterController>(); // Fetching the component at Start() to keep the variables private, less room for error.
	}

    void Update()
    {
        // input detection
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);
        bool sneaking = Input.GetKey(KeyCode.LeftControl);
	
        // Movement function using the input detection above.
        Move(inputDir, running, sneaking);

        // Jump handling, got its own funciton as its easier to transition to an animation character this way.
        if (Input.GetKey(KeyCode.Space))
        {
	        Jump();
        }

		Animate(input, running, sneaking);
    }
	void Animate(Vector2 _input, bool _run, bool _sneak)
	{
		if(playerAnimator == null) 
		{ 
			return; 
		}

		if (_input.magnitude > 0 )
		{
			if (_run) 
			{ 
				playerAnimator.Run(); 
			}
			else if (_sneak) 
			{ 
				playerAnimator.Sneak(); 
			}
			else 
			{ 
				playerAnimator.Walk();
			}
            
        }
        else
        {
			playerAnimator.Idle();
        }
    }

	void Move(Vector2 inputDir, bool running, bool sneaking)
	{
		if (inputDir != Vector2.zero && !bUseCameraControlRotation)
		{
			float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}

		if(bUseCameraControlRotation)
		{
			float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}
		
		float targetSpeed =  walkSpeed * inputDir.magnitude;  //default
		
		if (running) 
		{
            targetSpeed = runSpeed * inputDir.magnitude;
        }
		if (sneaking) 
		{
            targetSpeed = 0.25f * walkSpeed * inputDir.magnitude; 
		}
		
		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		if (velocityY > -5) { velocityY += Time.deltaTime * gravity; }
		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
		

        controller.Move(velocity * Time.deltaTime);
		currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

		switch (currentSpeed != 0)
		{
			case true:

			isMoving = true;
			

			break;

			case false:

			isMoving = false;
            

            break;
		}

        Vector3 velolat = transform.forward * currentSpeed;
		if(velolat.magnitude > 0)	
		{ 
			isMovingLateral = true;
		}
		else 
		{
            isMovingLateral = false;
        }
    }

	public void Jump()
	{
		if (controller.isGrounded)
		{
			float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;

		}
	}

	float GetModifiedSmoothTime(float smoothTime)
	{
		if (controller.isGrounded)
		{
			return smoothTime;
		}

		if (airControlPercent == 0)
		{
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}

	public void Teleport(Transform destination) 
	{
        //place the player

        controller.Move(destination.position - transform.position);
        transform.SetPositionAndRotation(destination.position, destination.rotation);
        cameraT.SetPositionAndRotation(destination.position, destination.rotation);


        Debug.Log("dest " + destination.name);
    }

}