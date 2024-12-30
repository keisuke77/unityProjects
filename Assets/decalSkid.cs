using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class decalSkid : MonoBehaviour
{
  public int Interval;
  public GameObject Decal;
  public float DestroyTime;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Clone",1,Time.deltaTime*Interval);
    }
public void Clone(){
   var obj = Instantiate(Decal, transform.position, transform.rotation);
   obj.transform.parent=null;
   keikei.delaycall(()=>obj.AddComponent(typeof(FadeOutDecalURP)),DestroyTime);
   
}
    // Update is called once per frame

}
