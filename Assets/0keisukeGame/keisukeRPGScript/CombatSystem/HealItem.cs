   using UnityEngine;

   [CreateAssetMenu(fileName = "New Heal Item", menuName = "Items/HealItem")]
    public class HealItem : ScriptableObject
    {
        public string itemName = "New Heal Item";
        public int healAmount = 10;

        public void Use(GameObject target)
        {
            var healthComponent = target.GetComponent<hpcore>();
            if (healthComponent != null)
            {
                healthComponent.heal(healAmount);
                Debug.Log($"{itemName} used on {target.name}, healed for {healAmount} points.");
            }
            else
            {
                Debug.LogWarning($"{target.name} does not have a Health component.");
            }
        }
    }