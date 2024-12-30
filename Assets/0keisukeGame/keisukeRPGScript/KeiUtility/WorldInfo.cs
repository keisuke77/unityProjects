using UnityEngine;

public class WorldInfo:MonoBehaviour
{
    public float Scale = 1;
    public static float scale = 1;
public static bool DataSave;
    public bool datasave;




    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        DataSave = datasave;
        scale = Scale;
    }
}