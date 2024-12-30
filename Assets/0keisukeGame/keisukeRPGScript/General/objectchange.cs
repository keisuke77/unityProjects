using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;  
public class ObjectChange : MonoBehaviour
{
 
    public GameObject[] objList;

    private int activeIndex = 0;
    private GameObject currentObj;

	public int firstIndex;

    private void Start()
    {
        HideObjects();
		ChangeToIndex(firstIndex);
    }

    private void Update()
    {
        // ここではUpdate内での処理は見受けられませんが、必要に応じて追加してください。
    }

    // オブジェクトの切り替えを行います
    public void Change()
    {
        activeIndex++;
        if (activeIndex >= objList.Length)
        {
            activeIndex = 0;
        }
        ChangeObject();
    }

    // 特定のインデックスに基づいてオブジェクトを切り替えます
    public void ChangeToIndex(int index)
    {
        if (index < 0 || index >= objList.Length) return;

        activeIndex = index;
        ChangeObject();
    } 
   // 名前でオブジェクトを切り替えます
    public void ChangeByName(string name)
    {
        int index = System.Array.FindIndex(objList, obj => obj.name == name);
        if (index >= 0)
        {
            ChangeToIndex(index);
        }
    }

    // 現在アクティブなオブジェクトを取得します
    public GameObject GetCurrentObject()
    {
        return currentObj;
    }

    // 全てのオブジェクトを非表示にします
    private void HideObjects()
    {
        foreach (var obj in objList)
        {
            obj.SetActive(false);
        }
        currentObj = null;
    }

    // アクティブなオブジェクトを変更します
    private void ChangeObject()
    {
        HideObjects();
        objList[activeIndex].SetActive(true);
        currentObj = objList[activeIndex];
    }
}
