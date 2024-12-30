using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class character
{
    public CombatCharactor CombatCharactor
    {
        set
        {
            ChatCharactorSet(value.chatCharactor); _CombatCharactor = value;
        }
        get { return _CombatCharactor; }
    }

    public CombatCharactor _CombatCharactor;
    [HideInInspector] public string layerName;
    [HideInInspector] public string name;
    [HideInInspector] public int power = 3;
    [HideInInspector] public Sprite Icon;
    public int hp;
    [HideInInspector] public int maxhp;
    [Header("説明")]
    [HideInInspector] public string Explain;
    [HideInInspector] public int defence;
    public GameObject bone;
    public GameObject mesh;
    public GameObject eyes;
    public Avatar avatar;
    public GameObject righthand, lefthand, rightfoot, leftfoot, weapon, body;
    public transformdata weaponstransform;

    public DelayEvents SetCallBack;
    public float HPRate()
    {
        return (float)hp / maxhp;
    }

    public void SetEyeByName(string name)
    {
        if (eyes == null) return;

        foreach (var item in eyes.GetAllChild(true))
        {
            if (eyes == item) continue;
            item.SetActive(item.name.Contains(name));
        }
    }
    public void SetUp() => ChatCharactorSet(CombatCharactor?.chatCharactor);
    public void ChatCharactorSet(ChatCharactor ChatCharactor)
    {
        layerName = (ChatCharactor.animLayerName==null||ChatCharactor.animLayerName=="")? ChatCharactor.name:ChatCharactor.animLayerName;
        name = ChatCharactor.name;
        power = ChatCharactor.Power;
        Icon = ChatCharactor.icon;
        hp = ChatCharactor.CurrentHP;
        maxhp = ChatCharactor.MaxHP;
        Explain = ChatCharactor.Explain;
        defence = ChatCharactor.Defence;

    }
    public void Set(charactorchange charactorchange)
    {
        foreach (var Elementss in charactorchange.Elements)
        {
            Elementss.bone.SetActive(false);
            Elementss.mesh.SetActive(false);
        }
     bone?.SetActive(true);
        mesh?.SetActive(true);

        charactorchange.anim.Rebind();
        charactorchange.anim.avatar = avatar;
        SetCallBack?.Execute();

        charactorchange.anim.SetLayerWeight(charactorchange.anim.GetLayerIndex(layerName), 1);

        if (charactorchange.weapons != null)
        {
            charactorchange.weapons.transform.parent = righthand.transform;
        }
        //武器の装着
        if (weaponstransform != null && charactorchange.weapons != null)
        {
            transformenter(charactorchange.weapons.transform, weaponstransform);
        }

    }
    public void transformenter(Transform trans, transformdata transformdata)
    {

        trans.localPosition = transformdata.pos;
        trans.localRotation = transformdata.rotation;

    }
}

public interface GetBodyPart
{
    GameObject GetRightHand();
    GameObject GetLeftHand();
    GameObject GetLeftFoot();
    GameObject GetRightFoot(); GameObject GetWeapon();
    GameObject GetBody();
}

public class charactorchange : SelectBehabior<character>, GetBodyPart
{
    public Dropdown DropDown;
    public GameObject weapons;
    public Animator anim;
    [Button("Reload", "リロード")]
    public int q;
    [Header("入れても入れなくてもよい")]
    public CharactorUI charactorUI;
    string temptext;
    int hairetucheck;
    public List<string> m_DropOptions;
    public float CharactorApeareDelay;
    public GameObject ChangeSpawn;
    public DoTweenSeri DoTweenSeri;
    public GameObject GetRightHand() => CurrentElement.righthand;
    public GameObject GetLeftHand() => CurrentElement.lefthand;
    public GameObject GetLeftFoot() => CurrentElement.leftfoot;
    public GameObject GetRightFoot() => CurrentElement.rightfoot;
    public GameObject GetWeapon() => CurrentElement.weapon;
    public GameObject GetBody() => CurrentElement.body;

    public void SetCurrentCharacterByData(CombatCharactor combatCharactor){ 
        SetCurrentElementByCondition(n => n.CombatCharactor == combatCharactor);
    }
    public void SetCurrentCharacterByName(string name) {
       SetCurrentElementByCondition(n => n.name == name);
    }
    
    void OnDisable()
    {
        this.enabled = true;
    }

  
    void OnEnable()
    {
        keikei.delaycall(Reload, 0.3f);
    }
    public void Reload()
    {
        if (CurrentElement == null)
        {
            keikei.delaycall(Reload, 1);
            return;
        }
        CurrentElement.Set(this);
    }
    void Start()
    {
        Elements = Elements.OrderBy(n => n.name.Length)
                             .ThenBy(n => n.name).ToList();
        TryGetComponent(out hpcore);

        if (DropDown)
        {
             //DropDownの要素にリストを追加
            Elements.ForEach(x=> m_DropOptions.Add(x.name));
            DropDown.ClearOptions();
            DropDown.AddOptions(m_DropOptions);
        }

    }

