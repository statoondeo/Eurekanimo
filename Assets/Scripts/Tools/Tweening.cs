using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe utilitaire avec les méthodes de Tweening
/// </summary>
public static class Tweening
{
	#region Méthodes exposées

	public static Coroutine MoveTo(this RectTransform rectTransform, Vector2 targetPosition, float duration, Func<float, float> tweening)
	{
		return (GameManager.Instance.StartCoroutine(MoveToRoutine(rectTransform, targetPosition, duration, tweening)));
	}

	public static Coroutine MoveTo(this Transform transform, Vector3 targetPosition, float duration, Func<float, float> tweening)
	{
		return (GameManager.Instance.StartCoroutine(MoveToRoutine(transform, targetPosition, duration, tweening)));
	}

	public static Coroutine ZoomTo(this Transform transform, Vector3 targetZoom, float duration, Func<float, float> tweening)
	{
		return (GameManager.Instance.StartCoroutine(ZoomToRoutine(transform, targetZoom, duration, tweening)));
	}

	public static Coroutine ColorTo(this SpriteRenderer sprite, Color targetColor, float duration, Func<float, float> tweening)
	{
		return (GameManager.Instance.StartCoroutine(ColorToRoutine(sprite, targetColor, duration, tweening)));
	}

	public static Coroutine ColorTo(this Image image, Color targetColor, float duration, Func<float, float> tweening)
	{
		return (GameManager.Instance.StartCoroutine(ColorToRoutine(image, targetColor, duration, tweening)));
	}

	#endregion

	#region Fonctions de tweening

	public static float SinInOut(float progress)
	{
		return (-(Mathf.Cos(Mathf.PI * progress) - 1) / 2);
	}

	public static float QuintIn(float progress)
	{
		return (Mathf.Pow(progress, 5));
	}

	public static float QuintOut(float progress)
	{
		return (1 - Mathf.Pow(1 - progress, 5));
	}

	public static float QuintInOut(float progress)
	{
		return (progress < 0.5f ? 16.0f * Mathf.Pow(progress, 5) : 1 - Mathf.Pow(-2 * progress + 2, 5) / 2);
	}

	public static float ElasticOut(float progress)
	{
		const float c4 = (2 * Mathf.PI) / 3;
		return (progress == 0 ? 0 : progress == 1 ? 1 : Mathf.Pow(2, -10 * progress) * Mathf.Sin((progress * 10 - 0.75f) * c4) + 1);
	}

	#endregion

	#region Coroutines de transitions

	private static IEnumerator MoveToRoutine(this RectTransform rectTransform, Vector2 targetPosition, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		Vector2 originPosition = rectTransform.anchoredPosition;
		while (ttl < duration)
		{
			if (null == rectTransform) yield break;
			rectTransform.anchoredPosition = Vector2.Lerp(originPosition, targetPosition, tweening(ttl / duration));
			yield return (null);
			ttl += Time.deltaTime;
		}
		rectTransform.anchoredPosition = targetPosition;
	}

	private static IEnumerator MoveToRoutine(this Transform transform, Vector3 targetPosition, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		Vector3 originPosition = transform.position;
		while (ttl < duration)
		{
			if (null == transform) yield break;
			transform.position = Vector3.Lerp(originPosition, targetPosition, tweening(ttl / duration));
			yield return (null);
			ttl += Time.deltaTime;
		}
		transform.position = targetPosition;
	}

	private static IEnumerator ZoomToRoutine(Transform transform, Vector3 targetZoom, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		Vector3 originZoom = transform.localScale;
		while (ttl < duration)
		{
			if (null == transform) yield break;
			transform.localScale = Vector3.Lerp(originZoom, targetZoom, tweening(ttl / duration));
			yield return (null);
			ttl += Time.deltaTime;
		}
		transform.localScale = targetZoom;
	}

	private static IEnumerator ColorToRoutine(SpriteRenderer sprite, Color targetColor, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		Color originColor = sprite.color;
		while (ttl < duration)
		{
			if (null == sprite) yield break;
			sprite.color = Color.Lerp(originColor, targetColor, tweening(ttl / duration));
			yield return (null);
			ttl += Time.deltaTime;
		}
		sprite.color = targetColor;
	}

	private static IEnumerator ColorToRoutine(Image image, Color targetColor, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		Color originColor = image.color;
		while (ttl < duration)
		{
			if (null == image) yield break;
			image.color = Color.Lerp(originColor, targetColor, tweening(ttl / duration));
			yield return (null);
			ttl += Time.deltaTime;
		}
		image.color = targetColor;
	}

	#endregion
}