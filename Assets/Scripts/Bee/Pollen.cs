using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pollen : MonoBehaviour
{

    bool isCollected = false;

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
        if (isCollected)
            return;

        if (other.tag != "Player")
            return;

        Debug.Log("pollen" + other.name);

        transform.parent.parent = other.transform;
        transform.parent.localPosition = Vector3.zero;
        transform.localPosition = Vector3.right * 0.5f;

        isCollected = true;
    }
}
