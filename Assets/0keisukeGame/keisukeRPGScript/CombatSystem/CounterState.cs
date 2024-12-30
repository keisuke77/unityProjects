using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CounterState : LayerIndexStateMachine
{
    public String AnimBool="Counter";

    public float AnimSpeed=1;
    float defaultSpeed;
    protected override void OnHeaviestLayerEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {  
        Debug.Log("Counterrr");
        animator.SetBool(AnimBool,false);
        defaultSpeed=animator.speed;
        animator.speed=AnimSpeed;
    animator.GetComponent<hpcore>().CounterCallBack=()=>animator.SetBool(AnimBool,true);
    
    }
  protected override void  OnHeaviestLayerExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {  
        animator.speed=defaultSpeed;
    animator.SetBool(AnimBool,false);
     animator.GetComponent<hpcore>().CounterCallBack=null;
    
    }
}