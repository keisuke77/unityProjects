 using UnityEngine;
 
using System;
using System.Reflection;
public static class inputExtension
{
 public static void CopyAllFields(this object source,ref object target)
    {
        Type type = source.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            field.SetValue(target, field.GetValue(source));
        }
    }

    public static void CopyAllProperties(this object source,ref object target)
    {
        Type type = source.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            if (property.CanWrite)
            {
                property.SetValue(target, property.GetValue(source));
            }
        }
    }

     public static float GetAxis(this string[] axis)
    {
        foreach (var item in axis)
        {
            float n = item.GetAxis();
            if (n != 0)
            {
                return n;
            }
        }
        return 0;
    }
    public static float GetAxis(this string axis)
    {
        try
        {
            return Input.GetAxisRaw(axis);
        }
        catch (System.Exception e)
        {
            return 0;
        }
    }
  public static bool keydown(this KeyCode[] keycodes)
    {
        foreach (KeyCode item in keycodes)
        {
            if (item.keydown())
            {
                return true;
            }
        }
        return false;
    }

    public static bool keyup(this KeyCode[] keycodes)
    {
        foreach (KeyCode item in keycodes)
        {
            if (item.keyup())
            {
                return true;
            }
        }
        return false;
    }
  public static bool keydown(this KeyCode keyCode)
{
    switch (keyCode)
    {
        case KeyCode.Mouse0:
           return Input.GetMouseButtonDown(0);
          
        case KeyCode.Mouse1:
            return Input.GetMouseButtonDown(1); 
             case KeyCode.Mouse2:
            return Input.GetMouseButtonDown(2);
        // 追加したい他のマウスボタンや特別なキーコードの場合をここに追加
        default:
            return Input.GetKeyDown(keyCode);
    }
}
      public static bool keyup(this KeyCode KeyCode)
    { if (KeyCode==KeyCode.Mouse0)
        {
            return Input.GetMouseButtonUp(0);
        }
        if (KeyCode==KeyCode.Mouse1)
        {
            return Input.GetMouseButtonUp(1);
        }
        return Input.GetKeyUp(KeyCode);
    }

    public static bool keyhold(this KeyCode KeyCode)
    {
        return Input.GetKey(KeyCode);
    }
}