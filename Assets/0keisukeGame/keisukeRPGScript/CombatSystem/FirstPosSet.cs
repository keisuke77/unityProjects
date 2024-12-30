/// <summary>
/// This script is responsible/// //monobehavior

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPosSet :SelectBehabior<Transform>
{
    public Transform StageCenter;

    public override void Exit(Transform temp)
    {
        if (temp == null)
        {
            return;
        }
         temp.GetComponent<TargetFollowsMe>().enabled=false;
       
    }
    public override void ChangeCallBack()
    {
        CurrentElement.GetComponent<TargetFollowsMe>().enabled=true;
        CurrentElement.position = StageCenter.position;
    }

}