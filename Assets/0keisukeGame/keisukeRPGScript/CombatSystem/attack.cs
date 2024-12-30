using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public DamageInfo damageInfo;

    [Header("攻撃の設定(こっちが優先されるNUllだったらdamageInfoのせっていを使う)")]
    public AttackPram attackPram;

    [HideInInspector]
    public attackcore attackcore;
    public System.Action AttackEndCallback;

    public UnityEngine.Events.UnityEvent AttackEndEvent;



    public bool once;
    bool onceHit;

    public List<GameObject> AttackHitObj=new List<GameObject>();
    void Awake()
    {

        SetTrigger(true);
        attackcore = gameObject.root().GetComponent<attackcore>();
        ResistAttackPram(attackPram);

    }
     public bool HitCheck(GameObject obj)
    {
        return AttackHitObj.Contains(obj);
    }
    public void ClearHitObj()
    {
        AttackHitObj.Clear();
    }

    public void AddHitObj(GameObject obj)
    {
        AttackHitObj.Add(obj);
    }


    public void ResistAttackPram(AttackPram attackPram)
    {
        if (attackPram == null) return;

        if (damageInfo == null)
        {
            damageInfo = new DamageInfo();
        }
        damageInfo.damagValue = attackPram.damagevalue;
        damageInfo.forceValue = attackPram.forcepower;
        damageInfo.sequenceHit = attackPram.sequenceHit;
    }


    public void SetTrigger(bool trigger)
    {
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().isTrigger = trigger;
    }

    //attacktagがないのはattackcoreのタグが適用されるため
    public void RadiusAddDammage(float radius)
    {
        var search = gameObject.RadiusSearch<Collider>(radius);
        foreach (var item in search)
        {
            ToAttackCore(item.gameObject);
        }
    }

    public void ToAttackCore(GameObject attacked)
    {
        if (once && onceHit) return;

        if (HitCheck(attacked)) return;


        var damadable = false;

        if (attackcore)
        {
            damadable = attackcore.attackon(gameObject, attacked, damageInfo);
        }
        else
        {
            damadable = attacked.Damage(damageInfo, false, gameObject);
        }

        if (damadable)
        {
            onceHit = true;

            if (!damageInfo.sequenceHit)
            {
                AddHitObj(attacked);
                //SetTrigger(false);
            }
            if (AttackEndCallback != null)
            {
                AttackEndCallback();
                AttackEndCallback = null;
            }
            if (AttackEndEvent != null) AttackEndEvent.Invoke();

        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other != gameObject)
        {
            ToAttackCore(other);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            ToAttackCore(other.gameObject);
        }
    }
}
