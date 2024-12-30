using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class PUNdeskWorldStart : MonoBehaviour 
{
   public string AvatorName="DeskWorld";

    public Vector3 InstantiatePos;

    public CameraManager cameraManager;

   GameObject player;
    private void Start() {
          PhotonNetwork.IsMessageQueueRunning = true;
    player=  PhotonNetwork.Instantiate(AvatorName, InstantiatePos, Quaternion.identity);

      cameraManager.Param.trackTarget=player.transform;
   
    
    }

   public void message(){
        
    }
}