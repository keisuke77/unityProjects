using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallGameManager : MonoBehaviour {

public enum States
{
    BallMove,
    BallFall
}    
public Transform BallPos,BeforeBallPos;
public Text text;
public int Score;
public controll move;
public GameObject particle;
public float MoveRange;
    public List<Sprite> sprites;
    public static BallGameManager instance;
public States states;
    public GameObject BallPrefab;

    private void Awake() {
        instance=this;
    }
    private void Start() {
        Spawn();
        ChangeMoveState();
    }
public void BallMove(){

}
public GameObject currentBall;
Rigidbody2D currentRB;
Collider2D collider2D;

public GameObject BeforeSpawn;
public void Spawn(){
    if (BeforeSpawn!=null)
    {
currentBall=BeforeSpawn;
currentRB= currentBall.GetComponent<Rigidbody2D>();
collider2D=currentBall.GetComponent<Collider2D>();
collider2D.enabled=false;
currentRB.isKinematic=true;
currentBall.transform.position=BallPos.position;

    }
BeforeSpawn=Instantiate(BallPrefab,BeforeBallPos.position,Quaternion.identity);
BeforeSpawn.GetComponent<MatrixBall>().BallID=(int)UnityEngine.Random.Range(0,sprites.Count/2);

var currentRBs= BeforeSpawn.GetComponent<Rigidbody2D>();
var collider2Ds=BeforeSpawn.GetComponent<Collider2D>();
collider2Ds.enabled=false;
currentRBs.isKinematic=true;


}
public void ChangeMoveState(){
     Spawn();
        states=States.BallMove;
  
}



private void Update() {
    text.text="スコア　"+Score.ToString();
 switch (states)
 {
    case States.BallMove:
    currentBall.transform.position =new Vector3( Mathf.Clamp( currentBall.transform.position.x,BallPos.position.x-MoveRange,BallPos.position.x+MoveRange),currentBall.transform.position.y,currentBall.transform.position.z);


currentBall.transform.Translate(keiinput.Instance.GetAxis(move),0,0);
    
if (Input.GetKeyDown( KeyCode.Space))
{
    
    states=States.BallFall;
}
    break;
      case States.BallFall:
    currentRB.isKinematic=false;
collider2D.enabled=true;
      break;


    
 }   
}

}