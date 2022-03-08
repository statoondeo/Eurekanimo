using UnityEngine;
using static EventManager;

public class GameplayUIPresenter : MonoBehaviour
{
	[SerializeField] protected RectTransform RightPanel;
	[SerializeField] protected ParticleSystem RightPanelParticles;

	[SerializeField] protected RectTransform WrongPanel;
	[SerializeField] protected AudioClip WrongPanelClip;

	[SerializeField] protected float TransitionsDuration;

	protected Vector2 RightPanelInitialPosition;
	protected Vector2 WrongPanelInitialPosition;

	protected void Awake()
	{
		EventManager.Instance.CreateEventListener(gameObject, Events.OnGridCorrectlyFilled, OnGridCorrectlyFilledCallback);
		EventManager.Instance.CreateEventListener(gameObject, Events.OnGridWronglyFilled, OnGridWronglyFilledCallback);
	}

	protected void Start()
	{
		RightPanelInitialPosition = RightPanel.anchoredPosition;
		WrongPanelInitialPosition = WrongPanel.anchoredPosition;
	}

	protected void OnGridCorrectlyFilledCallback(ModelEventArg eventArg)
	{
		StartCoroutine(RightPanel.MoveToRoutine(Vector2.zero, TransitionsDuration, Tweening.QuintOut));
		SoundManager.Instance.Play(SoundManager.Clips.VictoryTheme);
		RightPanelParticles.Play();
		EventManager.Instance.Raise(Events.OnGameNotReady);
	}

	protected void OnGridWronglyFilledCallback(ModelEventArg eventArg)
	{
		StartCoroutine(WrongPanel.MoveToRoutine(Vector2.zero, TransitionsDuration, Tweening.QuintOut));
		SoundManager.Instance.Play(SoundManager.Clips.DefeatTheme);
		EventManager.Instance.Raise(Events.OnGameNotReady);
	}

	public void OnMaskWrongPanel()
	{
		SoundManager.Instance.Play(SoundManager.Clips.ClickSound);
		StartCoroutine(WrongPanel.MoveToRoutine(WrongPanelInitialPosition, TransitionsDuration, Tweening.QuintIn));
		EventManager.Instance.Raise(Events.OnGameReady);
	}
}
