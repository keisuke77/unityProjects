using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Anger : MonoBehaviour
{
    // Add any necessary variables or properties here

    navchaise navchaise;
    WazaManagement wazaManagement;
    FlickerModel flickerModel;
    Animator animator;
    public BehaviorSelector AngerBehavior;
    public int ForceEndDamage = 10;
    public float angerDuration;
    public DelayEvents delayEvents;
    public Sprite angerSprite;
    public float spriteSize = 1;

    [Header("怒り状態になる間隔")]
    public Vector2 AngerInterval = new Vector2(90, 240);

    public void Awake()
    {

        TryGetComponent(out stan);
        animator = GetComponent<Animator>();
        navchaise = GetComponent<navchaise>();
        wazaManagement = GetComponent<WazaManagement>();
        flickerModel = GetComponent<FlickerModel>();
        GetComponent<hpcore>().damageCallback += HandleHpChanged;

        //設定周期で怒り状態になる
        StartCoroutine(AngerCycle());


    }

    IEnumerator AngerCycle()
    {
        while (true)
        {
            // 30から60秒のランダムな間隔で怒り状態に移行
            float randomDelay = Random.Range(AngerInterval.x, AngerInterval.y);
            yield return new WaitForSeconds(randomDelay);

            EnterAnger();
        }
    }

    public void HandleHpChanged(int damage)
    {
        if (damage > ForceEndDamage)
        {
            stan?.ReStart();
            stan?.StanStart();
            ExitAnger();
        }
    }
    waza MainWaza;
    //一時保存用
    float MinInterval, MaxInterval, Speed;
    BehaviorSelector MainBehavior;
    GameObject AngerImage;
    public bool isAnger;
    Stan stan;
    public void EnterAnger()
    {
        if (isAnger) return;
        isAnger = true;
        stan?.Stop();
        animator.SetBool("Anger", true);
        keikei.delaycall(() => animator.speed = 1.1f, 1);
        navchaise.ForceMelee();
        Speed = navchaise.Speed;
        flickerModel.Play(new FilkerParam(Color.red, 60, 0.3f, true));
        navchaise.Speed = 10;
        MainBehavior = navchaise.behaviourData;
        navchaise.behaviourData = AngerBehavior;
        AngerImage = angerSprite?.CreateImage(gameObject, new Vector3(0, 2, 0));
        AngerImage.transform.localScale = Vector3.one * spriteSize;
        delayEvents?.Execute();
        MainWaza = wazaManagement.MainWaza;
        MinInterval = MainWaza.MinInterval;
        MaxInterval = MainWaza.MaxInterval;
        MainWaza.MinInterval = 1;
        MainWaza.MaxInterval = 1;

        //angerDuration秒後にExitAngerを呼び出す
        this.Invoke("ExitAnger", angerDuration);
    }

    public void ExitAnger()
    {
        if (!isAnger) return;

        isAnger = false;
        flickerModel.Stop();
        animator.speed = 1;
        navchaise.ForceEnd();
        navchaise.behaviourData = MainBehavior;
        navchaise.Speed = Speed;
        AngerImage?.destroy();
        MainWaza.MinInterval = MinInterval;
        MainWaza.MaxInterval = MaxInterval;

        // Add any cleanup code here
    }
    private void Update()
    {
        // Add any update code here
    }

    // Add any other methods or event handlers here
}