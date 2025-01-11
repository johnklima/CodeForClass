using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollenDepot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        Debug.Log(other.name + "collide with depot");

        foreach(Transform pollen in other.transform)
        {
            pollen.parent = transform;
        }


    }

}
