using UnityEngine;

public class UIcameralook : MonoBehaviour
{Transform cam;
Transform trans;
     
     void Start() {
          
           cam=Camera.main.transform;
            trans=transform;
     }
     
     void FixedUpdate()
{
     trans.forward=cam.forward;
}
   
}