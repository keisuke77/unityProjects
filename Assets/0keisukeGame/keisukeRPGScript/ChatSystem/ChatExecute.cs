using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public class ChatExecute : Singleton<ChatExecute>
{
    public ChatData ChatData;
    public message message;
    public bool Autostart;

    System.Action EndCallBack;
    void Start()
    {
        if (Autostart)
            Play();
    }

    public void Play()
    {
        Execute(ChatData);
    }

    public void Execute(ChatData ChatData, System.Action EndCall = null)
    {
        StartCoroutine(Execute(ChatData.phases, 0, EndCall));
    }
    public void Executes(List<phase> phases, int num = 0, System.Action EndCall = null)
    {
        StartCoroutine(Execute(phases, num, EndCall));

    }
    List<GameObject> emotions;
    public IEnumerator Execute(List<phase> phases, int num = 0, System.Action EndCall = null)
    {

        emotions?.ForEach((x) => Destroy(x));
        emotions = new List<GameObject>();

        if (EndCall != null)
        {
            EndCallBack = EndCall;
        }
        Debug.Log("num" + num + "Count" + phases.Count);
        if (num >= phases.Count)
        {
            if (EndCallBack != null)
            {
                EndCallBack();
                EndCallBack = null;

            }
            yield break;
        }

        var phase = phases[num];
        System.Action Action;

        if (phase.SelectionPhases.Count > 1)
        {
            Action = () =>
            {
                message.StopUodate = true;
                SelectCreate(phase.SelectionPhases);
                message.isMessageing = true;
            };
        }
        else
        {
            Action = () => StartCoroutine(this.Execute(phases, num + 1));
        }
        yield return new WaitForSeconds(Time.deltaTime);
  yield return new WaitForSeconds(phase.delayTime);

        phase.objAnimations.ForEach(async (x) =>
        {
            Task<GameObject> obj = x.Execute(phase.ChatCharactor);
            emotions.Add(await obj);
        });
        
        message.ProgressKeyChange(phase.ProgressKey);
        message.SetMessagePanel(
            phase.message,
            false,
            phase.ChatCharactor?.icon,
            Action,
            phase.ChatCharactor?.name,
            num == 0
        );

        yield return null;
    }

    public GameObject TwoSelectObj;
    public GameObject ThreeSelectObj;
    public GameObject fourSelectObj;

    public void SelectCreate(List<SelectionPhase> SelectionPhases)
    {
        var obj = SelectionPhases.Count switch
        {
            2 => TwoSelectObj,
            3 => ThreeSelectObj,
            4 => fourSelectObj,
            _ => null // それ以外のケースに対するデフォルト値
        };
        GameObject instance = Instantiate(obj, message.gameObject.transform);

        int n = 0;
        foreach (var item in instance.GetComponentsInChildren<Text>())
        {
            item.text = SelectionPhases[n].text;
            n++;
        }
        n = 0;
        foreach (var item in instance.GetComponentsInChildren<Button>())
        {

            var selectphase = SelectionPhases[n];
            item.onClick.AddListener(() =>
            {
                message.isMessageing = false;
                selectphase.callBack();

                Executes(selectphase.phases, 0);
                Destroy(instance);

                message.StopUodate = false;

            });
            n++;
        }

    }
}
