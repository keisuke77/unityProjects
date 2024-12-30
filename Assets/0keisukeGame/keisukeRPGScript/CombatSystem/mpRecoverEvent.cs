using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mpRecoverEvent : MonoBehaviour, ICharaResist
{

    public mp mp;

    public List<hpcore> enemyhps, playerhps=new List<hpcore>();
   

    public float mpRecoverPer = 1;

    public void MPrecover(int num)
    {
        mp.Heal(num * mpRecoverPer);
    }
    public void Resist()
    {
      

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (var obj in enemyObjects)
        {
            hpcore hpCore = obj.GetComponent<hpcore>();
            if (hpCore != null)
            {
                enemyhps.Add(hpCore);
            }
        }
        foreach (var obj in playerObjects)
        {
            hpcore hpCore = obj.GetComponent<hpcore>();
            if (hpCore != null)
            {
                playerhps.Add(hpCore);
            }
        }
           enemyhps?.ForEach(hp => hp.damageCallback += MPrecover);
           playerhps?.ForEach(hp => hp.deathCallback += () => MPrecover(30));
 

    }
  

    }

