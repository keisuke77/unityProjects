using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodamage : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){

animator.GetComponent<hpcore>().nodamage=true;

}   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){

animator.GetComponent<hpcore>().nodamage=false;

}
    //    
    //}
}
