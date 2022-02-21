using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
	[SerializeField] protected CanvasGroup CanvasGroup;
	[SerializeField] protected float Duration;

	public void OnMouseEnter()
	{
		StartCoroutine(FadeAlpha(1.0f, Duration));
	}

	public void OnMouseExit()
	{
		StartCoroutine(FadeAlpha(0.0f, Duration));
	}

	protected IEnumerator FadeAlpha(float targetAlpha, float duration)
	{
		float ttl = 0.0f;
		float currentAlpha = CanvasGroup.alpha;
		while(ttl <= duration)
		{
			ttl += Time.deltaTime;
			CanvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, ttl / duration);
			yield return (null);
		}
		CanvasGroup.alpha = targetAlpha;
	}
}
