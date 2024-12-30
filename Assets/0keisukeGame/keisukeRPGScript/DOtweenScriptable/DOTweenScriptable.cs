using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[CreateAssetMenu(fileName = "DOTweenScriptable", menuName = "")]
public class DOTweenScriptable : ScriptableObject
{
   
    public DiretionEnum Diretion;
    public float DiretionPower;
    public Vector3 move;
    public Vector3 rot;
    public Vector3 Scale;
    public float JumpPower = 0;
    public float speed;
    public LoopType looptype;
    public int loopcount;
    public AnimationCurve Curve; 

    /**< Tweenのイージングのカーブ*/



    public Sequence GetSequence(Transform tra)
    {
        return GetTweernr(tra);
      
    }

    public Sequence GetTweernr(Transform tra)
    {
        
        Sequence Sequence = DOTween.Sequence();
       
                    
if (move!=Vector3.zero||DiretionPower!=0)
{
            Sequence.Join(
                tra.DOLocalMove((move+(tra.GetDirection(Diretion)*DiretionPower))*WorldInfo.scale, speed)
                    .SetRelative(true)
                    .SetLoops(loopcount, looptype)
                    .SetEase(Curve)
            );
}

if (rot!=Vector3.zero)
{
       Sequence.Join(
                tra.DORotate(rot, speed)
                    .SetRelative(true)
                    .SetLoops(loopcount, looptype)
                    .SetEase(Curve)
            );
}
         if (Scale!=Vector3.zero)
         {
              Sequence.Join(
                tra.DOScale(Scale, speed)
                    .SetRelative(true)
                    .SetLoops(loopcount, looptype)
                    .SetEase(Curve)
            );
         }
          
        
        return Sequence;
    }
}
