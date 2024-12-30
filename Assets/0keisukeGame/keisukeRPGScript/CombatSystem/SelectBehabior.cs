using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;

public class SelectBehabior<T> : MonoBehaviour, Inputable where T : class 
{

  public bool isInputable { get; set; } = true;
  public controll DecideKey;
  public controll AddKey;
  public controll DownKey;

  public int AddDownAxisNumber=-1;
  public DelayEvents ChangeEvent;

  public List<T> Elements;
  public T CurrentElement;

  public int active = 1000000;
  public int tempactive = -1;
  public System.Func<bool> ChangableCondition;

  public System.Action ChangeEvents;
  public virtual void DecideEvent() { }


  public void CurrentElementChange(T element)
  {
    active = Elements.IndexOf(element);
  }
  public T SetCurrentElementByCondition(Func<T, bool> condition)
  {
    if (Elements == null || Elements.Count == 0) return default(T);

    for (int i = 0; i < Elements.Count; i++)
    {
      if (condition(Elements[i]))
      {
        CurrentElementChange(i);
        return Elements[i];
      }
    }
    return default(T);
  }

  public void CurrentElementChange(int n)
  {
    active = n;
    UpdateElement();
  }
  public virtual void UpdateCallBack() { }
  public virtual void ChangeCallBack() { }


  List<T> tempElements;

  void Change()
  {
    ChangeCallBack();
    ChangeEvent?.Execute();

    if (ChangeEvents != null)
    {
      ChangeEvents();
    }

  }

  public virtual void Exit(T temp){}
int Index(int n)
{
  if (n==0)
  {
    return 0;
  }
    // ０除算を防ぐ
    if (Elements.Count == 0)
    {
        return 0; // 配列が空の場合は0を返す（使い方に応じて調整可能）
    }
    if (n>Elements.Count)
    {
        // nが負数の場合でも正しいインデックスを返す
    n = n % Elements.Count;

    }

    if (n < 0)
    {
        n += Elements.Count; // 負の値の場合、正のインデックスに変換
    }

    return n;
}
void UpdateElement(){
 if (Elements != null && Elements.Count > 0)
    {
      if (tempElements!=null)
      {
       if (Elements.SequenceEqual(tempElements)&&Index(active) == Index(tempactive))
      {
       return; 
      }
      }
     
      if (CurrentElement != null){
         Exit(CurrentElement);
      }
     
      CurrentElement = Elements[Index(active)]; 
     tempElements = new List<T>(Elements);  // Elementsをコピーして代入参照解除のため
 tempactive = active;
      Change();
     
    }
}

public virtual void KeyDown(){}
  void Update()
  {
    if (Elements != null) 
    {
      Elements.RemoveAll(x=>x==null);
    }
    UpdateElement();

    if ((ChangableCondition == null || ChangableCondition()) && isInputable)
    {
      if (keiinput.Instance.GetKey(DecideKey))
      {
        DecideEvent();
      }
      if (keiinput.Instance.GetKey(AddKey))
      {
        active++;
        KeyDown();
      }
      if (keiinput.Instance.GetKey(DownKey))
      {
        active--;
        KeyDown();
      }
      if (AddDownAxisNumber != -1&&Input.GetAxis("Axis "+AddDownAxisNumber)==0)
      {
       DontSequenceAxisChange=false; 
      }
      if (!DontSequenceAxisChange&&AddDownAxisNumber != -1 && Input.GetAxis("Axis "+AddDownAxisNumber)!=0)
      {
        DontSequenceAxisChange=true;
        if (Input.GetAxis("Axis "+AddDownAxisNumber) > 0)
        {
          active++;
        }
        else
        {
          active--;
        }
      }

    }



    UpdateCallBack();
  }
bool DontSequenceAxisChange;

}