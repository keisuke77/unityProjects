using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour
{
	[SerializeField]
	Transform _target = null;

	[SerializeField]
	float _speed = 0.5f;

	Vector3 _lookAtTarget;
	 void Start() {
		_lookAtTarget= _target.position;
	}
void LateUpdate()
{
	_lookAtTarget = Vector3.Lerp(_lookAtTarget, _target.position, Time.deltaTime * _speed);
		transform.LookAt(_lookAtTarget);
	
}
	
}
