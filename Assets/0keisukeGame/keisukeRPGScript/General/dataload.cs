using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PositionDataLoad : MonoBehaviour
{
   public Transform player;

   public bool AutoLoad;
private void Awake() {
  if (AutoLoad)
  {
    Load();
  }
}
[Button("Reset","Reset")]
public int x;[Button("Load","Load")]
public int y;[Button("Save","Save")]
public int z;
 public void Reset(){
    
   PlayerPrefs.DeleteKey("PlayerX");PlayerPrefs.DeleteKey("PlayerY");PlayerPrefs.DeleteKey("PlayerZ");
   }

  public void Save(){
    
    PlayerPrefs.SetFloat("PlayerX",player.position.x);
    PlayerPrefs.SetFloat("PlayerY",player.position.y);
    PlayerPrefs.SetFloat("PlayerZ",player.position.z);
   }
     public void Load(){
     if(PlayerPrefs.HasKey("PlayerX"))
        {
             Vector3 newPosition = Vector3.zero;
 newPosition.x = PlayerPrefs.GetFloat("PlayerX");
 newPosition.y = PlayerPrefs.GetFloat("PlayerY");
 newPosition.z = PlayerPrefs.GetFloat("PlayerZ");
 player.position = newPosition;
        }

     }    

  

}
