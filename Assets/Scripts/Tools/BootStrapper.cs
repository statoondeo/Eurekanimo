using UnityEngine;
using static EventManager;

/// <summary>
/// Classe d�finissant la phase de d�marrage du jeu
/// </summary>
public class BootStrapper
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void OnRuntimeMethodLoad()
	{
		// Cr�ation des managers qui restent toujours pr�sents sans avoir � �tre plac�s dans les sc�nes.
		// Gestion des sons/musiques
		SoundManager.Instantiate();

		// Gestion des �v�nements
		EventManager.Instantiate();

		// Gestion du jeu (transitions entre sc�nes)
		GameManager.Instantiate();

		// On d�marre le jeu par le menu
		EventManager.Instance.Raise(Events.OnSceneRequested, new OnSceneRequestedEventArg() { Scene = SceneNames.Menu });
	}
}
