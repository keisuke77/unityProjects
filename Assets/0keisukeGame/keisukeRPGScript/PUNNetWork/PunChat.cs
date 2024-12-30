using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TNRD;
using System.Collections.Generic;
using System.Linq;

public class PunChat : MonoBehaviour {
    
public InputField inputField;
public Text ChatData;

public PhotonTextView photonTextView;

    public void Send(){

      photonTextView.Text=  photonTextView.Text+ "\n"+PhotonNetwork.NickName+" ["+inputField.text+"]";
      inputField.text=null;
    }
     void Update() {
    ChatData.text= photonTextView.Text;  
    }
}