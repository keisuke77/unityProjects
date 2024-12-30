using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections.Generic;

[System.Serializable]
public class OutlineDetails
{
    public Color color;
    public Vector2 size;
}

[System.Serializable]
public class ImageEventPair
{
    public Image image;
    [HideInInspector] public Vector3 defaultScale;
    [Header("決定時のイベント")]
    public UnityEvent eventOnSelect;
    public UnityEvent preEventOnSelect;
    public UnityEvent OnEventExit;

    public bool FixPos;

    public void Select()
    {
        keikei.delaycall(() => preEventOnSelect.Invoke(), Time.deltaTime);
    }
    public void Decide()
    {


    }
}
[System.Serializable]
public class SelectMarker
{
    public Image image;
    public Vector3 offset;

    public void Execute(Transform trans)
    {

        image?.transform.DOMove(trans.position + offset, 0.5f).SetUpdate( true );
    }
}

public class ImageSelectorGrid : MonoBehaviour
{
    public List<ImageEventPair> imageEventPairs;

    public SelectMarker selectMarker;

    public Vector3 selectImgOffset;
    public float selectedScale = 1.2f;
    public float decisionPunchScale = 1.5f;
    public float duration = 0.2f;
    public int selectedIndex = 0;
    public AudioClip selectionChangedSound;
    public AudioClip selectionMadeSound;
    private AudioSource audioSource;
    public bool enableWrap = false;
    private bool isSelecting = false;
    public List<controll> decisionKey;  // The key used for making decision
    public UnityEvent onDecisionMade;  // Event that will be triggered when a decision is made
    public OutlineDetails outlineDetails;

    public float SelectableAngle = 45;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        foreach (var pair in imageEventPairs)
        {
            pair.defaultScale = pair.image.transform.localScale;
        }
        SelectImage(selectedIndex);
    }
    public controll Xaxis;
    public controll Yaxis;
    public float SequenceInterval = 0.5f;
    public bool Stop;
    public Vector2 dir;

    public bool SelectCenterPosX;
    private void Update()
    {
        int previousIndex = selectedIndex;
        dir = new Vector2(keiinput.Instance.GetAxis(Xaxis), keiinput.Instance.GetAxis(Yaxis));

        if (dir != Vector2.zero && !Stop)
        {
            Stop = true;
            keikei.delaycall(() => Stop = false, SequenceInterval);
            FindClosestImage(dir);

        }

        if (selectedIndex != previousIndex)
        {
            DeselectImage(previousIndex);
            SelectImage(selectedIndex);
            PlaySound(selectionChangedSound);
        }

        if (!isSelecting && keiinput.Instance.GetKeys(decisionKey))
        {
            onDecisionMade?.Invoke();  // Trigger the onDecisionMade event when decision key is pressed

            OnImageSelected(selectedIndex);
            PlaySound(selectionMadeSound);
        }
    }

    public float alpha = 1.0f; // 角度のウェイト
    public float beta = 0.1f; // 距離のウェイト

    private void FindClosestImage(Vector2 input)
    {
        ImageEventPair currentPair = imageEventPairs[selectedIndex];
        Vector3 direction = new Vector3(input.x, input.y, 0);
        ImageEventPair closestPair = null;
        float bestScore = Mathf.Infinity; // 最も低いスコアを持つペアを見つけるための変数

        foreach (ImageEventPair pair in imageEventPairs)
        {
            if (pair == currentPair)
                continue;

            Vector3 toOther = pair.image.transform.position - currentPair.image.transform.position;
            float dist = toOther.magnitude;
            float angle = Vector3.Angle(direction, toOther);

            if (angle < SelectableAngle)
            {
                float score = alpha * angle + beta * dist; // スコアリング関数

                if (score < bestScore)
                {
                    bestScore = score;
                    closestPair = pair;
                }
            }
        }

        if (closestPair != null)
        {
            selectedIndex = imageEventPairs.IndexOf(closestPair);
        }
        else if (enableWrap)
        {
            selectedIndex = (input.y > 0 || input.x > 0) ? 0 : imageEventPairs.Count - 1;
        }
    }

    public bool autoFit; List<Vector3> firstPos = new List<Vector3>();
    List<RectTransform> right = new List<RectTransform>();
    List<RectTransform> left = new List<RectTransform>();
    private void SelectImage(int index)
    {
        var imagePair = imageEventPairs[index];

        if (selectMarker != null)
        {
            selectMarker.Execute(imagePair.image.transform);
        }


        if (autoFit)
        {
            right.Clear();
            left.Clear();
            foreach (var item in imageEventPairs)
            {
                if (!item.FixPos)
                {
                    if (imagePair != item)
                    {
                        if (item.image.transform.position.x > imagePair.image.transform.position.x)
                        {
                            right.Add(item.image.GetComponent<RectTransform>());
                        }
                        else
                        {
                            left.Add(item.image.GetComponent<RectTransform>());
                        }

                    }
                }


            }

            float ChangePer = (selectedScale - 1) * (imagePair.image.GetComponent<RectTransform>().sizeDelta.x);
            right.ForEach(item => item.AddPosX(ChangePer));
            left.ForEach(item => item.AddPosX(-ChangePer));
        }

        float dis = 0;

        if (SelectCenterPosX)
        {
            dis = imagePair.image.transform.position.x - 900;
            imageEventPairs.ForEach(item => {
                if (!item.FixPos)
                {
                    item.image.transform.AddPosX(-dis);
                }
                });
        }


        imagePair.Select();
        imagePair.image.transform.AddPos(selectImgOffset);

        imagePair.image.transform.DOScale(imagePair.defaultScale * selectedScale, duration).SetUpdate( true );
        Outline outline = imagePair.image.gameObject.AddComponentIfnull<Outline>() as Outline;
        if (outline != null)
        {
            outline.effectColor = outlineDetails.color; // 共通のoutlineDetailsを使用
            outline.effectDistance = outlineDetails.size; // 共通のoutlineDetailsを使用
            outline.enabled = true;
        }
        int i = 0;
        foreach (var item in imageEventPairs)
        {
            if (!item.FixPos)
            {
            item.image.transform.DOMove(item.image.transform.position, 0.5f).From(firstPos[i]).SetUpdate( true );
            i++;
            }
         
        }
    }

    private void DeselectImage(int index)
    {
        var imagePair = imageEventPairs[index];
        imagePair.OnEventExit?.Invoke();
        firstPos.Clear();

        imageEventPairs.ForEach(pair =>
        {
            if (!pair.FixPos)
            {
                firstPos.Add(pair.image.transform.position);
            }
        });
        if (autoFit)
        {

            float ChangePer = (selectedScale - 1) * (imagePair.image.GetComponent<RectTransform>().sizeDelta.x);

            right.ForEach(item => item.AddPosX(-ChangePer));
            left.ForEach(item => item.AddPosX(ChangePer));

        }

        imagePair.image.transform.AddPos(-selectImgOffset);

        imagePair.image.transform.DOScale(imagePair.defaultScale, duration).SetUpdate( true );
        Outline outline = imagePair.image.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    private void OnImageSelected(int index)
    {
        isSelecting = true;
        imageEventPairs[index].eventOnSelect.Invoke();
        var image = imageEventPairs[index].image;
        image.transform.DOPunchScale(imageEventPairs[index].defaultScale * decisionPunchScale, duration).OnComplete(() =>
        {
            isSelecting = false;
        });
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
