using UnityEngine;
using DG.Tweening;

public class IKControl : MonoBehaviour
{
    private Animator avatar;
    public bool active;
    public Transform lookAt;
    public Transform body;
    public Transform leftHand, rightHand, leftFoot, rightFoot;
    public Transform leftElbow, rightElbow, leftKnee, rightKnee;

    private void Start()
    {
        InitAvatar();
    }

    public void Stop() => active = false;

    private void OnAnimatorIK(int layerIndex)
    {
        if (!active) return;
        if (avatar == null)
            InitAvatar();
        else
            MoveAvatar();
    }

    private void InitAvatar()
    {
        avatar = GetComponent<Animator>();
        if (avatar == null) return;

        SetBodyPartPosition(ref body, avatar.bodyPosition, avatar.bodyRotation);
        SetHandAndFootPositions();
        SetLimbPositions();
    }

    private void SetBodyPartPosition(ref Transform part, Vector3 position, Quaternion rotation)
    {
        if (part != null)
        {
            part.position = position;
            part.rotation = rotation;
        }
    }

    private void SetHandAndFootPositions()
    {
        SetBodyPartPosition(ref leftHand, avatar.GetBoneTransform(HumanBodyBones.LeftHand).position, Quaternion.identity);
        SetBodyPartPosition(ref rightHand, avatar.GetBoneTransform(HumanBodyBones.RightHand).position, Quaternion.identity);
        SetBodyPartPosition(ref leftFoot, avatar.GetBoneTransform(HumanBodyBones.LeftFoot).position, Quaternion.identity);
        SetBodyPartPosition(ref rightFoot, avatar.GetBoneTransform(HumanBodyBones.RightFoot).position, Quaternion.identity);
    }

    private void SetLimbPositions()
    {
        SetBodyPartPosition(ref leftElbow, avatar.GetBoneTransform(HumanBodyBones.LeftLowerArm).position, Quaternion.identity);
        SetBodyPartPosition(ref rightElbow, avatar.GetBoneTransform(HumanBodyBones.RightLowerArm).position, Quaternion.identity);
        SetBodyPartPosition(ref leftKnee, avatar.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position, Quaternion.identity);
        SetBodyPartPosition(ref rightKnee, avatar.GetBoneTransform(HumanBodyBones.RightLowerLeg).position, Quaternion.identity);
    }

 
    private void MoveAvatar()
    {
        avatar.bodyPosition = body.position;
        avatar.bodyRotation = body.rotation;
        if (lookAt != null)
        {
                 avatar.SetLookAtPosition(lookAt.position);
        avatar.SetLookAtWeight(1.0f, 0.3f, 0.6f, 1.0f, 0.5f);
        }
   
        SetIK(AvatarIKGoal.LeftHand, leftHand);
        SetIK(AvatarIKGoal.RightHand, rightHand);
        SetIK(AvatarIKGoal.LeftFoot, leftFoot);
        SetIK(AvatarIKGoal.RightFoot, rightFoot);

        SetIKHint(AvatarIKHint.LeftElbow, leftElbow);
        SetIKHint(AvatarIKHint.RightElbow, rightElbow);
        SetIKHint(AvatarIKHint.LeftKnee, leftKnee);
        SetIKHint(AvatarIKHint.RightKnee, rightKnee);

        SetIKWeights(1);
    }

    private void SetIK(AvatarIKGoal goal, Transform target)
    {
        if (target == null) return;

        avatar.SetIKPosition(goal, target.position);
        avatar.SetIKRotation(goal, target.rotation);
    }

    private void SetIKHint(AvatarIKHint hint,Transform target)
    {
          if (target == null) return;
        avatar.SetIKHintPosition(hint, target.position);
    }

    private void SetIKWeights(float weight)
    {
        SetWeight(AvatarIKGoal.LeftHand, weight);
        SetWeight(AvatarIKGoal.RightHand, weight);
        SetWeight(AvatarIKGoal.LeftFoot, weight);
        SetWeight(AvatarIKGoal.RightFoot, weight);

        SetHintWeight(AvatarIKHint.LeftElbow, weight);
        SetHintWeight(AvatarIKHint.RightElbow, weight);
        SetHintWeight(AvatarIKHint.LeftKnee, weight);
        SetHintWeight(AvatarIKHint.RightKnee, weight);
    }

    private void SetWeight(AvatarIKGoal goal, float weight)
    {
        avatar.SetIKPositionWeight(goal, weight);
        avatar.SetIKRotationWeight(goal, weight);
    }

    private void SetHintWeight(AvatarIKHint hint, float weight)
    {
        avatar.SetIKHintPositionWeight(hint, weight);
    }
}
