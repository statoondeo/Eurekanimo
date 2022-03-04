using System.Collections;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
	[SerializeField] ScriptableEvent OnSceneRequested;
	[SerializeField] Animator Animator;
	[SerializeField] float TransitionTime;
	[SerializeField] bool IntroTransition;
	[SerializeField] bool OuttroTransition;

	protected void Awake()
	{
		Debug.Assert(Animator != null, gameObject.name + "/Animator not set");
		Debug.Assert(TransitionTime != 0, gameObject.name + "/TransitionTime not set");
	}

	protected void Start()
    {
		if (IntroTransition)
		{
			Animator.SetTrigger("Intro");
			Invoke(nameof(DisableTransitionPanel), TransitionTime);
		}
	}

	public void OnSceneTransitionRequestedCallback(ScriptableEventArg eventArg)
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
		OnSceneRequested.Raise(new OnSceneRequestedEventArg() { Scene = scene });
	}
}
