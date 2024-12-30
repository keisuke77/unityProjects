using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;
using UnityEngine.Events;
public enum SequenceMethod
{
    Append,
    Join
}

[System.Serializable]
public class DoTweenSeri
{
    public bool TweeningPlayable;
    [System.Serializable]
    public class DOTweenSequence
    {
        public SequenceMethod SequenceMethod;

        public float Speed=1;
        public DOTweenScriptable DOTweenScriptable;
        public UnityEvent Event;
    }
 public UnityEvent StartEvent;
    public List<DOTweenSequence> DOTweenSequences;

    Sequence BeforeSequence;

    public Sequence Play(Transform tra)
    {
      if (BeforeSequence!=null)
        if (BeforeSequence.IsPlaying()&&!TweeningPlayable)
        {
            return null;
        }
         BeforeSequence = DOTween.Sequence();
        foreach (var item in DOTweenSequences)
        {
            Sequence temp = item.DOTweenScriptable.GetSequence(tra);
            switch (item.SequenceMethod)
            {
                case SequenceMethod.Append:
                    BeforeSequence.Append(temp);
                    break;

                case SequenceMethod.Join:
                    BeforeSequence.Join(temp);
                    break;
                default:
break;
            }
              BeforeSequence.AppendCallback(() =>
    {
       item.Event.Invoke();
    }); 
    
    BeforeSequence.timeScale= item.Speed;  
  
        }
        StartEvent.Invoke();
          return BeforeSequence;
    }
}
