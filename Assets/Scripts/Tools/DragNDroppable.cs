using UnityEngine;

public class DragNDroppable : MonoBehaviour
{
	public Vector3 InitialPosition { get; protected set; }

	public ScriptableToken ScriptableToken
	{
		get => mScriptableToken;
		set
		{
			if (mScriptableToken != value)
			{
				mScriptableToken = value;
				FaceRenderer.sprite = ScriptableToken.Sprite;
			}
		}
	}

	[Header("Gestion du drag'n'drop")]
	[Tooltip("Lorsqu'un token est 'draggé'"), SerializeField] protected ScriptableEvent OnTokenDragged;
	[Tooltip("Lorsqu'un token est 'droppé'"), SerializeField] protected ScriptableEvent OnTokenDropped;
	[Tooltip("Pour détection des dropzones"), SerializeField] protected LayerMask DroppableLayerMask;
	[Tooltip("Son lors du drag"), SerializeField] protected AudioClip DragClip;
	[Tooltip("Son lors du drop"), SerializeField] protected AudioClip DropClip;
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

	protected ScriptableToken mScriptableToken;
	protected Camera Camera;
	protected bool IsDragging;
	protected Vector3 PositionBeforeDragBegin;
	protected Vector3 Offset;
	protected Vector3 ZoomOutTarget;
	protected Vector3 ZoomInTarget;
	protected int InitialHaloSortingLayer;
	protected int InitialFaceSortingLayer;
	protected int InitialBackgroundSortingLayer;

	#region Messages Unity

	private void Awake()
	{
		Debug.Assert(null != OnTokenDragged, gameObject.name + "/OnTokenDragged not set!");
		Debug.Assert(null != OnTokenDropped, gameObject.name + "/OnTokenDropped not set!");
		Debug.Assert(null != DragClip, gameObject.name + "/DragClip not set!");
		Debug.Assert(null != DropClip, gameObject.name + "/DropClip not set!");
		Debug.Assert(null != BackgroundRenderer, gameObject.name + "/BackgroundRenderer not set!");
		Debug.Assert(null != HaloRenderer, gameObject.name + "/HaloRenderer not set!");
		Debug.Assert(null != FaceRenderer, gameObject.name + "/FaceRenderer not set!");
		Debug.Assert(null != StartNStopParticles, gameObject.name + "/StartNStopParticles not set!");
		Debug.Assert(null != MoveParticles, gameObject.name + "/MoveParticles not set!");
		Debug.Assert(0 != ZoomFactor, gameObject.name + "/ZoomFactor not set!");
		Debug.Assert(0 != TransitionsDuration, gameObject.name + "/TransitionsDuration not set!");
		Debug.Assert(0 != AppearDuration, gameObject.name + "/TransitionsDuration not set!");
	}

	private void Start()
	{
		Camera = Camera.main;
		InitialPosition = transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y + 16.0f, transform.position.z);
		ZoomInTarget = FaceRenderer.transform.localScale;
		ZoomOutTarget = new Vector3(ZoomInTarget.x * ZoomFactor, ZoomInTarget.y * ZoomFactor, ZoomInTarget.z);
		HaloRenderer.color = new Color(HaloRenderer.color.r, HaloRenderer.color.g, HaloRenderer.color.b, 0.0f);
		InitialHaloSortingLayer = HaloRenderer.sortingLayerID;
		InitialFaceSortingLayer = FaceRenderer.sortingLayerID;
		InitialBackgroundSortingLayer = BackgroundRenderer.sortingLayerID;
		StartCoroutine(transform.MoveToRoutine(InitialPosition, TransitionsDuration + Random.Range(0.0f, AppearDuration), Tweening.QuintOut));
	}

	private void OnMouseDown()
	{
		IsDragging = true;
		PositionBeforeDragBegin = transform.position;
		Offset = PositionBeforeDragBegin - Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));


		HaloRenderer.sortingLayerName = SortingLayer;
		FaceRenderer.sortingLayerName = SortingLayer;
		BackgroundRenderer.sortingLayerName = SortingLayer;

		SoundManager.Instance.PlayOneShot(DragClip);
		StartNStopParticles.Play();
		StartCoroutine(FaceRenderer.transform.ZoomToRoutine(ZoomOutTarget, TransitionsDuration, Tweening.QuintOut));
		StartCoroutine(HaloRenderer.AlphaToRoutine(1.0f, TransitionsDuration, Tweening.QuintOut));
		MoveParticles.Play();

		OnTokenDragged.Raise(new OnTokenDraggedScriptableEventArg(this));
	}

	private void OnMouseUp()
	{
		IsDragging = false;

		HaloRenderer.sortingLayerID = InitialHaloSortingLayer;
		FaceRenderer.sortingLayerID = InitialFaceSortingLayer;
		BackgroundRenderer.sortingLayerID = InitialBackgroundSortingLayer;

		SoundManager.Instance.PlayOneShot(DropClip);
		StartNStopParticles.Play();
		StartCoroutine(FaceRenderer.transform.ZoomToRoutine(ZoomInTarget, TransitionsDuration, Tweening.QuintOut));
		StartCoroutine(HaloRenderer.AlphaToRoutine(0.0f, TransitionsDuration, Tweening.QuintOut));
		MoveParticles.Stop();

		OnTokenDropped.Raise(new OnTokenDroppedScriptableEventArg(this, GetTargetDropZone()));
	}

	private void Update()
	{
		if (!IsDragging) return;

		// nouvelle position
		Vector3 newPosition = Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, PositionBeforeDragBegin.z - 1.0f)) + Offset;

		// On applique ce mouvement
		transform.position = newPosition;
	}

	#endregion

	public void Explode()
	{
		StartNStopParticles.Play();
	}

	public void MoveTo(Vector3 targetPosition)
	{
		StartCoroutine(transform.MoveToRoutine(targetPosition, TransitionsDuration, Tweening.QuintOut));
	}

	protected DropZone GetTargetDropZone()
	{
		Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, DroppableLayerMask);
		return (null == hit.collider ? null : hit.collider.GetComponent<DropZone>());
	}
}
