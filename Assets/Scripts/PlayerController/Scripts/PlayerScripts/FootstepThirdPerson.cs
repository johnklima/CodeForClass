using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepThirdPerson : MonoBehaviour
{
    
    
    public float hitDistance = 10;
    private FMOD.Studio.EventInstance footSoundInstance;


    private void Start()
    {
        footSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/grass");
    }
    private void Update()
    {
       
        footSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Camera.main.gameObject));

    }
    // Update is called once per frame
    public void PlaySurfaceSound(float volume)
    {

        
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;  //ground layer

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            //layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, layerMask))
            {
                if (hit.distance < hitDistance)
                {

                    if (hit.transform.tag == "grass")          //grass
                    {
                        ChangeSound(0, volume);
                    }
                    else if (hit.transform.tag == "dirt")    //dirt 
                    {
                        ChangeSound(1, volume);
                    }
                    else if (hit.transform.tag == "rock")    //rock
                    {
                        ChangeSound(2, volume);
                    }
                    else if (hit.transform.tag == "water")   //water
                    {
                        ChangeSound(3, volume);
                    }

                    Debug.Log(hit.transform.tag);

                }

            }   
    }
    void ChangeSound(int index, float volume)
    {
        
        if(index == 0)
        {
            footSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/grass");
        }
        if (index == 1)
        {
            footSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/dirt");
        }
        if (index == 2)
        {
            footSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/rock");
        }
        if (index == 3)
        {
            footSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/water");
        }


        footSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Camera.main.gameObject));
        footSoundInstance.setVolume(volume);
        footSoundInstance.start();
        
        footSoundInstance.release();

    }
    IEnumerator Countdown(float time)
    {
        Debug.Log("countdown");
        yield return new WaitForSeconds(time);

    }
}
