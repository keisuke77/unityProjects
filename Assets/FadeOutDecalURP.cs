using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FadeOutDecalURP : MonoBehaviour
{
    public float fadeSpeed = 0.5f; // どれくらい速くフェードアウトするか

    private DecalProjector decalProjector;

    private void Start()
    {
        decalProjector = GetComponent<DecalProjector>();
    }

    private void Update()
    {
        if (decalProjector != null)
        {
            float opacity = decalProjector.fadeFactor;
            opacity -= fadeSpeed * Time.deltaTime;
            decalProjector.fadeFactor = Mathf.Max(0, opacity);

            if (opacity <= 0)
            {
                Destroy(gameObject);  // 透明度が0になったらオブジェクトを破壊
            }
        }
    }
}
