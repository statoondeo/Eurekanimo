using System.Collections;
using UnityEngine;
using static EventManager;

public class LevelLoader : MonoBehaviour
{
	[SerializeField] protected Animator Animator;
	[SerializeField] protected float TransitionTime;
	[SerializeField] protected bool IntroTransition;
	[SerializeField] protected bool OuttroTransition;

	protected void Awake()
	{
		Debug.Assert(Animator != null, gameObject.name + "/Animator not set");
		Debug.Assert(TransitionTime != 0, gameObject.name + "/TransitionTime not set");

		EventManager.Instance.CreateEventListener(gameObject, Events.OnSceneTransitionRequested, OnSceneTransitionRequestedCallback);
	}

	protected void Start()
    {
		if (IntroTransition)
		{
			Animator.SetTrigger("Intro");
			Invoke(nameof(DisableTransitionPanel), TransitionTime);
		}
	}

	protected void OnSceneTransitionRequestedCallback(ModelEventArg eventArg)
	{
		StartCoroutine(TransitionToNextLevel((eventArg as OnSceneTransitionRequestedEventArg).Scene));
	}

	protected void DisableTransitionPanel()
	{
		Animator.gameObject.SetActive(false);
	}

	protected IEnumerator TransitionToNextLevel(SceneNames scene)
	{
		Animator.gameObject.SetActive(true);
		if (OuttroTransition)
		{
			Animator.SetTrigger("Outtro");
			yield return new WaitForSeconds(TransitionTime);
		}
		EventManager.Instance.Raise(Events.OnSceneRequested, new OnSceneRequestedEventArg() { Scene = scene });
	}
}
