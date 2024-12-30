using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using DG.Tweening;

public class HissatuWazaMpSingle : SelectBehabior<HissatuWazaData>,UIUpdateable
{

    public bool isUIUpdateable { get ; set; } = true;
    public List<Text> texts;
    public Animator anim;

    public hpcore hpcore;
    public mp mp;
    public DelayEvent changeEvent;
    public DelayEvent decideEvent;

    charactorchange charactorchange;

    void Start()
    {
        hpcore = GetComponent<hpcore>();
        charactorchange = GetComponent<charactorchange>();
        charactorchange.ChangeEvents += () =>
        {
            Elements = charactorchange.CurrentElement.CombatCharactor.hissatuWazaDatas;

        };

    }
    public void UIUpdate()
    {
        int i = 1;
        bool foundCurrent = false;

          if (mp.MP < CurrentElement.mpUseAmount)
        {
         texts[0].color = Color.black;   
    
        }else
        {
            texts[0].color = Color.white; 
           }

        foreach (var item in Elements)
        {
            if (CurrentElement?.name == item.name)
            {
                texts[0].text = item.name;
                foundCurrent = true;
            }
            else
            {
                if (i >= texts.Count)
                {
                    // texts配列のサイズを超えないようにするための対策
                    Debug.LogWarning("texts配列のサイズがElementsのサイズより小さいです。");
                    return;
                }

                texts[i].text = item.name;
                i++;
            }
        }

        if (!foundCurrent)
        {
            Debug.LogWarning("CurrentElementがElementsに存在しません。");
        }
    }

    public override void ChangeCallBack()
    {
       if (changeEvent.events == null)
       {
        return;
       }
        changeEvent?.Execute();


    }
   Vector3 startPos;
   public override void UpdateCallBack()
    {
        if (isUIUpdateable)UIUpdate();
        
      
    }

float cooltime=1;
    float lastTime;
    public override void DecideEvent()
    {
        if (hpcore.HP <= 0 || !anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle") || !(Time.time - lastTime > cooltime))
        {
            return;
        }
        lastTime = Time.time;


        if (mp.mpuse(CurrentElement.mpUseAmount))
        {
            anim.Play(CurrentElement.AnimName);
            decideEvent.Execute();
        }

    }





}