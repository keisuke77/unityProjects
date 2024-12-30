using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
[System.Serializable]
public class PUNSpawnObj
{
    [Header("リソースフォルダ内のprefab名")]
       public string Name;

    public Vector3 InstantiatePos;
public GameObject obj;
public void execute(){

PhotonNetwork.IsMessageQueueRunning = true;
obj=  PhotonNetwork.Instantiate(Name, InstantiatePos, Quaternion.identity);

}
}

public class PUNSpawn : MonoBehaviour 
{
public List<PUNSpawnObj> objs;

private void Start() {
    foreach (var item in objs)
    {
        item.execute();
    }
}

  
}