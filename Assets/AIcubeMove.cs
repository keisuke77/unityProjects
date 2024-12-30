using UnityEngine;using System.Collections;
using System.Collections.Generic;

public class AIcubeMove : MonoBehaviour
{
    cubemove cubemove;
    [Range(0.3f,10)]
    public float speed=1;
    void Start()
    {
        cubemove=GetComponent<cubemove>();
        StartCoroutine(ControllAI());
    }

IEnumerator ControllAI(){
    
yield return new WaitForSeconds(3);
    while (true)
    {cubemove.MoveCubeInput(RandomVector2());
        if(cubemove.isRotate)
        {
yield return new WaitForSeconds(speed);
        }
yield return new WaitForSeconds(Time.deltaTime);
    }
}
    public Vector2 RandomVector2(){
        Vector2 vec2=Vector2.zero;
        if(MyRandom.RandomBool())
        {
            if (MyRandom.RandomBool())
            {
                vec2.x=1;
            }else
            {
                 vec2.x=-1;
            }
            
        }else
        {
             if (MyRandom.RandomBool())
            {
                vec2.y=1;
            }else
            {
                 vec2.y=-1;
            }
        }
        return vec2;
    }
}
