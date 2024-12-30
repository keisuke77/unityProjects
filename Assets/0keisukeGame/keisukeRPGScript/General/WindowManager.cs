using UnityEngine;
using System.Collections.Generic;

public class WindowManager : MonoBehaviour {
    
    public List<controll> backcontrolls;
    Window CurrentWindow; 
    public Window firstWindow;
    private void Awake() {
        CurrentWindow=firstWindow;
        CurrentWindow.Setup();
    }

    public void SelectWindow(int num){
            if (CurrentWindow==null)
        {
         CurrentWindow=firstWindow;   
        }else
        {
            if (CurrentWindow.nextWindows==null)
            {
                return;
            }
        Window preWindow=CurrentWindow;
        CurrentWindow=CurrentWindow.nextWindows[num];
        CurrentWindow.Setup(preWindow);
        }
     }
    private void Update() {
        if (keiinput.Instance.GetKeys(backcontrolls))
        { 
            BackWindow();
           
        }
    }
    public void BackAndSelectWindow(int num){
        BackWindow();
        SelectWindow(num);
    }   
    public void BackWindow(){
        CurrentWindow?.Back(ref CurrentWindow);
  
        }

}