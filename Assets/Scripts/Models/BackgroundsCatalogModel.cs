using UnityEngine;

[CreateAssetMenu(menuName = "New Backgrounds Catalog", fileName = "New Backgrounds Catalog")]
public class BackgroundsCatalogModel : ScriptableObject
{
	public EventModel OnBackgroundSelected;
	public BackgroundModel[] ScriptableBackgrounds;
}