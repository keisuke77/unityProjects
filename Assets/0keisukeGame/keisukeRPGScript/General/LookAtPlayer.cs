using UnityEngine;
using DG.Tweening;
using System.Linq;
public class LookAtPlayer : MonoBehaviour
{
    public void NearLookAt()
    {
        GameObject players = gameObject.NearSearchTag("Player");

     

            // Use DOTween to smoothly rotate towards the target
            transform.DOLookAt(players.transform.position, 0.2f);

     
    }
    public void RandomLookAt()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // Filter out root objects and remove duplicates
        players = players.Where(player => player.transform.root == player.transform).Distinct().ToArray();




        if (players.Length > 0)
        {
            int randomIndex = Random.Range(0, players.Length);
            Transform target = players[randomIndex].transform;

            // Use DOTween to smoothly rotate towards the target
            transform.DOLookAt(target.position, 0.2f);

            // Optional: You can also specify additional parameters for the rotation animation
        }
        else
        {
            Debug.LogWarning("No players found with the 'Player' tag.");
        }
    }

    // Optional: Callback method for animation completion
    // private void OnLookAtComplete()
    // {
    //     Debug.Log("Rotation animation completed.");
    // }
}