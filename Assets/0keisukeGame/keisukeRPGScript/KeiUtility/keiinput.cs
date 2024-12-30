using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI_InputSystem.Base;
public class keiinput : MonoBehaviour
{
   

    public Button attackbutton;
    public Button jumpbutton;
    public Button throwbutton;
  
    public bool pose;
    public bool attack;
    public bool attackduring;
    public bool attackup;
    public bool dash;
    public bool dashduring;
    public bool dashup;
    public bool jump;  
    public bool jumpup;
    public bool jumpduring; 
    public bool Throw;
    public bool interaction;
    public bool interactionduring;
    public bool interactionup;
    public bool add;
    public bool down;
    public bool hissatu;
    public bool decide;
    public bool MouseDuring,MouseDuring2;
    public bool guard;
    public bool guardup;
   public Vector2 directionkey;
    public Vector2 rotationkey;
    public inputsetting inputsetting;
    public float time = 0.1f;
    WaitForSeconds wait;
    public bool inputsystemnew;
    public bool stop;

private static keiinput instance;
 public static keiinput Instance
    {
        get
        {
           
           
            if (instance == null)
            {

                instance = (keiinput)FindObjectOfType(typeof(keiinput));
                if (instance == null)
                {
                    Debug.LogWarning(typeof(keiinput) + "がシーンに存在しません。");
                }
            }
            return instance;
        }
    }

public void ChangeSetting(inputsetting inputsettings)=>inputsetting=inputsettings;


    public Vector2 GetDpad()
    {
        return directionkey;
    }

    public Vector2 GetRotate()
    {
        return new Vector3(
            inputsetting.rotatehorizonaxis.GetAxis(),
            inputsetting.rotateverticalaxis.GetAxis(),
            0
        );
    }
public void Stop(){
    stop=true;
}
public void Restart(){
    stop=false;
}
    private void OnDisable()
    {
        gameObject.GetComponent<keiinput>().enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (stop)
        {
            return;
        }
   
        attackbutton?.onClick.AddListener(() =>
        {
            attackupa();
        });
        jumpbutton?.onClick.AddListener(() =>
        {
            jumpa();
        });
        throwbutton?.onClick.AddListener(() =>
        {
            throwa();
        });
        wait = new WaitForSeconds(time);
    }

    IEnumerator functionName()
    {
        yield return null;
    }

    public void attacka()
    {
        if (stop)
        {
            return;
        }
        StartCoroutine("attacks");
    }

    public void attackupa()
    {
        if (stop)
        {
            return;
        }
        StartCoroutine("attackups");
    }

    public void jumpa()
    {
        if (stop)
        {
            return;
        }
        StartCoroutine("jumps");
    }

    public void throwa()
    {
        if (stop)
        {
            return;
        }
        StartCoroutine("throws");
    }

    public IEnumerator attacks()
    {
        attack = true;
        yield return wait;
        attack = false;
        yield return null;
    }

    public IEnumerator attackups()
    {
        attackup = true;
        yield return wait;
        attackup = false;
        yield return null;
    }

    public IEnumerator jumps()
    {
        jump = true;
        yield return wait;
        jump = false;
        yield return null;
    }

    public IEnumerator throws()
    {
        Throw = true;
        yield return wait;
        Throw = false;
        yield return null;
    }

    public string[] ControllerConnectionNames;

    // Update is called once per frame

   public Vector2 TryGetDpad()
    {
        try
        {
            return new Vector2(
                (float)inputsetting?.horizonaxis?.GetAxis(),
                (float)inputsetting?.verticalaxis?.GetAxis()
            );
        }
        catch (System.Exception e)
        {
            return Vector2.zero;
        }
    }
    public Vector2 TryGetRpad()
    {
        try
        {
            return new Vector2(
                (float)inputsetting?.rotatehorizonaxis?.GetAxis(),
                (float)inputsetting?.rotateverticalaxis?.GetAxis()
            );
        }
        catch (System.Exception e)
        {
            return Vector2.zero;
        }
    }

    public float GetAxis(controll input)
    {
        switch (input)
        {
            case controll.horizonaxis:
                return directionkey.x;
            case controll.verticalaxis:
                return directionkey.y;
            case controll.rotatehorizonaxis:
                return rotationkey.x;
            case controll.rotateverticalaxis:
                return rotationkey.y;
            default:
                return 0;
        }
    }
    
