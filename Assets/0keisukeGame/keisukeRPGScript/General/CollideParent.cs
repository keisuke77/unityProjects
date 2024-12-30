using UnityEngine;

public class CollideParent : MonoBehaviour {
public Transform parentTarget;
    private void OnCollisionStay(Collision other) {
        other.transform.parent=parentTarget;
    }
    private void OnCollisionExit(Collision other) {
         other.transform.parent=null;
    }
}