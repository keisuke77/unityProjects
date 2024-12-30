using UnityEngine;
using UnityEngine.Events;
//継承用
public class StaticEventBehavior : MonoBehaviour {
    public static System.Action<int> staticAction;
 public IDEvents iDEvents;
   void Awake() {
      staticAction+=(x)=>iDEvents?.Execute(x);
    }
   public void StaticEventCall(int id=0){
staticAction(id);

    }
}