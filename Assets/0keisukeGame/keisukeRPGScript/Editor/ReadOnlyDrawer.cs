using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer: PropertyDrawer
{
	/// <summary>
	/// OnGUI
	/// </summary>
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		GUI.enabled = false; // GUIを無効化して読み取り専用にする
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true; // GUIを再び有効化する
	}
}
#endif