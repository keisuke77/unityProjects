using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


[System.Serializable]
public class ControllPlayer
{
    public ControllPlayer(GameObject obj){
        this.obj=obj;
    }
    public GameObject obj;



    PlayerControll playerControll;
    Animator animator;
    navchaise navchaise;
    List<mp> mps;

    WazaManagement wazaManagement;
    [HideInInspector] public NavMeshAgent agent;

    dashgage dashgage;

    List<Inputable> inputables;
    List<UIUpdateable> uIUpdateables;
    combo combo;
    kaihi kaihi;
   public basehp basehp => obj.GetComponent<basehp>();
    public void Setup()
    {
        playerControll = obj.GetComponent<PlayerControll>();
        combo = obj.GetComponent<combo>();
        kaihi = obj.GetComponent<kaihi>();
        navchaise = obj.GetComponent<navchaise>();
        agent = obj.GetComponent<NavMeshAgent>();
        wazaManagement = obj.GetComponent<WazaManagement>();
         animator = obj.GetComponent<Animator>();
        dashgage = obj.GetComponent<dashgage>();
        inputables = obj.GetComponents<Inputable>().ToList();
        uIUpdateables = obj.GetComponents<UIUpdateable>().ToList();
        mps = obj.GetComponents<mp>().ToList();
    }

    public void IsControll(bool controll)
    {

        inputables?.ForEach(x => x.isInputable = controll);
        uIUpdateables?.ForEach(x => x.isUIUpdateable = controll);
        if (kaihi != null)
        {
            kaihi.autoKaihi = !controll;
        }
        if (combo != null)
        {
            combo.AutoAim = !controll;
        }
        if (playerControll != null)
        {
            playerControll.enabled = controll;
        }
        if (dashgage != null)
        {
            dashgage.enabled = controll;
        }
        if (wazaManagement != null)
        {
            wazaManagement.enabled = !controll;
          
        }
        if (controll == true && agent != null && agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
        if (navchaise != null)
        {
            navchaise.enabled = !controll;
        }
        if (animator != null)
        {
            animator.enabled = true;
        }
        mps?.ForEach(mp => mp.ImageLink = controll);
    }

}
[System.Serializable]
public class StatusUI
{
    public character charactor;
    public Text nameText;
    public Text ExplainText;
    public Image IconImage;
    public Image Hpvar;
    public Text HpText;
    Timer timer;

    public bool isMainCharactor;

    public void MainLoop()
    {
        nameText.text = charactor.name;
        IconImage.sprite = charactor.Icon;
        Hpvar.fillAmount = charactor.HPRate();


        if (HpText != null)
        {
            HpText.text = (charactor.HPRate() * 100).ToString("F0") + "%";

        }
        if (ExplainText != null)
        {
            ExplainText.text = charactor.Explain;

        }
    }
    public void Set(character charactors)
    {
        this.charactor = charactors;
    }

}

public class ControllPlayerChange : SelectBehabior<ControllPlayer>
{
    public CameraFollow cameraFollow;
    public DelayEvents allDeathEvent;

    public StatusUI currentIcons;
    public List<StatusUI> subIcons;
    public float UIupdateDelay = 0;
    private void Start()
    {
        Elements.ForEach(el => el.Setup());
        ChangableCondition = Changable;
    }

    bool Changable()
    {
        return !cameraFollow.isChangingPlayer;
    }
    public override void UpdateCallBack()
    {
        currentIcons.MainLoop();
        subIcons?.ForEach(x => x.MainLoop());
        foreach (var item in Elements)
        {
            if (item.basehp.HP == 0)
            {
                if (subIcons.Count != 0)
                {
                    subIcons[0].IconImage.sprite = item.obj.GetComponent<charactorchange>().GetCurrentChatCharactor.icon;
                    subIcons[0].IconImage.color = Color.gray;
                    subIcons[0].Hpvar.fillAmount = 0;
                    subIcons.RemoveAt(0);
                }
                item.IsControll(false);
                Elements.Remove(item);
                if (item.obj == CurrentElement.obj)
                {
                    keikei.delaycall(() => active++, 2);
                    //死んだ場合オートでメインが切り替わる
                }
                else
                {
                    UIUpdate();
                }
            }
        }

        if (Elements.Count == 0)
        {
            allDeathEvent?.Execute();
        }
    }
    public override void ChangeCallBack()
    {
        cameraFollow.ChangePlayer(CurrentElement.obj.transform);

        foreach (var item in Elements)
        {
            item.IsControll(false);

        }

        CurrentElement.IsControll(true);


        keikei.delaycall(() =>
        {
            UIUpdate();
        }, UIupdateDelay);

    }

    public void UIUpdate()
    {

        currentIcons.Set(CurrentElement.obj.GetComponent<charactorchange>().CurrentElement);
        int i = 0;
        foreach (var item in Elements)
        {
            //くれんとを除く
            if (item.obj != CurrentElement.obj)
            {
                subIcons[i].Set(item.obj.GetComponent<charactorchange>().CurrentElement);
                i++;
            }

        }
    }
}