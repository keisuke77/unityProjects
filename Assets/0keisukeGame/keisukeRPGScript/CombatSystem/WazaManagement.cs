using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(navchaise))]
[RequireComponent(typeof(Animator))]
public class WazaManagement : MonoBehaviour
{
    public navchaise navchaise;
    public waza MainWaza;
    public Animator anim;
    public WazaHpChange wazaHpChange;
    public hpcore hpcore;

    private bool isMelee = false;
    private float nextAttackTime = 0f; // 次の攻撃までの待機時間

    private void Awake()
    {
        hpcore.OnHpChanged += HandleHpChanged;
        anim = anim ?? GetComponent<Animator>(); // Animatorがnullなら取得
    }

    private void OnDestroy()
    {
        hpcore.OnHpChanged -= HandleHpChanged;
    }

    void Update()
    {
        if (!enabled) return; // クラスが無効になっていたら何もしない

        isMelee = navchaise.currentBehavior == BehaviorState.Melee;
        SelectWazaBasedOnHP();

        if (isMelee && MainWaza != null && anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            if (Time.time >= nextAttackTime) // 次の攻撃タイミングになったら
            {
                MeleeAttack();
                nextAttackTime = Time.time + UnityEngine.Random.Range(MainWaza.MinInterval, MainWaza.MaxInterval); // 次の攻撃時間を設定
            }
        }
    }

    private void MeleeAttack()
    {
        foreach (var item in MainWaza.wazalist)
        {
            anim.SetBool(item.name, false);
        }

        var wazas = MainWaza.wazalist;
        if (wazas.Length == 0) return;

        int index = kakuritu(wazas.Select(x => x.kakuritu).ToList());

        if (IsValidWaza(wazas[index]))
        {
            ExecuteWaza(wazas[index]);
        }
    }

    private void SelectWazaBasedOnHP()
    {
        if (wazaHpChange == null) return;

        foreach (var hpChange in wazaHpChange.wazaHpChanges)
        {
            if (hpcore.HP >= hpChange.hpBelow && hpcore.HP <= hpChange.hpAbove)
            {
                MainWaza = hpChange.waza;
                hpChange.delayEvents?.Execute();
                break;
            }
        }
    }

    private bool IsValidWaza(waza.wazas item)
    {
        return !anim.GetBool(item.name)
               && item.mindis <= navchaise.agentdestinationdis
               && navchaise.agentdestinationdis <= item.maxdis;
               
    }

    public int kakuritu(List<int> ratios)
    {
        int total = ratios.Sum();
        int randomValue = UnityEngine.Random.Range(0, total);

        int cumulative = 0;
        for (int i = 0; i < ratios.Count; i++)
        {
            cumulative += ratios[i];
            if (randomValue <= cumulative)
            {
                return i;
            }
        }
        return 0;
    }

    private void ExecuteWaza(waza.wazas item)
    {
        anim.SetBool(item.name, true);
        anim.SetTrigger(item.name);

        try
        {
            SendMessage(item.MethodNmae);
        }
        catch (System.Exception)
        {
            Debug.LogError("Failed to call method: " + item.MethodNmae);
        }

        item.motions?.Play(gameObject);
    }

    private void HandleHpChanged(int newHp)
    {
        SelectWazaBasedOnHP();
    }
}
