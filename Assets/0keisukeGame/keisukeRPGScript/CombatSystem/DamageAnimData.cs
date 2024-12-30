using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="DamageAnimData",menuName="ScriptableObjects/DamageAnim")]
public class DamageAnimData : ScriptableObject
{
    public List<DamageAnim> damageAnims;

    public void Execute(Animator anim,float damage){
        foreach (var item in damageAnims)
{
   
    if (item.mindamage<damage&&item.maxdamage>=damage)
    {
        //アニメーション止まるバグ起こるからいじるな！
        anim.CrossFadeAnimation(item.name,5);

    }
}
    }
}

[System.Serializable]
public struct DamageAnim
{[Range(0,100)]
    public int mindamage,maxdamage;

    public string name;


}
