using UnityEngine;
using System.Collections;

public static class MonoBehaviourExtensions
{
    public static void ExecuteAfterDelay(this MonoBehaviour monoBehaviour, System.Action action, float delay)
    {
        monoBehaviour.StartCoroutine(ExecuteCoroutine(action, delay));
    }

    private static IEnumerator ExecuteCoroutine(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
