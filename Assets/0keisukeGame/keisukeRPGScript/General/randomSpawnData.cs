using UnityEngine;

[CreateAssetMenu(fileName = "New RandomSpawnData", menuName = "ScriptableObjects/RandomSpawnData")]
public class RandomSpawnData : ScriptableObject
{
    public int num; // Number of objects to spawn
    public GameObject obj; // Object to spawn
    public int distance; // Distance of random vector
    public LayerMask targetLayer; // Layer for raycast collision
    public float spawnDelay = 0.5f; // Delay between spawns
}