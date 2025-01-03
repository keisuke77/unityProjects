using System.Collections.Generic;
using UnityEngine;


public class ParticleCollider : effect
{
    [SerializeField]
    private GameObject spawnObj;

    public GameObject s=>spawnObj;
    
    [SerializeField]
    private GameObject spawnObj2;
    public GameObject parentEffect;
    public bool jumpEffect = true;
    public float explosionSpeed = 0;
    public bool playerHitObjSpawn = true;
    public bool enemyHitObjSpawn = false;
    public bool hitObjSpawn = false;
    public bool childSpawn;

    private ParticleSystem _particleSystem;
    private List<ParticleCollisionEvent> particleCollisionEventList = new List<ParticleCollisionEvent>();
    private List<GameObject> spawns = new List<GameObject>();

    void Start()
    {
        _particleSystem = this.gameObject.GetComponent<ParticleSystem>();
    }

    public void ObjSpawns(GameObject other)
    {
        _particleSystem.GetCollisionEvents(other, particleCollisionEventList);
        Vector3 collisionHitPos = particleCollisionEventList[0].intersection;

        if (spawnObj != null)
        {
            spawns.Add(Instantiate(spawnObj, collisionHitPos, Quaternion.identity));
        }

        if (spawnObj2 != null)
        {
            spawns.Add(Instantiate(spawnObj2, collisionHitPos, Quaternion.identity));
        }

        if (childSpawn)
        {
            foreach (var obj in spawns)
            {
                obj.transform.position = Vector3.zero;
                obj.transform.SetParent(other.transform);
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        
        Debug.Log("particleCollide");
        damage(other.gameObject.root());
        
        if (other.eroottag())
        {
            if (enemyHitObjSpawn)
            {
                ObjSpawns(other);
                Destroy(gameObject);
            }
        }
        else if (other.proottag())
        {
            GameObject obj = other.root();
               if (parentEffect != null)
            {
                var e = Instantiate(parentEffect, other.gameObject.transform);
                e.transform.parent = other.gameObject.transform;
            }

          
            if (playerHitObjSpawn)
            {
                ObjSpawns(other);
                Destroy(gameObject);
            }

            if (jumpEffect)
            {
                jumpForce(obj); // Assuming JumpForce function exists
            }

            if (explosionSpeed != 0)
            {
                var velocity = (other.transform.position - transform.position).normalized * explosionSpeed;
                // Assuming obj has a function called PlayerAddForce
                obj.PlayerAddForce(velocity);
            }
        }
        else if (hitObjSpawn)
        {
            ObjSpawns(other);
            Destroy(gameObject);
        }
    }
}
