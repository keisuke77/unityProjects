using System.Collections.Generic;
using UnityEngine;

public class ItemDataAssigner : MonoBehaviour
{
    [Header("Automatically fetch all ItemData assets")]
    public List<ItemData> itemDataList; // 子オブジェクトに設定するアイテムデータのリスト

    void Start()
    {
        LoadAllItemData(); // プロジェクト内のすべてのItemDataを取得
        AssignItemData();
    }

    /// <summary>
    /// プロジェクト内のすべてのItemData ScriptableObjectを取得
    /// </summary>
    private void LoadAllItemData()
    {
        // ResourcesフォルダからすべてのItemDataをロード
        itemDataList = new List<ItemData>(Resources.LoadAll<ItemData>(""));

        if (itemDataList.Count == 0)
        {
            Debug.LogWarning("No ItemData assets found in the Resources folder. Please add ItemData to the Resources folder.", this);
        }
        else
        {
            Debug.Log($"Loaded {itemDataList.Count} ItemData assets.");
        }
    }

    /// <summary>
    /// 子オブジェクトのItemDisplayコンポーネントにItemDataを設定
    /// </summary>
    public void AssignItemData()
    {
        // 子オブジェクト以下の全てのItemDisplayコンポーネントを取得
        var itemDisplays = GetComponentsInChildren<ItemDisplay>();

        // リストとコンポーネント数の確認
        if (itemDataList.Count < itemDisplays.Length)
        {
            Debug.LogWarning("ItemData list has fewer elements than the number of ItemDisplay components. Some components will not be assigned.", this);
        }

        // アイテムデータを順番に設定
        for (int i = 0; i < itemDisplays.Length; i++)
        {
            if (i < itemDataList.Count)
            {
                itemDisplays[i].SetItem(itemDataList[i]); // ItemDisplayのSetItem関数を呼び出す
            }
            else
            {
                Debug.LogWarning($"No ItemData assigned for ItemDisplay on {itemDisplays[i].gameObject.name}.", this);
            }
        }
    }
}
