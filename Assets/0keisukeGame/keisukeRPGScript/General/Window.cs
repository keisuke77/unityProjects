using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class Window
{
   Canvas canvas;
   public float delay;
   public float FadeSpeed = 1;
   public CanvasGroup canvasGroup;
   public List<Window> nextWindows;

   public DelayEvents OnEvent, OffEvent;
   [HideInInspector]
   public Window preWindow;
   public Window(GameObject Object)
   {

   }
   public void Off()
   {
      delays?.Kill();
      if(canvas!=null)
      canvas.enabled = false;
      if (canvasGroup != null){
            canvasGroup.gameObject.SetActive(false);
      canvasGroup.FadeOut(0.1f);
      }
   
      OffEvent?.Execute();
   }
   Tween delays;
   public void On()
   {
      OnEvent?.Execute();
      delays = keikei.delaycall(() =>
      {

         if (canvasGroup != null)
         {

            canvasGroup.FadeIn(FadeSpeed);
            if(canvas!=null)
            canvas.enabled = true;
            canvasGroup.gameObject.SetActive(true);
         }
      }, delay);
   }
   public void Setup(Window _preWindow = null)
   {
      if (canvasGroup != null)
      {
         canvas = canvasGroup.gameObject.GetComponent<Canvas>();
      }
      if (_preWindow != null)
      {
         preWindow = _preWindow;
         preWindow.Off();
      }

      On();
   }
   public void Back(ref Window currentWindow)
   {
      if (preWindow != null)
      {
         Off();
         preWindow.On();

      }
      currentWindow = preWindow;
   }

}
