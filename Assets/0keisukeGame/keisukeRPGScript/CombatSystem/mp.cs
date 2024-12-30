using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WebSocketSharp;

public class mp : MonoBehaviour
{
    public float maxMP = 100;
    public float MP = 100;
    public Image mpimage;
    public float increaseTime = 15f;

    public UnityEvent MpFullEvent;
    bool Frag;
    public bool ImageLink=true;

    public string imgName;

    private void Awake() {
        
        if (!imgName.IsNullOrEmpty())
        {
            mpimage=GameObject.Find(imgName).GetComponent<Image>();
        }
    }

    public float GetMpPercent()
    {
        return MP / maxMP;
    }
    void Update()
    {
      if (increaseTime!=0)
      {
          MP+= maxMP*Time.deltaTime / increaseTime;
      }
          MP = Mathf.Clamp(MP, 0, maxMP);
        if (MP==maxMP&&Frag)
        {
            Frag=false;
            MpFullEvent.Invoke();
        }else if(MP<maxMP)
        {
            Frag=true;
        }

        if (ImageLink&&mpimage != null)
        {
            mpimage.fillAmount = (float)MP / maxMP;
        }
    }
    public void Heal(float num)
    {
        MP += num;
        warning.instance?.message("気力が" + num + "回復した");
    }

    public bool mpuse(float value)
    {
        if (MP < value)
        {
               return false;
        }
        else
        {
            MP -= value;
            return true;
        }
    }
}
