#if UNITY_EDITOR
using UnityEditor;
using System;


/// <summary>
/// GUILayoutの表示を補助するためのクラス
/// </summary>
public static class GUILayoutHelper
{
    /// <summary>
    /// 値の変更の確認を開始する。
    /// </summary>
    /// <param name="onShowGUI">GUIの表示をする処理</param>
    /// <param name="onChanged">変更したときの処理</param>
    public static void BeginChangeCheck (Action onShowGUI, Action onChanged)
    {
        EditorGUI.BeginChangeCheck();
        onShowGUI();
        if (EditorGUI.EndChangeCheck()) onChanged();
    }


    /// <summary>
    /// 水平にUIの表示を開始する。
    /// </summary>
    public static void BeginHorizontal (Action onShowGUI)
    {
        EditorGUILayout.BeginHorizontal();
        onShowGUI();
        EditorGUILayout.EndHorizontal();
    }


    /// <summary>
    /// 垂直にUIの表示を開始する。
    /// </summary>
    public static void BeginVertical (Action onShowGUI)
    {

        EditorGUILayout.BeginVertical();
        onShowGUI();
        EditorGUILayout.EndVertical();
    }

}
#endif