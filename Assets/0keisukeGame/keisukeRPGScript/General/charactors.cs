using UnityEngine;

[CreateAssetMenu(fileName = "charactors", menuName = "charactorscreate")]
public class charactors : ScriptableObject
{
    
[System.Serializable]
public class character{
public string name;
public GameObject bone;
public GameObject mesh;
public Avatar avatar;
public GameObject righthand;
public GameObject lefthand;
public GameObject rightfoot;
public GameObject leftfoot;
public transformdata weaponstransform;


   }
    
}