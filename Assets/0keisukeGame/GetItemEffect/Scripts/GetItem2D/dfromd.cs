using UnityEngine;

public class dfromd : MonoBehaviour
{
      public GameObject obj;          //3D座標から2D座標に変換したいオブジェクト
    public GameObject confirmation; //座標がちゃんと変換されているか確認用
public Camera camera;
    void Start()
    {
        //カメラを平行投影にする ここ一番重要！透視投影のままだとうまく座標変換できません
        camera.orthographic = true;
    }

    // Update is called once per frame
    void Update()
    {
        confirmation.transform.position = Change2DPos(obj);
    }

    Vector2 Change2DPos(GameObject obj3D)
    {
        //Camera.mainになっていますが設定したいCameraがあれば変更してくさい。
        Vector2 pos2D=Camera.main.ViewportToWorldPoint(Camera.main.WorldToViewportPoint(obj3D.transform.position));
        return pos2D;
    }
}
