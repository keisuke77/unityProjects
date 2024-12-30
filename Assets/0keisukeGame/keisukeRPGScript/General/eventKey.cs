using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

using System.Collections;

[System.Serializable]
public class MultKey
{
    public List<KeyCode> keyCodes;
    public bool Execute(){
        if (keyCodes.Count==0)
        {
            return false;
        }
        bool down=true;
        foreach (var item in keyCodes)
        {
            if (!Input.GetKey(item))
        {
            down=false;
        }  
        }
      return down;
    }

}
public class eventKey : MonoBehaviour
{

public KeyCode key;
public MultKey multKey;
public controll controll;
public UnityEvent eve;
    void Update()
    {
        if (multKey!=null)
        {
            if (multKey.Execute())
            {
                 eve.Invoke();
            }
        }
        if (key.keydown())
        {
            eve.Invoke();
        }
        if (keiinput.Instance.GetKey(controll))
        {
             eve.Invoke();
        }
    }
}