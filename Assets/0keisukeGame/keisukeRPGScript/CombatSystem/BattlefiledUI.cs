using UnityEngine;
using UnityEngine.UI;
public class BattlefieldUI : MonoBehaviour
{
    public BattleFieldData battleFieldData;
    public Image BattleStage;
    public TMPro.TextMeshProUGUI BattleName;

    // Update is called once per frame
    void Update()
    {if (battleFieldData != null)
    {
        BattleStage.sprite = battleFieldData.sequenceBattleData.stageImage;
        BattleName.text = battleFieldData.sequenceBattleData.stageName;
    }
    }
}