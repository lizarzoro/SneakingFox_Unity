using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaycastChase : MonoBehaviour
{
    Ray theRay;
    public Color rayColor;
    RaycastHit rayHit;
    bool follow;

    private NavMeshAgent agent;
    public GameObject getHim;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        theRay = new Ray(transform.position, -transform.forward * 10);
        Debug.DrawRay(transform.position, -transform.forward * 10, rayColor);

        if(Physics.Raycast(transform.position, -transform.forward, 10))
        {
            follow = true;
        }

        if(follow == true)
        {
            agent.SetDestination(getHim.transform.position);
        }
    }
}
