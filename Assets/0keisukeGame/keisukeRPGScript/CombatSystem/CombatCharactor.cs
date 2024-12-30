using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "RPG/CombatCharactor", menuName = "New Unity Project (1)/CombatCharactor", order = 0)]
public class CombatCharactor :ScriptableObject{

    public List<HissatuWazaData> hissatuWazaDatas;

    public ChatCharactor chatCharactor;

    public WazaHpChange WazaHpChange;
    public float attackCoolTime,guardCoolTime,dashCoolTime;
}