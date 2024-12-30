using UnityEngine;using UnityEngine.UI;

public class ToggleEvent : MonoBehaviour {
    public Toggle toggle;
public DelayEvent delayEventOn;
public DelayEvent delayEventOff;
    public void Execute(){
if (toggle.isOn)
{
    delayEventOn.Execute();
}else
{
    delayEventOff.Execute();
}
    }
}