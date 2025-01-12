using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAchievments : MonoBehaviour
{
    //I am saying, in my game, there are no more than 16 achievements, end of story
    //so why make it any more complicated than a fixed array?
    public Transform [] achievments = new Transform[16];
    private int emptyAchieve = 0;

    public void AddAchievment(Transform _acheive)
    {
        achievments[emptyAchieve] = _acheive;
        //increment the next empty achs.
        emptyAchieve++;
        //just let it error if out of bounds

        //we could also just grow the list here  if we want

    }
}
