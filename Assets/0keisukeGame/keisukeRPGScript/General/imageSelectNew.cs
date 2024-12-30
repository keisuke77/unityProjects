using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class imageSelectNew : ObjectChanger<imageSelectNew.spriteevent>
{
    public Material selectmaterial;
public ObjectChanger<Image> img;
    public System.Action decideaction;
    public Color SelectColor;
    public UnityEvent afterevents;
    [Range(-3,3)] public float SelectBigScale = 1f;
    [Range(-3,3)] public float DecidePunchPower = 0.1f;
    public Vector2 outlineSize;
    public KeyCode AddKey;
    public KeyCode DownKey;
    public KeyCode DecideKey;
    public Color OutlineColor;

    [System.Serializable]
    public class spriteevent
    {
        [HideInInspector]
        public Material defaultmatearial;
        [HideInInspector]
        public Color defaultcolor;
        public Image sprite;
        public Text text;
        public UnityEvent events;
        public System.Action decideaction;
        public System.Action Selectaction;
        [HideInInspector]
        public Outline outline;
        [HideInInspector]
        public Vector3 defaultscale;
    }

    void Start()
    {
      Execute(InitializeSpriteEvent);
    
    }

    private void InitializeSpriteEvent(spriteevent item)
    {
        if(item.sprite != null)
        {
            item.defaultscale = item.sprite.transform.localScale;
            item.outline = item.sprite.gameObject.AddComponentIfnull<Outline>();
            item.defaultmatearial = item.sprite.material;
            item.defaultcolor = item.sprite.color;
        }

        if(item.text != null)
        {
            item.defaultscale = item.text.transform.localScale;
            item.outline = item.text.gameObject.AddComponentIfnull<Outline>();
            item.defaultmatearial = item.text.material;
            item.defaultcolor = item.text.color;
        }

        item.outline.effectColor = OutlineColor;
        item.outline.effectDistance = outlineSize;
    }

    protected override void OnObjectChanged()
    {
        select();
    }

    void select()
    {
    
         Execute(InitializeSpriteEvent);
        

        var currentEvent = GetCurrentObject();
        if(currentEvent.sprite != null)
        {
            SetSpriteProperties(currentEvent.sprite);
        }
        
        if(currentEvent.text != null)
        {
            SetTextProperties(currentEvent.text);
        }
        
        currentEvent.outline.enabled = true;
        currentEvent.Selectaction?.Invoke();
    }

    private void SetSpriteProperties(Image sprite)
    {
        sprite.color = SelectColor;
        sprite.material = selectmaterial;
        sprite.gameObject.transform.DOScale(sprite.transform.localScale * SelectBigScale, 0.2f);
    }

    private void SetTextProperties(Text text)
    {
        text.color = SelectColor;
        text.material = selectmaterial;
        text.gameObject.transform.DOScale(text.transform.localScale * SelectBigScale, 0.2f);
    }

    bool once;

    public void decide()
    {
        if(once) return;

        once = true;

        var currentEvent = GetCurrentObject();
        if(currentEvent.sprite != null)
        {
            currentEvent.sprite.gameObject.transform.DOPunchScale(Vector3.one * DecidePunchPower, 0.5f, 2, DecidePunchPower).OnComplete(() => once = false);
            
            if(currentEvent.sprite.gameObject.GetComponent<Button>() != null)
            {
                currentEvent.sprite.gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }

        if(currentEvent.text != null)
        {
            currentEvent.text.gameObject.transform.DOPunchScale(Vector3.one * DecidePunchPower, 0.5f, 2, DecidePunchPower).OnComplete(() => once = false);
        }

        currentEvent.events?.Invoke();

        if(currentEvent.decideaction != null)
        {
            currentEvent.decideaction(); 
        }

        decideaction?.Invoke();
        afterevents?.Invoke();
    }

    void Update()
    {
        if(AddKey.keydown())
        {
            AddChange();
        }
        if(DownKey.keydown())
        {
            DownChange();
        }
        if(DecideKey.keydown())
        {
            decide();
        }
    }
}
