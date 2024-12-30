using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "SequenceBattleData", menuName = "Combat System/Sequence Battle Data")]
public class SequenceBattleData : ScriptableObject
{
    // Add your variables and properties here
   public List<Group> groups;

   public String stageName;
   public String title;
    public String description;
    public Sprite stageImage;
    public int Difficulty;
}
[System.Serializable]

public class Group{
 
 public List<CombatCharactor> CombatCharactors;

 public Vector3 PositionBetween;
 public string name;

}