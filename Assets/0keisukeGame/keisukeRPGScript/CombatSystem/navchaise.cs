using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;





[RequireComponent(typeof(NavMeshAgent))]
public class navchaise : MonoBehaviour, IForceIdle, IMove
{
    public UnityEngine.Events.UnityEvent discoverevent;

    [Header("Debug用 実際の追いかけるポイントとの距離 AgentDestinationはポイントと重ならないようにずらしてあるからこれとは違う")]
    public float agentdestinationdis;
    public bool Stop { get; set; }
    public bool stop;
    public float Speed = 4;

    public BehaviorState currentBehavior;
    public BehaviorSelector behaviourData;
    public BehaviorSelector damagedBehaviourData;
    public Transform point;

    public bool lookat;
    public float patrolnexttime = 8;
    public int patrollarea = 10;

    public string ChaiseTag = "Player";
    public float minwalk = 1;
    private NavMeshAgent agent;

   
    basehp basehp;
    Animator anim;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        basehp = GetComponent<basehp>();
        
    }

    public void AddForce(Vector3 direction)
    {
        agent.enabled = false;
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().AddForce(direction);
        }
        agent.enabled = true;
    }

    public void pointupdate()
    {
        point = gameObject.NearSearchTagNotDeath(ChaiseTag)?.root().transform;
        Debug.Log("target Set" + point);
        if (point == null)
        {
            Debug.Log("target Missed navaChaise");
            point = transform;
        }
        if (agent.isOnNavMesh)
        {

            agent?.SetDestination(point.position + ((transform.position - point.position).normalized * closedistancerate));
        }
    }



    public float closedistancerate = 1;
    public void meleemode()
    {
        //navmeshに設定されている目的値の距離
        //実際の目的値との距離 
        pointupdate();
        if (agentdestinationdis > minwalk)
        {
            agent.speed = Speed;
        }
        else
        {
            agent.speed = 0;
        }

        if (lookat)
        {
            Vector3 targetPosition = new Vector3(point.localPosition.x, transform.position.y, point.localPosition.z);
            transform.DOLookAt(targetPosition, 1f);
        }

    }


    float patroltime;

    //ポイントはつかわない。ポイント使うとagentdestinationdisの値が変わってmeleemodeになるから
    public void patrolmode()
    {
        patroltime -= Time.deltaTime;
        if (lookat)
        {
            transform.DOLookAt(agent.destination, 0.1f);
        }

        if (agent.remainingDistance < 2 || patroltime < 0)
        {
            patroltime = patrolnexttime;
            if (agent.isOnNavMesh)
                agent.destination = transform.position + keikei.randomvectornotY(patrollarea);

        }
    }
    public void ForcePatrol(float duration)
    {
        currentBehavior = BehaviorState.Patrol;
        Speed = 400;
        ForceBehaviorDuration = true;
        keikei.delaycall(() => { ForceEnd(); Speed = 4; }, duration);
    }
    public void ForceMelee(float duration = 0)
    {
        currentBehavior = BehaviorState.Melee;
        ForceBehaviorDuration = true;
        if (duration != 0)
            keikei.delaycall(ForceEnd, duration);
    }
    public void ForceStop(float duration = 0)
    {
        currentBehavior = BehaviorState.Stop;
        ForceBehaviorDuration = true;
        if (duration != 0)
            keikei.delaycall(ForceEnd, duration);


    }
    public void ForceEscape(float duration)
    {
        currentBehavior = BehaviorState.Escape;
        ForceBehaviorDuration = true;
        keikei.delaycall(ForceEnd, duration);
    }
    public void ForceEnd()
    {
        ForceBehaviorDuration = false;
    }

    public float AnimSpeed = 0.5f;

    // Update is called once per frame
    Vector3 lastposition;
    public bool ForceBehaviorDuration = false;


    public void ChangeBehavior(BehaviorState state)
    {
        if (ForceBehaviorDuration) return;
        currentBehavior = state;

    }
    [HideInInspector]
    public bool UseData = true;
    void FixedUpdate()
    {
        stop = Stop;
        if (agent) agent.enabled = !Stop;

        if (Stop)return;
        
        if (point == null)
        {
            pointupdate();
        }

        agentdestinationdis = Vector3.Distance(point.position, transform.position);

        BehaviorSelector useBehaviourData = null;
        if (UseData)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Damage") && basehp != null)
            {
                useBehaviourData = behaviourData;
            }
            else
            {
                useBehaviourData = damagedBehaviourData;
            }

            ChangeBehavior(useBehaviourData.DetermineBehavior(basehp.HP, agentdestinationdis, currentBehavior));


        }



        if (escape != null)
        {
            escape.enabled = false;
        }
        if (currentBehavior != BehaviorState.Stop)
        {
            anim.FloatTo("speed", (transform.position - lastposition).magnitude * AnimSpeed, 0.6f);
            lastposition = transform.position;
            agent.speed = Speed;
        }
    

        switch (currentBehavior)
        {
            case BehaviorState.Patrol: patrolmode(); break;
            case BehaviorState.Melee: meleemode(); break;
            case BehaviorState.Stop: Stopmode(); break;
            case BehaviorState.Escape: Escape(); break;

            default: break;
        }    
   // 現在のAnimatorの状態
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // Attack状態に入ったかどうかをチェック
        if (stateInfo.IsTag("Attack"))
        {
            if (!isInAttackState)
            {
                // Attack状態に初めて入ったとき
                ForceStop();
                isInAttackState = true; // Attack状態に入ったことを記録
            }
        }
        else
        {
            if (isInAttackState)
            {
                // Attack状態から抜けたとき
                ForceEnd();
                isInAttackState = false; // Attack状態から抜けたことを記録
            }
        }


    }

      private bool isInAttackState = false; // 前のフレームでAttack状態にいたかどうか


    Escape escape;
    public void Stopmode()
    {
        anim.FloatTo("speed", 0);
        agent.speed = 0;
    }
    public void NavEnable(int enable)
    {
        Stop = enable == 0;
        Debug.Log("navenable" + enable);
    }
    void Escape()
    {

        escape = gameObject.AddComponentIfnull<Escape>();
        escape.navMeshAgent = agent;
        escape.enabled = true;
        escape.ChaiseTag=gameObject.tag=="Player"?"Enemy":"Player";
        escape.Execute();
    }


}
