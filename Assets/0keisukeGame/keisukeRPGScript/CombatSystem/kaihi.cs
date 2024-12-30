using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using System.Linq;
public class kaihi : MonoBehaviour, Inputable, ICharaResist
{

    // Start is called before the first frame update
    public bool isInputable { get; set; } = true;
    public hpcore hpcore;
    public mp mp;
    public controll input;
    [Range(0, 2)]
    public float kaihiCoolDown, kaihiDuration;
    float time;
    public Animator anim;
    [Range(0, 1)] public float useDashGage;
    Tween kaihiTween;
    public bool autoKaihi;
    public bool Guard;
    public float kaihiMove;
    void KaihiSuccees()
    {
        kaihiTween.Kill();
        hpcore.OnDamageText("かいひ！");
        hpcore.noHitAction -= KaihiSuccees;
        keikei.delaycall(() => hpcore.nodamage = false, 1);
    }

    public void kaihiExecute(float duration = 0.5f)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) return;
        if (!(time > kaihiCoolDown)) return;
        if (!mp.mpuse(useDashGage * mp.maxMP)) return;

        time = 0;
        if (Guard)
        {
            hpcore.isGuarding = true;
            keikei.delaycall(() => hpcore.isGuarding = false, 2);
        }
        else
        {
            transform.DOLocalMove(transform.forward*kaihiMove,0.5f).SetRelative(true);
            hpcore.nodamage = true;
            hpcore.noHitAction += KaihiSuccees;
            kaihiTween = keikei.delaycall(() =>
            {
                hpcore.nodamage = false;
                hpcore.noHitAction -= KaihiSuccees;
            }, duration);
        }

        //今はかいひもがーども同じアニメーションステート
        anim.SetTrigger("kaihi");


    }

    public void Resist()
    {
        var obj = GameObject.FindGameObjectsWithTag("Enemy");
        obj.Where(n => n.root()).Distinct();
        foreach (var item in obj)
        {
            if (item.TryGetComponent(out attackcore attackcore))
            {
                attackcore.TriggerOnCallBack += NearAttackerApper;
                Debug.Log("kaihi resist");

            }
        }

    }

    public void NearAttackerApper(GameObject attacker)
    {
        if (!autoKaihi) return;
        float distance = Vector3.Distance(attacker.transform.position, transform.position);
        if (distance < 4)
        {
            kaihiExecute(kaihiDuration);

        }

    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (keiinput.Instance.GetKey(input) && isInputable)
        {
            kaihiExecute(kaihiDuration);
        }
    }
}
