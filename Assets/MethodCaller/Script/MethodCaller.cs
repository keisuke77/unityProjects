using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class MethodCaller : MonoBehaviour
{
	public UnityEvent call;
	public KeyCode key;



	public void MethodCall(MethodData methodData)
	{
		methodData.Execute();

	}
}
