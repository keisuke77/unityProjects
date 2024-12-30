using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using UnityEngine.Events;

using Photon.Pun;
using Cysharp.Threading.Tasks;  // UniTaskのためのnamespaceを追加



[System.Serializable]
public class basehp : MonoBehaviourPunCallbacks, IPunObservable
{
    public int maxHP = 100;
    public int HP = 100;
    public Text hptext;
    public Image hpImage;
    public Slider hpslider;
    public bool deathUIdestroy;
    public System.Action deathEvent;
    public bool DeathOnce;

    public bool DebugHPMugen;

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 自身のアバターのスタミナを送信する
            stream.SendNext(HP);
        }
        else
        {
            // 他プレイヤーのアバターのスタミナを受信する
            HP = (int)stream.ReceiveNext();
        }
    }
    int previousHP;
    private void Start()
    {
        previousHP = HP;
    }
    public void Update()
    {

        if (HP == 0)
        {
            gameObject.Stop();
        }

        if (previousHP == HP)
            return;

        HP = Mathf.Clamp(HP, 0, maxHP);
        if (hpImage != null)
        {
            float targetFillAmount = (float)HP / maxHP;
            // 変化量に応じたアニメーション時間を調整
            hpImage.DOFillAmount(targetFillAmount, 0.5f).SetEase(Ease.OutQuad);
            // 振動させる処理
        }
        if (hptext != null)
        {
            hptext.text = name + maxHP + "/" + HP;
        }


        if (hpslider != null)
        {
            hpslider.maxValue = maxHP;
            float valueDifference = Mathf.Abs(HP - previousHP);
            float animationDuration = Mathf.Clamp(valueDifference / maxHP, 0.4f, 1f); // 変化量に応じたアニメーション時間を調整
            hpslider.DOValue(HP, animationDuration).SetEase(Ease.OutQuad);
        }

        if (HP <= 0 && !DeathOnce)
        {
            DeathOnce = true;
            death();
        }
        previousHP = HP;
    }

    // HPが変更されたときのイベントを定義 
    public event Action<int> OnHpChanged;

    public void damage(int amount)
    {

        if (!DebugHPMugen && amount > 0)
        {
            HP = HP - amount;
            OnHpChanged?.Invoke(HP); // HPが変更されたのでイベントを呼び出す
        }
    }

    public void heal(int amount)
    {
        HP = HP + amount;
        
        OnHpChanged?.Invoke(HP); // HPが変更されたのでイベントを呼び出す
        
    }
    public void death()
    {
        if (deathUIdestroy)
        {
            if (hpslider)
                Destroy(hpslider.gameObject);
            if (hpImage)
                Destroy(hpImage.gameObject);

            if (hptext)
                Destroy(hptext.gameObject);
        }
        deathEvent();
    }
}
public class hpcore : basehp
{
    public ShakeParameters shakeParameters;
    public DamageAnimData damageAnimData;
    public Animator anim;
    public GameObject damageTextPrefab;
    public Transform damageTextPos;
    FlickerModel FlickerModel;
    public bool DamagedRedMesh = true;
    public bool nodamage = false; public GameObject killer;
    public EffectAndParticle HitEffect;
    public int defence;
    public int defaultdefencepower;

    public List<DelayEvent> damageEvent;
    public bool AttackerLook = true;
    [Range(0, 20)]

    public GameObject healparticle;

    public bool deathonce;
    public UnityEvent deathEvent;
    public EffectAndParticle deatheffect;
    public CameraManager.Parameter deathCameraPram;
    public bool damageshake;

    public ChatDataAction chatDataAction;
    public Action noHitAction;
    public Action HpHealAction;
    public Action<int> damageCallback;
    public Action deathCallback;

    public void muteki(float time, System.Action ac = null)
    {
        nodamage = true;
        keikei.delaycall(
            () =>
            {
                nodamage = false;
                ac();
            },
            time
        );
    }

    void Awake()
    {
        base.deathEvent = () => Death();
        anim = GetComponent<Animator>();
        if (DamagedRedMesh)
        {
            FlickerModel = gameObject.AddComponentIfnull<FlickerModel>() as FlickerModel;
        }
        defence = defaultdefencepower;
        if (damageTextPrefab == null)
        {
            damageTextPrefab = (GameObject)Resources.Load("DamagePopup");
        }

        SetUp();
    }

    public virtual void SetUp() { }

    public virtual void hpheal(int amount)
    {
        base.heal(amount);
        OnDamageText("かいふく");

        HpHealAction();
        if (healparticle)
        {
            var obj = Instantiate(healparticle, transform.position, Quaternion.identity) as GameObject;
            obj.transform.parent = transform;
        }
    }

