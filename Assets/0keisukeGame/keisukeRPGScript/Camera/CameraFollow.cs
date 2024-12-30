using System;
using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;          // プレイヤーのTransform
    public Transform enemy;           // 敵のTransform
    public float distanceFromPlayer;  // プレイヤーからカメラまでの距離
    public Vector3 offset;            // カメラのオフセット
    public LayerMask obstacleLayer;   // 障害物のレイヤー

    public float smoothSpeed = 5f;    // カメラの移動スピード

    public controll axisX;
    public bool axisReverse;
    public float closePer = 1;

    public float RotateSpeed = 1;
    public Camera maincamera;
    public float AngleFree = 180;
    public float angle;
    Vector3 GetCameraPos(Transform target)
    {
        Vector3 directionFromEnemyToTarget = (target.position - enemy.position).normalized;
        float dis = Vector3.Distance(target.position, enemy.position);
        Vector3 desiredCameraPosition = target.position + directionFromEnemyToTarget * distanceFromPlayer / MathF.Max(1, (dis / closePer)) + offset;

        if (Physics.Linecast(target.position, desiredCameraPosition, out RaycastHit hit, obstacleLayer))
        {
            desiredCameraPosition = hit.point;
        }
        desiredCameraPosition.y = transform.position.y;



        angle += keiinput.Instance.GetAxis(axisX) * RotateSpeed * (axisReverse ? -1 : 1);
        angle = Mathf.Clamp(angle, -(1 / dis) * AngleFree, (1 / dis) * AngleFree);

        return desiredCameraPosition.RotateAround(target.position, angle);

    }

    void Update()
    {
        if (enemy == null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        }
        
        if (player == null || enemy == null) return;

        transform.position = Vector3.Lerp(transform.position, GetCameraPos(player), smoothSpeed * Time.deltaTime);
        transform.LookAt((enemy.position + player.position) / 2 + offset);
    }
    public bool isChangingPlayer = false;

    public bool ChangePlayer(Transform newPlayer)
    {
        if (!isChangingPlayer)
        {
            StartCoroutine(SmoothPlayerChange(newPlayer));
        }
        return !isChangingPlayer;
    }

    IEnumerator SmoothPlayerChange(Transform newPlayer)
    {
        isChangingPlayer = true;
        float originalSpeed = smoothSpeed;
        smoothSpeed = 5f; // 低い速度でスムーズに移動開始
        player = newPlayer; // プレイヤーを切り替え
        yield return new WaitForSeconds(1.0f); // 1秒待つ
        smoothSpeed = originalSpeed; // 元の速度に戻す
        isChangingPlayer = false;
    }
    public bool isChangingEnemy = false;
    public bool ChangeEnemy(Transform newEnemy)
    {
        if (!isChangingEnemy)
        {
            StartCoroutine(SmoothEnemyChange(newEnemy));
        }
        return !isChangingEnemy;
    }

    IEnumerator SmoothEnemyChange(Transform newEnemy)
    {
        isChangingEnemy = true;
        float originalSpeed = smoothSpeed;
        smoothSpeed = 5f; // 低い速度でスムーズに移動開始
        enemy = newEnemy; // プレイヤーを切り替え
        yield return new WaitForSeconds(1.0f); // 1秒待つ
        smoothSpeed = originalSpeed; // 元の速度に戻す
        isChangingEnemy = false;
    }
}
