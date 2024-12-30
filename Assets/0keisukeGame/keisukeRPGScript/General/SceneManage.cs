using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{ //必須クラス
    public string beforeScene;
public static SceneManage Instance;

public System.Action ReloadSceneAction;

    public void Awake()
    {

   Instance=this;

        transform.parent = null;
       
    }
void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

void OnDisable()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // ここにシーンがロードされたときの処理を書く
    ReloadSceneAction();
}


    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackScene()
    {
        LoadScene(beforeScene);
    }

    public void FadeBackScene()
    {
        FadeLoadScene(beforeScene);
    }

    public void LoadScene(string scenename)
    {
        beforeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scenename);
    }

    public void FadeLoadScene(string scenename)
    {
        Fade.LastScreenFade.FadeIn(2f, () => {LoadScene(scenename); Fade.LastScreenFade.FadeOut(2);});
         
    }

    public void FadeInoutScene(string scenename)
    {
        Fade.LastScreenFade.FadeIn(
            1f,
            () =>
            {
                LoadScene(scenename);
                ;
                keikei.delaycall(() => Fade.LastScreenFade.FadeOut(1, null), 1f);
            }
        );
    }
}
