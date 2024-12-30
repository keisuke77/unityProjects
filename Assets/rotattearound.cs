using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;







public class rotattearound : MonoBehaviour
{
    public int duration=1;
    public int rotateAngle=90;
    public Vector3 rotateAxis=Vector3.up;
    public Transform Target;

    // Start is called before the first frame update
  


public bool Tweening;

float prevVal;		// 前回の角度


public Tween DoRotateAround(float endValue, float durations)
{
    if (Tweening)
    {
        return null;
    }
    Tweening=true;
    prevVal = 0.0f;
    
    // durationの時間で値を0～endValueまで変更させて公転処理を呼ぶ
    Tween ret = DOTween.To(x => RotateAroundPrc(x), 0.0f, endValue, durations).OnComplete(()=>Tweening=false);

    return ret;
}

/// <summary>
/// 公転処理
/// </summary>
/// <param name="value"></param>
private void RotateAroundPrc(float value)
{
    // 前回との差分を計算
    float delta = value - prevVal;
    
    // Y軸周りに公転運動
    transform.RotateAround(Target.position, rotateAxis, delta);
    
    // 前回の角度を更新
    prevVal = value;
}


private void Start() {
    rotateon();
}

public void rotateon(){
    DoRotateAround(rotateAngle,duration);
}
public void rotateoff(){
 DoRotateAround(-rotateAngle,duration);
  }
 
}
