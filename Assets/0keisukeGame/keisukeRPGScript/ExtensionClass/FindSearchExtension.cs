using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public static class FindSearchExtension
{
    public static GameObject FindAllChild(this GameObject obj, string objname)
    {
        foreach (GameObject item in obj.GetAllChildrenAndSelf())
        {
            if (item.name == objname)
            {
                return item;
            }
        }
        return null;
    }

    public static GameObject ChildFind(this Transform trans, string name)
    {
        var child = trans.GetComponentsInChildren<Transform>();
        foreach (Transform item in child)
        {
            if (item.gameObject.name == name)
            {
                return item.gameObject;
            }
        }
        return null;
    }

  

    

    public static GameObject[] RadiusSearchTag(this GameObject nowObj, string tagName, float radius)
    {
        float tmpDis = 0; //距離用一時変数
        //string nearObjName = "";    //オブジェクト名称
        List<GameObject> targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            //自身と取得したオブジェクトの距離を取得
            tmpDis = (obs.transform.position - nowObj.transform.position).sqrMagnitude;
            if (tmpDis < radius)
            {
                targetObj.Add(obs);
            }
        }

        return targetObj.ToArray();
    }

    public static T[] RadiusSearch<T>(this UnityEngine.GameObject nowObj, float radius)
        where T : UnityEngine.Component
    {
        float tmpDis = 0; //距離用一時変数
        //string nearObjName = "";    //オブジェクト名称
        List<T> targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で.取得する
        foreach (
            UnityEngine.GameObject obs in UnityEngine.Object.FindObjectsOfType(
                typeof(UnityEngine.GameObject)
            )
        )
        {
            //自身と取得したオブジェクトの距離を取得
            tmpDis = (obs.transform.position - nowObj.transform.position).sqrMagnitude;
            if (tmpDis < radius * radius)
            {
                targetObj.Add(obs.GetComponent<T>());
            }
        }

        return targetObj.ToArray();
    }
}
