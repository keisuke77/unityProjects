using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.VFX;
public class VFXAnim : MonoBehaviour
{[Header("VFXPlay()で実行")]
    public VisualEffect[] VisualEffects;
    // Start is called before the first frame update
   public void VFXPlay(int n=0)
    {
        VisualEffects[n].Play();
    }

 
}
