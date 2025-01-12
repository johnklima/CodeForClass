using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator anim;
    public FootstepThirdPerson footstep;
    private float volume = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator> ();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Walk()
    {
        ResetAnimParams();
        volume = 1.0f;
        anim.SetBool("Walk", true);        
        return true;

    }

    public bool Idle()
    {
        ResetAnimParams();
        volume = 1.0f;
        anim.SetBool("Idle", true);        
        return true;

    }

    public bool Sneak()
    {
        ResetAnimParams();
        volume = 0.25f;
        anim.SetBool("Sneak", true);        
        return true;

    }

    public bool Run()
    {
        ResetAnimParams();
        volume = 1.0f;
        anim.SetBool("Run", true);
        return true;

    }

    public void ResetAnimParams()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Sneak", false);
        anim.SetBool("Run", false);

    }
    public void TriggerSound()
    {
        Debug.Log("trigger sound");

        if(footstep)
            footstep.PlaySurfaceSound(volume);
    }
}
