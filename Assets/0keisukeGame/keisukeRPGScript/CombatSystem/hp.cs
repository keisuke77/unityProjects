using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class hp : hpcore
{
    public flashscrean flashscrean;
    public override void SetUp()
    {
   }
    public override void OnDamage(int damage)
    {
     
     
        if (flashscrean != null)
        {
            flashscrean?.damage();
        }
    }

    public override void OnDeath()
    {   }
}
