using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class spawnobject :LayerIndexStateMachine
{ public enum MyEnumType
    {
        enter,
        stay,
        exit
    }
   public GameObject obj;
    public MyEnumType timing;

public bool destroy;
public bodypart bodyparts;
public Vector3 offSet;
public Vector3 rotate;
public float delay;


    public void Spawn(Animator animator){  Debug.Log("2" + layerIndex + "");
     
        keikei.delaycall(()=> { Instantiate(obj,bodyparts.Getbodypart(animator.gameObject).transform.position+offSet,Quaternion.Euler(rotate));
        if (destroy)
        {
             Destroy(animator.gameObject);

        }
       
     } ,delay);
          Debug.Log("3" + layerIndex + "");
    
}
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    protected override void OnHeaviestLayerEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("1" + layerIndex + "");
        
        if (timing==MyEnumType.enter)
        { Spawn(animator);
         }
    }
          protected override void OnHeaviestLayerUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timing==MyEnumType.stay)
        {
       Spawn(animator);   
        }
        
          }
          
          protected override void OnHeaviestLayerExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (timing==MyEnumType.exit)
        {
         Spawn(animator);
        }
        
          }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
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
}
