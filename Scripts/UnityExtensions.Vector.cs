using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static partial class UnityExtensions
{
    public static Vector3 SetX (this Vector3 origin, float value = 0) => new Vector3(value, origin.y, origin.z);
    public static Vector3 SetY (this Vector3 origin, float value = 0) => new Vector3(origin.x, value, origin.z);
    public static Vector3 SetZ (this Vector3 origin, float value = 0) => new Vector3(origin.x, origin.y, value);

    public static Vector2 XY (this Vector3 origin) => new Vector2(origin.x, origin.y);
    public static Vector2 XZ (this Vector3 origin) => new Vector2(origin.x, origin.z);
    public static Vector2 YZ (this Vector3 origin) => new Vector2(origin.y, origin.z);

    //public static float 
}