    public void SetEyeByName(string name)
    {
        CurrentElement.SetEyeByName(name);
    }
    public void OnTrail()
    {
        if (CurrentElement.weapon != null && CurrentElement.weapon.GetComponent<TrailRenderer>() != null)
        {
            CurrentElement.weapon.GetComponent<TrailRenderer>().enabled = true;
        }
    }
    public void OffTrail()
    {
        if (CurrentElement.weapon != null && CurrentElement.weapon.GetComponent<TrailRenderer>() != null)
        {
            CurrentElement.weapon.GetComponent<TrailRenderer>().enabled = false;
        }
    }
    public void characterhide()
    {
        foreach (var Elementss in Elements)
        {
            Elementss.mesh.SetActive(false);
        }
    }
    Sequence ChangeTween;

    public ChatCharactor GetCurrentChatCharactor
    {
        get { return CurrentElement.CombatCharactor.chatCharactor; }
    }
    public override void ChangeCallBack()
    {
        Reload();
        if (ChangeSpawn != null)
        {
            Instantiate(ChangeSpawn, transform.position, Quaternion.identity);
        }
       
        if (ChangeTween != null)
        {
            ChangeTween.Complete();

        }
        if (hpcore != null)
        {    if (WorldInfo.DataSave)
            {
                  hpcore.OnHpChanged+=(x)=>GetCurrentChatCharactor.CurrentHP = x;
            }else
            {
                GetCurrentChatCharactor.CurrentHP=GetCurrentChatCharactor.MaxHP;
            }
            hpcore.HP = GetCurrentChatCharactor.CurrentHP;
            hpcore.maxHP = GetCurrentChatCharactor.MaxHP;
            hpcore.defence = GetCurrentChatCharactor.Defence;
          
            
                }
      
        if (TryGetComponent(out WazaManagement WazaManagement))
        {
            WazaManagement.wazaHpChange = CurrentElement.CombatCharactor.WazaHpChange;
        }
        if (TryGetComponent(out combo combo))
        {
            combo.mp.increaseTime = CurrentElement.CombatCharactor.attackCoolTime;
}
        if (TryGetComponent(out kaihi kaihi))
        {
            kaihi.mp.increaseTime = CurrentElement.CombatCharactor.guardCoolTime;
        }
        if (TryGetComponent(out dashgage dash))
        {
            dash.mp.increaseTime = CurrentElement.CombatCharactor.dashCoolTime;
        }
       
        if (TryGetComponent(out AnimSpeedChangeState animspeed))
        {
            animspeed.runSpeed = GetCurrentChatCharactor.RunSpeed;
            animspeed.idleSpeed = GetCurrentChatCharactor.IdleSpeed;
            animspeed.walkSpeed = GetCurrentChatCharactor.WalkSpeed;
        }
        if (TryGetComponent(out attackcore attackcore))
        {
            attackcore.basedamagevalue = GetCurrentChatCharactor.Power;
            attackcore.baseforcepower = GetCurrentChatCharactor.knockBack;
        }
        if (TryGetComponent(out PlayerControll PlayerControll))
        {
            PlayerControll.TransformMoveSpeed = GetCurrentChatCharactor.Speed;
        }
        if (TryGetComponent(out navchaise navchaise))
        {
            navchaise.Speed = GetCurrentChatCharactor.Speed*2;
        }
        {

        }

        if (GetComponent<Jump>() != null)
        {
            GetComponent<Jump>().DefaultJumpSpeed = GetCurrentChatCharactor.JumpPower;
        }

           
            CurrentElement.Set(this);

        try
        {
            ChangeTween = DoTweenSeri?.Play(transform);


        }
        catch (System.Exception)
        {

            throw;
        }
    }
    
    hpcore hpcore;
    // Update is called once per frame
    void LateUpdate()
    {
        if (charactorUI != null)
        {
            charactorUI.chatCharactor = GetCurrentChatCharactor;
        }

        if (hpcore != null && CurrentElement != null)
        {
          
            CurrentElement.hp = hpcore.HP;
            CurrentElement.maxhp = hpcore.maxHP;
        }
        if (DropDown != null)
        {
            if (temptext != DropDown.captionText.text)
            {
                hairetucheck = 0;
                temptext = DropDown.captionText.text;

                foreach (var item in Elements)
                {
                    hairetucheck++;
                    if (item.name == DropDown.captionText.text)
                    {
                        active = hairetucheck;
                    }
                }
            }
        }
    }
}
