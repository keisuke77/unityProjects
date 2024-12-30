using UnityEngine;
using UnityEngine.UI;

public class SliderContololer : MonoBehaviour {
    
    public AxisValue positive,negative;

public float Sensivilty=1;
Slider slider;
private void Awake() {
    slider=GetComponent<Slider>();
}
private void Update() {
    if (positive.Execute())
    {
        slider.value+=Sensivilty;
    }
      if (negative.Execute())
    {
        slider.value-=Sensivilty;
    }
}
}