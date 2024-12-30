using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[SerializeField]
public class PlayerControll : MonoBehaviour, IMove,Inputable
{
    public bool isInputable { get; set; } = true;
    

    [SerializeField]
    private Animator anim;
    public float TransformMoveSpeed = 1;//ベース

    [HideInInspector]
    public float _transformMoveSpeed { get; set; }
    Quaternion targetRotation;
    Camera cam;
    public bool isTransformMove;
    public bool Stop { get; set; }
    public Vector3 direction;
    public float CollideCheckDistance = 1f;

    [Header("アニメーションの速度最高値　基本最高値は１"), Range(0, 10)]
    public float animspeed = 1;


    public bool AngleChangeBrake;
    public float animDamper = 0.1f;
    public AnimationCurve translateCurve;
    public int FrameRate;
    public bool simplemove;
    void Start()
    {
        Application.targetFrameRate = FrameRate;

        cam = Camera.main;
    }

    void OnDisable() => anim.SetFloat("speed", 0);



    void OnEnable() => targetRotation = transform.rotation;

    void Update() => CalculateMove();


    void CalculateMove()
    {
        if (Stop)
        {
            anim.SetFloat("speed", 0);
            return;
        }

        if(isInputable){
              direction = transform.CameraDirection(cam, keiinput.Instance.directionkey);
        }else{
            direction=Vector3.zero;
            

        }
      
        if (direction.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        // 前フレームからの回転量をあらかじめ求める
        var diffRotation = Quaternion.Inverse(transform.rotation) * targetRotation;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            600 * Time.deltaTime
        );


        // 回転した角度と軸（ローカル空間）を求める
        diffRotation.ToAngleAxis(out var angle, out var axis);

        anim.SetFloat("deltaAngler", angle, 0.1f, Time.deltaTime);
        angle = angle / 360;
        if (AngleChangeBrake)
        {
            anim.SetFloat("speed", (direction.magnitude - angle) * animspeed * keiinput.Instance.directionkey.magnitude, animDamper, Time.deltaTime);
        }
        else
        {
            anim.SetFloat("speed", direction.magnitude * animspeed * keiinput.Instance.directionkey.magnitude, animDamper, Time.deltaTime);

        }
        if (simplemove)
        {
            anim.SetFloat("speed", (int)(keiinput.Instance.directionkey.magnitude * animspeed));

        }
        direction=direction.normalized;
        float moveAmount= Time.deltaTime * translateCurve.Evaluate(anim.GetFloat("speed")) * _transformMoveSpeed * WorldInfo.scale;

        if (isTransformMove)
        {
           
                Vector3 movement = direction * moveAmount;
                transform.position += movement;
           

        }
    }
}
