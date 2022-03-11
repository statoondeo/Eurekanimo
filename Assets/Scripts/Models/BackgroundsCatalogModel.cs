using UnityEngine;

[CreateAssetMenu(menuName = "New Backgrounds Catalog", fileName = "New Backgrounds Catalog")]
public class BackgroundsCatalogModel : ScriptableObject
{
	protected static readonly string PLAYERPREFS_SELECTED_BACKGROUND = "SelectedBackground";

	[SerializeField] protected BackgroundModel[] ScriptableBackgrounds;

	protected int currentBackground = -1;
	public int CurrentIndex
	{
		get
		{
			if (-1 == currentBackground) currentBackground = PlayerPrefs.GetInt(PLAYERPREFS_SELECTED_BACKGROUND, 0);
			return (currentBackground);
		}
		set
		{
			currentBackground = value;
			PlayerPrefs.SetInt(PLAYERPREFS_SELECTED_BACKGROUND, CurrentIndex);
		}
	}

	protected int backgroundsCount = -1;
	public int BackgroundsCount
	{
		get
		{
			if (-1 == backgroundsCount) backgroundsCount = ScriptableBackgrounds.Length;
			return (backgroundsCount);
		}
	}

	protected int layersCount = -1;
	public int LayersCount
	{
		get
		{
			if (-1 == layersCount) layersCount = ScriptableBackgrounds[currentBackground].LayersCount;
			return (layersCount);
		}
	}

	public string GetName(int backgroundIndex)
	{
		return (ScriptableBackgrounds[backgroundIndex].GetName());
	}

	public Sprite GetCurrentSprite(int layerIndex)
	{
		return (ScriptableBackgrounds[currentBackground].GetSprite(layerIndex));
	}

	public float GetCurrentSpeed(int layerIndex)
	{
		return (ScriptableBackgrounds[currentBackground].GetSpeed(layerIndex));
	}
}