using UnityEngine;

[CreateAssetMenu(menuName = "New Gameplay", fileName = "New Gameplay")]
public class ScriptableGameplay : ScriptableObject
{
	public ScriptableEvent OnMenuSceneRequested;
	public ScriptableEvent OnVictorySceneRequested;
	public ScriptableEvent OnDefeatSceneRequested;
}
