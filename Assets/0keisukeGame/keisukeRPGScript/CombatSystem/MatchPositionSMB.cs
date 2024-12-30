
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPositionSMB : StateMachineBehaviour
{
    private Transform target;

    [Header("Match Settings")]
    [SerializeField] AvatarTarget targetBodyPart = AvatarTarget.Root;
    [SerializeField, MinMaxSlider(0, 1)] Vector2 effectiveRange;

    [Header("Assist Settings")]
    [SerializeField, Range(0, 1)] float assistPower = 1;
    [SerializeField, Range(0, 10)] float assistDistance = 1;

    MatchTargetWeightMask weightMask;
    bool isSkip = false;
    bool isInitialized = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isInitialized == false)
        {
            var weight = new Vector3(assistPower, 0, assistPower);
            weightMask = new MatchTargetWeightMask(weight, 0);
            isInitialized = true;
        } 
        target=animator.gameObject.NearSearchTag("Enemy").transform;
        isSkip = Vector3.Distance(target.position, animator.rootPosition) > assistDistance;
     
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isSkip == true || animator.IsInTransition(layerIndex))
            return;

        if (stateInfo.normalizedTime > effectiveRange.y)
        {
            animator.InterruptMatchTarget(false);
        }
        else
        {
            animator.MatchTarget(
                target.position,
                animator.bodyRotation,
                targetBodyPart,
                weightMask,
                effectiveRange.x,
                effectiveRange.y
            );
        }
    }
}

public interface IMatchTarget
{
    Vector3 TargetPosition { get; }
}
#endif
