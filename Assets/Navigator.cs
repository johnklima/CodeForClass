using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour
{

    public Vector3 goalPos;
    public bool navigate = false;  //trigger
    NavMeshAgent agent;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        anim = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //simple navigation routine
        if(navigate && anim.GetBool("Run") )
        {
            agent.SetDestination(goalPos);
            navigate = false;
            //give air
            return;
        }

        
        if (agent.remainingDistance < 0.5f && anim.GetBool("Run"))
        {
            Debug.Log("I'm here!!!");
            
            anim.SetBool("Run", false);
            anim.SetBool("Idle", true);
            agent.isStopped = true;
        }
      

    }
}
