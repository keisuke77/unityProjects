using UnityEngine;

public class GroundCast : MonoBehaviour {
      public float groundDistance = 0.2f; // Adjust as needed
 public LayerMask groundMask;
  public bool isGrounded;

  public Vector3 CharactorOffSet;
  
    public RaycastHit hit;
  
 public bool Custom(float dis){
   return Physics.Raycast(transform.position+CharactorOffSet, -transform.up, out hit, dis, groundMask);

  }
    void Update() {
 isGrounded = Physics.Raycast(transform.position+CharactorOffSet, -transform.up, out hit, groundDistance, groundMask);

       
  }
}