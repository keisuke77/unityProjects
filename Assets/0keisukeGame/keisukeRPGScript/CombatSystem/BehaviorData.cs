using System.Collections.Generic;
using UnityEngine;
public enum BehaviorState
{
    Escape, Melee, Stop, Patrol
}

[CreateAssetMenu(fileName = "BehaviorData", menuName = "BehaviorData")]
public class BehaviorData : ScriptableObject
{
    public List<BehaviorRule> behaviors;

 
    // 特定のHPと距離に基づいて、適切なBehaviorStateを決定する
    
}
[System.Serializable]
public class BehaviorSelector{
    public BehaviorData behaviorData;
    public List<float> BeforeTimes=new List<float>{0,0,0,0,0,0,0};

   public BehaviorState DetermineBehavior(float currentHP, float distance, BehaviorState currentBehavior)
{
    if (BeforeTimes.Count==0)
    {
         BeforeTimes=new List<float>{0,0,0,0,0,0,0};
    }
    int i = -1;
    foreach (var rule in behaviorData.behaviors)
    {
        i++;
        
        // BeforeTime[i] をローカル変数にコピー
        float beforeTime = BeforeTimes[i];
        
        // ローカル変数を ref で渡す
        if (rule.Check(currentHP, distance, ref beforeTime))
        {
            Debug.Log("abcd");
            currentBehavior = rule.DetermineBehaviorPer(currentBehavior);
            
            // 変更された beforeTime をリストに戻す
            BeforeTimes[i] = beforeTime;
            
            return currentBehavior;
        }
    }
    return currentBehavior; // 変化なし
}

}


[System.Serializable]
public class BehaviorPer
{
 [Range(0, 100)]    public float kakuritu;
    public BehaviorState behaviorState;
}

[System.Serializable]
public class BehaviorRule
{
    public float MinHP, MaxHp;
    public float MinDis, MaxDis;
    public List<BehaviorPer> behaviorPers;

    public float interval=1;
public bool Check(float currentHP, float distance,ref float BeforeTime){


     if (Mathf.Abs(  Time.time-BeforeTime )>interval&&currentHP >= MinHP && currentHP <= MaxHp && distance >= MinDis && distance <= MaxDis)
            {
                BeforeTime=Time.time;
                return true;
            }
            return false;

}
    // BehaviorPerリストから確率に基づいてBehaviorStateを決定する
    public BehaviorState DetermineBehaviorPer(BehaviorState temp)
    {
        float total = 0;
        foreach (var per in behaviorPers)
        {
            total += per.kakuritu;
        }
        float randomPoint = Random.Range(0, total);
        foreach (var per in behaviorPers)
        {
            if (randomPoint < per.kakuritu)
            {
                return per.behaviorState;
            }
            else
            {
                randomPoint -= per.kakuritu;
            }
        }

        return temp; // デフォルトの行動
    }
}
