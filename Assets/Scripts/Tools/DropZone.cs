using System.Collections;
using UnityEngine;

public class DropZone : MonoBehaviour
{
	[SerializeField] protected float AppearDuration;
	[SerializeField] protected float TransitionsDuration;
	[SerializeField] protected float ZoomFactor;

	protected SpriteRenderer SpriteRenderer;
	protected Collider2D Collider;
	protected Vector3 ZoomInScale;
	protected Vector3 BaseZoomScale;
	protected Vector3 ZoomOutScale;
	protected Vector3 InitialPosition;

	private void Awake()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
		Collider = GetComponent<Collider2D>();

		Debug.Assert(null != SpriteRenderer, gameObject.name + "/SpriteRenderer not set!");
		Debug.Assert(null != Collider, gameObject.name + "/Collider not set!");
		Debug.Assert(0 != AppearDuration, gameObject.name + "/AppearDuration not set!");
		Debug.Assert(0 != TransitionsDuration, gameObject.name + "/TransitionsDuration not set!");
		Debug.Assert(0 != ZoomFactor, gameObject.name + "/ZoomFactor not set!");
	}

	private void Start()
	{
		InitialPosition = transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y + 16.0f, transform.position.z);
		Collider.enabled = false;
		BaseZoomScale = transform.localScale;
		ZoomOutScale = new Vector3(BaseZoomScale.x * ZoomFactor, BaseZoomScale.y * ZoomFactor, BaseZoomScale.z);
		ZoomInScale = new Vector3(BaseZoomScale.x / ZoomFactor, BaseZoomScale.y / ZoomFactor, BaseZoomScale.z);
		StartCoroutine(transform.MoveToRoutine(InitialPosition, TransitionsDuration + Random.Range(0.0f, AppearDuration), Tweening.QuintOut));
	}

	public void ColorTo(Color targetColor)
	{
		StartCoroutine(SpriteRenderer.ColorToRoutine(targetColor, TransitionsDuration, Tweening.QuintOut));
	}

	public void OnTokenDraggedCallback(ScriptableEventArg eventArg)
	{
		Collider.enabled = true;
	}

	public void OnTokenDroppedCallback(ScriptableEventArg eventArg)
	{
		Collider.enabled = false;
	}

	public void OnDropZoneEmptiedCallback(ScriptableEventArg eventArg)
	{
		if (this == (eventArg as OnDropZoneEmptiedScriptableEventArg).DropZone)
		{
			StartCoroutine(QuintOutElasticOutZoom(ZoomInScale));
		}
	}

	public void OnDropZoneFilledCallback(ScriptableEventArg eventArg)
	{
		if (this == (eventArg as OnDropZoneFilledScriptableEventArg).DropZone)
		{
			StartCoroutine(QuintOutElasticOutZoom(ZoomOutScale));
		}
	}

	protected IEnumerator QuintOutElasticOutZoom(Vector3 targetZoomScale)
	{
		yield return (transform.ZoomToRoutine(targetZoomScale, TransitionsDuration * 0.5f, Tweening.QuintOut));
		yield return (transform.ZoomToRoutine(BaseZoomScale, TransitionsDuration * 0.5f, Tweening.ElasticOut));
	}
}
