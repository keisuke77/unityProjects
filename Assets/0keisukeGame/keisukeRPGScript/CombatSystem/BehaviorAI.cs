using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BehaviorAI : MonoBehaviour, ICharaResist
{
    // Declare any variables or properties here
    public navchaise navchaise;

    public float interval=0.1f;
    public mp attckmp;

    Animator anim;

    public List<GameObject> targets;

    public void Resist()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy").ToList();
    }

    public BehaviorState GetBestBehavior()
    {
        //最適な行動を返す
        //敵がいなかったら攻撃しない

     
        if (targets.Count == 0)
        {
            return BehaviorState.Patrol;
        }
        //mpが足りなかったら攻撃できないので逃げる
        if (attckmp.GetMpPercent() < 0.8f)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                return BehaviorState.Stop;
            }else
            {
                 return BehaviorState.Escape;
            }
           
        }
        else
        {
            //mpが足りていたら攻撃する

            //10m以内の敵だけ抽出
           targets.Where(n => Vector3.Distance(n.transform.position, transform.position) < 10);
            if (targets.Count == 0)
            {
                return BehaviorState.Melee;
            }
            //アタック中の敵がいたら逃げる
            if (targets.Exists(n => n.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Attack")))
            {
                return BehaviorState.Escape;
            }
            else
            {
                //アタック中の敵がいなかったら攻撃する
                return BehaviorState.Melee;

            }
        }


    }

IEnumerator UpdateBehavior()
{
    while (true)
    {
        navchaise.ChangeBehavior(GetBestBehavior());
        yield return new WaitForSeconds(interval);
    }
}
void Awake()
{
    navchaise.UseData=false;
    anim=GetComponent<Animator>();
    StartCoroutine(UpdateBehavior());

}



    // Add any other methods or event handlers here
}