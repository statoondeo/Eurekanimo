using UnityEngine;

[CreateAssetMenu(menuName = "New Backgrounds Catalog", fileName = "New Backgrounds Catalog")]
public class ScriptableBackgroundsCatalog : ScriptableObject
{
	public ScriptableEvent OnBackgroundSelected;
	public ScriptableBackground[] ScriptableBackgrounds;
}