    public bool damage(int damage, bool crit, GameObject col)
    {
        return this.damage(damage, false, col.Collider());
    }

    public void OnDamageText(string st, bool crit = false)
    {

        Vector3 pos = damageTextPos ? damageTextPos.position : transform.position;

        pos = pos + new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));

        if (damageTextPrefab != null)
        {
            GameObject dmgText = Instantiate(
                damageTextPrefab, pos
             ,
                Quaternion.identity
            );
            dmgText.GetComponent<DamagePopup>().SetUp(st);

            if (crit)
            {
                dmgText.transform.localScale *= 1.5f;
                if (dmgText.GetComponent<TextMesh>() != null)
                {
                    dmgText.GetComponent<TextMesh>().color = Color.red;

                }
            }
        }
    }
    public System.Action CounterCallBack;

    public bool isGuarding;
    public bool damage(
        int damage,
        bool crit = false,
        Collider attackedColl = null,
        GameObject killers = null
    )
    {
        if (HP == 0 || anim.GetBool("dead") || nodamage || damage == 0)
        {
            if (noHitAction != null)
                noHitAction();
            return false;
        }


        if (CounterCallBack != null)
        {
            CounterCallBack();
            CounterCallBack = null;
            return false;
        }
        // float stopTime=0.5f;
        // ヒットストップを実装してアニメーションスピード０にする
        // if (anim != null)
        // {
        //     anim.speed = 1;
        //     DOVirtual.DelayedCall(0.1f, () => anim.speed = 0); // 遅延を入れてアニメーションスピードを0にする
        //     // 一定時間後にアニメーションスピードを元に戻す
        //     DOVirtual.DelayedCall(stopTime, () => anim.speed = 1);
        // }

        // if (killer != null)
        // {
        //     Animator killerAnim = killer.GetComponent<Animator>();
        //     if (killerAnim != null)
        //     {
        //         DOVirtual.DelayedCall(0.1f, () => killerAnim.speed = 0); // 遅延を入れて攻撃者のアニメーションスピードを0にする
        //         // 一定時間後にアニメーションスピードを元に戻す
        //         DOVirtual.DelayedCall(stopTime, () => killerAnim.speed = 1);
        //     }
        // }


        killer = killers;

        damage -= defence;
        damage = Mathf.Clamp(damage, 1, 9999);
        base.damage(damage);

        if (isGuarding)
        {
            damage = 1;
            OnDamageText("ガード！");

        }

        if (damage > 0)
        {
            if (anim != null && HP > 0 && !isGuarding)
            {

                anim.SetFloat("hp", HP);
                anim.SetTrigger("damage");
                anim.SetInteger("damagevalue", damage);
                if (damageAnimData != null)
                {
                    damageAnimData.Execute(anim, damage);
                }

            }

            HitEffect?.Execute(killer ? killer.transform : transform, attackedColl);
            GetComponent<attackcore>()?.allofftriger();


            if (damageCallback != null)
            {
                damageCallback(damage);
            }
            if (damageEvent != null)
            {
                damageEvent.ForEach(x => x.Execute());
            }




        }


        if (AttackerLook)
        {
            if (killer != null)
            {
                transform.LookAt(killer.transform, Vector3.up);
            }

        }
        if (damageshake)
        {
            CameraShake.TriggerShake(shakeParameters);

        }


        if (FlickerModel != null)
        {

            FlickerModel?.damagecolor();
        }

        OnDamageText(damage.ToString());

        OnDamage(damage);
        return true;
    }

    public virtual void OnDamage(int damage) { }
    public bool DeathTransformTweenStop;
    public void Death()
    {
        if (deathonce) return;

        deathonce = true;

        if (deathCameraPram != null && CameraManager.instance != null)
        {
            CameraManager.instance.TweenPram(deathCameraPram);

        }

        gameObject.Stop();
        gameObject.root().tag = "dead";

        if (anim != null)
        {
            anim.SetBool("death", true);
            anim.SetBool("dead", true);
            anim.SetTrigger("death");
            anim.SetTrigger("dead");
        }
        if (deathEvent != null)
        {
            deathEvent.Invoke();
        }
        if (deathCallback != null)
        {
            deathCallback();
        }
        if (TryGetComponent(out attackcore attackcore))
        {
            attackcore.allofftriger();
        }
        //武器のコライダを切る
        if (deatheffect != null)
        {
            deatheffect.Execute(transform, null);

        }


        if (chatDataAction != null)
        {
            chatDataAction.EndEvent.AddListener(OnDeath);
            chatDataAction.Play();

        }


    }

    public virtual void OnDeath() { }

    public virtual void damagestop() { }

    public virtual void recover() { }

    // Update is called once per frame

}
