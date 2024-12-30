using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
public class goalcube : MonoBehaviour
{bool goalonce;
public GameObject kamifubuki;
public GameObject explosion;
public UnityEvent eve;
public DOTweenScriptable DOTweenScriptable;
public Text TimeText;
	public static List<GameObject> cubes;
    
    // Start is called before the first frame update

     void Awake()
 {
	cubes=new List<GameObject>();
	
		
	
 }
    void Start()
    {
        StartCoroutine("cubeSet");
    }
    
    IEnumerator cubeSet()
    {
      yield return new WaitForSeconds(Time.deltaTime);  
foreach (var item in cubes)
{
   DOTweenScriptable.GetSequence(item.transform); 
   yield return new WaitForSeconds(Time.deltaTime);
}

        yield return null;

    }

    void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.tag=="cube")
    {if (!goalonce)
    {goalonce=true;
        StartCoroutine( Goal());
    }
       
    }
    }
    IEnumerator Goal()
    {
stop=true;
        foreach (var item in  cubes.ToArray())
    {
        if (item!=null)
        {
              var a=  Instantiate(kamifubuki,item.transform.position,Quaternion.identity); 
      Destroy(a,3);
     var b=  Instantiate(explosion,item.transform.position,Quaternion.identity); 
      Destroy(explosion,3);
     Destroy(item);
      yield return new WaitForSeconds(Time.deltaTime);
   
        }
     }
       eve.Invoke();
    }

    float time;
    bool stop;
 
 
  void Update()
    {
        if (!stop)
        {time+=Time.deltaTime;
            TimeText.text=time.ToString();
        }
    }
}