    public bool GetKeys(List<controll> input)
    {foreach (var item in input)
    {
        if(GetKey(item)){
            return true;        }
       
    }return false;
      
     }
     public bool MouseDown,MouseUp;
   public bool MouseDown2,MouseUp2;
      public bool GetKey(controll input)
    {
        switch (input)
        {
            case controll.attackkey:
                return attack; 
                 case controll.poseKey:
                return pose; 
                 case controll.attackkeyduring:
                return attackduring; 
                case controll.mouseDown:
                return MouseDown;    
                case controll.mouseDuring:
                return MouseDuring;   
                case controll.mouseUp:
                return MouseUp; 
   case controll.mouseDown2:
                return MouseDown2; 
                case controll.mouseUp2:
                return MouseUp2; 
                 case controll.mouseDuring2:
                return MouseDuring2; 

                
                case controll.none:
                return false;
            case controll.dashkey:
                return dash;
            case controll.jumpkey:
                return jump;
            case controll.hissatukey:
                return hissatu; case controll.guardkey:
                return guard; case controll.throwkey:
                return Throw; case controll.interactionkey:
                return interaction; case controll.addkey:
                return add; case controll.downkey:
                return down; case controll.decidekey:
                return decide; 
                case controll.horizonaxis:
                return directionkey.x != 0;
            case controll.verticalaxis:
                return directionkey.y != 0;
                
                 case controll.attackkeyup:
                return attackup;
            case controll.dashkeyup:
                return dashup;
            case controll.jumpkeyup:
                return jumpup;
            case controll.guardkeyup:
                return guardup; case controll.interactionkeyup:
                return interactionup; 
            default:
                return false;
        }
    }
    void Update()
    {
        
       Debug.Log("emable keiinpu");
        ControllerConnectionNames = Input.GetJoystickNames();

        if (stop)
        {
            directionkey = Vector2.zero;
            rotationkey = Vector2.zero;
            return;
        }
    directionkey = TryGetDpad();
       rotationkey= TryGetRpad();
  
  if (UIInputSystem.ME != null){
    Vector2 Check=UIInputSystem.ME.GetAxis(JoyStickAction.Movement);
          if (Check != Vector2.zero)
        { directionkey=Check;
        }
        Check=UIInputSystem.ME.GetAxis(JoyStickAction.CameraLook);
          if (Check != Vector2.zero)
        { rotationkey=Check;
        }
     
            }
MouseDown=Input.GetMouseButtonDown(0);
MouseUp=Input.GetMouseButtonUp(0);
MouseDown2=Input.GetMouseButtonDown(1);
MouseUp2=Input.GetMouseButtonUp(1);
if (MouseDown)
{

  MouseDuring=true;
}else if (MouseUp)
{
  MouseDuring=false;
}if (MouseDown2)
{

  MouseDuring2=true;
}else if (MouseUp2)
{
  MouseDuring2=false;
}

                 
        if (dash)
        {
            dashduring = true;
        }
        if (dashup)
        {
            dashduring = false;
        }   if (jump)
        {
            jumpduring = true;
        }
        if (jumpup)
        {
            jumpduring = false;
        }
        if (interaction)
        {
            interactionduring = true;
        }
        if (interactionup)
        {
            interactionduring = false;
        }
        if (attack)
        {
            attackduring = true;
        }
        if (attackup)
        {
            attackduring = false;
        }
 pose = inputsetting.posekey?.keydown() ?? false;
interaction = inputsetting.interactionkey?.keydown() ?? false;
interactionup = inputsetting.interactionkey?.keyup() ?? false;
add = inputsetting.addkey?.keydown() ?? false;
down = inputsetting.downkey?.keydown() ?? false;
decide = inputsetting.decidekey?.keydown() ?? false;
dash = inputsetting.dashkey?.keydown() ?? false;
dashup = inputsetting.dashkey?.keyup() ?? false;
attack = inputsetting.attackkey?.keydown() ?? false;
attackup = inputsetting.attackkey?.keyup() ?? false;
guard = inputsetting.guardkey?.keydown() ?? false;
guardup = inputsetting.guardkey?.keyup() ?? false;
Throw = inputsetting.throwkey?.keydown() ?? false;
jump = inputsetting.jumpkey?.keydown() ?? false;
jumpup = inputsetting.jumpkey?.keyup() ?? false;
hissatu = inputsetting.hissatukey?.keydown() ?? false;


    }
}


[System.Serializable]
public class AnimBoolName
{
    public Animator Anim;
    public string attack;

    public string dash;

    public string jump;
    public string Throw;
    public string interaction;

    public string add;
    public string down;
    public string hissatu;
    public string decide;


    void Key(keiinput keiinput){

  

    }
  public void Update(keiinput keiinput)
{ 
    if (keiinput.attack)
    {
        Anim.SetTrigger(attack);
    }
    if (attack != "")
        Anim.SetBool(attack, keiinput.attackduring);
    
    if (dash != "")
        Anim.SetBool(dash, keiinput.dashduring);
    
    if (jump != "")
        Anim.SetBool(jump, keiinput.jump);
    
    if (Throw != "")
        Anim.SetBool(Throw, keiinput.Throw);
    
    if (interaction != "")
        Anim.SetBool(interaction, keiinput.interaction);
    
    if (add != "")
        Anim.SetBool(add, keiinput.add);
    
    if (down != "")
        Anim.SetBool(down, keiinput.down);
    
    if (hissatu != "")
        Anim.SetBool(hissatu, keiinput.hissatu);
    
    if (decide != "")
        Anim.SetBool(decide, keiinput.decide);
}


}
