

using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using System.Threading.Tasks;



using System.Threading;


[System.Serializable]
public class EffectAndParticle
{
    public GameObject Particle;
    public Effekseer.EffekseerEffectAsset Effect;
    public float duration = 0;
    public float delay = 0;
    Effekseer.EffekseerHandle handle;
    GameObject instance;
 public void Execute(Transform trans, Collider coll = null)
    {
        if (trans!=null)
        {
Execute(trans.position,coll);
        }
    }
    public void Execute(Vector3 pos, Collider coll = null)
    {
               
        keikei.delaycall(
            () =>
            {
                End();
                if (Particle != null)
                {
                    if (coll != null )
                    {
                        instance = Particle.Instantiate(coll.ClosestPointOnBounds(pos));
                    }
                    else 
                    {
                        instance = Particle.Instantiate(pos);
                    }
                }
                if (Effect != null)
                {
                    if (coll != null )
                    {
                        handle = Effekseer.EffekseerSystem.PlayEffect( Effect,coll.ClosestPointOnBounds(pos));
                    }
                    else
                    {
                        handle = Effekseer.EffekseerSystem.PlayEffect( Effect,pos);
                    }
                }
                if (duration > 0)
                {
                    keikei.delaycall(End, duration);
                }
            },
           Time.deltaTime+ delay
        );
    }

    public void End()
    {
      
            handle.Stop();
       
        
        if (instance != null)
        {
            keikei.destroy(instance);
        }
    }
}

public class keikei : MonoBehaviour
{ 
   public static System.Action ac;
      public static float navspeed;
      public static Transform charaLookAtPosition;
    public static Collider spherecheck;
public static Effekseer.EffekseerEffectAsset chargeeffect;
public static GameObject player;
public static Transform dokan;
public static Animator playeranim; 
public static GameObject explosion;
public static Effekseer.EffekseerHandle handle;

public static Camera cameramain;


public static void destroy(GameObject obj,float time){

Destroy(obj,time);
}
  public static void Destroys(Component com){

Destroy(com);
  } public static void Destroys(Behaviour com){

Destroy(com);
  }
 public static void Destroys(MonoBehaviour com){

Destroy(com);
  }


  




public static GameObject instantiate(GameObject obj,Vector3 pos,Quaternion rot){

 var o= Instantiate(obj,pos,rot);
  return o;
}
public static RaycastHit mousePositionObj(){
     if (Input.GetMouseButtonDown(0))
        {  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
      
            if (Physics.Raycast(ray, out hit))
            {Debug.Log(hit.collider.gameObject.name);
                return hit;
            }
           
        } return (RaycastHit)default;
}
public static GameObject instantiate(GameObject obj,Transform trans,bool parent=false){
GameObject o= Instantiate(obj,trans.position,trans.rotation);
if (parent)
{
  o.transform.parent=trans;
}
  return o;
}

    




 public static Vector2 GetDpad()
{
    return new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
}













public static void uijump(Transform rectTransform,float power=50){  
  if (DOTween.IsTweening(rectTransform))  
  {
     rectTransform.DORestart();

  }else
  {
    rectTransform.DOLocalJump(
    rectTransform.localPosition, // 移動終了地点
    power,                    // ジャンプの高さ
    1,                     // ジャンプの総数
    1                 // 演出時間
);
  }
 
  }


  
 public static void destroy(GameObject obj){
  if (obj!=null)
  {
  Destroy(obj);
  }
  
  }
    

    
  
    public static Effekseer.EffekseerHandle Effspawn(Effekseer.EffekseerEffectAsset effect,Transform trans){
    handle = Effekseer.EffekseerSystem.PlayEffect(effect, trans.position);
// transformの回転を設定する。
    handle.SetRotation(trans.rotation);
return handle;

    }  
      public static void Effspawn(Effekseer.EffekseerEffectAsset[] effect,Transform trans){

foreach (Effekseer.EffekseerEffectAsset item in effect)
{
  Effspawn(item,trans);
}

    }  
    public static void Effspawn(Effekseer.EffekseerEffectAsset effect,Transform trans,Quaternion rot){
handle = Effekseer.EffekseerSystem.PlayEffect(effect, trans.position);
    // transformの回転を設定する。
    handle.SetRotation(rot);


    } 
    
     
    









public static void colliderset(GameObject obj){

  if (obj.GetComponent<Collider>()!=null)
               {
                  Collider col= obj.GetComponent<Collider>();
                  col.enabled=true;
                  col.isTrigger=false;
               }else
               {obj.AddComponent<MeshCollider>();
                  Collider col= obj.GetComponent<Collider>();
                 col.enabled=true;
                  col.isTrigger=false;
             
               }
}






public static Timer loopCall(System.Action action,int interval){
 Timer timer = null;
        timer = new Timer((state) =>
        {
            action();
        }, null, 0, interval);
        return timer;
}

public static Tween delaycall(System.Action action,float delay){
  if (delay==0)
  {
    action(); return null;
  }else
  {
    
return  DOVirtual.DelayedCall(delay, () => action(),false).SetUpdate( true );
  }
}



   
public static Vector3 randomvector()  // 目的地を生成、xとyのポジションをランダムに値を取得 
    {
   return randomvector(1); }
public static Vector3 randomvectornotY()  // 目的地を生成、xとyのポジションをランダムに値を取得 
    {
   return randomvectornotY(1);
    }
    public static Vector3 randomvector(int a)  // 目的地を生成、xとyのポジションをランダムに値を取得 
    {
    Vector3 randomPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
   randomPos*=a;
   return randomPos;
    } public static Vector3 randomvectornotY(int a)  // 目的地を生成、xとyのポジションをランダムに値を取得 
    {
    Vector3 randomPos = new Vector3(Random.Range(-a, a), 0, Random.Range(-a, a));
   return randomPos;
    }


public static RaycastHit raycast(){
  Ray ray = cameramain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
      
            if (Physics.Raycast(ray, out hit))
            {
               return hit;
                  }
                  return hit;
}

 
  
          
     
    public static void fadedeathchildall(GameObject a){
 
        foreach (Transform child in a.transform)
        {
          var mat= child.GetComponent<Renderer>().material;
 var sequence = DOTween.Sequence(); //Sequence生成
    //Tweenをつなげる
    sequence.Append(mat.DOFade(0.0F, 1f)).AppendCallback(()=>
    {
Destroy(child.gameObject);
    });

        }
           }
           
       public static void fadedeathchildall(GameObject a,float duration){
 
        foreach (Transform child in a.transform)
        {
          fadedeath(child.gameObject,duration);
        }
           }
           
       public static void fadedeath(GameObject a,float num=1){
 
    var fader=  a.AddComponent<fader>()as fader;
         fader.isFadeOut=true;
         Destroy(a,3);
        
           }
     
    public static void explosionget(GameObject exp){
explosion=exp;

    }
public static bool kakuritu(int often){
 var b=(int)UnityEngine.Random.Range(100,0);
          
 if (often>b)
            { return true;
            }else
            {
                return false;
            }

}


public static void deathexplosion(GameObject target,GameObject explosions){
 var a= Instantiate(explosions,target.transform.position,Quaternion.identity);
Destroy(target);
  Destroy(a,2);
    }
    
    
   

public static void deathexplosion(GameObject target){
 var a= Instantiate(explosion,target.transform.position,Quaternion.identity);
Destroy(target);
  Destroy(a,2);
    }

}