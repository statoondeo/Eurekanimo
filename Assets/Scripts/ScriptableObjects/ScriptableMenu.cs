using UnityEngine;

[CreateAssetMenu(menuName = "New Menu", fileName = "New Menu")]
public class ScriptableMenu : ScriptableObject
{
	public ScriptableFormsCatalog ScriptableFormsCatalog;

	public ScriptableEvent OnGameplaySceneRequested;
	public AudioClip Music;
}
