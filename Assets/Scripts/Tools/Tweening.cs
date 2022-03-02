using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Classe utilitaire avec les méthodes de Tweening
/// </summary>
public static class Tweening
{
	#region Fonctions de tweening

	/// <summary>
	/// Départ progressif
	/// </summary>
	/// <param name="progress"></param>
	/// <returns></returns>
	public static float QuintIn(float progress)
	{
		return (Mathf.Pow(progress, 5));
	}

	/// <summary>
	/// Amortissement de la fin du mouvement
	/// </summary>
	/// <param name="progress"></param>
	/// <returns></returns>
	public static float QuintOut(float progress)
	{
		return (1 - Mathf.Pow(1 - progress, 5));
	}

	/// <summary>
	/// Retour élastique
	/// </summary>
	/// <param name="progress"></param>
	/// <returns></returns>
	public static float ElasticOut(float progress)
	{
		const float c4 = (2 * Mathf.PI) / 3;
		return (progress == 0 ? 0 : progress == 1 ? 1 : Mathf.Pow(2, -10 * progress) * Mathf.Sin((progress * 10 - 0.75f) * c4) + 1);
	}

	#endregion

	#region Fonctions de transitions

	/// <summary>
	/// Déplacement
	/// </summary>
	/// <param name="transform">Objet à déplacer</param>
	/// <param name="targetPosition">Destination</param>
	/// <param name="duration">Durée</param>
	/// <param name="tweening">Méthode</param>
	/// <returns></returns>
	public static IEnumerator MoveToRoutine(this RectTransform rectTransform, Vector2 targetPosition, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		Vector2 originPosition = rectTransform.anchoredPosition;
		while (ttl < duration)
		{
			rectTransform.anchoredPosition = Vector2.Lerp(originPosition, targetPosition, tweening(ttl / duration));
			yield return (null);
			ttl += Time.deltaTime;
		}
		rectTransform.anchoredPosition = targetPosition;
	}

	/// <summary>
	/// Déplacement
	/// </summary>
	/// <param name="transform">Objet à déplacer</param>
	/// <param name="targetPosition">Destination</param>
	/// <param name="duration">Durée</param>
	/// <param name="tweening">Méthode</param>
	/// <returns></returns>
	public static IEnumerator MoveToRoutine(this Transform transform, Vector3 targetPosition, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		Vector3 originPosition = transform.position;
		while (ttl < duration)
		{
			transform.position = Vector3.Lerp(originPosition, targetPosition, tweening(ttl / duration));
			yield return (null);
			ttl += Time.deltaTime;
		}
		transform.position = targetPosition;
	}

	/// <summary>
	/// Zoom
	/// </summary>
	/// <param name="transform">Objet à zoom</param>
	/// <param name="targetZoom">Taille à atteindre</param>
	/// <param name="duration">Durée</param>
	/// <param name="tweening">Méthode</param>
	/// <returns></returns>
	public static IEnumerator ZoomToRoutine(this Transform transform, Vector3 targetZoom, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		Vector3 originZoom = transform.localScale;
		while (ttl < duration)
		{
			transform.localScale = Vector3.Lerp(originZoom, targetZoom, tweening(ttl / duration));
			yield return (null);
			ttl += Time.deltaTime;
		}
		transform.localScale = targetZoom;
	}

	/// <summary>
	/// alpha
	/// </summary>
	/// <param name="transform">sprite</param>
	/// <param name="targetZoom">Alpha à atteindre</param>
	/// <param name="duration">Durée</param>
	/// <param name="tweening">Méthode</param>
	/// <returns></returns>
	public static IEnumerator AlphaToRoutine(this SpriteRenderer sprite, float targetValue, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		float originValue = sprite.color.a;
		while (ttl < duration)
		{
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(originValue, targetValue, tweening(ttl / duration)));
			yield return (null);
			ttl += Time.deltaTime;
		}
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, targetValue);
	}

	/// <summary>
	/// Couleur
	/// </summary>
	/// <param name="transform">sprite</param>
	/// <param name="targetZoom">Alpha à atteindre</param>
	/// <param name="duration">Durée</param>
	/// <param name="tweening">Méthode</param>
	/// <returns></returns>
	public static IEnumerator ColorToRoutine(this SpriteRenderer sprite, Color targetColor, float duration, Func<float, float> tweening)
	{
		float ttl = 0.0f;
		Color originColor = sprite.color;
		while (ttl < duration)
		{
			sprite.color = Color.Lerp(originColor, targetColor, tweening(ttl / duration));
			yield return (null);
			ttl += Time.deltaTime;
		}
		sprite.color = targetColor;
	}

	#endregion
}