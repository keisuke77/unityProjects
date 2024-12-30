using UnityEngine;
using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
public class CameraPramManager : MonoBehaviour {
    public List<CameraManager.Parameter> Params;
 
    [SerializeField]
    CameraManager _CameraManager;

    public float TweenDuration=1;

    [Button( "Execute", "実行",0 )]
    public int ResetButton1;   
 
    

public void Execute(int num){
    _CameraManager.TweenPram(Params[num],TweenDuration);
}


    


}