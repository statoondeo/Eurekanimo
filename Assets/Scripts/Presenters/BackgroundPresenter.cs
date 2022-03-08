using TMPro;
using UnityEngine;

public class BackgroundPresenter : MonoBehaviour
{
	protected static readonly string PLAYERPREFS_SELECTED_BACKGROUND = "SelectedBackground";

	[SerializeField] protected BackgroundsCatalogModel BackgroundData;
	public TMP_Dropdown Dropdown;
	public SpriteRenderer[] Layers;

	protected int CurrentBackground;
	protected int NbBackgrounds;
	protected int NbLayers;
	protected float[] TextureUnitSize;

	private void Start()
	{
		NbBackgrounds = BackgroundData.ScriptableBackgrounds.Length;
		NbLayers = BackgroundData.ScriptableBackgrounds[0].Layers.Length;

		CurrentBackground = PlayerPrefs.GetInt(PLAYERPREFS_SELECTED_BACKGROUND, 0);

		if (null != Dropdown)
		{
			for (int i = 0; i < NbBackgrounds; i++)
			{
				Dropdown.options.Add(new TMP_Dropdown.OptionData() { text = BackgroundData.ScriptableBackgrounds[i].Name });
			}
			Dropdown.RefreshShownValue();
			Dropdown.value = CurrentBackground;
		}

		// On applique le background trouvé
		LoadBackgrounds();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
		}

		// On déplace les backgrounds
		for (int i = 0; i < NbLayers; i++)
		{
			Layers[i].transform.position += Vector3.left * BackgroundData.ScriptableBackgrounds[CurrentBackground].Speeds[i] * Time.deltaTime;
			if (Mathf.Abs(Layers[i].transform.position.x) >= TextureUnitSize[i])
			{
				float offsetX = Layers[i].transform.position.x % TextureUnitSize[i];
				Layers[i].transform.position = new Vector3(offsetX, Layers[i].transform.position.y);
			}
		}
	}

	protected void LoadBackgrounds()
	{
		TextureUnitSize = new float[NbLayers];
		for (int i = 0; i < NbLayers; i++)
		{
			Layers[i].sprite = BackgroundData.ScriptableBackgrounds[CurrentBackground].Layers[i];
			Sprite sprite = Layers[i].sprite;
			Texture2D texture = sprite.texture;
			TextureUnitSize[i] = texture.width / sprite.pixelsPerUnit;
		}
	}

	public void OnChangeBackground(int newBackground)
	{
		// On applique le background demandé
		if (newBackground != CurrentBackground)
		{
			CurrentBackground = newBackground;
			PlayerPrefs.SetInt(PLAYERPREFS_SELECTED_BACKGROUND, CurrentBackground);
			LoadBackgrounds();
			Dropdown.value = CurrentBackground;
		}
	}
}
