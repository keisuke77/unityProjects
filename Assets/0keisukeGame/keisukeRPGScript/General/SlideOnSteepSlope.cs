using UnityEngine;

public class SlideOnSteepSlope : MonoBehaviour
{
    private float maxClimbAngle = 45.0f;
    public float dashClimbAngle, defaultClimbAngle = 45.0f;
    public float slidingSpeed = 5.0f;  // 基本の滑る速度
    public float slidingSpeedIncreaseFactor = 0.5f;  // 坂の角度が急であるほど加速するための係数
    public Transform leg;
    public Jump jump;
    public dashgage dashgage;

    void FixedUpdate()
    {
        if (jump.isJumping) return;
        if (dashgage.isDashing) { maxClimbAngle = dashClimbAngle; } else { maxClimbAngle = defaultClimbAngle; }

        if (IsOnSteepSlope(out Vector3 slopeNormal, out float slopeAngle))
        {
            // 坂の角度が急であるほどスピードを上げる
            float adjustedSlidingSpeed = slidingSpeed + (slopeAngle - maxClimbAngle) * slidingSpeedIncreaseFactor;
            Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, slopeNormal).normalized;
            transform.position += slideDirection * adjustedSlidingSpeed * Time.deltaTime;
        }
    }

    bool IsOnSteepSlope(out Vector3 slopeNormal, out float slopeAngle)
    {
        RaycastHit hit;

        if (Physics.Raycast(leg.position, Vector3.down, out hit, 1.5f))
        {
            slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            slopeNormal = hit.normal;
            if (slopeAngle > maxClimbAngle)
            {
                return true;
            }
        }

        slopeNormal = Vector3.up;
        slopeAngle = 0.0f;
        return false;
    }
}
