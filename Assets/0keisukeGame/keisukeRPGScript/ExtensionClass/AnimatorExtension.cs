using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;  // UniTaskのためのnamespaceを追加
using Unity.VisualScripting;

using System.Collections;

using UnityEngine.Animations;
using UnityEngine.Playables;


public static class AnimatorExtension
{
    public static AnimationClip GetCurrentClipFromMostWeightedLayer(this Animator animator)
    {
        int layerCount = animator.layerCount;
        int mostWeightedLayerIndex = animator.GetHeaviestLayerIndex();

  

        // 最も重いレイヤーを特定
    

        // 最も重いレイヤーのアニメーションクリップを取得
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(mostWeightedLayerIndex);
        if (clipInfo.Length > 0)
        {
            return clipInfo[0].clip;
        }

        // アニメーションがない場合はnullを返す
        return null;
    }

     public static void BlendFromCurrentState(this Animator animator, AnimationClip newClip, float duration=0.5f, bool revertBack = true)
    {
        CoroutineHelper.Instance.StartCoroutine(BlendFromAndToCurrentRoutine(animator, newClip, duration, revertBack));
    }

    private static IEnumerator BlendFromAndToCurrentRoutine(Animator animator, AnimationClip newClip, float duration, bool revertBack)
    {
        // 現在の currentState のアニメーションクリップを取得
        var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if(currentClipInfo.Length == 0)
        {
            yield break;
        }

        var currentClip = currentClipInfo[0].clip;

        // 現在のクリップから新しいクリップへブレンド
        var graph = animator.BlendBetweenClips(currentClip, newClip, 0);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float blendWeight = Mathf.Lerp(0, 1, elapsed / duration);
            var mixer = (AnimationMixerPlayable)graph.GetRootPlayable(0);
            mixer.SetInputWeight(0, 1f - blendWeight);
            mixer.SetInputWeight(1, blendWeight);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 新しいクリップの再生時間分待機
        yield return new WaitForSeconds(newClip.length - duration); 

        if (revertBack)
        {
            // 新しいクリップから元のクリップへブレンド
            elapsed = 0f;

            while (elapsed < duration)
            {
                float blendWeight = Mathf.Lerp(1, 0, elapsed / duration);
                var mixer = (AnimationMixerPlayable)graph.GetRootPlayable(0);
                mixer.SetInputWeight(0, 1f - blendWeight);
                mixer.SetInputWeight(1, blendWeight);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        graph.Destroy(); // PlayableGraphを破棄
    }

    public static PlayableGraph BlendBetweenClips(this Animator animator, AnimationClip originalClip, AnimationClip newClip, float initialWeight)
    {
       PlayableGraph graph = PlayableGraph.Create();
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        var playableOutput = AnimationPlayableOutput.Create(graph, "Animation", animator);

        var mixer = AnimationMixerPlayable.Create(graph, 2);
        playableOutput.SetSourcePlayable(mixer);

        var originalPlayable = AnimationClipPlayable.Create(graph, originalClip);
        graph.Connect(originalPlayable, 0, mixer, 0);

        var newClipPlayable = AnimationClipPlayable.Create(graph, newClip);
        graph.Connect(newClipPlayable, 0, mixer, 1);
        
        mixer.SetInputWeight(0, 1 - initialWeight);
        mixer.SetInputWeight(1, initialWeight);

        graph.Play();

        return graph;   }



[System.Serializable]
public class FloatTween
{
   public Animator anim;
   public string name;
   public float target;
   public float duration=1;

public int id;
   public void Execute(){
    anim.FloatTo(name,target,duration);
   }
}
    public static void FloatTo(this Animator anim,string name,float n,float duration=1){
DOVirtual.Float(anim.GetFloat(name), n, duration, value =>{anim.SetFloat(name,value);});
    }
      public static bool GetTrigger(this Animator animator, string triggerName)
    {
        return animator.GetBool(triggerName);
    }

       public static async UniTask WaitForTransitionToEndAsync(this Animator animator)
    {
        while (animator.IsInTransition(0))
        {
            await UniTask.Yield();  // 次のフレームまで待機
        }
    }


    public static int GetHeaviestLayerIndex(this Animator animator)
    {
        int layerCount = animator.layerCount;
        float maxWeight = 0f;
        int heaviestLayerIndex = -1;

        for (int i = 1; i < layerCount; i++) // start from 1 to exclude layer 0
        {
            float currentWeight = animator.GetLayerWeight(i);
            if (currentWeight > maxWeight)
            {
                maxWeight = currentWeight;
                heaviestLayerIndex = i;
            }
        }

        return heaviestLayerIndex;
    }
}