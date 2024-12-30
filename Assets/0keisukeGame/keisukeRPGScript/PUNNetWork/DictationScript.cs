#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Photon.Pun;


public class DictationScript : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private TMPro.TextMeshProUGUI? m_Hypotheses;

    [SerializeField]
    private TMPro.TextMeshProUGUI? m_Recognitions;

    private DictationRecognizer m_DictationRecognizer;

string before;
     void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(before);
        } else {
           before = (string)stream.ReceiveNext(); 
        }
        
    }


public void dictationEnd(DictationCompletionCause cause){
 m_Hypotheses.text="";  
 m_Recognitions.text="";
 PhotonTextView.instance.Text=PhotonTextView.instance.Text+photonView.Owner.NickName+"\n"+before;
 before="";
    Debug.Log(cause);

    m_DictationRecognizer.Start();
}


string InsertLineBreaks(string input, int maxLength)
{
    string result = "";
    int index = 0;
    while (index < input.Length)
    {
        result += input.Substring(index, Mathf.Min(maxLength, input.Length - index)) + "\n";
        index += maxLength;
    }
    return result;
}

 void Update() {
 m_Recognitions.text=before;
}
    void Start()
    {
       if (!photonView.IsMine)return;

Debug.Log("Setupdicataion");
        m_DictationRecognizer = new DictationRecognizer();
m_DictationRecognizer.DictationComplete+=dictationEnd;

        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
          
           text = InsertLineBreaks(text, 20);
        before=before+text;
              };

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            Debug.LogFormat("Dictation hypothesis: {0}", text);
            m_Hypotheses.text += text;
        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        m_DictationRecognizer.Start();
    }
}
#endif