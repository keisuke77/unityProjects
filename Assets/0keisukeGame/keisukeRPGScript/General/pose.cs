using UnityEngine;
using DG.Tweening;
using System;

using TNRD;
using System.Collections.Generic;
using System.Linq;

public class pose : MonoBehaviour
{

    [Header("動きを止める　メニューを動かす際のkeycodeは自分で指定")] public GameObject Player;
    [Header("演出用ライトセットしなくてもどっちでもよい")]
    public Light lights;

    public List<SerializableInterface<IMove>> move;

    float orivalue;
    public bool TimeScaleStop;
    private void Awake()
    {
        if (lights != null)
        {
            orivalue = lights.intensity;

        }

    }
    private void OnEnable()
    {
        move?.PropertyChange<IMove>(x =>
        {
            // ここで move に対して行いたい処理を記述
            x.Stop = true;
        });
        if (TimeScaleStop)
        {
            Time.timeScale = 0;
        }
        if (lights != null)
        {

            DOTween.To(() => lights.intensity, (x) => lights.intensity = x, 60000, 0.4f);

        }
        Player.SetActive(false);

        if (GameObjectExtension.GetComponentsInActiveScene<IMove>(out var navchaises, false))
        {
            PoseEnd = null;
            foreach (var item in navchaises)
            {

                item.Stop = true;
                PoseEnd += () => item.Stop = false;

            }
        }

    }
    public Action PoseEnd;
    private void OnDisable()
    {
        Player.SetActive(true);
        move?.PropertyChange<IMove>(x =>
        {
            // ここで move に対して行いたい処理を記述
            x.Stop = false;
        });
        if (TimeScaleStop)
        {
            Time.timeScale = 1;
        }
        if (lights != null)
        {
            DOTween.To(() => lights.intensity, (x) => lights.intensity = x, orivalue, 0.4f);
        }


        keiinput.Instance.stop = false;
        PoseEnd?.Invoke();

    }
}
