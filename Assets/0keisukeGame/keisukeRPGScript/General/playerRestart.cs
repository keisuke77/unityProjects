using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerRestart : StateMachineBehaviour
{
 

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      animator.gameObject.Restart();
   
    } 
   
}