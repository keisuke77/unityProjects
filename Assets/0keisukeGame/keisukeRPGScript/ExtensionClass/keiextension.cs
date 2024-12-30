using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;

/// <summary>
/// 専用のランダムクラス
/// </summary>
public static class MyRandom
{
    /// <summary>
    /// bool型の乱数を取得する
    /// </summary>
    /// <returns>bool型の乱数</returns>
    public static bool RandomBool()
    {
        return UnityEngine.Random.Range(0, 2) == 0;
    }
}

[System.Serializable]
public class animstringbool
{
    public string boolname;
    public bool check;
}


[System.Serializable]
public class damagevalue
{
    public bool ondamage;
    public int value;
    public bool player;
}



public enum direction
{
    forward,
    back,
    left,
    right,
    up,
    down
}

/// <summary>
/// Listの拡張クラス
/// </summary>
public static class keiextension
{
 

 public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // fromMin から fromMax の範囲の値を toMin から toMax の範囲に圧縮
        float fromRange = fromMax - fromMin;
        float toRange = toMax - toMin;

        // 割合を計算
        float scale = toRange / fromRange;

        // toMin に新しい範囲内での値をマップ
        return toMin + (value - fromMin) * scale;
    }



  
    public static Effekseer.EffekseerHandle handle;

    public static void ScaleUpper(this Transform trans, float speed = 1)
    {
        Vector3 basescale = trans.localScale;
        trans.localScale = Vector3.zero;
        trans.DOScale(basescale, speed);
    }
   public static void AddAction(this Button btn,System.Action ac){
btn.onClick.AddListener(()=>ac());
   }
    public static void scalechange(this GameObject target, float num, float speed = 1)
    {
        target.transform.DOScale(
            target.transform.localScale * num, // スケール値
            speed // 演出時間
        );
    }

    public static void delaycall(this System.Action action, float time)
    {
        keikei.delaycall(action, time);
    }

    public static int stringindexScene(this string name)
    {
        return SceneManager.GetSceneByName(name).buildIndex - 1;
    }

    public static void ButtonEventSet(this Button btn, System.Action ac)
    {
        btn.onClick.AddListener(() => ac());
    }

    public static GameObject yesno(
        this string st,
        System.Action yes = null,
        System.Action no = null
    )
    {
        var im = ("YesNo").GameObjectResourcesLoad();
        im = im.Instantiate(im.transform);
        im.transform.ScaleUpper();
        im.transform.ChildFind("yesbutton").GetComponent<Button>().onClick.AddListener(() => yes());
        im.transform
            .ChildFind("yesbutton")
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                keikei.destroy(im);
            });
        im.transform
            .ChildFind("nobutton")
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                keikei.destroy(im);
            });
        im.transform.ChildFind("nobutton").GetComponent<Button>().onClick.AddListener(() => no());
        im.transform.ChildFind("QuestionText").GetComponent<Text>().DOText(st, 1);
        return im;
    }

    public static GameObject CreateImage(this Sprite sprite, GameObject obj)
    {
        return sprite.CreateImage(obj, Vector3.zero);
    }

    public static GameObject CreateImage(this Sprite sprite, GameObject obj, Vector3 pos)
    {
        var im = ("CustomImage").GameObjectResourcesLoad();
        im = im.Instantiate(obj.transform);
        im.gameObject.AddComponentIfnull<UIcameralook>();
        im.GetComponent<SpriteRenderer>().sprite = sprite;
        im.transform.parent = obj.transform;
        im.transform.localPosition = pos;
        return im;
    }

    public static GameObject CreateMesImage(
        this string st,
        GameObject obj,
        Vector3 posi,
        float duration = 3
    )
    {
        var im = ("CustomTextImage").GameObjectResourcesLoad();
        im = im.Instantiate(obj.transform);
        im.gameObject.AddComponentIfnull<UIcameralook>();
        im.transform
            .GetChild(0)
            .GetComponent<Text>()
            .DOText(st, 1f)
            .OnComplete(() =>
            {
                keikei.delaycall(() => im.destroy(), duration);
            });
        im.transform.parent = obj.transform;
        im.transform.localPosition = posi;
        return im;
    }

    public static GameObject GameObjectResourcesLoad(this string name)
    {
        return (GameObject)Resources.Load(name);
    }

  

    public static void transformreset(this Transform trans)
    {
        Vector3 p = trans.position;
        trans.localPosition = Vector3.zero;
        trans.gameObject.root().transform.position = trans.gameObject.root().transform.position + p;
    }

  
    public static void boolset(this GameObject obj, animstringbool animstringbool)
    {
        obj.GetComponent<Animator>().SetBool(animstringbool.boolname, animstringbool.check);
    }

    public static T2 Childremovecomponentandattach<T1, T2>(this GameObject obj)
    {
        foreach (var item in obj.GetAllChildrenAndSelf())
        {
            item.removecomponentandattach<T1, T2>();
        }
        return default(T2);
    }

    public static T2 ForceChildremovecomponentandattach<T1, T2>(this GameObject obj)
    {
        if (obj.Childremovecomponentandattach<T1, T2>() == null)
        {
            obj.AddComponent(typeof(T2));
            return obj.GetComponent<T2>();
        }
        else
        {
            return obj.Childremovecomponentandattach<T1, T2>();
        }
    }

    public static T2 removecomponentandattach<T1, T2>(this GameObject obj)
    {
        if (obj.GetComponent<T1>() != null)
        {
            obj.AddComponent(typeof(T2));
            keikei.Destroys(obj.GetComponent<T1>() as Component);
            return obj.GetComponent<T2>();
        }
        return default(T2);
    }

    /// <summary>
    /// 渡されたメソッドを指定時間後に実行する
    /// </summary>
    public static IEnumerator DelayMethod(this MonoBehaviour mono, float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    public static Vector3 To(this Vector3 vec, Vector3 to, float rate)
    {
        Vector3 move = to - vec;

        return vec + (move * rate);
    }

    public static T debug<T>(this T t)
    {
        Debug.Log(t);
        return t;
    }

    public static T[] debug<T>(this T[] t)
    {
        foreach (T item in t)
        {
            item.debug();
        }
        return t;
    }

    public static float grounddistances(this Transform trans, LayerMask targetLayer)
    {
        RaycastHit hitInfo;
        if (
            Physics.SphereCast(
                trans.position + trans.up,
                0.1f,
                Vector3.down,
                out hitInfo,
                Mathf.Infinity,
                targetLayer
            )
        )
        {
            return trans.position.y - hitInfo.point.y;
        }
        else
        {
            return Mathf.Infinity;
        }
    }

    public static void destroy(this GameObject obj, float time = 0)
    {
        keikei.destroy(obj, time);
    }

    public static System.Action fadedestroy(this GameObject obj, float time = 1)
    {
        System.Action ac = () =>
        {
            keikei.fadedeath(obj, time);
        };
        return ac;
    }

    public static GameObject SetActive(this Component com, bool bools)
    {
        com.gameObject.SetActive(bools);
        return com.gameObject;
    }

    public static void destroyObject(this GameObject obj, float power = 1)
    {
        var random = new System.Random();
        var min = -3;
        var max = 3;
        foreach (var item in obj.GetAllChild())
        {
            if (item.GetComponent<Collider>() != null)
            {
                keikei.delaycall(
                    () =>
                    {
                        item.GetComponent<Collider>().enabled = true;
                        item.GetComponent<Collider>().isTrigger = false;
                    },
                    0.3f
                );
            }
        }
        obj.AddComponentsIfNullInChildren<Rigidbody>();
        obj.GetComponentsInChildren<Rigidbody>()
            .ToList()
            .ForEach(r =>
            {
                r.isKinematic = false;
                r.transform.SetParent(null);
                r.gameObject.fadedestroy().delaycall(2);
                var vect = new Vector3(
                    random.Next(min, max),
                    random.Next(0, max),
                    random.Next(min, max)
                );
                vect *= power;
                r.AddForce(vect, ForceMode.Impulse);
                r.AddTorque(vect, ForceMode.Impulse);
            });
        obj.destroy();
    }

    public static bool nullchecks(this Effekseer.EffekseerEffectAsset obj)
    {
        if (obj)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string total(this List<string> list)
    {
        return string.Join("", list);
        ;
    }


  

    public static T rootaddcomponent<T>(this GameObject obj)
    {
        GameObject objs = obj.transform.root.gameObject;
        if (objs.GetComponent<T>() == null)
        {
            objs.AddComponent(typeof(T));
            return objs.GetComponent<T>();
        }

        return objs.GetComponent<T>();
    }

    public static List<T> replace<T>(this List<T> objs, T ob, T obs)
    {
        int a = objs.IndexOf(ob);
        int b = objs.IndexOf(obs);

        objs[a] = obs;
        objs[b] = ob;

        return objs;
    }


    public static float NotMini(this float num)
    {
        if (num < 0.1f && num > -0.1f)
        {
            num = 0;
        }
        return num;
    }

    public static bool distanceCheck(this Vector3 vec, int dis)
    {
        return vec.sqrMagnitude > dis * dis;
    }

    public static void addforce(
        this MonoBehaviour mono,
        GameObject obj,
        int forcepowers,
        Transform trans
    )
    {
        mono.StartCoroutine(
            obj.transform.AddForces((obj.transform.position - trans.position) * forcepowers)
        );
    }

    

    public static IEnumerator AddForces(this Transform obj, Vector3 force, System.Action ac = null)
    {
        var forcepart = force / 10;
        var temp = forcepart;

        while (forcepart.sqrMagnitude < force.sqrMagnitude)
        {
            obj.Translate(temp);
            forcepart += temp;

            yield return new WaitForSeconds(0.0001f);
        }
        ac();
        yield return null;
    }

 
    public static void ratecheck(float rate)
    {
        var ran = UnityEngine.Random.value;
        if (rate > ran)
        {
            return;
        }
    }

    public static List<T> GetComponent<T>(this List<GameObject> objs)
    {
        List<T> coms = new List<T>();
        foreach (GameObject item in objs)
        {
            coms.Add(item.GetComponent<T>());
        }
        return coms;
    }

    public static void delayAndwhilecall(
        this MonoBehaviour mono,
        System.Action action,
        float delay,
        float duration
    )
    {
        keikei.delaycall(() => mono.whilecall(action, duration), delay);
    }

    public static void whilecall(this MonoBehaviour mono, System.Action action, float time)
    {
        mono.StartCoroutine(whilecall(action, time));
    }

    public static IEnumerator whilecall(System.Action action, float time, float deltaTime = 0)
    {
        while (deltaTime < time)
        {
            action();
            deltaTime += Time.deltaTime;
        }
        yield return null;
    }

    static List<GameObject> objs;

    public static Vector3 TwoPositionCloseRate(this GameObject From, GameObject To, float rate)
    {
        Vector3 pos = (1 - rate) * From.transform.position + rate * To.transform.position;

        return pos;
    }

    public static void MoveTargetRate(
        this GameObject From,
        GameObject To,
        float rate,
        float time = 1,
        System.Action action = null
    )
    {
        From.transform.DOMove(From.TwoPositionCloseRate(To, rate), time).OnComplete(() => action());
    }

    public static void enabled(this Canvas can, bool boo)
    {
        can.enabled = boo;
    }

    public static float GetAnimationClipLength(this Animator animator, string clipName)
    {
        float clipLength = 0f;

        var rac = animator.runtimeAnimatorController;
        var clips = rac.animationClips.Where(x => x.name == clipName);
        foreach (var clip in clips)
        {
            clipLength = clip.length;
        }
        return clipLength;
    }

    public static void deatheventset(this GameObject self, UnityEvent events)
    {
        self.AddComponent(typeof(DestroyEvent));
        self.GetComponent<DestroyEvent>().events = events;
    }




    public static Collider Collider(this GameObject obj)
    {
        return obj.GetComponentIfNotNull<Collider>();
    }
}
