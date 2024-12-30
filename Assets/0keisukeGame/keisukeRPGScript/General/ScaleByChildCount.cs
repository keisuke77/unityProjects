using UnityEngine;

public class ScaleByChildCount : MonoBehaviour
{
    public Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 maxScale = new Vector3(1.0f, 1.0f, 1.0f);
    public int maxChildCount = 10;
    
    private int lastChildCount = 0;

    void Start()
    {
        UpdateChildScales();
        lastChildCount = transform.childCount;
    }

    // フレームごとに子要素の数を監視し、変化があればスケールを更新
    void LateUpdate()
    {
        int currentChildCount = transform.childCount;
        if (currentChildCount != lastChildCount)
        {
            UpdateChildScales();
            lastChildCount = currentChildCount;
        }
    }

    void UpdateChildScales()
    {
        int childCount = transform.childCount;
        Vector3 newScale = Vector3.Lerp(maxScale, minScale, Mathf.Clamp01((float)childCount / maxChildCount));

        foreach (Transform child in transform)
        {
            child.localScale = newScale;
        }
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateChildScales();
        }
    }
}
