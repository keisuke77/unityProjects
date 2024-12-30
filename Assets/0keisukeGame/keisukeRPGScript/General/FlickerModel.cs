
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
[System.Serializable]
public class FilkerParam
{
    [SerializeField, Header("ダメージを受けたときの点滅色")]
    public Color flickerColor = Color.red;
    [SerializeField]
    public float flickerDuration = 0.5f, flickerSpeed = 0.1f;
    public bool isLerp;

    public FilkerParam(Color flickerColor, float flickerDuration, float flickerSpeed, bool isLerp)
    {
        this.flickerColor = flickerColor;
        this.flickerDuration = flickerDuration;
        this.flickerSpeed = flickerSpeed;
        this.isLerp = isLerp;
    }
}


public class FlickerModel : MonoBehaviour
{
    [SerializeField, Header("ダメージを受けたときの点滅色")]
    protected Color flickerColor = Color.red;
    [SerializeField]
    protected float flickerDuration = 0.5f, flickerSpeed = 0.1f;
    public bool isLerp;
    protected Dictionary<Material, Color> _materialAndInitialColors;
    protected virtual void Start()
    {
        StartCoroutine(DetectAllRenderer(gameObject));
    }

    //使うときはこれを呼び出します
    public void damagecolor()
    {

        StartCoroutine(ImageFlicker.Flicker(_materialAndInitialColors, flickerColor, flickerSpeed, flickerDuration, isLerp));
    }

    Coroutine coroutine;
    public void Stop()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public void Play(FilkerParam filkerParam)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

      coroutine=  StartCoroutine(ImageFlicker.Flicker(_materialAndInitialColors, filkerParam.flickerColor, filkerParam.flickerSpeed, filkerParam.flickerDuration, filkerParam.isLerp));

    }
    public void Play(FilkerParamscriptable filkerParam)
    {

        Play(filkerParam.fillerPram);
    }

    public void defaultcolorset(Color color)
    {

        StartCoroutine(defaultcolorsets(gameObject, color));
    }


    protected virtual IEnumerator defaultcolorsets(GameObject target, Color color)
    {

        _materialAndInitialColors = new Dictionary<Material, Color>();
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        foreach (Renderer render in renderers)//レンダラーすべてを調べる
        {

            foreach (Material material in render.materials)//マテリアルすべてを調査-
            {
                if (material.HasProperty("_Color"))
                {
                    _materialAndInitialColors.Add(material, color);
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }

    }
    //一度にすると時間かかりそうなのでコルーチンを使う
    //カラーの初期値も必要なのでDictionary型を使う
    //オブジェクトの持つカラー付きマテリアルをカラーと一緒にいれる
    protected virtual IEnumerator DetectAllRenderer(GameObject target)
    {
        _materialAndInitialColors = new Dictionary<Material, Color>();
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        foreach (Renderer render in renderers)//レンダラーすべてを調べる
        {

            foreach (Material material in render.materials)//マテリアルすべてを調査
            {
                if (material.HasProperty("_Color"))
                {
                    _materialAndInitialColors.Add(material, material.color);
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
//色を点滅
//初期値のカラーをマテリアルごとに設定しておかないといけないので辞書型を使った
public class ImageFlicker
{

    public static IEnumerator Flicker(Dictionary<Material, Color> matAndInitialColors/*マテリアルと初期設定してあるカラー*/,
        Color flickerColor/*点滅する色*/, float flickerSpeed/*点滅の速さ*/, float flickerDuration/*点滅する時間,秒*/, bool isLerp = false)
    {

        if (matAndInitialColors.Count == 0)
        {
#if UNITY_EDITOR
            Debug.Log("マテリアルがセットできていない:コルーチンを使っているが数フレームで取得できるはず");
#endif
            yield break;
        }
        float flickerStop = Time.time + flickerDuration;
        var wait = new WaitForSeconds(flickerSpeed);

        while (Time.time < flickerStop)
        {

            foreach (Material mat in matAndInitialColors.Keys)
            {
                if (isLerp)
                {
                    mat.DOColor(flickerColor, flickerSpeed);
                }
                else
                {
                    mat.color = flickerColor;
                }
            }
            yield return wait;
            foreach (KeyValuePair<Material, Color> kvp in matAndInitialColors)
            {
                kvp.Key.color = kvp.Value;
            }
            yield return wait;
        }
    }
    //追記
    //オブジェクトをプールする場合、OnDisableとかで呼び出さないと赤色のまま出現することがあります
    public static void ColorInitialize(Dictionary<Material, Color> matAndInitialColors)
    {
        if (matAndInitialColors == null || matAndInitialColors.Count == 0)
        {
#if UNITY_EDITOR
            Debug.Log("マテリアルがセットできていない:コルーチンを使っているが数フレームで取得できるはず");
#endif
            return;
        }
        foreach (KeyValuePair<Material, Color> kvp in matAndInitialColors)
        {
            kvp.Key.color = kvp.Value;
        }
    }

}