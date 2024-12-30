using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LookAtNearTag: StateMachineBehaviour
{
    public string tag="Player";
    
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {  

       var point = animator.gameObject.NearSearchTag(tag).root().transform;
       animator.transform.DOLookAt(point.position,0.5f);
    }
}
