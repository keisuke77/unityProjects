
 
using UnityEngine;
using System.Collections;
 
public class movelist: MonoBehaviour {
 
 public string animname;
 public Animator anim;
	[SerializeField] private Transform[] target;	//　カメラの到達点
	[SerializeField] private float waittime=1;	//　カメラの移動速度
	[SerializeField] private float duration;	
    public bool movestart;	
    public float rotatespeed;	//　カメラの背景の色
    Transform trans;
    public int num=0;
	private Vector3 moveVelocity;			//　現在の移動の速度
	private float xVelocity;			//　現在の回転の速度
	private float yVelocity;			//　現在の回転の速度
	private float zVelocity;			//　現在の回転の速度
	private Vector3 allRotateVelocity;		//　全ての角度を変える場合に使用する速度
	private float startTime;	
    Transform currenttarget;
    		//　開始時間
 
	void Start () {
		trans=transform;
		anim=GetComponent<Animator>();
currenttarget=target[num];
	}


public void GotoNextPoint(){
num++;
num=num%target.Length;
currenttarget=target[num];
movestart=true;

}
public void changetarget(Transform Target){



startTime = Time.time;
currenttarget=Target;

movestart=true;
}


	void Update () {
		if (!movestart)
		{
startTime = Time.time;
			return;
		}else if((trans.position-currenttarget.position).sqrMagnitude<1f)
		{
			movestart=false;
            Invoke("GotoNextPoint",waittime);
			
		}
anim.SetBool(animname,movestart);
		//　位置をスムーズに動かす
//		trans.position = Vector3.SmoothDamp (trans.position, currenttarget.position, ref moveVelocity, moveSpeed * Time.deltaTime, maxSpeed);
		//　位置をスムーズに動かすSmoothStep版
		var t = (Time.time - startTime) / duration;
		var xPos = Mathf.SmoothStep (trans.position.x, currenttarget.position.x, t);
		var yPos = Mathf.SmoothStep (trans.position.y, currenttarget.position.y, t);
		var zPos = Mathf.SmoothStep (trans.position.z, currenttarget.position.z, t);
		trans.position = new Vector3(xPos, yPos, zPos);
        trans.rotation = Quaternion.Slerp(trans.rotation, Quaternion.LookRotation(currenttarget.position - trans.position), rotatespeed);
   
 
	
		//　カメラの背景色を変更
			}
}
 