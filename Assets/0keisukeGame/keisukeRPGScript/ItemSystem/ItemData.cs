using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;  // アイテムの名前
    [TextArea]
    public string description;  // アイテムの説明
    public Sprite icon;  // アイコン
    public int price;  // 値段
    public int stocknum;
    public Buf buf;
    [Header("バフの強さ")]
    public int Strong;
}

public enum Buf{
    Power,HP,Speed,Guard,MP
}