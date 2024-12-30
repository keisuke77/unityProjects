using UnityEngine;

using UnityEngine.Events;
public class enableEvent : MonoBehaviour
{
    private void Start() {
        
        starteve.Invoke();
    }
private void OnEnable() {
    enaeve.Invoke();
}private void OnDisable() {
      disaeve.Invoke();
}
public UnityEvent enaeve;
public UnityEvent starteve;
public UnityEvent disaeve;
 
}