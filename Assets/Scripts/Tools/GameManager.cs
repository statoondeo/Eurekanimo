using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe transverse au jeu
/// gérant l'accès aux ressources et les bascules entre scènes.
/// </summary>
public class GameManager : Singleton<GameManager>
{
	protected override void Awake()
	{
		base.Awake();
		// Création des observers
		OnSceneRequestedListener = CreateScriptableEventListener("Data/Events/OnSceneRequested", Instance.OnSceneRequestedCallback);
	}

	/// <summary>
	/// Fonction utilitaire permettant de créer un observateur d'évènement
	/// </summary>
	/// <param name="eventAssetPath">ScriptableEvent asset à charger</param>
	/// <param name="callback">Fonction callback</param>
	/// <returns>Listener</returns>
	protected static ScriptableEventListener CreateScriptableEventListener(string eventAssetPath, UnityAction<ScriptableEventArg> callback)
	{
		// Création de l'évenement au format Unity
		UnityEvent<ScriptableEventArg> unityEvent = new UnityEvent<ScriptableEventArg>();
		unityEvent.AddListener(callback);

		// Chargement de l'asset associée
		ScriptableEvent scriptableEvent = Resources.Load<ScriptableEvent>(eventAssetPath);

		// Création du listener
		ScriptableEventListener eventListener = new ScriptableEventListener(scriptableEvent, unityEvent);

		// Liaison entre le listener et l'événement
		scriptableEvent.RegisterListener(eventListener);

		// On démarre l'écoute
		eventListener.Resume();

		return (eventListener);
	}

	#region Callback des évènements écoutés

	protected void OnSceneRequestedCallback(ScriptableEventArg eventArg)
	{
		LoadScene((eventArg as OnSceneRequestedEventArg).Scene);
	}

	#endregion

	// Evènements écoutés
	// Transitions de scènes  demandées
	protected ScriptableEventListener OnSceneRequestedListener;

	// Changements de scène
	public static void LoadScene(SceneNames sceneName)
	{
		SceneManager.LoadScene(sceneName.ToString());
	}
}
