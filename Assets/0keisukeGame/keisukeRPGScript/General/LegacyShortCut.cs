  using UnityEngine;
using DG.Tweening;

public static class LegacyShortCut
{

    public static CharactorClass cclass(this GameObject obj)
    {
        return obj.root().GetComponentIfNotNull<CharactorClass>();
    }
    
    
    
    }