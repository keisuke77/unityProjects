using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

    //生成するゲームオブジェクト

 public RandomSpawnData randomSpawnData;
    private RaycastHit hitInfo;

   
    
    public void RandomSpawn(RandomSpawnData data)
    {
        randomSpawnData = data;
        StartCoroutine(SpawnObjects());
    }
    

    private IEnumerator SpawnObjects()
    {
        for (int i = 0; i < randomSpawnData.num; i++)
        {
            // Generate a random vector in a random direction and check if it hits the ground with a raycast
            Vector3 randomPosition = transform.position + GetRandomVectorNotY(randomSpawnData.distance);

            if (Physics.Raycast(randomPosition, Vector3.down, out hitInfo, Mathf.Infinity, randomSpawnData.targetLayer))
            {
                // Spawn the object at the hit position and destroy it after a certain delay
                Obj=Instantiate(randomSpawnData.obj, hitInfo.point, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Raycast did not hit anything. Consider adjusting the starting position or ray length.");
            }

            // Delay until the next spawn
            yield return new WaitForSeconds(randomSpawnData.spawnDelay);
        }
    }

    private Vector3 GetRandomVectorNotY(float distance)
    {
        Vector3 randomVector = new Vector3(Random.Range(-distance, distance), 0, Random.Range(-distance, distance));
        return randomVector;
    }


public void spawnrotfix(GameObjectVector obj){

Obj=Instantiate (obj.obj, transform.position+transform.forward*obj.vector3.x+transform.up*obj.vector3.y, obj.obj.transform.rotation);

      
}

public void spawn(GameObjectVector obj){

Obj=Instantiate (obj.obj, transform.position+transform.forward*obj.vector3.x+transform.up*obj.vector3.y, transform.rotation);
           
      
}

public void spawnParent(GameObjectVector obj){

Obj=Instantiate (obj.obj, transform.position+transform.forward*obj.vector3.x+transform.up*obj.vector3.y, transform.rotation,transform);
           
      
}
public void SpawnParent(GameObject obj){

Obj=Instantiate (obj, gameObject.transform.position, transform.rotation,transform);
           
      
}
public void Spawn(GameObject obj){

Obj=Instantiate (obj, gameObject.transform.position, transform.rotation);
           
      
}

GameObject Obj;
GameObject tempObj;

void Update()
{
   if (Obj!=tempObj)
   {
    Obj.transform.localScale=Obj.transform.lossyScale*WorldInfo.scale;
    tempObj=Obj;
   } 
}
}
