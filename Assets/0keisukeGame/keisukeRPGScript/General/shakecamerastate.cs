using UnityEngine;

public class shakecamerastate : StateMachineBehaviour {

    public ShakePram shakePram;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
       var a=Camera.main.gameObject.GetComponent<CameraShake>();
       a.Shake(shakePram);
    }
}