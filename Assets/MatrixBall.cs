using System.Collections;
using System.Collections.Generic;
using Animancer.FSM;
using UnityEngine;

public class MatrixBall : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    public int BallID;
    void Start()
    {
       
    }
public void PowerUP(){
BallID++;
BallGameManager.instance.Score+=BallID;
}
bool cooldown;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale=Vector3.one*(BallID+1);
        
spriteRenderer.sprite=BallGameManager.instance.sprites[BallID];
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(BallGameManager.instance.currentBall==gameObject&&BallGameManager.instance.states==BallGameManager.States.BallFall)
        BallGameManager.instance.ChangeMoveState();

      

          
        if(!cooldown&&BallID==other.gameObject.GetComponent<MatrixBall>().BallID){ 
            gameObject.GetComponent<MatrixBall>().PowerUP();
cooldown=true;
keikei.delaycall(()=>cooldown=false,0.3f);
            Instantiate(BallGameManager.instance.particle,transform.position,Quaternion.identity);
               Destroy(other.gameObject);
        }
       
        
    }
}
