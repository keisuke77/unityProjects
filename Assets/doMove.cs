using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class doMove : MonoBehaviour
{
    public Vector3 Vector;
    [SerializeField]
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(Vector,speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
