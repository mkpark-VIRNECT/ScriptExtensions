#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Collections.Generic;
[CustomEditor(typeof(UnityEngine.Object),true)]
public class ObjectInspector : Editor
{
    Dictionary<string, bool> methodFoldOutTable = new();
    Dictionary<string, bool> parentFoldOutTable = new();
    Dictionary<string, object[]> parameterTable = new();
    GUIStyle foldoutStyle;

    public override void OnInspectorGUI ()
    {
        if (target is Transform)
            return;
        DrawDefaultInspector();
        GUILayout.Space(10);
        SetStyle();

        string classKey = target.GetType().FullName;
        if (!parentFoldOutTable.ContainsKey(classKey))
        {
            parentFoldOutTable[classKey] = true;
        }

        parentFoldOutTable[classKey] = EditorGUILayout.BeginToggleGroup("Method Buttons", parentFoldOutTable[classKey]);
        if (parentFoldOutTable[classKey])
        {
            var methods = target.GetType().GetRuntimeMethods();
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == typeof(ButtonAttribute))
                    {
                        GUILayout.BeginVertical("HelpBox");
                        {
                            string key = target.GetType().Name + method.Name;
                            if (!methodFoldOutTable.ContainsKey(key))
                            {
                                methodFoldOutTable[key] = true;
                            }
                            methodFoldOutTable[key] = EditorGUILayout.BeginFoldoutHeaderGroup(methodFoldOutTable[key], method.Name, foldoutStyle);
                            if (methodFoldOutTable[key])
                            {
                                var parameterInfos = method.GetParameters();
                                if (!parameterTable.ContainsKey(key))
                                {
                                    parameterTable[key] = new object[parameterInfos.Length];
                                }
                                var parameters = parameterTable[key];
                                var parameterIdx = 0;
                                foreach (var parameterInfo in parameterInfos)
                                {
                                    DrawMethodParameter(parameterInfo, parameterIdx, ref parameters);
                                    ++parameterIdx;
                                }
                                if (GUILayout.Button(text: "Invoke"))
                                {
                                    method.Invoke(target, parameters);
                                }
                            }
                            EditorGUILayout.EndFoldoutHeaderGroup();
                        }
                        GUILayout.EndVertical();
                    }
                }
            }
        }
        EditorGUILayout.EndToggleGroup();
    }

    void SetStyle ()
    {
        foldoutStyle = EditorStyles.foldoutHeader;
        foldoutStyle.margin = new RectOffset() { left = 18, right = 10 };
    }

    void DrawMethodParameter (ParameterInfo parameterInfo, int paramIdx, ref object[] parameters)
    {
        var parameterType = parameterInfo.ParameterType;
        bool hasDefaultValue = parameterInfo.HasDefaultValue;
        var defaultValue = parameterInfo.DefaultValue;
        var parameterName = parameterInfo.Name;

        if (parameters[paramIdx] == null)
        {
            if (hasDefaultValue)
            {
                parameters[paramIdx] = defaultValue;
            }
            else
            {
                try
                {
                    parameters[paramIdx] = Activator.CreateInstance(parameterType);
                }
                catch (Exception e)
                {
                    parameters[paramIdx] = default;
                }
            }
        }

        if (parameterType == typeof(int))
        {
            parameters[paramIdx] = EditorGUILayout.IntField(parameterName, ( int ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(float))
        {
            parameters[paramIdx] = EditorGUILayout.FloatField(parameterName, ( float ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(double))
        {
            parameters[paramIdx] = EditorGUILayout.DoubleField(parameterName, ( double ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(string))
        {
            parameters[paramIdx] = EditorGUILayout.TextField(parameterName, ( string ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(Vector2))
        {
            parameters[paramIdx] = EditorGUILayout.Vector2Field(parameterName, ( Vector2 ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(Vector3))
        {
            parameters[paramIdx] = EditorGUILayout.Vector3Field(parameterName, ( Vector3 ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(Vector4))
        {
            parameters[paramIdx] = EditorGUILayout.Vector4Field(parameterName, ( Vector4 ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(Quaternion))
        {
            parameters[paramIdx] = EditorGUILayout.Vector4Field(parameterName, ( Vector4 ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(Bounds))
        {
            parameters[paramIdx] = EditorGUILayout.BoundsField(parameterName, ( Bounds ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(BoundsInt))
        {
            parameters[paramIdx] = EditorGUILayout.BoundsIntField(parameterName, ( BoundsInt ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(Color))
        {
            parameters[paramIdx] = EditorGUILayout.ColorField(parameterName, ( Color ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(AnimationCurve))
        {
            parameters[paramIdx] = EditorGUILayout.CurveField(parameterName, ( AnimationCurve ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(Enum))
        {
            parameters[paramIdx] = EditorGUILayout.EnumFlagsField(parameterName, ( Enum ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(Gradient))
        {
            parameters[paramIdx] = EditorGUILayout.GradientField(parameterName, ( Gradient ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(UnityEngine.Object))
        {
            parameters[paramIdx] = EditorGUILayout.ObjectField(parameterName, ( UnityEngine.Object ) parameters[paramIdx], parameterType, true);
        }
        else if (parameterType == typeof(Rect))
        {
            parameters[paramIdx] = EditorGUILayout.RectField(parameterName, ( Rect ) parameters[paramIdx]);
        }
        else if (parameterType == typeof(RectInt))
        {
            parameters[paramIdx] = EditorGUILayout.RectIntField(parameterName, ( RectInt ) parameters[paramIdx]);
        }
    }
}

#endif