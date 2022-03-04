using UnityEngine;

/// <summary>
/// Controller permettant de g�rer le flow de la sc�ne de jeu
/// </summary>
public class GameplayManager : MonoBehaviour
{
	[SerializeField] protected ScriptableGameplay ScriptableGameplay;
	[SerializeField] protected GameplayView GameplayView;

	/// <summary>
	/// Contr�le des donn�es manipul�es
	/// </summary>
	private void Awake()
	{
		Debug.Assert(null != ScriptableGameplay, gameObject.name + "/ScriptableGameplay not set!");
		Debug.Assert(null != GameplayView, gameObject.name + "/GameplayView not set!");
	}

	public void OnMenuClick()
	{
		SoundManager.Instance.Play(SoundManager.Clips.ClickSound);
		ScriptableGameplay.OnSceneRequested.Raise(new OnSceneTransitionRequestedEventArg() { Scene = SceneNames.Menu });
	}
}
