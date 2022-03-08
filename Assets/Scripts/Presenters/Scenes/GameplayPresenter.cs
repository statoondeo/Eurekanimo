using UnityEngine;
using static EventManager;

/// <summary>
/// Controller permettant de g�rer le flow de la sc�ne de jeu
/// </summary>
public class GameplayPresenter : MonoBehaviour
{
	public void OnMenuClick()
	{
		EventManager.Instance.Raise(Events.OnSceneTransitionRequested, new OnSceneTransitionRequestedEventArg() { Scene = SceneNames.Menu });
	}
}
