using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

[System.Serializable]
public class AttackPart
{
    public Collider _Collider;
    public string name;


    [HideInInspector]
    public attack attack;


    public AttackPart(GameObject obj, AttackPram attackPram)
    {
        GameObject SettingObject = attackPram.bodypart.Getbodypart(obj);
        Create(SettingObject, attackPram);

    }
    public attack Create(GameObject obj, AttackPram attackPram)
    {
        if (obj.GetComponent<Collider>() != null)
        {
            _Collider = obj.GetComponent<Collider>();
        }
        else
        {
            var Collider = obj.AddComponent<BoxCollider>();
            Collider.size = Vector3.one * attackPram.ColliderSize;
            _Collider = Collider;
        }

        name = attackPram.name;
        attack = _Collider.gameObject.AddComponentIfnull<attack>();
        if (attack != null)
            attack.ResistAttackPram(attackPram);
        ColliderSet(false);
        return attack;
    }

   
    public void ColliderSet(bool enable)
    {
        if (_Collider)
        {
            if (enable)
            {
                attack.ClearHitObj();
            }
            if (_Collider != null)
            {
                _Collider.isTrigger = true;
                _Collider.enabled = enable;
            }
        }
    }

}

public class attackcore : MonoBehaviour
{
    [Header("ここで設定すればAnimatorから名前指定でコライダ操作できる")]
    public List<AttackPart> AttackParts;

    public AttackPart CurrentAttackPart;
    public int basedamagevalue;
    public float baseforcepower;
    public string AttackableTag = "Player";
    public bool overrideTag = true;

    [SerializeField, Range(0, 1)]
    private float CritRate = 0.1f;

    [SerializeField]
    private float CritMultiplier = 3f;

    [Header("これを設定するとhpが０になったときすぐ攻撃がいかなくなる")]
    public hpcore hpcore;
    //一つでもトリガーがオンになっているか
    public bool isOnTrigger => AttackParts.Exists(n => n._Collider.isTrigger);
    public void CurrentAttackPartSet(string name)
    {
        foreach (var item in AttackParts)
        {
            if (item.name == name)
            {
                CurrentAttackPart = item;
            }
        }
    }

    //追加したものをそのままcurrentにするか
    public void AddAtackPart(AttackPart attackPart, bool current = true)
    {
        if (AttackParts == null)
        {
            AttackParts.Add(attackPart);
            if (current)
            {
                CurrentAttackPart = attackPart;
            }
            return;
        }

        bool check = false;
        foreach (var item in AttackParts)
        {
            if (item.name == attackPart.name)
            {
                check = true;
            }

        }

        if (!check)
        {
            AttackParts.Add(attackPart);
        }
        if (current)
        {
            CurrentAttackPart = attackPart;
        }
    }

    void Update()
    {

        foreach (var item in AttackParts)
        {
            if (item.attack != null)
            {
                item.attack.attackcore = this;
            }

        }

    }

    public void AddAtackPartCurrent()
    {

        AddAtackPart(CurrentAttackPart);
    }



    private void Start()
    {
        allofftriger();
    }
    public void allofftriger()
    {
        foreach (var item in AttackParts)
        {
            item.ColliderSet(false);
        }
        CurrentAttackPart?.ColliderSet(false);
    }


    public void ontrigerName(string name)
    {
        foreach (var item in AttackParts)
        {
            if (item.name == name)
            {
                item.ColliderSet(true);
               
            }
        }
    }
    public System.Action<GameObject> TriggerOnCallBack;
    public void offtrigerName(string name)
    {
        foreach (var item in AttackParts)
        {
            if (item.name == name)
            {
                item.ColliderSet(false);
            }
        }
    }

    public void ontriger()
    {
        CurrentAttackPart.ColliderSet(true);
      
        if (TriggerOnCallBack != null)
        {
            TriggerOnCallBack(gameObject);
        }

    }
    public void ontrigerPram(AttackPram attackPram)
    {
        var Part=new AttackPart(gameObject, attackPram);
        AddAtackPart(Part);
        ontriger();
        keikei.delaycall(()=>offtrigerName(Part.name),attackPram.duration);
   
    }

    public void offtriger()
    {
        CurrentAttackPart.ColliderSet(false);
    }


    public bool attackon(
        GameObject attacker,
        GameObject attackedObj,
       DamageInfo damageInfos
     
    )
    {
        if (hpcore?.HP < 0) return false;
        
        DamageInfo damageInfo = damageInfos.ShallowCopy();
        var crit = Random.value <= CritRate;

        damageInfo.damagValue = crit == true ? (int)(damageInfo.damagValue * CritMultiplier) : damageInfo.damagValue;
        damageInfo.damagValue += basedamagevalue;
        damageInfo.forceValue += baseforcepower;
        if (overrideTag) damageInfo.attackableTag = AttackableTag;

        var damadable = attackedObj.Damage(damageInfo, crit, attacker);

    

        return damadable;
    }
}
