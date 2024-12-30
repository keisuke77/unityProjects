using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ObjectChanger<T> :MonoBehaviour
{
    public int activeIndex = 0;
    public T nowObj;

    [SerializeField]
    protected List<T> objList;

    protected virtual List<T> ObjList => objList;

    public void AddChange()
    {
        activeIndex++;
        if (activeIndex >= ObjList.Count)
        {
            activeIndex = 0;
        }
        ChangeObject();
    }

    public void DownChange()
    {
        activeIndex--;
        if (activeIndex < 0)
        {
            activeIndex = ObjList.Count - 1;
        }
        ChangeObject();
    }

    public void Change(int index)
    {
        if (index < 0 || index >= ObjList.Count) return;
        activeIndex = index;
        ChangeObject();
    }
    public void Execute(System.Action<T> func){
    foreach (var item in ObjList)
    {
        func(item);
    }
    }

    public T GetCurrentObject()
    {
        return nowObj;
    }

    private void ChangeObject()
    {
        if (ObjList.Count > 0)
        {
            nowObj = ObjList[activeIndex];
            OnObjectChanged();
        }
    }

    protected virtual void OnObjectChanged() { }
}
