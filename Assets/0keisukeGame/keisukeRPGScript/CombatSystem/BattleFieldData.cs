using UnityEngine;
[CreateAssetMenu(fileName = " BattleFieldData", menuName = "ScriptableObjects/JavaScriptSnippet", order = 1)]
public class BattleFieldData : ScriptableObject
{
    public SequenceBattleData sequenceBattleData;
    public void Set(SequenceBattleData sequenceBattleData)
    {
        this.sequenceBattleData = sequenceBattleData;
    }

}
