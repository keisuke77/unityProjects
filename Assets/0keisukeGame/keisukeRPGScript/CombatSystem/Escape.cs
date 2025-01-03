using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class Escape : MonoBehaviour
{
    public List<Transform> players;
    public NavMeshAgent navMeshAgent;
    public float checkDistance = 5.0f;
    public int checkAngleIncrement = 10;
    public LayerMask raycastLayerMask;

    public string ChaiseTag = "Player";

    private int walkableMask;
public bool auto;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        // Ensure that the "Walkable" area exists before retrieving its mask
        int walkableArea = NavMesh.GetAreaFromName("Walkable");
        if (walkableArea >= 0) 
        {
            walkableMask = 1 << walkableArea;
        }
        else 
        {
            Debug.LogWarning("No NavMesh area named 'Walkable' found.");
            walkableMask = -1;  // All areas
        }
    }
public void Execute(){
  if (players == null)
  {
     players = GameObject.FindGameObjectsWithTag(ChaiseTag).Distinct().Select(n => n.transform).ToList();
  }
     

  if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            Flee();
        }  if (navMeshAgent.hasPath)
    {
        transform.DOLookAt(navMeshAgent.destination, 0.5f);
    }
}
    void Update()
    {if (auto)
    {
        Execute();
    }
      
    }
void Flee()
{
    Vector3 bestDirection = transform.forward;
    float bestScore = float.MinValue;

    for (int i = 0; i < 360 - checkAngleIncrement; i += checkAngleIncrement)
    {
        Vector3 testDir = Quaternion.Euler(0, i, 0) * transform.forward;
        Vector3 testPos = transform.position + testDir * checkDistance;

        if (NavMesh.SamplePosition(testPos, out NavMeshHit hit, checkDistance, walkableMask))
        {
            float score = ScorePosition(hit.position);

            // Check for potential dead-ends or traps by sampling some points ahead
            for (float distance = 1.0f; distance <= checkDistance; distance += 1.0f)
            {
                Vector3 furtherTestPos = hit.position + testDir * distance;
                if (NavMesh.SamplePosition(furtherTestPos, out NavMeshHit furtherHit, 1.0f, walkableMask))
                {
                    score += ScorePosition(furtherHit.position) * 0.5f;  // Give less weight to further points
                }
                else
                {
                    score -= 10.0f;  // Penalize if a point isn't on the NavMesh, indicating a potential dead-end
                }
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = testDir;
            }
        }
    }

    navMeshAgent.SetDestination(transform.position + bestDirection * checkDistance);
}


     float ScorePosition(Vector3 testPos)
    {
        float score = 0.0f;

        // 複数プレイヤー全体との距離を考慮
        foreach (Transform player in players)
        {
            float playerDist = (player.position - testPos).magnitude;
            score += playerDist;

            if (Physics.Raycast(testPos, player.position - testPos, out RaycastHit hit, playerDist) && hit.distance < playerDist)
            {
                score -= hit.distance;
            }
        }

        return score;
    }
}