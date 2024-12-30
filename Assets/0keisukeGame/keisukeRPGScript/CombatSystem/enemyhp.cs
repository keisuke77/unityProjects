using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(FlickerModel))]
[RequireComponent(typeof(Animator))]
public class enemyhp : hpcore
{
  
    public UnityEvent events; 
    public System.Action VanishEvent;
    


    public override void OnDamage(int damage)
    {

    }

    public override void damagestop()
    {
        if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
        {
            gameObject.GetComponentIfNotNull<UnityEngine.AI.NavMeshAgent>().enabled = false;
        }
    }

    public override void recover()
    {
        if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
        {
            gameObject.GetComponentIfNotNull<UnityEngine.AI.NavMeshAgent>().enabled = true;
        }
    }



    public override void OnDeath()
    {

        if (anim != null)
        {
            keikei.delaycall(() => deathend(), 6f);
        }
        else
        {
            deathend();
        }

    }

    //アニメーションコントローラから実行

    public void deathend()
    {


        VanishEvent += () =>
        {

            events.Invoke();
        };
    }
}
