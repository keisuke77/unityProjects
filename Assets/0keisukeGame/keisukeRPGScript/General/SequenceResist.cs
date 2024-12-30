using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class SequenceResist : MonoBehaviour
{
    public CharacterControllerSequence character;

    // タスク定義用クラス
    [System.Serializable]
    public class Task
    {
        public TaskType type;       // タスクの種類
        public Transform target;    // 移動先（MoveToの場合）
        public float duration;      // 停止・会話時間（IdleやTalkの場合）
        public bool run;            // 移動時に走るかどうか（MoveToの場合）
    }

    public enum TaskType
    {
        MoveTo,
        Idle,
        Talk
    }

    // タスク配列
    public List<Task> tasks;

    void Start()
    {
        // タスクを順番に実行
        foreach (var task in tasks)
        {
            switch (task.type)
            {
                case TaskType.MoveTo:
                    character.MoveTo(task.target, task.run);
                    break;
                case TaskType.Idle:
                    character.Idle(task.duration);
                    break;
                case TaskType.Talk:
                    character.Talk(task.duration);
                    break;
            }
        }
    }
}
