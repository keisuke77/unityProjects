using UnityEngine;

public class forces : MonoBehaviour
{
    public float rotateSpeed=1;
    public float thrust=1;
    public Rigidbody rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main; // 主カメラを取得する
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // カメラの方向に基づいて入力を変換する
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        
        camForward.y = 0; // 上下の移動を防ぐためにyを0に設定
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // 変換された入力を使用してキャラクターを移動させる
        Vector3 moveDirection = (camForward * v + camRight * h).normalized;

        rb.AddForce(moveDirection * thrust);
        transform.Rotate(0, h * rotateSpeed, 0);
    }
}
