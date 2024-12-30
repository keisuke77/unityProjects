using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class DotweenMove : LayerIndexStateMachine
{

    public float delay;
    public DoTweenSeri DoTweenSeri;
    public DiretionEnum dir;
    public float speed;
    public float MoveAmount;
    public LayerMask groundMask;

    public EffectAndParticle HitEffect;
    private Vector3 lastPosition;
    public float RayLength = 0.3f;

    public string HitBool;



    void Kill(int num=0)=> sequence.Kill();

    float time;
     Sequence sequence = null;


    protected override void OnHeaviestLayerEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        keikei.delaycall(()=>{
        if (layerUse && animator.GetHeaviestLayerIndex() != this.layerIndex)
        {
            return;
        }
        animator.SetBool(HitBool, false);
        lastPosition = animator.transform.position;

        if (animator.TryGetComponent(out hpcore hpComponent))
        {
            hpComponent.OnHpChanged += Kill;
        }

           if (dir != DiretionEnum.No)
        {
         var s=  animator.gameObject.transform.DOLocalMove(animator.gameObject.transform.GetDirection(dir) * MoveAmount, speed).SetRelative(true);
         sequence.Append(s);
        }
        else
        {
            sequence = DoTweenSeri.Play(animator.gameObject.transform);

        }

        sequence.OnUpdate(() =>
     {
         time += Time.deltaTime;
         Vector3 currentPosition = animator.transform.position;
         Vector3 direction = (currentPosition - lastPosition).normalized;
         lastPosition = currentPosition;
         Debug.Log(direction);
         Vector3 startPosition = animator.transform.position;

         // SphereCastの結果を可視化
         if (Physics.Raycast(startPosition + animator.transform.forward, direction, out RaycastHit hit, RayLength, groundMask))
         {
             Debug.Log("CollieeTweening" + animator.gameObject.name);
             Kill(0);
             animator.SetBool(HitBool, true);

             if (HitEffect != null && time > 0.2f)
             {
                 HitEffect.Execute(hit.point);

             }

             // ヒットした位置までのレイを描画
             Debug.DrawRay(startPosition, direction * hit.distance, Color.red);
         }
         else
         {
             // レイキャストがヒットしなかった場合、最大距離までのレイを描画
             Debug.DrawRay(startPosition, direction * RayLength, Color.black);
         }

     });

        },delay);

    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override protected void OnHeaviestLayerExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0;
        hpcore hpComponent = animator.GetComponent<hpcore>();

        if (hpComponent != null)
        {
            hpComponent.OnHpChanged -= Kill;
        }
        Kill(0);
    }


}
