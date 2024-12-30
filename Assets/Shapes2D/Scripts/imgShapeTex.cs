using UnityEngine;using UnityEngine.UI;

public class imgShapeTex : MonoBehaviour {
    public Image img;
public Shapes2D.Shape shape;
    private void Update() {
        shape.settings.fillTexture=  img.sprite.texture;
    }
}