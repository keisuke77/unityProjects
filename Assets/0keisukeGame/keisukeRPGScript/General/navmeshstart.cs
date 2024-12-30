using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navmeshstart : StateMachineBehaviour
{
   public float speed;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       var nav= animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
       nav.speed=speed;
    }

    
}
