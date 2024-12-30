using UnityEngine;

public class CharactorClass : MonoBehaviour
{
 
    public Effekseer.EffekseerEmitter emitter;
  
 	void Awake () 
	{
		emitter=gameObject.AddComponentIfnull<Effekseer.EffekseerEmitter>();
	
	}
}