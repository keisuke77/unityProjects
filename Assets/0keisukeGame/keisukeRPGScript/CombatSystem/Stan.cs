using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stan : MonoBehaviour
{
    public float StanTime;
    public Animator animator;
    public GameObject StanEffect;

    private bool stop;
    private bool isStunned;
    private Coroutine stanCoroutine;

    public void Stop() => stop = true;
    public void ReStart() => stop = false;

    public void StanStart()
    {
        if (stop || isStunned)
        {
            return;
        }

        stanCoroutine = StartCoroutine(StanCoroutine());
    }

    IEnumerator StanCoroutine()
    {
        isStunned = true;
        Debug.Log("StanStart");
        animator.SetBool("Stan", true);

        GameObject effect = null;
        // gameobjectの頭上にエフェクト表示する
        if (StanEffect != null)
        {
            effect = Instantiate(StanEffect, gameObject.GetHeadPosition(), Quaternion.identity, transform);
        }

        yield return new WaitForSeconds(StanTime);
        Debug.Log("StanEnd");
        animator.SetBool("Stan", false);
        Destroy(effect);
        isStunned = false;
    }
}