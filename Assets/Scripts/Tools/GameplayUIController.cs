using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
	[SerializeField] protected RectTransform RightPanel;
	[SerializeField] protected ParticleSystem RightPanelParticles;
	[SerializeField] protected AudioClip RightPanelClip;

	[SerializeField] protected RectTransform WrongPanel;
	[SerializeField] protected AudioClip WrongPanelClip;

	[SerializeField] protected float TransitionsDuration;

	protected Vector2 RightPanelInitialPosition;
	protected Vector2 WrongPanelInitialPosition;

	private void Start()
	{
		RightPanelInitialPosition = RightPanel.anchoredPosition;
		WrongPanelInitialPosition = WrongPanel.anchoredPosition;
	}

	public void OnGridCorrectlyFilled()
	{
		StartCoroutine(RightPanel.MoveToRoutine(Vector2.zero, TransitionsDuration, Tweening.QuintOut));
		SoundManager.Instance.Play(SoundManager.Clips.VictoryTheme);
		RightPanelParticles.Play();
	}
	public void OnGridWronglyFilled()
	{
		StartCoroutine(WrongPanel.MoveToRoutine(Vector2.zero, TransitionsDuration, Tweening.QuintOut));
		SoundManager.Instance.Play(SoundManager.Clips.DefeatTheme);
	}

	public void OnMaskWrongPanel()
	{
		SoundManager.Instance.Play(SoundManager.Clips.ClickSound);
		StartCoroutine(WrongPanel.MoveToRoutine(WrongPanelInitialPosition, TransitionsDuration, Tweening.QuintIn));
	}
}
