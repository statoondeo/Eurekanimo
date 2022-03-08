using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe transverse au jeu
/// gérant les bascules entre scènes.
/// </summary>
public class GameManager : Singleton<GameManager>
{
	private static readonly string PREFAB_PATH = "Prefabs/GameManager";

	public static GameObject Instantiate()
	{
		return (GameObject.Instantiate(Resources.Load(PREFAB_PATH)) as GameObject);
	}

	protected override void Awake()
	{
		base.Awake();

		// Création des observers
		OnSceneRequestedListener = EventManager.Instance.CreateEventListener(gameObject, EventManager.Events.OnSceneRequested, Instance.OnSceneRequestedCallback);
	}

	#region Callback des évènements écoutés

	protected void OnSceneRequestedCallback(ModelEventArg eventArg)
	{
		LoadScene((eventArg as OnSceneRequestedEventArg).Scene);
	}

	#endregion

	// Evènements écoutés
	// Transitions de scènes  demandées
	protected EventListener OnSceneRequestedListener;

	// Changements de scène
	protected void LoadScene(SceneNames sceneName)
	{
		SceneManager.LoadScene(sceneName.ToString());
	}
}
