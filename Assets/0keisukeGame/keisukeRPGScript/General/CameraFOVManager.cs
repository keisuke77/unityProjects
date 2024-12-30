using UnityEngine;
using System.Collections;

public class CameraFOVManager : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void SetFOV(float newFOV)
    {
        cam.fieldOfView = newFOV;
    }
 public void Animation(float targetFOV)
    {
        StartCoroutine(AnimateFOV(targetFOV));
    }
    public IEnumerator AnimateFOV(float targetFOV, float duration=0.5f)
    {
        float initialFOV = cam.fieldOfView;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cam.fieldOfView = Mathf.Lerp(initialFOV, targetFOV, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.fieldOfView = targetFOV;
    }
}
