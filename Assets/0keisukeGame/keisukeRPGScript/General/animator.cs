using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToWeen
{
    public String name;
    public int To;
    public float duration;
}

public class animator : MonoBehaviour
{Animator anim;

public List<AnimatorExtension.FloatTween> floatTweens;

    // Start is called before the first frame update
    void Awake()
    {
        anim=GetComponent<Animator>();
    }public void SetBoolTrue(string name)
{
    Debug.Log("Animation Event triggered.");
    if (anim == null)
    {
        Debug.Log("Animator is not assigned!");
        return;
    }

    anim?.SetBool(name, true);
    Debug.Log($"Set {name} to true.");
}

    
public void SetBoolfalse(string name){

anim?.SetBool(name,false);

}
public void Blend(AnimationClip clip){

anim?.BlendFromCurrentState(clip);

}

}
