using UnityEngine;

public static class Tweening
{
	public static float QuintOut(float progress)
	{
		return (1 - Mathf.Pow(1 - progress, 5));
	}
}
