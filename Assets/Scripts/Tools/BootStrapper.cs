using UnityEngine;
using static EventManager;

/// <summary>
/// Classe définissant la phase de démarrage du jeu
/// </summary>
public class BootStrapper
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void OnRuntimeMethodLoad()
	{
		// Création des managers qui restent toujours présents sans avoir à être placés dans les scènes.
		// Gestion des sons/musiques
		SoundManager.Instantiate();

		// Gestion des évènements
		EventManager.Instantiate();

		// Gestion du jeu (transitions entre scènes)
		GameManager.Instantiate();

		// On démarre le jeu par le menu
		EventManager.Instance.Raise(Events.OnSceneRequested, new OnSceneRequestedEventArg() { Scene = SceneNames.Menu });
	}
}
