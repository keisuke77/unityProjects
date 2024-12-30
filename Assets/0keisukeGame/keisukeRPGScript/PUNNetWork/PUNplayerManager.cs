using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TNRD;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
public class PUNplayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IOnPhotonViewPreNetDestroy
{


    public List<SerializableInterface<Inputable>> move;
    void Start()
    {
        if (!photonView.IsMine)
        {
            foreach (SerializableInterface<Inputable> item in move)
            {
                item.Value.isInputable = true;
            }
        }
        else
        {
            foreach (SerializableInterface<Inputable> item in move)
            {
                item.Value.isInputable = false;
            }

        }

    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            foreach (SerializableInterface<Inputable> item in move)
            {
                item.Value.isInputable = true;
            }
        }

    }
    // ネットワークオブジェクトが破棄される直前に呼ばれるコールバック
    void IOnPhotonViewPreNetDestroy.OnPreNetDestroy(PhotonView rootView)
    {
        Debug.Log($"{rootView.name}({rootView.ViewID}) が破棄されます");
        ChatExecute.Instance.message.SetMessage(rootView.Owner.NickName + "が出て行った");
    }

    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.Sender.IsLocal)
        {
            Debug.Log("自身がネットワークオブジェクトを生成しました");
        }
        else
        {
            Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
            ChatExecute.Instance.message.SetMessage(info.photonView.Owner.NickName + "がやってきたぞ");
        }

    }
}