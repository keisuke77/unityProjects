using UnityEngine;


 


public class HealOnTouch : MonoBehaviour
{
    public HealItem healItem;
    public GameObject Effect;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("aaguh");
        var otherGameObject = other.gameObject.root();
        if (otherGameObject.CompareTag("Player"))
        {
            healItem.Use(otherGameObject);
            if (Effect != null)
            {
                Instantiate(Effect, otherGameObject.transform.position, Quaternion.identity,otherGameObject.transform);
            }
            Destroy(gameObject); // Optionally destroy the item after use
        }
    }
}