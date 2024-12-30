using DG.Tweening;
using UnityEngine;

public class CircularMove  : StateMachineBehaviour
{
      public int Layer;
    public Vector3 center = new Vector3(0, 0, 0); // 円の中心点
    public float radius = 5f; // 円の半径
    public float duration = 5f; // 一周するのにかかる時間

public float delay = 0;
    public Tween tween;
    public AnimationCurve curve;

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

    

        // DOTweenで円を描くように移動
      tween= animator.transform.DOLocalPath(CreateCirclePath(center, radius), duration, PathType.Linear, PathMode.Full3D)
            .SetLoops(-1) // 無限ループ
            .SetEase(curve).SetRelative(true).SetDelay(delay);
    }
        override public void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {

        tween.Kill();
    }
    // 円のパスを生成するメソッド
    Vector3[] CreateCirclePath(Vector3 center, float radius)
    {
        int segments = 36; // 円の分割数（多いほど滑らかになる）
        Vector3[] path = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            path[i] = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        }

        return path;
    }
}
