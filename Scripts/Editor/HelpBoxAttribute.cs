using UnityEngine;
using System;
using UnityEditor;

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute), true)]
    public class HelpBoxAttributeDrawer : DecoratorDrawer
    {
        // Necessary since some properties tend to collapse smaller than their content
        public override float GetHeight ()
        {
            var helpBoxAttribute = attribute as HelpBoxAttribute;
            if (helpBoxAttribute == null) return base.GetHeight();
            var helpBoxStyle = ( GUI.skin != null ) ? GUI.skin.GetStyle("helpbox") : null;
            if (helpBoxStyle == null) return base.GetHeight();
            return Mathf.Max(40f, helpBoxStyle.CalcHeight(new GUIContent(helpBoxAttribute.text), EditorGUIUtility.currentViewWidth) + 4);
        }
        // Draw a disabled property field
        public override void OnGUI (Rect position)
        {
            var helpBoxAttribute = ( ( HelpBoxAttribute ) attribute );
            EditorGUI.HelpBox(position,helpBoxAttribute.text, helpBoxAttribute.messageType);
            //EditorGUI.PropertyField(position, property, label, true);            
        }

    }
}
#endif

[AttributeUsage(AttributeTargets.All)]
public class HelpBoxAttribute : PropertyAttribute
{
    public readonly string text;
    public readonly MessageType messageType;
    public HelpBoxAttribute (string text = "", MessageType messageType = MessageType.Info)
    {
        this.text = text;
        this.messageType = messageType;
    }
}