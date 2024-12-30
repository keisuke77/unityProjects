using UnityEngine;

public class PlayerGravityBody : MonoBehaviour
{
    public PlanetGravity planet;
    public float gravityAcceleration = 1f;
    public float maxFallSpeed = 10f;
    public LayerMask groundLayer; // Ground layers for raycast

    private Vector3 velocity = Vector3.zero;

 

  void Update() 
    {
        if (planet.IsInGravityRange(transform.position))
        {
            if (!IsGrounded())
            {
                ApplyGravity();
                MovePlayer();
            }

            RotateToGravityDirection();
        }
        else
        {
            // If not in gravity range, reset velocity
            velocity = Vector3.zero;
        }
    }

    bool IsGrounded()
    {
        float distanceToGround = 1.1f;
        return Physics.Raycast(transform.position, -transform.up, distanceToGround, groundLayer);
    }

    void ApplyGravity()
    {
        Vector3 gravityDirection = planet.GetGravityDirection(transform.position);
        velocity += gravityDirection * gravityAcceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxFallSpeed);
    }

    void MovePlayer()
    {
       transform.position += velocity * Time.deltaTime;
         }

    void RotateToGravityDirection()
    {
        Vector3 gravityDirection = planet.GetGravityDirection(transform.position);
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }
}
