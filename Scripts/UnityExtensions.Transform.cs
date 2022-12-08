using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static partial class UnityExtensions
{
	public static void SetTransform (this Transform source, Transform dest, bool ignoreScale = false)
	{
		source.position = dest.position;
		source.rotation = dest.rotation;

		if (ignoreScale == false)
		{
			source.localScale = dest.localScale;
		}
	}

	/// <summary>
	/// 위치, 회전, 스케일을 전부 초기화 시킴.
	/// </summary>
	public static void ResetTransform (this Transform source, bool ignoreRotation, bool ignoreScale, Space space = Space.Self)
	{
		if (space == Space.World)
		{
			if (ignoreRotation == false)
				source.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			else
				source.position = Vector3.zero;
		}
		else
		{
			source.localPosition = Vector3.zero;
			if (ignoreRotation == false)
				source.localRotation = Quaternion.identity;
		}

		if (ignoreScale == false) source.localScale = Vector3.one;
	}

	/// <summary>
	/// 위치, 회전, 스케일을 전부 초기화 시킴.
	/// </summary>
	public static void ResetTransform (this Transform source, Space space = Space.Self)
	{
		if (space == Space.World)
		{
			source.position = Vector3.zero;
			source.rotation = Quaternion.identity;
		}
		else
		{
			source.localPosition = Vector3.zero;
			source.localRotation = Quaternion.identity;
		}

		source.localScale = Vector3.one;
	}

	public static Transform FindChildWithName (this Transform source, string name)
	{
		for (int i = 0; i < source.childCount; i++)
		{
			var child = source.GetChild(i);
			if (child.name.Equals(name))
			{
				return child;
			}
			else if (child.childCount > 0)
			{
				var grandchild = child.FindChildWithName(name);
				if (grandchild != null)
					return grandchild;
			}
			else
			{
				return null;
			}
		}
		return null;
	}
}
