using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Token : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[HideInInspector] public RectTransform RectTransform;
	[HideInInspector] public Slot Container;

	[HideInInspector] public ScriptableToken ScriptableToken
	{
		get => mScriptableToken;
		set
		{
			mScriptableToken = value;
			Image.sprite = mScriptableToken?.Sprite;
		}
	}

	[SerializeField] protected ScriptableEvent OnDragStarted;
	[SerializeField] protected ScriptableEvent OnDragEnded;
	[SerializeField] protected Canvas Canvas;
	[SerializeField] protected Image Image;
	[SerializeField] protected CanvasGroup HaloCanvasGroup;
	[SerializeField] protected CanvasGroup CanvasGroup;
	[SerializeField] protected float FadingDuration;
	[SerializeField] protected float GotoPositionDuration;

	protected ScriptableToken mScriptableToken;
	protected Vector2 PositionBeforeDrag;
	protected Slot DroppableBeforeDrag;
	protected Vector2 InitialPosition;
	protected bool HaloFadingRunning;
	protected Coroutine HaloFadingCoroutineHandler;

	private void Start()
	{
		RectTransform = GetComponent<RectTransform>();
		InitialPosition = RectTransform.anchoredPosition;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		OnDragStarted.Raise();
		if (null != HaloFadingCoroutineHandler) StopCoroutine(HaloFadingCoroutineHandler);
		HaloFadingCoroutineHandler = StartCoroutine(FadeTo(HaloCanvasGroup, 1.0f, FadingDuration));
		PositionBeforeDrag = RectTransform.anchoredPosition;
		DroppableBeforeDrag = Container;
		if (null != Container)
		{
			Container.ContainedToken = null;
			Container = null;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		RectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (null == Container)
		{
			if (null == DroppableBeforeDrag)
			{
				GotoPosition(PositionBeforeDrag);
			}
			else
			{
				GotoPosition(InitialPosition);
			}
		}
		if (null != HaloFadingCoroutineHandler) StopCoroutine(HaloFadingCoroutineHandler);
		HaloFadingCoroutineHandler = StartCoroutine(FadeTo(HaloCanvasGroup, 0.0f, FadingDuration));
		OnDragEnded.Raise();
	}

	protected void HideToken()
	{
		CanvasGroup.blocksRaycasts = false;
	}

	protected void ShowToken()
	{
		CanvasGroup.blocksRaycasts = true;
	}

	public void GotoPosition(Vector2 newPosition)
	{
		StartCoroutine(ReturnToPosition(newPosition, GotoPositionDuration));
	}

	public void ReturnToInitialPosition()
	{
		Container = null;
		GotoPosition(InitialPosition);
	}

	public void OnDragStartedCallback()
	{
		HideToken();
	}
	public void OnDragEndedCallback()
	{
		ShowToken();
	}

	protected IEnumerator ReturnToPosition(Vector2 targetPosition, float duration)
	{
		if (null != RectTransform)
		{
			float ttl = 0.0f;
			Vector2 originalPosition = RectTransform.anchoredPosition;
			while (ttl <= duration)
			{
				ttl += Time.deltaTime;
				RectTransform.anchoredPosition = Vector2.Lerp(originalPosition, targetPosition, Tweening.QuintOut(ttl / duration));
				yield return (null);
			}
			RectTransform.anchoredPosition = targetPosition;
		}
	}

	protected IEnumerator FadeTo(CanvasGroup target, float targetValue, float duration)
	{
		float ttl = 0.0f;
		float originalValue = target.alpha;
		while (ttl < duration)
		{
			ttl += Time.deltaTime;
			target.alpha = Mathf.Lerp(originalValue, targetValue, Tweening.QuintOut(ttl / duration));
			yield return (null);
		}
		target.alpha = targetValue;
	}
}
