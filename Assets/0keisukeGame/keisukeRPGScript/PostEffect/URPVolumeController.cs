using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class URPVolumeController : MonoBehaviour
{
    public Volume volume;
    public float transitionSpeed = 1.0f; // この値を大きくすると、アニメーションは速くなります



    public void SetVolumeWeight(float targetWeight)
    {
        StartCoroutine(AnimateVolumeWeightChange(targetWeight));
    }

    private IEnumerator AnimateVolumeWeightChange(float targetWeight)
    {
        float startWeight = volume.weight;

        float elapsed = 0f;
        while(elapsed < 1f)
        {
            elapsed += Time.deltaTime * transitionSpeed;
            volume.weight = Mathf.Lerp(startWeight, targetWeight, elapsed);
            yield return null;
        }
        volume.weight = targetWeight;
    }
}
