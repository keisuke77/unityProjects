    using UnityEngine;
    using System.Collections.Generic;

    [CreateAssetMenu(fileName = "WazaHpChange", menuName = "ScriptableObjects/WazaHpChange", order = 1)]
    public class WazaHpChange : ScriptableObject
    {
       

        public List<WazaHpChangeEntry> wazaHpChanges;
    } [System.Serializable]
        public class WazaHpChangeEntry
        {
            [Header("このHp以上&以下だったらこの技を選択")]
            public int hpAbove; // HPの最小値
            public int hpBelow; // HPの最大値
            public waza waza;
            public DelayEvents delayEvents;
           
        }