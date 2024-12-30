using UnityEngine;

public class InputCameraCircle : MonoBehaviour
{
    public float radius = 5.0f; // 円の半径
    public float speed = 1.0f;  // 円運動の速度の倍率

    private float angleX = 0.0f; // X軸回転の角度
    private float anglez = 0.0f; // Y軸回転の角度
public CameraFollow cameraFollow;
public controll xaxis;
public Transform lookat;
    void Update()
    {
        // 入力を取得
        float inputX = keiinput.Instance.GetAxis(xaxis); // デフォルトで矢印キーとA/Dキーに対応
     
        // 入力に基づいて角度を更新
        angleX += inputX * speed * Time.deltaTime;
      
        // X軸とY軸周りの円運動を計算
        float x = radius * Mathf.Cos(angleX);
        float z = radius * Mathf.Sin(angleX);

        // カメラの位置を更新
        cameraFollow.offset = new Vector3(x, cameraFollow.offset.y, z);

        // カメラが常に中心を向くようにする
        transform.LookAt(lookat.position);
    }
}
