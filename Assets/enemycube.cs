using UnityEngine;

public class enemycube : MonoBehaviour
{
    public GameObject killEffect;
void OnTriggerEnter(Collider other)
{
    if (other.gameObject.tag=="cube")
    {   Instantiate(killEffect,other.gameObject.transform);
        Destroy(other.gameObject);
    }
}
}