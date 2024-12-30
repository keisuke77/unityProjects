using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class methodexecute : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    public enum MyEnumType
    {
        enter,
        stay,
        exit
    }

   public string methodname;
    public MyEnumType timing;
    public bool charhikisu;
    public string hikisu;
    public bool inthikisu;
    public int hikisu2;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (timing==MyEnumType.enter)
        {    
         callmethod(animator);
         
          }
    }
          override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timing==MyEnumType.stay)
        {
           callmethod(animator);
        }
          }
          
          public void callmethod(Animator obj){ 
            
            if (charhikisu)
            {
                 obj.gameObject.SendMessage(methodname,hikisu);
         
            }else if(inthikisu)
            {
                 obj.gameObject.SendMessage(methodname,hikisu2);
         
            }else
            {
               obj.gameObject.SendMessage(methodname);
            }
               
          }
          override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (timing==MyEnumType.exit)
        {
           
         callmethod(animator);
        }
        
          }
}
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

