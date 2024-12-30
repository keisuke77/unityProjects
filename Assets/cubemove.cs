using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class cubemove : MonoBehaviour
{
	public static bool cubesonce;
   public Material defaultmaterial;
   public Material selectmaterial;
    Vector3 rotatePoint = Vector3.zero;  //回転の中心
	Vector3 rotateAxis = Vector3.zero;
    float rotateAngle=90;   //回転軸
	float cubeAngle = 0f;                //回転角度
 
	float cubeSizeHalf;                  //キューブの大きさの半分
	public bool isRotate = false;               //回転中に立つフラグ。回転中は入力を受け付けない
 public static cubemove current;

	void Start(){
		goalcube.cubes.Add(gameObject);
		cubeSizeHalf = transform.localScale.x / 2f;
        current=this;
	}/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	private void OnDestroy()
	{
		goalcube.cubes.Remove(gameObject);
	}
 Quaternion cameravelc;



public void MoveCubeInput(Vector2 input){
//回転中は入力を受け付けない
		if (isRotate||Camera.main.GetComponent<rotattearound>().Tweening)
			return;
	
		
	
　　　　　//操作を受け付けたら回転軸と回転方向決定
		if (input.x>0) {
			rotatePoint =  new Vector3(cubeSizeHalf, -cubeSizeHalf, 0f);
			rotateAxis = new Vector3 (0, 0, -1);
		}
		if (input.x<0) {
			rotatePoint =new Vector3(-cubeSizeHalf, -cubeSizeHalf, 0f);
			rotateAxis = new Vector3 (0, 0, 1);
		}
		if (input.y<0) {
			rotatePoint =  new Vector3(0f, -cubeSizeHalf, cubeSizeHalf);
			rotateAxis = new Vector3 (1, 0, 0);
		}
		if (input.y>0) {
			rotatePoint =  new Vector3(0f, -cubeSizeHalf, -cubeSizeHalf);
			rotateAxis = new Vector3 (-1, 0, 0);
		}

         //カメラとオブジェクトの方向取得してそれに応じて回転方向と軸の位置変更
		 cameravelc=Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y,Vector3.up);
         rotateAxis=cameravelc*rotateAxis;
         rotatePoint=cameravelc*rotatePoint;
		 //デフォルト回転角度が９０度
         rotateAngle=90;
		// 入力がない時はコルーチンを呼び出さないようにする
		if (rotatePoint == Vector3.zero)
			return;
		StartCoroutine (MoveCube());
        
}



	void Update(){
		 if (GetComponent<AIcubeMove>()!=null)
		{
			return;
		}
		//現在クリックされたゲームオブジェクトがキューブだったらそれを動かすものに変更
		if (GetClickedGameObject.clickedGameObject!=null)
    {
          if (GetClickedGameObject.clickedGameObject.tag=="cube")
       {
        current=GetClickedGameObject.clickedGameObject.GetComponent<cubemove>();
         current.enabled=true;
         current.gameObject.GetComponent<Renderer>().material=selectmaterial;
       } 
    }
     if (current!=null)
     {
             if (current!=this)
    {
         this.gameObject.GetComponent<Renderer>().material=defaultmaterial;

    
        this.enabled=false;

    }
     }
   
        MoveCubeInput(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));
	
		
	}
 
	IEnumerator MoveCube(){

rotatePoint+=transform.position;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
	 

        if (Physics.BoxCast(transform.position,Vector3.one* cubeSizeHalf/2,(rotatePoint-transform.position+new Vector3(0,cubeSizeHalf,0)),out hit,Quaternion.identity,cubeSizeHalf+0.1f))
        {
           if (hit.transform.gameObject.tag=="cube")
           {
            if (Physics.BoxCast(transform.position+new Vector3(0,cubeSizeHalf*2,0),Vector3.one*cubeSizeHalf/2,(rotatePoint-transform.position+new Vector3(0,cubeSizeHalf,0)),out hit,Quaternion.identity,cubeSizeHalf+0.1f))
            {

				isRotate = false;
		rotatePoint = Vector3.zero;
		rotateAxis = Vector3.zero;
               yield break;
            }
            rotatePoint.y=transform.position.y+cubeSizeHalf;
            rotateAngle=180;
           }
             }
		//回転中のフラグを立てる
		isRotate = true;
 
		//回転処理
		float sumAngle = 0f; //angleの合計を保存
		while (sumAngle < rotateAngle) {
			cubeAngle = 15f; //ここを変えると回転速度が変わる
			sumAngle += cubeAngle;
 
			// 90度以上回転しないように値を制限
			if (sumAngle > rotateAngle) {
				cubeAngle -= sumAngle - rotateAngle; 
			}
			transform.RotateAround (rotatePoint, rotateAxis, cubeAngle);
 
			yield return null;
		}
      
        while (!Physics.BoxCast(transform.position, Vector3.one*cubeSizeHalf/2,-Vector3.up,out hit,Quaternion.identity,cubeSizeHalf+0.1f))
        {
          
         transform.DOMoveY(transform.position.y-1,Time.deltaTime);


          yield return new WaitForSeconds(Time.deltaTime);
          if (Physics.Raycast(transform.position, Vector3.right,out hit,cubeSizeHalf+0.1f)
          ||Physics.Raycast(transform.position, -Vector3.right,out hit,cubeSizeHalf+0.1f)
          ||Physics.Raycast(transform.position, -Vector3.forward,out hit,cubeSizeHalf+0.1f)
          ||Physics.Raycast(transform.position, Vector3.forward,out hit,cubeSizeHalf+0.1f)
          )
          {
            
          }
        }
		//回転中のフラグを倒す
		isRotate = false;
		rotatePoint = Vector3.zero;
		rotateAxis = Vector3.zero;
 
		yield break;
	}
}
