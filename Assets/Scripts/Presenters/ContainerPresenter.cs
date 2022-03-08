using System.Collections;
using UnityEngine;
using static EventManager;

public class ContainerPresenter : MonoBehaviour
{
	[SerializeField] protected float AppearDuration;
	[SerializeField] protected float TransitionsDuration;
	[SerializeField] protected float ZoomFactor;

	protected SpriteRenderer SpriteRenderer;
	protected Collider2D Collider;
	protected Vector3 BaseZoomScale;
	protected Vector3 ZoomOutScale;
	protected Vector3 InitialPosition;
	protected bool PreviouslytHovered;

	protected void Awake()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
		Collider = GetComponent<Collider2D>();

		Debug.Assert(null != SpriteRenderer, gameObject.name + "/SpriteRenderer not set!");
		Debug.Assert(null != Collider, gameObject.name + "/Collider not set!");
		Debug.Assert(0 != AppearDuration, gameObject.name + "/AppearDuration not set!");
		Debug.Assert(0 != TransitionsDuration, gameObject.name + "/TransitionsDuration not set!");
		Debug.Assert(0 != ZoomFactor, gameObject.name + "/ZoomFactor not set!");

		EventManager.Instance.CreateEventListener(gameObject, Events.OnContainerEmptied, OnContainerEmptiedCallback);
		EventManager.Instance.CreateEventListener(gameObject, Events.OnContainerFilled, OnContainerFilledCallback);
		EventManager.Instance.CreateEventListener(gameObject, Events.OnTokenDragged, OnTokenDraggedCallback);
		EventManager.Instance.CreateEventListener(gameObject, Events.OnTokenDropped, OnTokenDroppedCallback);
		EventManager.Instance.CreateEventListener(gameObject, Events.OnContainerHovered, OnContainerHoveredCallback);
	}

	protected IEnumerator Start()
	{
		InitialPosition = transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y + 16.0f, transform.position.z);
		Collider.enabled = false;
		BaseZoomScale = transform.localScale;
		ZoomOutScale = new Vector3(BaseZoomScale.x * ZoomFactor, BaseZoomScale.y * ZoomFactor, BaseZoomScale.z);
		PreviouslytHovered = false;
		yield return (new WaitForSeconds(Random.Range(0.0f, AppearDuration)));
		yield return (StartCoroutine(transform.MoveToRoutine(InitialPosition, TransitionsDuration, Tweening.QuintOut)));
	}

	protected void OnContainerHoveredCallback(ModelEventArg eventArg)
	{
		if (this == (eventArg as OnContainerHoveredEventArg).Container)
		{
			PreviouslytHovered = true;
			// On s'agrandit
			StartCoroutine(transform.ZoomToRoutine(ZoomOutScale, TransitionsDuration, Tweening.QuintOut));
		}
		else if (PreviouslytHovered)
		{
			PreviouslytHovered = false;
			// Retour à la taille normale
			StartCoroutine(transform.ZoomToRoutine(BaseZoomScale, TransitionsDuration, Tweening.QuintOut));    
		}
	}

	public void ColorTo(Color targetColor)
	{
		StartCoroutine(SpriteRenderer.ColorToRoutine(targetColor, TransitionsDuration, Tweening.QuintOut));
	}

	protected void OnTokenDraggedCallback(ModelEventArg eventArg)
	{
		Collider.enabled = true;
	}

	protected void OnTokenDroppedCallback(ModelEventArg eventArg)
	{
		Collider.enabled = false;
	}

	protected void OnContainerEmptiedCallback(ModelEventArg eventArg)
	{
		if (this == (eventArg as OnDropZoneEmptiedScriptableEventArg).DropZone)
		{
			StartCoroutine(transform.ZoomToRoutine(BaseZoomScale, TransitionsDuration, Tweening.ElasticOut));
		}
	}

	protected void OnContainerFilledCallback(ModelEventArg eventArg)
	{
		if (this == (eventArg as OnDropZoneFilledScriptableEventArg).DropZone)
		{
			PreviouslytHovered = false;
			StartCoroutine(transform.ZoomToRoutine(BaseZoomScale, TransitionsDuration, Tweening.ElasticOut));
		}
	}
}
