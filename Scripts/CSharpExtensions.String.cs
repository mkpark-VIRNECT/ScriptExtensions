using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static partial class CSharpExtensions
{
    #region Color Setter
    public static string Red (this string source) => $"<color=#FF0000>{source}</color>";
    public static string Green (this string source) => $"<color=#00FF00>{source}</color>";
    public static string Blue (this string source) => $"<color=#0000FF>{source}</color>";
    public static string Yellow (this string source) => $"<color=#FFFF00>{source}</color>";
    public static string Cyan (this string source) => $"<color=#00FFFF>{source}</color>";
    public static string Magenta (this string source) => $"<color=#FF00FF>{source}</color>";
    public static string Gray (this string source) => $"<color=#888888>{source}</color>";
    public static string Black (this string source) => $"<color=#000000>{source}</color>";
    public static string Color (this string source, Color color) => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{source}</color>"; 
    #endregion
}
