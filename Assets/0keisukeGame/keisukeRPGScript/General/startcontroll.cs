using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class startcontroll : MonoBehaviour
{


   public navchaise navchaise;
   public enemyhp enemyhp;
   public NavMeshAgent agent;
  

    
    private void Awake()
    {
        
        navchaise=GetComponent<navchaise>();
        enemyhp=GetComponent<enemyhp>();
        agent=GetComponent<NavMeshAgent>();
        
agent.enabled=false;
enemyhp.enabled=false;
navchaise.enabled=false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
public void setstart(){
agent.enabled=true;
enemyhp.enabled=true;
navchaise.enabled=true;
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
