using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOMBercube : MonoBehaviour
{
    int upcubenumber;
    public Text text;
    public int UpcubeCount;
    public GameObject explosion;
    float cubeSizeHalf;
    [Range(0,10)]
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        cubeSizeHalf = transform.localScale.x / 2f;
      
    }

    // Update is called once per frame
    void Update()
    {
           RaycastHit hit;RaycastHit[] hits;
        // Does the ray intersect any objects excluding the player layer
	 
upcubenumber=0;

while (Physics.Raycast(transform.position+new Vector3(0,upcubenumber*2*cubeSizeHalf,0) , Vector3.up,out hit,cubeSizeHalf+0.1f))
{
    if (hit.transform.gameObject.tag=="cube")
 {
upcubenumber++;
    }
}

text.text=(UpcubeCount-upcubenumber).ToString();
      if ((UpcubeCount-upcubenumber)==0)
      {
        
hits=Physics.SphereCastAll(transform.position, cubeSizeHalf*2*radius,-Vector3.up);
if (hits!=null)
{
    foreach (var item in hits)
    {
        
  if (item.transform.gameObject.tag=="cube")
 {
Instantiate(explosion,item.transform.position,Quaternion.identity);
Destroy(item.transform.gameObject);
    }
    }
}		

Instantiate(explosion,transform.position,Quaternion.identity);
Destroy(gameObject);

      }
}
}