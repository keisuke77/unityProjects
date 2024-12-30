using UnityEngine;

public class TargetFollowsMe : MonoBehaviour
{
    public Transform targetObject; // 追従させたいターゲットオブジェクト
    private Vector3 initialRelativePosition; // 初期の相対位置
    private Quaternion initialRelativeRotation; // 初期の相対回転

    void Awake()
    {
        // 初期の相対位置と回転を保存
        initialRelativePosition = targetObject.position - transform.position;
        initialRelativeRotation = Quaternion.Inverse(transform.rotation) * targetObject.rotation;
    }

    void LateUpdate() // 君の動きが反映された後にターゲットを追従させる
    {
        // 君のオブジェクトの位置と回転に応じて targetObject の位置と回転を更新
        targetObject.position = transform.position + transform.rotation * initialRelativePosition;
        targetObject.rotation = transform.rotation * initialRelativeRotation;
    }
}
