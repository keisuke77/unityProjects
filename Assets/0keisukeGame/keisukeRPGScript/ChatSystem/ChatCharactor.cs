using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RPG/ChatCharactor", menuName = "New Unity Project (1)/ChatCharactor", order = 0)]
public class ChatCharactor : ScriptableObject {

    [Header("名前")]
    public string name;

   

    [Header("アイコン")]
    public Sprite icon;

    [Header("移動速度（実際の）")]
    public float Speed = 1;

    [Header("歩行速度（アニメーションの）")]
    public float WalkSpeed;

    [Header("走行速度（アニメーションの）")]
    public float RunSpeed;

    [Header("ジャンプ力")]
    public float JumpPower;

    [Header("立ち速度（アニメーションの）")]
    public float IdleSpeed;

    [Header("最大HP")]
    public int MaxHP;

    [Header("現在のHP")]
    public int CurrentHP;

    [Header("攻撃力")]
    public int Power;

    [Header("ノックバック力")]
    public float knockBack;

    [Header("防御力")]
    public int Defence;

    [Header("説明")]
    public string Explain;

    public float HpPer=>(float)CurrentHP/MaxHP;
     public string animLayerName;
}