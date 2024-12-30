using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using DG.Tweening;
using System;

/// <summary>
/// GameObjectの拡張クラス
/// </summary>
[System.Serializable]
public class DamageInfo
{
    [Range(0, 1000)]
    public int damagValue;
    public float forceValue;
    public bool sequenceHit;
    public string attackableTag;
    public object Clone()                            // シャローコピーになります。
    {
        return MemberwiseClone();
    }　
    public DamageInfo ShallowCopy()                          //シャローコピー
    {
        return (DamageInfo)this.Clone();
    }
}

public static class HPSystemExtension
{
    //万能ダメージクラス
   public static GameObject NearSearchTag(this GameObject nowObj, string tagName, Func<GameObject, bool> condition = null)
    {
        float tmpDis = 0f; // 距離用一時変数
        float nearDis = float.MaxValue; // 最も近いオブジェクトの距離を大きい値に初期化
        GameObject targetObj = null; // オブジェクト

        // タグ指定されたオブジェクトを配列で取得
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tagName);
        if (taggedObjects.Length == 0)
        {
            return null; // 見つからなかった場合はnullを返す
        }

        foreach (GameObject obs in taggedObjects)
        {
            // 条件を満たさないオブジェクトは無視
            if (condition != null && !condition(obs))
            {
                continue;
            }

            // 自身と取得したオブジェクトの距離を取得
            tmpDis = (obs.transform.position - nowObj.transform.position).sqrMagnitude;

            // オブジェクトの距離が近いかどうかを比較
            if (nearDis > tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }
        }

        return targetObj; // 最も近かったオブジェクトを返す
    }

    // デフォルトの「死亡していないオブジェクトを探す」メソッド
    public static GameObject NearSearchTagNotDeath(this GameObject nowObj, string tagName)
    {
        return nowObj.NearSearchTag(tagName, obs => obs.root().GetComponent<hpcore>().HP != 0);
    }

    public static void ColliderDataInput(this GameObject a_collider, GameObject a_object, ref Vector3 a_vector, float Power = 30)
    {
        a_vector.Set(
            a_object.transform.position.x - a_collider.transform.position.x,
            0f,
            a_object.transform.position.z - a_collider.transform.position.z
        );
        a_vector.Normalize();
        a_vector *= Power;
    }
    public static void PropertyChange<T>(this List<TNRD.SerializableInterface<T>> moves, System.Action<T> propertyChangeAction) where T : class
    {
        foreach (var move in moves)
        {
            propertyChangeAction(move.Value);
        }
    }



    public static async void CrossFadeAnimation(this Animator animator, string name, int CrossFadeSmoothLevel, System.Func<bool> Check = null)
    {

        // 遷移が終わるまで待機
        await animator.WaitForTransitionToEndAsync();

        if (Check == null || Check())
        {
            // 遷移が終わった後にCrossFadeを行う
            animator.CrossFadeInFixedTime(name, Time.deltaTime * CrossFadeSmoothLevel);

        }

    }
    public static List<IMove> Stop(this GameObject g,float delay=0)
    {

        var list = g.root().GetComponentsInChildren<IMove>();
        foreach (var item in list)
        {

            item.Stop = true;
        }
        if (delay > 0)
        {
            DOVirtual.DelayedCall(delay, g.Restart);
            Debug.Log("FirstStop");
        }
        
        return list.ToList();
    }
    public static void Restart(this GameObject g)
    {

        foreach (var item in g.root().GetComponentsInChildren<IMove>())
        {

            item.Stop = false;
        }
    }







    public static bool Damage(this GameObject attacked,
     DamageInfo damageInfo,
      bool crit = false, GameObject attacker = null
  )
    {

        return attacked.Damage(damageInfo.damagValue, damageInfo.attackableTag, crit, attacker, damageInfo.forceValue);
    }

    public static bool CollideCheck(this Transform transform,Vector3 direction,float distance)
    {
        
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance))
        {
            return false;
        }
        return true;
    }
  
    public static bool Damage(
        this GameObject attacked,
        int damagevalue,
        string objtag,
        bool crit = false,
        GameObject attacker = null,
        float forcepower = 0
    )
    {
        var rootObj = attacked.root();
        bool damadable = false;
        if (rootObj.CompareTag(objtag))
        {
            if (rootObj.GetComponent<hpcore>() != null)
            {
                damadable = rootObj
                    .GetComponent<hpcore>()
                    .damage(damagevalue, crit, attacked.Collider(), attacker);
            }
            Debug.Log(damadable);
            if (damadable && forcepower > 0)
            {
                var attackedAnimator = attacked.root().GetComponent<Animator>();
                var attackerAnimator = attacker.root().GetComponent<Animator>();

                if (attackerAnimator != null && attackedAnimator != null)
                {
                    attackerAnimator.speed = 0;
                    attackedAnimator.speed = 0;
                }

                if (attacked.root().GetComponent<ForceableObj>() != null)
                {
                    DOVirtual.DelayedCall(0.1f, () =>
                    {
                        if (attackerAnimator != null && attackedAnimator != null)
                        {
                            attackerAnimator.speed = 1;
                            attackedAnimator.speed = 1;
                        }
                        attacked.root().GetComponent<ForceableObj>().AddForce(attacker.root(), forcepower);
                    });
                }
                if (attacked.root().GetComponent<DOForce>() != null)
                {
                    DOVirtual.DelayedCall(0.1f, () =>
                    {
                        if (attackerAnimator != null && attackedAnimator != null)
                        {
                            attackerAnimator.speed = 1;
                            attackedAnimator.speed = 1;
                        }
                        attacked.root().GetComponent<DOForce>().AddForce(attacker.root(), forcepower);
                    });
                }
            }
            return damadable;
        }
        return false;
    }

    public static void PlayerAddForce(this GameObject attacked, Vector3 force)
    {
        attacked.root().GetComponent<ForceableObj>().AddForce(force);
    }

    public static void collset(this GameObject obj, int damagevalue, string AttackableTag = "Enemy")
    {
        if (obj.GetComponent<Collider>() == null)
        {
            var col = obj.AddComponent(typeof(BoxCollider)) as Collider;
            col.isTrigger = true;
            col.enabled = false;
        }

        var attack = obj.AddComponentIfnull<attack>() as attack;
        attack.damageInfo.damagValue = damagevalue;
        attack.damageInfo.attackableTag = AttackableTag;
    }

    public static void damageset(this GameObject obj, int damagevalue)
    {
        obj.GetComponentIfNotNull<attackcore>().basedamagevalue = damagevalue;
    }



    public static GameObject Getbodypart(this bodypart bodypart, GameObject obj)
    {
        switch (bodypart)
        {
            case bodypart.righthand:

                return obj.GetComponent<GetBodyPart>()
                    .GetRightHand();
                break;
            case bodypart.lefthand:

                return obj.GetComponent<GetBodyPart>()
                    .GetLeftHand();
                break;
            case bodypart.rightfoot:

                return obj.GetComponent<GetBodyPart>()
                    .GetRightFoot();
                break;
            case bodypart.leftfoot:
                return obj.GetComponent<GetBodyPart>()
                    .GetLeftFoot();
                break;
            case bodypart.weapons:
                return obj.GetComponent<GetBodyPart>()
                    .GetWeapon(); break;
            case bodypart.body:
                return obj.GetComponent<GetBodyPart>()
                    .GetBody(); break;
            case bodypart.no:
                return obj;
                break;
            default:
                return null;
                break;
        }
    }
}
