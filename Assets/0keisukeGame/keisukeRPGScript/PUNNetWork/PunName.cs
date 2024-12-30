using Photon.Pun;
using TMPro;
using UnityEngine;

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class PunName : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI nameLabel;
    private void Start() {
   
        // プレイヤー名とプレイヤーIDを表示する
        nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
        gameObject.root().name=photonView.Owner.NickName;
    }
    private void Update() {
       transform.forward= Camera.main.transform.forward;
    }
}