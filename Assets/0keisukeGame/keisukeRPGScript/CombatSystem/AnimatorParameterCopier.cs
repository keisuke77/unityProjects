using UnityEngine;

public class AnimatorParameterAndWeightCopier : MonoBehaviour
{
    public Animator sourceAnimator;
    public Animator targetAnimator;

    void Update()
    {
        CopyAnimatorParametersAndWeights(sourceAnimator, targetAnimator);
    }

    void CopyAnimatorParametersAndWeights(Animator source, Animator target)
    {
        if (source == null || target == null)
        {
            Debug.LogError("Source or Target Animator is not assigned.");
            return;
        }

        // 全てのパラメーターをコピー
        foreach (AnimatorControllerParameter param in source.parameters)
        {
            switch (param.type)
            {
                case AnimatorControllerParameterType.Float:
                    target.SetFloat(param.name, source.GetFloat(param.name));
                    break;
                case AnimatorControllerParameterType.Int:
                    target.SetInteger(param.name, source.GetInteger(param.name));
                    break;
                case AnimatorControllerParameterType.Bool:
                    target.SetBool(param.name, source.GetBool(param.name));
                    break;
                case AnimatorControllerParameterType.Trigger:
                    if (source.GetBool(param.name))
                    {
                        target.SetTrigger(param.name);
                    }
                    break;
            }
        }

        // 各レイヤーのweightをコピー
        int layerCount = source.layerCount;
        for (int i = 0; i < layerCount; i++)
        {
            float sourceWeight = source.GetLayerWeight(i);
            target.SetLayerWeight(i, sourceWeight);
        }
    }
}
