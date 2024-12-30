using UnityEngine;
using System.Collections;
using DG.Tweening;

public class NearObjectChaise : MonoBehaviour
{
    public float chaseSpeed = 1f; // The speed at which the object chases the target
    public Transform target;
    float interval = 1;
    public float intervalMax = 1;
    public string tag = "Player";

    public void ChangeTarget()
    {
        target = gameObject.NearSearchTag(tag).root().transform;
        if (target != null)
        {
            transform.DOLookAt(target.position, chaseSpeed);
            transform.DOMove(target.position, Vector3.Distance(transform.position, target.position) / chaseSpeed);
        }
    }

    private void Update()
    {
        if (target == null)
        {
            ChangeTarget();
        }
        else
        {
            interval += Time.deltaTime;
            if (interval > intervalMax)
            {
                ChangeTarget();
                interval = 0;
            }
        }
    }
}