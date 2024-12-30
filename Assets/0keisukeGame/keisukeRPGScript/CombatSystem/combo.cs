using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class combo : MonoBehaviour, IMove, Inputable
{
   
    public bool Stop { get; set; }

    public bool isInputable { get; set; } = true;

    [Header("コンボ進行必要なタイミング")]
    [Range(0, 1)]
    public float ProgressTimePercent = 0.7f;
    public float comboCompletedCooldown = 2.0f;
    public float comboTimeout = 1.0f;
    public float longPressInterval = 1.0f;
    public List<string> comboAnimations;
    private float buttonPressTime;
    private bool attackReserved = false;

    private bool isAttacking = false;
    public int comboCount = 0;
    private float lastAttackTime = 0f;
    [Range(0, 100)]
    public int CrossFadeSmoothLevel, CrossFadeSmoothLevelBack;

    [Range(0, 10)]
    public float AnimSpeed;

    private Animator animator;

    public int MaxComboCount;
    public mp mp;
    public float MPUseMount;
    public float CollideCheckDistance = 1f;
    public System.Action ComnoEndCallBack;
    public bool progressUseMp;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    bool AnimAttack;
    void Update()
    {
         
        if (IsCooldownOver() && attackReserved)
        {
            attackReserved = false;
            ProgressCombo();
        }
        HandleAttack();

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (!AnimAttack)
            {
                gameObject.Stop();
            }
            AnimAttack = true;
        }else
        {
            if (AnimAttack)
            {
                gameObject.Restart();
            }
            AnimAttack = false;
        }
    }


    void HandleAttack()
    {
        if (isAttacking) return;
        bool isSinglePress;
        bool isLongPressing;
        if (isInputable)
        {
            isSinglePress = keiinput.Instance.attack;
            isLongPressing = keiinput.Instance.attackduring;

        }
        else
        {
            isSinglePress = false;
            isLongPressing = false;
        }


        if (isSinglePress)
        {

            if (IsCooldownOver())
            {
                ProgressCombo();
            }
            else
            {
                attackReserved = true;
            }
        }
        else if (isLongPressing)
        {
            buttonPressTime += Time.deltaTime;
            if (buttonPressTime >= longPressInterval && IsCooldownOver())
            {
                ProgressCombo();
                buttonPressTime = 0f; // ここでリセットして、次の長押しコンボまでの間隔を計測します
            }
        }

        if (comboCount > 0 && Time.time - lastAttackTime > comboTimeout)
        {
            ComnoEnd();
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                animator.CrossFadeAnimation(DefaultState, CrossFadeSmoothLevelBack);
            }

        }


        //例外処理
        if (comboCount == 0 && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {

            animator.CrossFadeAnimation(DefaultState, CrossFadeSmoothLevelBack);

        }
    }

    public string DefaultState = "Move";

    bool IsCooldownOver()
    {
        float currentCooldown = (comboCount == MaxComboCount) ? comboCompletedCooldown : animator.GetCurrentClipFromMostWeightedLayer().length * ProgressTimePercent;
        bool cooldownOver = Time.time - lastAttackTime > currentCooldown;
        return cooldownOver;
    } // ダメージを受けた場合
    public bool noAttackable()
    {
        return (Stop && !nowCombo) || animator.GetBool("damage")|| !(animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")||animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) ;
    }
    bool nowCombo = false;
    public void ComnoEnd()
    {
        comboCount = 0; // コンボをリセット
        nowCombo = false;
    }
    void ProgressCombo()
    {

        if (noAttackable())
        {
            ComnoEnd();
            return; // 以降のコンボ進行処理をスキップ
        }


        if (progressUseMp)
        {

            if (!mp.mpuse(MPUseMount))
            {
                return;
            }
        }
        else if (comboCount == 0)
        {

            if (!mp.mpuse(MPUseMount))
            {
                return;
            }

        }



        comboCount++;
        if (comboCount > MaxComboCount)
        {
            comboCount = 1;
            attackReserved = false;
            return;

        }
        nowCombo = true;
        StartAttack(comboCount);

    }

    public bool AutoAim=false;
    void StartAttack(int attackLevel)
    {

        isAttacking = true;
        lastAttackTime = Time.time;
        Vector3 direction=Vector3.zero;
        GameObject attackTarget=gameObject.NearSearchTagNotDeath("Enemy");
        if (AutoAim&&attackTarget!=null)
        {
            //敵の方向取得
             direction= (attackTarget.transform.position - transform.position).normalized;
        }else
        {
             direction = transform.CameraDirection(Camera.main, keiinput.Instance.directionkey).normalized;
      
        }
         if (direction.magnitude > 0.2f&&transform.CollideCheck(direction, CollideCheckDistance*WorldInfo.scale))
        {
            transform.DOLocalMove(direction * WorldInfo.scale, 0.2f).SetRelative(true);
            transform.DORotate(new Vector3(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0), 0.2f);
        }
        if (attackLevel <= comboAnimations.Count)
        {
            animator.SetFloat("AnimSpeed", AnimSpeed);

            animator.CrossFadeAnimation(comboAnimations[attackLevel - 1], CrossFadeSmoothLevel);
        }

        isAttacking = false;
    }
    private Coroutine executeAllCombosCoroutine;

    public void AutoAllCombo()
    {
        if (executeAllCombosCoroutine == null)
        {
            executeAllCombosCoroutine = StartCoroutine(ExecuteAllCombos());
        }
    }

    private IEnumerator ExecuteAllCombos()
    {
        for (int i = 0; i < MaxComboCount; i++)
        {
            ProgressCombo(); // コンボを進行させる
            yield return new WaitForSeconds(0.3f); // 次のコンボまでの遅延
           //if (Vector3.Distance(transform.position, gameObject.NearSearchTag("Enemy").transform.position) > 6f)
           //{
           //   executeAllCombosCoroutine = null;
           //    yield break; // コルーチンを終了
           //}
            yield return new WaitForSeconds(animator.GetCurrentClipFromMostWeightedLayer().length - 0.3f); // 次のコンボまでの遅延


        }
        executeAllCombosCoroutine = null;
    }
}
