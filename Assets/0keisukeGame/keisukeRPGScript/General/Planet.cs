// PlanetGravity.cs
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float gravityStrength = 10f;
    public float gravityRadius = 5f;  // 重力影響の範囲

    public Vector3 GetGravityDirection(Vector3 targetPosition)
    {
        return (transform.position - targetPosition).normalized;
    }

    public bool IsInGravityRange(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= gravityRadius;
    }
}

// PlayerGravityBody.cs
