using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterControllerSequence : MonoBehaviour
{
    public Animator animator; // Animator コンポーネント
    public float stopDistance = 1f; // 停止距離
    public float walkSpeed = 2f; // 歩行速度
    public float runSpeed = 5f; // 走行速度
    private NavMeshAgent agent; // NavMeshAgent コンポーネント

    // タスクキュー
    private Queue<System.Action> taskQueue = new Queue<System.Action>();
    private bool isProcessing = false; // 現在タスク実行中かどうか

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (agent.remainingDistance <= stopDistance && !agent.pathPending && isProcessing)
        {
            isProcessing = false;
            CompleteCurrentTask();
        }
       
      // NavMeshAgentの速度を取得
        Vector3 velocity = agent.velocity;

        // 移動している場合に回転を行う
        if (velocity.magnitude > 0.1f)
        {
            // Y軸の回転のみ考慮するように水平ベクトルを計算
            Vector3 direction = new Vector3(velocity.x, 0, velocity.z).normalized;

            // 進行方向を計算
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 徐々に回転するように補間
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
        }

        // Animatorのspeedパラメータを更新
        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    // タスクを登録
    public void AddTask(System.Action task)
    {
        taskQueue.Enqueue(task);
        if (!isProcessing)
        {
            ProcessNextTask();
        }
    }

    // 次のタスクを実行
    private void ProcessNextTask()
    {
        if (taskQueue.Count > 0)
        {
            isProcessing = true;
            var task = taskQueue.Dequeue();
            task.Invoke();
        }
        else
        {
            isProcessing = false;
        }
    }

    // 現在のタスクを完了
    private void CompleteCurrentTask()
    {
        agent.isStopped = true;
        ProcessNextTask();
    }

    // 特定地点に移動するタスクを登録
    public void MoveTo(Transform target, bool run = false)
    {
        AddTask(() =>
        {
            agent.isStopped = false;
            agent.speed = (run ? runSpeed : walkSpeed)*WorldInfo.scale;
            agent.SetDestination(target.position);
        });
    }

    // 会話タスクを登録
    public void Talk(float duration = 3f)
    {
        AddTask(() =>
        {
            agent.isStopped = true;
            animator.SetTrigger("Talk");
            Invoke(nameof(CompleteCurrentTask), duration);
        });
    }

    // 停止タスクを登録
    public void Idle(float duration = 2f)
    {
        AddTask(() =>
        {
            agent.isStopped = true;
            animator.SetFloat("speed", 0f);
            Invoke(nameof(CompleteCurrentTask), duration);
        });
    }
}
