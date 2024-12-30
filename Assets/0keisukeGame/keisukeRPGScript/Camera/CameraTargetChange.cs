using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Linq;


public class CameraTargetChange : SelectBehabior<Transform>
{
    // Declare any variables or properties here
    public CameraFollow cameraFollow;
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(100, 100);
    TextureAttacher textureAttacher;
    void Awake()
    {
        textureAttacher = new TextureAttacher(texture, textureSize);

    }
    void Start(
    )
    {
        Elements = GameObject.FindGameObjectsWithTag("Enemy").Select(x => x.transform).ToList();

    }
    public override void KeyDown(){
           Elements = GameObject.FindGameObjectsWithTag("Enemy").Select(x => x.transform).ToList();
 
    }

    public override void ChangeCallBack()
    {
        cameraFollow.ChangeEnemy(CurrentElement);
        Elements = GameObject.FindGameObjectsWithTag("Enemy").Select(x => x.transform).ToList();

    }

    public override void UpdateCallBack()
    {
        
        textureAttacher.UpdatePosition(CurrentElement.position, Camera.main);

        // Update code here
    }

    // Declare any other methods or event handlers here
}
[System.Serializable]
public class TextureAttacher
{
    private Texture2D texture;
    private Vector2 textureSize;
    private RawImage rawImage;
    private RectTransform rectTransform;

    public TextureAttacher(Texture2D texture, Vector2 textureSize)
    {
        this.texture = texture;
        this.textureSize = textureSize;

        Initialize();
    }

    private void Initialize()
    {
        // UIキャンバスを探すか作成する
        Canvas canvas = null;

        GameObject canvasObject = new GameObject("Canvas");
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // テクスチャを表示するためのRawImageを作成
        GameObject imageObject = new GameObject("AttachedTexture");
        imageObject.transform.SetParent(canvas.transform);

        rectTransform = imageObject.AddComponent<RectTransform>();
        rawImage = imageObject.AddComponent<RawImage>();

        // テクスチャとサイズを設定
        rawImage.texture = texture;
        rectTransform.sizeDelta = textureSize;
    }

    public void UpdatePosition(Vector3 worldPosition, Camera camera)
    {
        // ワールド座標をスクリーン座標に変換
        Vector3 screenPosition = camera.WorldToScreenPoint(worldPosition);

        // スクリーン座標をUIの座標に変換し、テクスチャを表示
        rectTransform.position = screenPosition;
    }
}
