 using Photon.Pun;
 using UnityEngine;
using UnityEngine.UI;

 public class PUNChatLog : MonoBehaviourPunCallbacks
 {
    public string Text;
public Text text;
private void Update() {
    text.text=Text;
}
   

  [PunRPC]
     private void RpcSendMessage(string message) {
         Text+=message;
     }
 }