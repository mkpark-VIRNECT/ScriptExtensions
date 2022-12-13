using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static partial class UnityExtensions
{

	public static AnimationCurve FlipValue (this AnimationCurve curve, float pivotValue = 1)
	{
		var newCurve = new AnimationCurve();
		for (int i = 0; i < curve.keys.Length; i++)
		{
			newCurve.AddKey(new Keyframe(curve.keys[i].time, pivotValue - curve.keys[i].value));
		}

		return newCurve;
	}

	public static AnimationCurve FlipTime (this AnimationCurve curve, float pivotTime = 1)
	{
		var newCurve = new AnimationCurve();
		for (int i = 0; i < curve.keys.Length; i++)
		{
			newCurve.AddKey(new Keyframe(pivotTime - curve.keys[i].time, curve.keys[i].value));
		}

		return newCurve;
	}
}
