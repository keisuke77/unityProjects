using UnityEngine;


public interface IDamagable
{ 
    void AddDamage(float damagevalue);
}

public interface IMove
{
   public bool Stop{get;set;} 
   
}


interface IForceIdle
{
    void AddForce(Vector3 force);
}
