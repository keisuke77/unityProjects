using UnityEngine;

public class MaterialReplacer : MonoBehaviour
{
    public GameObject targetObject;    // マテリアルを変更する対象のオブジェクト
    public Material originalMaterial;  // 置き換えられる元のマテリアル
    public Material newMaterial;       // 新しいマテリアル

    // originalMaterialに設定されている部分をnewMaterialに置き換える
    public void SetToNew()
    {
        ReplaceMaterial(targetObject, originalMaterial.name, newMaterial);
    }

    // newMaterialに設定されている部分をoriginalMaterialに置き換える
    public void SetToOriginal()
    {
        ReplaceMaterial(targetObject, newMaterial.name, originalMaterial);
    }

    private void ReplaceMaterial(GameObject obj, string targetMaterialName, Material replacement)
    {
        Debug.Log("replace");
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null && rend.materials != null)
        {
            Material[] mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.Split(' ')[0] == targetMaterialName) // Split is used to handle the case where Unity appends " (Instance)" to the material name
                {
                    mats[i] = replacement;
                }
            }
            rend.materials = mats;
        }
    }
}
