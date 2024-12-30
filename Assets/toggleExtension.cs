using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleExtension : MonoBehaviour
{
    // Start is called before the first frame update
    Toggle toggle;
    public static bool StaticBool;
    void Start()
    {
        toggle=GetComponent<Toggle>();
    }
public void ToggleChange(){

    toggle.isOn=!toggle.isOn;
    StaticBool=toggle.isOn;
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
