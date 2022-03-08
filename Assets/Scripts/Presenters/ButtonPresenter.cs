using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EventManager;

public class ButtonPresenter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] protected bool DefaultInteractable;
	[SerializeField] protected Events[] ActivatingEvents;
	[SerializeField] protected Events[] DeactivatingEvents;
	[SerializeField] protected Image ButtonHalo;
	[SerializeField] protected ParticleSystem ButtonExplosion;
	[SerializeField] protected float TransitionSpeed;
	[SerializeField] protected Color DisabledButtonColor;

	protected RectTransform RectTransform;
	protected Button Button;
	protected Image ButtonImage;
	protected Color HaloOriginalColor;
	protected Color HaloHoveredColor;

	protected void Awake()
	{
		RectTransform = GetComponent<RectTransform>();
		Button = GetComponentInChildren<Button>();
		ButtonImage = Button.GetComponent<Image>();

		Debug.Assert(null != Button, gameObject.name + "/Button not set!");
		Debug.Assert(null != ActivatingEvents, gameObject.name + "/ActivatingEvents not set!");
		Debug.Assert(null != DeactivatingEvents, gameObject.name + "/DeactivatingEvents not set!");

		for (int i = 0, nbItems = ActivatingEvents.Length; i < nbItems; i++)
		{
			EventManager.Instance.CreateEventListener(gameObject, ActivatingEvents[i], OnActivateCallback);
		}
		for (int i = 0, nbItems = DeactivatingEvents.Length; i < nbItems; i++)
		{
			EventManager.Instance.CreateEventListener(gameObject, DeactivatingEvents[i], OnDeactivateCallback);
		}
	}

	protected void Start()
	{
		Button.interactable = DefaultInteractable;
		HaloOriginalColor = ButtonHalo.color;
		HaloHoveredColor = new Color(ButtonHalo.color.r, ButtonHalo.color.g, ButtonHalo.color.b, 1.0f);
		ButtonImage.color = Button.interactable ? Color.white : DisabledButtonColor;
	}

	protected void OnActivateCallback(ModelEventArg eventArg)
	{
		if (!Button.interactable)
		{
			Button.interactable = true;
			StartCoroutine(Tweening.ColorToRoutine(ButtonImage, Color.white, TransitionSpeed, Tweening.QuintOut));
			MoveElastic();
		}
	}

	protected void MoveElastic()
	{
		Vector2 initialPosition = RectTransform.anchoredPosition;
		RectTransform.anchoredPosition += new Vector2(100, 0);
		StartCoroutine(Tweening.MoveToRoutine(RectTransform, initialPosition, TransitionSpeed, Tweening.ElasticOut));
	}

	protected void OnDeactivateCallback(ModelEventArg eventArg)
	{
		if (Button.interactable)
		{
			StartCoroutine(Tweening.ColorToRoutine(ButtonHalo, HaloOriginalColor, TransitionSpeed, Tweening.QuintOut));
			StartCoroutine(Tweening.ColorToRoutine(ButtonImage, DisabledButtonColor, TransitionSpeed, Tweening.QuintOut));
			MoveElastic();
			Button.interactable = false;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!Button.interactable) return;
		StartCoroutine(Tweening.ColorToRoutine(ButtonHalo, HaloHoveredColor, TransitionSpeed, Tweening.QuintOut));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!Button.interactable) return;
		StartCoroutine(Tweening.ColorToRoutine(ButtonHalo, HaloOriginalColor, TransitionSpeed, Tweening.QuintOut));
	}

	public void OnPointerClick()
	{
		if (!Button.interactable) return;
		SoundManager.Instance.Play(SoundManager.Clips.ClickSound);
		ButtonExplosion.Play();
	}
}