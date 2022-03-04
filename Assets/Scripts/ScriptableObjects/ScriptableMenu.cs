using UnityEngine;

[CreateAssetMenu(menuName = "New Menu", fileName = "New Menu")]
public class ScriptableMenu : ScriptableObject
{
	public ScriptableFormsCatalog ScriptableFormsCatalog;

	public ScriptableEvent OnSceneRequested;
	public ScriptableEvent OnFormSelected;
	public ScriptableEvent OnFormNotSelected;
	public SoundManager.Clips Music;
}
