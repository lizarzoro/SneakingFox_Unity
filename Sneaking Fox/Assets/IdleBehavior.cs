using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    public Transform sightStart, sightEnd;

    void Update()
    {
        Raycasting();
        Behaviours();
    }

    void Raycasting()
    {
        Debug.DrawLine(sightStart.position, sightEnd.position, Color.green);
    }
    void Behaviours()
    {

    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isFollowing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}
