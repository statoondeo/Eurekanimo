using UnityEngine;
using static EventManager;

/// <summary>
/// Controller permettant de gérer le flow de la scène de jeu
/// </summary>
public class GameplayPresenter : MonoBehaviour
{
	public void OnMenuClick()
	{
		EventManager.Instance.Raise(Events.OnSceneTransitionRequested, new OnSceneTransitionRequestedEventArg() { Scene = SceneNames.Menu });
	}
}
