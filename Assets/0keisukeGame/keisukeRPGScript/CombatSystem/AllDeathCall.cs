using System.Collections.Generic;
using UnityEngine;

public class AllDeathCall : MonoBehaviour
{
 
    private void Update()
    {
        CheckAllDeath(characters);
    }
    public List<GameObject> characters;
    public bool allDead=false;

    public DelayEvents delayEvents;
    public System.Action AllDeathCallBack;

    public void ChangeCharacters(List<GameObject> characters)
    {
        this.characters = characters;
        allDead = false;
    }
    public void CheckAllDeath(List<GameObject> characters)
    {
        foreach (GameObject character in characters)
        {
            hpcore stats = character.GetComponent<hpcore>();
            if (stats != null && stats.HP > 0)
            {
                return;
            }
        }
        OnAllDeath();
    }

    private void OnAllDeath()
    {
        Debug.Log("全てのキャラクターが死亡しました。");
        if (!allDead)
        {
            allDead = true;
            // ここでイベントを呼び出す
            delayEvents?.Execute();
            AllDeathCallBack?.Invoke();
        }
        // ここでイベントを呼び
    }


}