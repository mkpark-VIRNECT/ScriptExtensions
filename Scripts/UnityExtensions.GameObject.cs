using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

public static partial class UnityExtensions
{
	public static void SetLayerRecursively (this GameObject go, string layerName)
	{
		go.layer = LayerMask.NameToLayer(layerName);
		for (int i = 0; i < go.transform.childCount; i++)
		{
			var child = go.transform.GetChild(i).gameObject;
			child.SetLayerRecursively(layerName);
		}
	}

	public static bool ContainsInLayerMask (this GameObject go, params LayerMask[] layerMasks)
	{
		for (int i = 0; i < layerMasks.Length; i++)
		{
			if (layerMasks[i] == ( layerMasks[i] | ( 1 << go.layer ) ))
			{
				return true;
			}
		}

		return false;
	}

	public static T AddOrGetComponent<T> (this GameObject go) where T : Component
	{
		return go.GetComponent<T>() ?? go.AddComponent<T>();
	}

	public static Component AddOrGetComponent (this GameObject go, System.Type type)
	{
		return go.GetComponent(type) ?? go.AddComponent(type);
	}

	public static T AddClonedComponent<T> (this GameObject go, T comp) where T : Component
	{
		Type type = comp.GetType();
		T addComp = go.AddComponent<T>();
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		PropertyInfo[] pinfos = type.GetProperties(flags);
		foreach (var pinfo in pinfos)
		{
			if (pinfo.CanWrite)
			{
				try
				{
					pinfo.SetValue(addComp, pinfo.GetValue(comp, null), null);
				}
				catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
			}
		}
		FieldInfo[] fields = type.GetFields();
		foreach (System.Reflection.FieldInfo field in fields)
		{
			try
			{
				field.SetValue(addComp, field.GetValue(comp));
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}
		}
		return addComp;
	}
}
