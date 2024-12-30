using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
[System.Serializable]
public enum Emotion
{
    Normal,Anger,Sad,Surprised,hurry,happy,wander
}
[System.Serializable]
public class objAnimation
{
   public string objName;
   public AnimationClip animationClip;
   public Emotion emotion;
   public async Task<GameObject> Execute(ChatCharactor chatCharactor){
　　　GameObject obj=GameObject.Find(objName??chatCharactor.name);
     if (obj==null)return null;
if (animationClip!=null)
{
      obj.GetComponent<Animator>()?.BlendFromCurrentState(animationClip);

}
    
      obj.GetComponent<Animator>().SetTrigger(emotion.ToString());
     // アドレスを指定して読み込む
     string assetName=emotion switch{
       Emotion.hurry=>"あせり",
       Emotion.Sad=>"かなしみ",Emotion.Anger=>"いかり",Emotion.wander=>"ふしぎ",Emotion.Surprised=>"びっくり",Emotion.happy=>"よろこび",_=>null

     }; 

         if (assetName != null)
        {var handle = Addressables.InstantiateAsync(assetName,obj.transform); 
         await handle.Task; // Wait for the instantiation to complete
         handle.Result.destroy(6);
         return handle.Result;
      
        }
       return null;

   }
}


 [System.Serializable]
    public class phase
    {
        [TextArea]
    public string message;
    public List<objAnimation> objAnimations;
    public ChatCharactor ChatCharactor;
   public List<SelectionPhase> SelectionPhases;

   public List<controll> ProgressKey;

   public float delayTime;

    }
    
     [System.Serializable]
     public class SelectionPhase
    {
    public List<phase> phases;
    public string text;
    
    public int Id;
    public System.Action<int> CallBack;
     public void callBack()=>CallBack.Invoke(Id);
   
    }

 [System.Serializable]
    public class ChatDataAction
    {
        public ChatData chatData;
        public UnityEvent EndEvent;
public float StartDelayTime;

      public void Play(){
     
              
        }
    }
[CreateAssetMenu(fileName = "RPG/ChatData", menuName = "New Unity Project (1)/ChatData", order = 0)]
public class ChatData : ScriptableObject {

public List<phase> phases;

}