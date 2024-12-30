using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine.Events;

public class message : MonoBehaviour
{
    //　表示するメッセージ
    [SerializeField]
    [TextArea(1, 20)]
    private string allMessage =
        "今回はRPGでよく使われるメッセージ表示機能を作りたいと思います。\n"
        + "メッセージが表示されるスピードの調節も可能であり、改行にも対応します。\n"
        + "改善の余地がかなりありますが、               最低限の機能は備えていると思われます。\n"
        + "ぜひ活用してみてください。\n<>"
        + "あああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああ"
        + "あああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああああ";

    //　使用する分割文字列
    [SerializeField]
    private string splitString = "<>";
    public System.Action MessageEndCallback;
    public UnityEvent MessageEndEvent;
   

    //　テキストスピード
    [SerializeField]
    private float textSpeed = 0.1f;

    public Canvas messagecanvas;
    public bool auto;
    public Image icon;
    
    [Header("絶対必要")]
    public Text text;
    public Text name;
    [Header("絶対必要")]
    public CanvasGroup canvasGroup;

    //　マウスクリックを促すアイコン
    public List<controll> controlls; 

    public UnityEvent messsageOnEvent;
    string[] mes;
    public int mesNum = 0;
    [Button( "Test", "実行")]
    public int ResetButton1;  
    public void Test(){
        SetMessage(allMessage);
    }
    List<controll> defaultControlls;
    void Awake()
    {
        defaultControlls = controlls;
        finish();
    }


  

    public void UIFade(float a, float duration, System.Action ac = null)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(a, duration).OnComplete(() => {

            
        if(ac!=null){
ac();
        }
        
        }
        );
    }

    public void SetMessage(string message)
    {
        SetMessagePanel(message);
    }
public Ease ease;

public void ProgressKeyChange(List<controll> controll)
{
    Debug.Log("ProgressKeyChange"+defaultControlls);
    if (controll==null)
    {
        controlls=defaultControlls;
    }else
    {
        if (controll.Count==0)
        {
            controlls=defaultControlls;
        }else
        {
             this.controlls = controll;
        }
        
    }
   
}
    public void SetMessagePanel(
        string message,
        bool autos = false,
        Sprite icons = null,
        System.Action ac = null,
        string names = "",bool anim=true
    )
    {
       
      
        if (message == ""||isMessageing==true)
        {
            return;
        }
 isMessageing=true;
 mesNum = 0;

        MessageEndCallback = ac;
        MessageEndCallback+=()=>MessageEndEvent.Invoke();
        auto = autos;
       try
        {messsageOnEvent?.Invoke();
            
        }catch{}
        if (icon!=null)
        {
             icon.sprite = icons;
        } 
        if (messagecanvas!=null&&anim)
        {
                   messagecanvas.transform.localScale = Vector3.one * 0.2f;
        messagecanvas.transform.DOScale(1f, 0.6f).SetEase(ease, 5f);
     
        }
        if (name!=null)
        {
            name.text = names;
        }
       
       
        //　分割文字列で一回に表示するメッセージを分割する
        mes = Regex.Split(
            message,
            @"\s*" + splitString + @"\s*",
            RegexOptions.IgnorePatternWhitespace
        );
        OnDown();
    }
public bool StopUodate;
    //画面とかタップされたらよばれる関数


    public void OnDown()
    {
        if (StopUodate||!isMessageing)
        {
            return;
        }
       
        //textがTweenしているかどうか
        if (DOTween.IsTweening(text))
        {
            
            text.DOComplete();
            //Tween中なら即終了
            text.DOKill(false);
        }
        else
        { 
           if (mesNum > mes.Length - 1)
            {
                finish();
                return;
            }
            //一度textを無にしてから書き込もうね
            text.text = "";
            text.DOText(mes[mesNum], mes[mesNum].Length * textSpeed);
            mesNum++;
                
        }
    }
    private void Update() {
          if (isMessageing){
        if (keiinput.Instance.GetKeys(controlls))
        {
           
                                 OnDown();
                            }
        }
        
        if (isMessageing&&switchs)
        {
            UIFade(1,0.7f);
            switchs=false;
        }
         if (!isMessageing&&!switchs)
        {
            UIFade(0,0.8f);
            switchs=true;
        }
       


    }
bool switchs;
 
public bool isMessageing=false;
    public void finish()
    {   isMessageing=false;
            if (MessageEndCallback!=null)
        {
        MessageEndCallback();
       MessageEndCallback=null;
        }
       
       
    }
}
