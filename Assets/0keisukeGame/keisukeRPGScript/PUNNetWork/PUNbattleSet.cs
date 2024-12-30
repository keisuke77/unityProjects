using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class PUNbattleSet : MonoBehaviour 
{
   public string AvatorName="DeskWorld";

       public Vector3 InstantiatePos;

   GameObject player;
    void Awake() {
    PhotonNetwork.IsMessageQueueRunning = true;
    player=  PhotonNetwork.Instantiate(AvatorName, InstantiatePos, Quaternion.identity);
    var controller = FindFirstObjectByType<ControllPlayerChange>();
    controller.Elements.Add(new ControllPlayer(player));

    }

   public void message(){
        
    }
}