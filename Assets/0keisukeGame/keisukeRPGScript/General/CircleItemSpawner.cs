using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;         // スポーンさせるアイテムのプレハブ
    public float spawnRadius = 5f;        // スポーンさせる円の半径
    public float spawnInterval = 2f;      // スポーンの周期
    public int maxItemCount = 10;         // 存在できる最大アイテム数
    public float minDistanceFromHpcore = 3f; // hpcoreコンポーネントを持つオブジェクトからの最小距離

    private List<GameObject> spawnedItems = new List<GameObject>();
    private float nextSpawnTime = 0f;

    void Update()
    {

   
       if (Time.time >= nextSpawnTime && spawnedItems.Count < maxItemCount)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition()+transform.position;
            if (IsFarEnoughFromHpcore(spawnPosition))
            {
                SpawnItem(spawnPosition);
                nextSpawnTime = Time.time + spawnInterval;
            }
        }  // null（削除されたオブジェクト）をリストから一括で削除
        spawnedItems?.RemoveAll(item => item == null);
    
       
    }

    // 円周上にランダムなスポーン位置を取得
    Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Cos(angle) * spawnRadius;
        float z = Mathf.Sin(angle) * spawnRadius;
        return new Vector3(x, 0, z);  // 高さは0固定
    }

    // スポーン位置がhpcoreから十分離れているか確認
    bool IsFarEnoughFromHpcore(Vector3 spawnPosition)
    {
        foreach (var hpcore in FindObjectsOfType<hpcore>())
        {
            if (Vector3.Distance(spawnPosition, hpcore.transform.position) < minDistanceFromHpcore)
            {
                return false;
            }
        }
        return true;
    }
    

    // アイテムをスポーンさせる
    void SpawnItem(Vector3 spawnPosition)
    {
        GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        spawnedItems.Add(newItem);
    }
}
