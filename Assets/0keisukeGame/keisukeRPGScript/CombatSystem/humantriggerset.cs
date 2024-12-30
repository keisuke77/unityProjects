using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum bodypart
{
    no,
    righthand,
    lefthand,
    rightfoot,
    leftfoot,
    weapons,
    body
}
public class humantriggerset : StateMachineBehaviour
{

    [Header("キャラクターチェンジがついてるもののみ")]

    public int Layer;
    public AttackPram attackPram;


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var attackcore = animator.GetComponent<attackcore>();
        attackcore.allofftriger();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        if (animator.GetHeaviestLayerIndex() != Layer)
        {
            return;
        }

        var attackcore = animator.gameObject.GetComponent<attackcore>();

        if (attackPram.bodypart != bodypart.no)
        {
            attackcore.AddAtackPart( new AttackPart(animator.gameObject.root(), attackPram));
      
        }

      

    }
}
