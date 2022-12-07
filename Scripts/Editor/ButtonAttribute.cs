#if UNITY_EDITOR
using System;



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


#endif