using UnityEngine;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using System.Reflection;

#if UNITY_EDITOR
//namespace UnityEditor
//{
//    [CustomPropertyDrawer(typeof(ButtonAttribute), true)]
//    public class ButtonAttributeDrawer : DecoratorDrawer
//    {

//        public override float GetHeight ()
//        {
//            return base.GetHeight();
//            //public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
//            //var height = EditorGUI.GetPropertyHeight(property, label, true);
//            //return height;
//        }

//        public override void OnGUI (Rect position)
//        {
//            var buttonAttribute = ( ( ButtonAttribute ) attribute );
//            //EditorGUILayout.HelpBox(helpBoxAttribute.text, helpBoxAttribute.messageType);
            
//            if(EditorGUI.LinkButton(position, buttonAttribute.text))
//            {
//                Debug.Log("btn click");
//                //EditorGUI.PropertyField(position, property, label, true);
//            }
//        }
//    }
//}
#endif

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute
{
    public readonly string text;
    public readonly object[] parameters;
    public ButtonAttribute (string text = "", params object[] args)
    {
        this.text = text;
        this.parameters = args;
    }
}

