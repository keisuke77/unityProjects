using UnityEngine;

public class BGM : MonoBehaviour {

    public AudioClip audioClip;
    private void Start() {
        SoundManager.Instance.PlayBgm(audioClip);
    }

    public void PlaySE(AudioClip se){
         SoundManager.Instance.PlaySe(se);

    }
}