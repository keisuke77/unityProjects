using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System;

[System.Serializable]
public class DelayEvent
{
    public float DelayTime;
    public UnityEvent events;
public bool OnceCall;
bool once;
    public void Execute(){
        if (events==null)
        {
            return;
        }
        if (OnceCall)
        {
            if (!once)
            {once=true;
                keikei.delaycall(()=>events?.Invoke(),DelayTime);
    
            }
                 }else
        {
             keikei.delaycall(()=>events?.Invoke(),DelayTime);
  
        }

           }
  
}
[System.Serializable]
public class IDEvents{
    [Serializable]
    public struct pair
    {
    public DelayEvents Event;
    public int Id;
    }
   public List<pair> pairs;
public void Execute(int id){
    if(pairs==null)return;
    foreach (var item in pairs)
    {if (item.Id.Equals(id))
    {
        item.Event.Execute();
    }
        
    }
}

}

[System.Serializable]
public class DelayEvents
{
    public List<DelayEvent> delayEvents;
    public void Execute(){
       delayEvents?.ForEach(x=>x?.Execute());
    }

}
public class talk : StaticEventBehavior
{
    public Button button;
     public IconGenerate IconGenerate;
    GameObject Talker;

    public IDEvents TalkIDEvents;
    public void CallEvent(int n)=>TalkIDEvents.Execute(n);

    public static talk nowTalk;
public ChatCameras ChatCameras;

public Transform LookTalkBefore;

    [Button( "TalkEvent", "実行")]
public int a;
public Quaternion DefaultRotation,DefaultRotationLookAtBefore;
       public bool isTalking;

          public bool BeforeLookAt;
public List<controll> controll;
    // Start is called before the first frame update
    void Start()
    {
        ChatCameras.chatCameras.ForEach((x)=>x.ChatData.phases.ForEach((a)=>a.SelectionPhases.ForEach((b)=>b.CallBack=this.CallEvent)));
        DefaultRotation=transform.rotation;
    if (LookTalkBefore!=null) DefaultRotationLookAtBefore=LookTalkBefore.rotation;
      IconGenerate.SetUp(gameObject);
        Exit();
    }


	GameObject temp;

     public void TalkEvent(GameObject obj){
Talker=obj;
TalkEvent();
     }
     
    public void TalkEvent()
    {    
        Talker.transform.DOLookAt(transform.position, 1, AxisConstraint.Y);
         transform.DOLookAt(Talker.transform.position, 1, AxisConstraint.Y);
        
        Talker.Stop();
          Talker.root().GetComponent<AnimBoolSet>().Stop = true; 
        isTalking=true;
         temp=Talker;
      StaticEventCall(0);
      
var BeforeParam=CameraManager.instance.CloneParam;
        ChatCameras.Execute(0);
          
        ChatCameras.EndCall=()=>{
       StaticEventCall(1);
      
     CameraManager.instance.TweenPram(BeforeParam);
       isTalking=false;
                temp.Restart();
                temp.root().GetComponent<AnimBoolSet>().Stop = false; 
               transform.DORotate(DefaultRotation.eulerAngles, 1);
      
        };  Exit(false);
    
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.proottag())
        {   
            Talker = collisionInfo.gameObject; 
            nowTalk=this;
   
              if (button != null)
            {
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() =>
                {
                    TalkEvent();
                    return;
                });
            }
            IconGenerate.On();
        }
    }

    public void Exit(bool OriRotate=true)
    {   if (OriRotate)
    {
          transform.DORotate(DefaultRotation.eulerAngles, 1);
     
    } 
        
        IconGenerate.Off();
        Talker = null;
           if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.proottag())
        {
            Exit();

        }
    }
 
    // Update is called once per frame
    void Update()
    {
        if (Talker != null)
        {
            if (keiinput.Instance.GetKeys(controll))
            {
                nowTalk.TalkEvent();
            }
            if (BeforeLookAt&&Vector3.Angle(transform.forward,Talker.transform.position-transform.position)<100)
            {       Debug.Log(Vector3.Angle(transform.forward,Talker.transform.position-transform.position));
                LookTalkBefore?.DOLookAt(Talker.transform.position, 1, AxisConstraint.Y);
            }
               }else
               {
                if (!isTalking&&BeforeLookAt)LookTalkBefore?.DORotate(DefaultRotationLookAtBefore.eulerAngles, 1);
    
               }
    }
}
