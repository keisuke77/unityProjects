using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
public class SequenceBattle : MonoBehaviour
{
    // Declare any variables or properties here
    public BattleFieldData CurrentStageSelect;
    private SequenceBattleData sequenceBattleData;
    public AllDeathCall allDeathCall;
    public int CurrentGroupIndex = 0;
    public float NextBattleDelay = 1.0f;
    public GameObject EnemyUITemplate;
    public Transform UIroot;
    public System.Action BattleStepUpCallBack;

    public TMPro.TextMeshProUGUI BattleTargetNameDisplay;

    public GameObject EnemyPrefab;
    public DelayEvents BattleEndEvent;

    public FirstPosSet stageSelect;

  
    private void Awake()
    {
       
        if (CurrentStageSelect == null)return;
     
       sequenceBattleData= CurrentStageSelect.sequenceBattleData;
       if (BattleTargetNameDisplay != null)
       {
        BattleTargetNameDisplay.text = sequenceBattleData.title+"があらわれた！";
       
       }
        stageSelect.SetCurrentElementByCondition(x => x.name.Contains(sequenceBattleData.stageName));
        BattleSet(0);
    }
    List<GameObject> nowGroups = new List<GameObject>();


    
    public void BattleSet(int index)
    {
        CurrentGroupIndex = index;
        if (index >= sequenceBattleData.groups.Count)
        {
            BattleEndEvent.Execute();
            Debug.Log("バトル終了");
            return;
        }
        if (nowGroups != null)
        nowGroups.ForEach(enemy => Destroy(enemy));
        nowGroups.Clear();
        Group nowGroup = sequenceBattleData.groups[index];
        AddCharactor(nowGroup);  
        allDeathCall.AllDeathCallBack = () =>
        { 
              BattleStepUpCallBack?.Invoke();
            keikei.delaycall(() => BattleSet(index + 1), NextBattleDelay);
        };
    }

     public List<GameObject> AddCharactor(SequenceBattleData group)
    {
        return AddCharactor(group.groups[0]);
    }
 public List<GameObject> AddCharactorRandom(SequenceBattleData group)
{
    Debug.Log("ランダムキャラクター追加");  
    // シャッフルして一つ目を選ぶ
    group.groups[0].CombatCharactors.Shuffle();
    CombatCharactor selectedCharactor = group.groups[0].CombatCharactors[0]; // 1つだけ選択

    // Groupオブジェクトをコレクション初期化子で初期化
    Group newGroup = new Group ();

    newGroup.CombatCharactors = new List<CombatCharactor> { selectedCharactor };
    newGroup.PositionBetween = group.groups[0].PositionBetween;

    // AddCharactorメソッドに渡す
    var objs=AddCharactor(newGroup);
    keikei.delaycall(() =>{ objs.ForEach(x=>x.GetComponent<hpcore>().HP=10);  }
        , Time.deltaTime*4);
    return objs;
}



    public List<GameObject> AddCharactor(Group group)
    {
        Group nowGroup = group;
        List<CombatCharactor> nowCombatCharactors = nowGroup.CombatCharactors;
        int nowIndex = 0;
        List<GameObject> AddObjs=new List<GameObject>();
        
        foreach (CombatCharactor combatCharactor in nowCombatCharactors)
        {
            Vector3 position = EnemyPrefab.transform.position + (nowGroup.PositionBetween * (nowIndex - nowCombatCharactors.Count / 2));
            GameObject enemy = Instantiate(EnemyPrefab, position, EnemyPrefab.transform.rotation);
            //しばらく動かない
            enemy.Stop(5);
            // UIを作る
            GameObject UIobj= Instantiate(EnemyUITemplate.gameObject,EnemyUITemplate.transform.position,EnemyUITemplate.transform.rotation,UIroot);
            UIobj.GetComponent<CharactorUI>().chatCharactor=combatCharactor.chatCharactor;
            enemy.GetComponent<hpcore>().deathCallback+=()=>Destroy(UIobj);

            keikei.delaycall(() =>{
                enemy.GetComponent<charactorchange>().SetCurrentCharacterByData(combatCharactor);
   UIobj.GetComponent<CharactorUI>().character=enemy.GetComponent<charactorchange>().CurrentElement;
         
            }
        , Time.deltaTime*3);
           AddObjs.Add(enemy);
           nowIndex++;
        }
        
        nowGroups.AddRange(AddObjs);
        keikei.delaycall(() =>    GameObjectExtension.FindObjectsWithInterface<ICharaResist>().ToList().ForEach(x=>x.Resist())
     , 0.1f);
      allDeathCall.ChangeCharacters(nowGroups);
      return AddObjs;
    }

    // Declare any other methods or event handlers here
}



