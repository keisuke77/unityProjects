using UnityEngine;
using UnityEditor;

public class charactorResist : MonoBehaviour
{

    public character character;
    [Header("オフの場合ルートのキャラチェンジコンポ参照")]
    public bool parent;
    public void Set()
    {
        if (parent)
        {
            gameObject.GetComponentInParent<charactorchange>().CurrentElementChange(character);

        }
        else
        {
            gameObject.root().GetComponent<charactorchange>().CurrentElementChange(character);

        }

    }
    public ChatCharactor ChatCharactor
    {
        get
        {
            return character.CombatCharactor.chatCharactor;
        }
    }

    void Awake()
    {
        character.SetUp();
        if (parent)
        {
            if (gameObject.GetComponentInParent<charactorchange>() != null)
            {
                gameObject.GetComponentInParent<charactorchange>().Elements.Add(character);
            }
        }
        else
        {
            if (gameObject.root().GetComponent<charactorchange>() != null)
            {
                gameObject.root().GetComponent<charactorchange>().Elements.Add(character);
            }
        }







    }
}