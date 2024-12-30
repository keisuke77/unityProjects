using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public class AxisValue
{
   public controll axis;
   [Range(-1, 1)]
   public float axisValue;

   public bool Sequence;
   bool on;
   public bool Execute()
   {
      if (axis == controll.none)
      {
         return false;
      }
      if (!Sequence)
      {
         if (axisValue == (float)keiinput.Instance.GetAxis(axis))
         {
            return true;
         }
         else
         {
            return false;
         }
      }
      else
      {

         if (axisValue == (float)keiinput.Instance.GetAxis(axis) && on)
         {
            on = false;
            return true;
         }
         else if ((float)keiinput.Instance.GetAxis(axis) == 0 && !on)
         {
            on = true;
            return false;
         }

      }

      return false;
   }
}
public class DoTween : MonoBehaviour
{
   public bool enablePlay;
   public bool disablePlay;
   public DoTweenSeri DoTweenSeri;
   public Sequence seq;
   public KeyCode PlayButtton;

   public AxisValue PlayAxis;

   [Button("Play", "Play")]
   public int a;
   public void Play()
   {
      seq = DoTweenSeri.Play(transform);
   }
   private void Awake()
   {
      if (enablePlay && !seq.IsActive())
      {
         Play();

      }
   }
   Sequence sequence;
   
   void Raycast()
   {

      if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.3f))
      {
         Debug.Log("CollieeTweening" + gameObject.name);

         if (HitEffect != null )
         {
            sequence.Kill();
            HitEffect.Execute(hit.point);
         }
      }
   }

   public EffectAndParticle HitEffect;


   public void DOPlay(DOTweenScriptable dOTweenScriptable)
   {
    
      sequence = dOTweenScriptable.GetSequence(transform).Play();
      sequence.onUpdate += Raycast;
   }
   private void OnEnable()
   {
      if (enablePlay && !seq.IsActive())
      {
         Play();

      }
   }
   void OnDisable()
   {
      if (disablePlay && !seq.IsActive())
      {
         Play();

      }
   }
   public void Pause()
   {

      seq.TogglePause();
   }
   public void ReStart()
   {
      // トゥイーンの再開
      seq.Play();
   }
   private void Update()
   {
      if (PlayButtton.keydown())
      {
         Play();
      }

      if (PlayAxis.Execute())
      {
         Play();
      }
   }
   public void Repead(int num)
   {
      seq.timeScale = num;
   }
}
