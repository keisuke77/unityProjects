
using UnityEngine; using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] public class DriftNavMeshController : MonoBehaviour
 { public float maxDriftAngle = 30f; public float rotationSpeed = 2f;
private NavMeshAgent agent;
private bool isDriftingLeft;

void Start()
{
    agent = GetComponent<NavMeshAgent>();
    agent.updateRotation = false;
    agent.angularSpeed = 0; // エージェントの回転速度を0に設定
    isDriftingLeft = false; // ドリフトの方向を初期化
}

void Update()
{
    if (!agent.pathPending && agent.remainingDistance <= 0.5f)
    {
        // 目的地に近づいた場合、ドリフトを停止
        return;
    }

    float currentSpeedRatio = agent.velocity.magnitude / agent.speed;
    float currentDriftAngle = maxDriftAngle * currentSpeedRatio;

    Vector3 toTarget = (agent.destination - transform.position).normalized;
    Quaternion targetRotation = Quaternion.LookRotation(toTarget);
    Quaternion driftRotation;

    // 目的地との角度を計算
    float angleToTarget = Vector3.Angle(transform.forward, toTarget);

    // ドリフトの方向を決める
    if (angleToTarget > 90f)
    {
        // 目的地が後ろにある場合、ドリフトの方向を反転
        isDriftingLeft = !isDriftingLeft;
    }
    else if (angleToTarget > 45f)
    {
        // 目的地が横にある場合、ドリフトの方向をランダムに決める
        isDriftingLeft = Random.value > 0.5f;
    }
    else
    {
        // 目的地が前にある場合、ドリフトの方向を変えない
    }

    // ドリフトの角度を調整
    if (agent.remainingDistance < 1f)
    {
        // 目的地が近い場合、ドリフトの角度を大きくする
        currentDriftAngle *= 2f;
    }
    else if (agent.remainingDistance > 5f)
    {
        // 目的地が遠い場合、ドリフトの角度を小さくする
        currentDriftAngle *= 0.5f;
    }

    if (isDriftingLeft)
    {
        driftRotation = Quaternion.Euler(0, -currentDriftAngle, 0) * targetRotation;
    }
    else
    {
        driftRotation = Quaternion.Euler(0, currentDriftAngle, 0) * targetRotation;
    }

    transform.rotation = Quaternion.Slerp(transform.rotation, driftRotation, rotationSpeed * Time.deltaTime);
}
 }