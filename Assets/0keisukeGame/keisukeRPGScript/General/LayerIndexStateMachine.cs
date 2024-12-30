using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LayerIndexStateMachine : StateMachineBehaviour
{
    public bool layerUse;
    public int layerIndex;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {Debug.Log("--0" + layerIndex + "");
    
        if (!layerUse||(layerUse&&animator.GetHeaviestLayerIndex() == this.layerIndex))
        {
              Debug.Log("0" + layerIndex + "");
    
            OnHeaviestLayerEnter(animator, stateInfo, layerIndex);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         if (!layerUse||(layerUse&&animator.GetHeaviestLayerIndex() == this.layerIndex))
        {
            OnHeaviestLayerUpdate(animator, stateInfo, layerIndex);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
          if (!layerUse||(layerUse&&animator.GetHeaviestLayerIndex() == this.layerIndex))
     {
            OnHeaviestLayerExit(animator, stateInfo, layerIndex);
        }
    }

    // Virtual method to be overridden in derived classes for entering the heaviest layer
    protected virtual void OnHeaviestLayerEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Default implementation can be empty or contain base behavior
    }

    // Virtual method to be overridden in derived classes for updating the heaviest layer
    protected virtual void OnHeaviestLayerUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Default implementation can be empty or contain base behavior
    }

    // Virtual method to be overridden in derived classes for exiting the heaviest layer
    protected virtual void OnHeaviestLayerExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Default implementation can be empty or contain base behavior
    }
}
