using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using DG.Tweening;

public class agentControll : MonoBehaviour
{
    public NavMeshAgent agent;
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (agent != null){
            Vector3 diffPos=agent.transform.position-agent.destination;
            Debug.Log("エージェント差分"+diffPos.sqrMagnitude);
        }
    }
}