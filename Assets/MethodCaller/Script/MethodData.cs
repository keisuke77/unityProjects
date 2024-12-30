using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "MethodData", menuName = "ScriptableObjects/methodcaller", order = 1)]
public class MethodData : ScriptableObject
{


    public string methodName;
    public Object hikisuu;

    public void Set(string methodName, Object hikisuu)
    {
        this.methodName = methodName;
        this.hikisuu = hikisuu;
    }

    public void Execute()
    {
       var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
       var allObjects = currentScene.GetRootGameObjects();
       allObjects.ToList().ForEach(x => x.SendMessage(methodName, hikisuu, SendMessageOptions.DontRequireReceiver));
    }
}
