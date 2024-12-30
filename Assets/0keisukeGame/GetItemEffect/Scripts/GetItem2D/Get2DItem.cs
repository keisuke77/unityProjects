using UnityEngine;

[CreateAssetMenu(fileName = "Get2DItem", menuName = "")]
public class Get2DItem : ScriptableObject
{
     public GIE.GetItemEffectType mGetItemEffectType = GIE.GetItemEffectType.Explostion_First;
        public string mItemName = "coin";
        public int mItemNumber = 10;

        public void Play(Vector3 position){

            GIE.GetItemEffect.mInstance.GetItem(mItemName, mItemNumber,position, null, mGetItemEffectType);
  
        }
        public void Play(Transform trans){
           Play(Camera.main.WorldToScreenPoint(trans.position));
        }
}