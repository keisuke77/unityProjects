using UnityEngine;

[CreateAssetMenu(fileName = "AttackPram", menuName = "My project (1)/AttackPram", order = 0)]
public class AttackPram : ScriptableObject {
    [Header("ダメージ値")]
    public int damagevalue;
    
    [Header("ノックバック力")]
    public int forcepower;
    
    [Header("連続ヒットするかどうか")]
    public bool sequenceHit;
    
    [Header("体の部位")]
    public bodypart bodypart;
    
    [Header("コライダーサイズ")]
    public float ColliderSize = 0.1f;

    [Header("持続時間")]
    public float duration;
}