using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InputMove : StateMachineBehaviour
{
  public int Layer;
  public float Movevalue=1.5f;
  public float MoveTime=1;
  public float RotateTime=0.2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
     
        if (animator.GetHeaviestLayerIndex()!=Layer)
        {
            return;
        }
        Transform transform = animator.transform;

        Vector3 direction = transform.CameraDirection(Camera.main, keiinput.Instance.directionkey).normalized;
        if (direction.magnitude > 0.2f)
        {
            transform.DOLocalMove(direction * Movevalue, MoveTime).SetRelative(true);
            transform.DORotate(new Vector3(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0), RotateTime);
        }


    }
}