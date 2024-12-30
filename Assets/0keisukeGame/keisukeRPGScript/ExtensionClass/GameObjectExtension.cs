//  GameObjectExtension.cs
//  http://kan-kikuchi.hatenablog.com/entry/GetComponentInParentAndChildren
//
//  Cr
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// GameObjectの拡張クラス
/// </summary>
/// 
/// 
/// 
/// 
/// 
/// 
public static class GameObjectExtension{
 public static Vector3 GetHeadPosition(this GameObject gameObject)
    {
        // 最初に無効な初期値を設定
        Bounds combinedBounds = new Bounds(gameObject.transform.position, Vector3.zero);

        // 自分自身とすべての子要素のRendererコンポーネントを取得
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        // バウンディングボックスを順番に統合
        if (renderers.Length > 0)
        {
            combinedBounds = renderers[0].bounds;  // 最初のRendererで初期化
            foreach (var renderer in renderers)
            {
                combinedBounds.Encapsulate(renderer.bounds);  // Boundsを統合
            }
        }

        // オブジェクト全体（親と子）を含む頭上座標を返す
        return new Vector3(gameObject.transform.position.x, combinedBounds.max.y, gameObject.transform.position.z);
    }
     public static List<T> FindObjectsWithInterface<T>() where T : class
    {
        List<T> interfaces = new List<T>();
        MonoBehaviour[] allObjects = Object.FindObjectsOfType<MonoBehaviour>();

        foreach (var obj in allObjects)
        {
            T component = obj as T;
            if (component != null)
            {
                interfaces.Add(component);
            }
        }

        return interfaces;
    }
public static Camera NowCameraGet(){
GameObjectExtension.GetComponentsInActiveScene<Camera>(out var cams,false);
        Camera temp = null;
        foreach (var item in cams)
        {
            if (temp == null)
            {
                temp = item;
            }
            if (item.depth > temp.depth)
            {
                temp = item;
            }
        }
        return temp;
}
     
   
     // 通常trueしか指定しないのでデフォルト引数をtrueにしてます
 public static bool GetComponentsInActiveScene<T>(out T[] resultComponents, bool includeInactive = true)
{
    // ActiveなSceneのRootにあるGameObject[]を取得する
    var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

    // 結果を格納するリスト
    List<T> resultList = new List<T>();
    
    foreach (var item in rootGameObjects)
    {
        // includeInactive = true を指定すると GameObject が非活性なものからも取得する
        var components = item.GetComponentsInChildren<T>(includeInactive);
        resultList.AddRange(components);
    }

    // 結果を out パラメータに設定
    resultComponents = resultList?.ToArray();
    return resultComponents!=null;
}

  public static GameObject Clone(this GameObject go )
    {
        var clone = GameObject.Instantiate( go ,go.transform.parent) as GameObject;
        return clone;
    }
 
 public static List<GameObject> GetAllChildren(this GameObject gameObject)
        {
            Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
            var allChildren = new List<GameObject>(childTransforms.Length);

            foreach (var child in childTransforms)
            {
                if (child.gameObject != gameObject) allChildren.Add(child.gameObject);
            }

            return allChildren;
        }

        public static List<GameObject> GetAllChildrenAndSelf(this GameObject gameObject)
        {
            Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
            var allChildren = new List<GameObject>(childTransforms.Length);

            foreach (var child in childTransforms)
            {
                allChildren.Add(child.gameObject);
            }

            return allChildren;
        } /// <summary>
    /// コンポーネントを削除します
    /// </summary>
    public static void RemoveComponent<T>(this Component self) where T : Component
    {
        GameObject.Destroy(self.GetComponent<T>());
    }

        public static Mesh GetMesh(this GameObject gameObject)
        {
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null) return meshFilter.sharedMesh;

            SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null) return skinnedMeshRenderer.sharedMesh;

            return null;
        }

        public static bool IsRendererEnabled(this GameObject gameObject)
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null) return meshRenderer.enabled;

            SkinnedMeshRenderer skinnedRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            if (skinnedRenderer != null) return skinnedRenderer.enabled;

            return false;
        }

        public static Bounds GetInstantiatedBounds(this GameObject prefab)
		{
			GameObject go = MonoBehaviour.Instantiate(prefab);
			go.transform.position = prefab.transform.position;
			Bounds bounds = new Bounds(go.transform.position, Vector3.zero);
			foreach (Renderer r in go.GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(r.bounds);
			}
			foreach (Collider c in go.GetComponentsInChildren<Collider>())
			{
				bounds.Encapsulate(c.bounds);
			}
			MonoBehaviour.DestroyImmediate(go);
			return bounds;
		}

  
  
  /// <summary>
  /// 親や子オブジェクトも含めた範囲から指定のコンポーネントを取得する
  /// </summary>
  public static T GetComponentInParentAndChildren<T>(this GameObject gameObject) where T : UnityEngine.Component {

    if(gameObject.GetComponentInParent<T>() != null){
      return gameObject.GetComponentInParent<T>();
    }
    if(gameObject.GetComponentInChildren<T>() != null){
      return gameObject.GetComponentInChildren<T>();
    }

    return gameObject.GetComponent<T>();
  }
  
  /// <summary>
  /// 親や子オブジェクトも含めた範囲から指定のコンポーネントを全て取得する
  /// </summary>
  public static List<T> GetComponentsInParentAndChildren<T>(this GameObject gameObject) where T : UnityEngine.Component {
    List<T> _list = new List<T>(gameObject.GetComponents<T>());

    _list.AddRange (new List<T>(gameObject.GetComponentsInChildren<T>()));
    _list.AddRange (new List<T>(gameObject.GetComponentsInParent<T>()));

    return _list;
  }  

}