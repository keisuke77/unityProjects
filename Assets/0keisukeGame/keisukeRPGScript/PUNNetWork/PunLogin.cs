using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class PunLogin : MonoBehaviourPunCallbacks
{

   public InputField inputField;
   public Button button;

   public string firstSceneName;
    private void Start() {
        button.onClick.AddListener(Connect);
       }
    public void Connect(){
     // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
             // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = inputField.text;

        PhotonNetwork.ConnectUsingSettings();
        button.interactable = false;
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    // ルームへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        
      PhotonNetwork.IsMessageQueueRunning = false;

    SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Single);
 
    
    }
}