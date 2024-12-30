using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StopStart : MonoBehaviour
{
    bool on;
    public List<IMove> imove;
    public void Execute(bool on)
    {
        imove.ForEach(x => x.Stop = !on);
    }

    void Awake()
    {
        imove = gameObject.root().GetComponentsInChildren<IMove>().ToList();
    }

    void Update()
    {
       
    }

}