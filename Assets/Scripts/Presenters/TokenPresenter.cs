using System.Collections;
using UnityEngine;
using static EventManager;

public class TokenPresenter : MonoBehaviour
{
	public Vector3 InitialPosition { get; protected set; }

	public TokenModel ScriptableToken
	{
		get => mScriptableToken;
		set
		{
			if (mScriptableToken != value)
			{
				mScriptableToken = value;
				FaceRenderer.sprite = ScriptableToken.GetSprite();
			}
		}
	}

	[Header("Gestion du drag'n'drop")]
	[Tooltip("Pour détection des dropzones"), SerializeField] protected LayerMask DroppableLayerMask;
	[Tooltip("Sorting Layer pendant le dnd"), SerializeField] protected string SortingLayer;

	[Header("Comportement visuel pendant le dnd")]
	[Tooltip("Background du token"), SerializeField] protected SpriteRenderer BackgroundRenderer;
	[Tooltip("Valeur faciale du token"), SerializeField] protected SpriteRenderer FaceRenderer;
	[Tooltip("Halo affiché pendant le dnd"), SerializeField] protected SpriteRenderer HaloRenderer;
	[Tooltip("Particules au 'drag' et au 'drop'"), SerializeField] protected ParticleSystem StartNStopParticles;
	[Tooltip("Particules au 'move'"), SerializeField] protected ParticleSystem MoveParticles;
	[Tooltip("Zoom du token"), SerializeField] protected float ZoomFactor;
	[Tooltip("Durée des transitions"), SerializeField] protected float TransitionsDuration;
	[Tooltip("Vitesse d'apparition au début de la scène"), SerializeField] protected float AppearDuration;

	protected TokenModel mScriptableToken;
	protected Camera Camera;
	protected bool IsDraggable;
	protected bool IsDragging;
	protected Vector3 PositionBeforeDragBegin;
	protected Vector3 Offset;
	protected Vector3 ZoomOutTarget;
	protected Vector3 ZoomInTarget;
	protected int InitialHaloSortingLayer;
	protected int InitialFaceSortingLayer;
	protected int InitialBackgroundSortingLayer;
	protected ContainerPresenter LastHoveredContainer;
	protected Color HaloRendererInitialColor;
	protected Color HaloRendererTargetColor;

	#region Messages Unity

	private void Awake()
	{
		Debug.Assert(null != BackgroundRenderer, gameObject.name + "/BackgroundRenderer not set!");
		Debug.Assert(null != HaloRenderer, gameObject.name + "/HaloRenderer not set!");
		Debug.Assert(null != FaceRenderer, gameObject.name + "/FaceRenderer not set!");
		Debug.Assert(null != StartNStopParticles, gameObject.name + "/StartNStopParticles not set!");
		Debug.Assert(null != MoveParticles, gameObject.name + "/MoveParticles not set!");
		Debug.Assert(0 != ZoomFactor, gameObject.name + "/ZoomFactor not set!");
		Debug.Assert(0 != TransitionsDuration, gameObject.name + "/TransitionsDuration not set!");
		Debug.Assert(0 != AppearDuration, gameObject.name + "/TransitionsDuration not set!");

		EventManager.Instance.CreateEventListener(gameObject, Events.OnGameNotReady, OnGameNotReadyCallback);
		EventManager.Instance.CreateEventListener(gameObject, Events.OnGameReady, OnGameReadyCallback);
	}

	private IEnumerator Start()
	{
		Camera = Camera.main;
		InitialPosition = transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y + 16.0f, transform.position.z);
		ZoomInTarget = FaceRenderer.transform.localScale;
		ZoomOutTarget = new Vector3(ZoomInTarget.x * ZoomFactor, ZoomInTarget.y * ZoomFactor, ZoomInTarget.z);
		HaloRendererInitialColor = HaloRenderer.color;
		HaloRendererTargetColor = new Color(HaloRenderer.color.r, HaloRenderer.color.g, HaloRenderer.color.b, 1.0f);
		InitialHaloSortingLayer = HaloRenderer.sortingLayerID;
		InitialFaceSortingLayer = FaceRenderer.sortingLayerID;
		InitialBackgroundSortingLayer = BackgroundRenderer.sortingLayerID;
		IsDraggable = true;
		yield return (new WaitForSeconds(Random.Range(0.0f, AppearDuration)));
		yield return (transform.MoveTo(InitialPosition, TransitionsDuration, Tweening.QuintOut));
	}

	protected void OnMouseDown()
	{
		if (!IsDraggable) return;

		IsDragging = true;
		LastHoveredContainer = null;

		PositionBeforeDragBegin = transform.position;
		Offset = PositionBeforeDragBegin - Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

		HaloRenderer.sortingLayerName = SortingLayer;
		FaceRenderer.sortingLayerName = SortingLayer;
		BackgroundRenderer.sortingLayerName = SortingLayer;

		StartNStopParticles.Play();
		FaceRenderer.transform.ZoomTo(ZoomOutTarget, TransitionsDuration, Tweening.QuintOut);
		HaloRenderer.ColorTo(HaloRendererTargetColor, TransitionsDuration, Tweening.QuintOut);
		MoveParticles.Play();

		EventManager.Instance.Raise(Events.OnTokenDragged, new OnTokenDraggedScriptableEventArg() { Token = this });
	}

	protected void OnMouseUp()
	{
		if (!IsDraggable) return;
		IsDragging = false;

		HaloRenderer.sortingLayerID = InitialHaloSortingLayer;
		FaceRenderer.sortingLayerID = InitialFaceSortingLayer;
		BackgroundRenderer.sortingLayerID = InitialBackgroundSortingLayer;

		StartNStopParticles.Play();
		FaceRenderer.transform.ZoomTo(ZoomInTarget, TransitionsDuration, Tweening.QuintOut);
		HaloRenderer.ColorTo(HaloRendererInitialColor, TransitionsDuration, Tweening.QuintOut);
		MoveParticles.Stop();

		EventManager.Instance.Raise(Events.OnTokenDropped, new OnTokenDroppedScriptableEventArg() { Token = this, DropZone = GetTargetDropZone() });
	}

	protected void Update()
	{
		if (!IsDraggable) return;
		if (!IsDragging) return;

		// nouvelle position
		Vector3 newPosition = Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, PositionBeforeDragBegin.z - 1.0f)) + Offset;

		// On applique ce mouvement
		transform.position = newPosition;

		// On informe du survol
		ContainerPresenter dropZone = GetTargetDropZone();
		if (dropZone != LastHoveredContainer)
		{
			LastHoveredContainer = dropZone;
			EventManager.Instance.Raise(Events.OnContainerHovered, new OnContainerHoveredEventArg() { Container = LastHoveredContainer });
		}
	}

	#endregion

	public void Explode()
	{
		StartNStopParticles.Play();
	}

	public Coroutine MoveTo(Vector3 targetPosition)
	{
		return (transform.MoveTo(targetPosition, TransitionsDuration, Tweening.QuintOut));
	}

	protected ContainerPresenter GetTargetDropZone()
	{
		Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, DroppableLayerMask);
		return (null == hit.collider ? null : hit.collider.GetComponent<ContainerPresenter>());
	}

	protected void OnGameNotReadyCallback(ModelEventArg eventArg)
	{
		IsDraggable = false;
	}

	protected void OnGameReadyCallback(ModelEventArg eventArg)
	{
		IsDraggable = true;
	}
}
