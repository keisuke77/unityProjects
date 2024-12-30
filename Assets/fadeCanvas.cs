using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeCanvas : MonoBehaviour
{ 
   public CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        if (canvasGroup==null)
        {
             canvasGroup=GetComponent<CanvasGroup>();
 
        }
          }
public void FadeOut(float duration){
    canvasGroup.FadeOut(duration);
}
 
public void FadeIn(float duration){
    canvasGroup.FadeIn(duration);
}
 
}
