using UnityEngine;

/// <summary>
/// Controller permettant de gérer le flow de la scène de jeu
/// </summary>
public class GameplayManager : MonoBehaviour
{
	[SerializeField] protected ScriptableGameplay ScriptableGameplay;
	[SerializeField] protected GameplayView GameplayView;

	/// <summary>
	/// Contrôle des données manipulées
	/// </summary>
	private void Awake()
	{
		Debug.Assert(null != ScriptableGameplay, gameObject.name + "/ScriptableGameplay not set!");
		Debug.Assert(null != GameplayView, gameObject.name + "/GameplayView not set!");
	}

	public void OnMenuClick()
	{
		ScriptableGameplay.OnMenuSceneRequested.Raise(ScriptableEventArg.Empty);
	}
}
