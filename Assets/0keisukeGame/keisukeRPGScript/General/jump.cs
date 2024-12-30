using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Jump : MonoBehaviour, IMove
{
  public float jumpSpeed = 10f; // Adjust as needed
  public float maxJumpSpeed;
  [Header("max時の点滅")]

  public FlickerModel flickerModel;
  public float gravity = 9.81f; // Adjust as needed
  public float moveSpeed = 5f; // Adjust as needed
  Camera cam;
  private NavMeshAgent agent;
  private Vector3 velocity;
  [HideInInspector]
  public bool isJumping;

  public bool MaxAutoJump;
  public GameObject Dust;
  public GroundCast groundCast;
  public bool Stop { get; set; }
  Rigidbody rb;
  Animator anim;

  public float EndAnimDistanse = 0.5f;
  public float DefaultJumpSpeed = 3;
  public Image image;

  public Vector3 ChargeScale;
  Vector3 defaultScale;

  public int JumpCount = 1;
  public PlayerControll playerControll;
  public float JumpCoolDownTime = 0.5f;
  void Start()
  {
    anim = GetComponent<Animator>();// Assuming the ground check position is at the character's feet
    defaultScale = transform.localScale;
    cam = Camera.main;
    rb = GetComponent<Rigidbody>();
    agent = GetComponent<NavMeshAgent>();
  }
  Vector3 LastPos;
  public float NeedMaxJumpSeconds = 1;
  public float jumpOverDelayTime = 0.1f;

  Tween tween, tween2;
  int defaultJumpCount;

  private void Awake()
  {
    defaultJumpCount = JumpCount;
  }
  private void Update()
  {
    if (Stop) return;

    if (groundCast.Custom(EndAnimDistanse) && velocity.y < 0)
    {
      JumpEndAnim();
    }
    if (groundCast.isGrounded && velocity.y < 0)
    {
      JumpEnd();
    }


    if (keiinput.Instance.jump && groundCast.isGrounded && !isJumping && !Charge)
    {
      JumpPrepair();
      tween = transform.DOScale(ChargeScale*WorldInfo.scale, NeedMaxJumpSeconds);

      tween2 = transform.DOMove((ChargeScale*WorldInfo.scale - defaultScale) / 2, NeedMaxJumpSeconds).SetRelative(true);
    }
    if ((keiinput.Instance.jumpup || (MaxAutoJump && jumpSpeed == maxJumpSpeed)) && Charge && groundCast.isGrounded && !isJumping)
    {
      tween.Kill(true);
      tween2.Kill(true);
      JumpNow();
      transform.DOScale(defaultScale, 0.2f);
    }

  }
  Quaternion BeforeRot;
  float moveSpeedAmount;
  public float acceleration = 0.1f;

  public void Chaging()
  {
    if (jumpSpeed < maxJumpSpeed)
    {
      jumpSpeed += Time.deltaTime * (maxJumpSpeed - DefaultJumpSpeed) / NeedMaxJumpSeconds;
    }
    else if (jumpSpeed > maxJumpSpeed)
    {
      jumpSpeed = maxJumpSpeed;
      flickerModel?.damagecolor();
    }
  }
  private void FixedUpdate()
  {
    if (Stop) return;

    if (image != null)
    {
      float lerpValue = 1 - Mathf.InverseLerp(DefaultJumpSpeed, maxJumpSpeed, jumpSpeed);
      image.fillAmount = lerpValue;
    }

    if (Charge)
    {
      transform.position = LastPos;
      Chaging();
    }
    else
    {
      jumpSpeed = DefaultJumpSpeed;
    }
    if (isJumping)
    {
      // Handle gravity
      velocity.y -= gravity * Time.deltaTime;
      if (velocity.y < 0)
      {
        anim.SetBool("JumpFall", true);
        anim.SetBool("Jump", false);
        if (JumpCount > 1)
        {
          if (keiinput.Instance.jumpduring)
          {
            Chaging();
            velocity.y = -0.3f;
            moveSpeedAmount = 0;
          }
          if (keiinput.Instance.jumpup)
          {
            JumpCount--;
            anim.SetBool("JumpFall", false);
            JumpNow();

          }
        }

      }
      // Handle horizontal movement
      Vector3 move = transform.CameraDirection(cam, keiinput.Instance.directionkey);
      Debug.Log(Vector3.Dot(move, initialRot));
      float DelateSpeed = (Vector3.Dot(move, initialRot)).Remap(1, -1, 0, -1);
      moveSpeedAmount += (DelateSpeed * acceleration);
      moveSpeedAmount = Mathf.Clamp(moveSpeedAmount, 0, moveSpeed);
      Debug.Log(move);

      transform.position += (velocity + (move * moveSpeedAmount)) * Time.deltaTime*WorldInfo.scale;
      rb.useGravity = false;
    }
    else
    {
      rb.useGravity = true;
    }

    LastPos = transform.position;
  }
  bool Charge;
  public void JumpEndAnim()
  {

    anim.SetBool("JumpEndAnim", true);

  }
  public void JumpEnd()
  {

    JumpCount = defaultJumpCount;
    anim.SetBool("JumpStart", false);
    anim.SetBool("Jump", false);
    anim.SetBool("JumpEnd", true);
    anim.SetBool("JumpFall", false);

    Instantiate(Dust, groundCast.hit.point, Quaternion.identity);
    Stop = true;
    keikei.delaycall(() => Stop = false, JumpCoolDownTime);

    velocity.y = 0f;
    isJumping = false;
    if (agent)
      agent.enabled = true;
    if (playerControll)
    {
      playerControll.Stop = false;
    }


  }
  public void JumpPrepair()
  {
    anim.SetBool("JumpStart", true);
    anim.SetBool("JumpEnd", false);
    anim.SetBool("JumpEndAnim", false);

    Charge = true;
  }
  Vector3 initialRot;
  public void JumpNow()
  {
    if (playerControll)
    {
      playerControll.Stop = true;
    }

    moveSpeedAmount = moveSpeed;
    initialRot = transform.forward;
    anim.SetBool("Jump", true);
    anim.SetBool("JumpEnd", false);
    velocity.y = jumpSpeed;
    Charge = false;
    isJumping = true;

    if (agent) agent.enabled = false;
  }
}