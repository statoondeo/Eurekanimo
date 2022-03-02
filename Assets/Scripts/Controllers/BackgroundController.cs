using TMPro;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
	protected static readonly string PLAYERPREFS_SELECTED_BACKGROUND = "SelectedBackground";

	[SerializeField] protected ScriptableBackgroundsCatalog BackgroundData;
	[SerializeField] protected BackgroundView BackgroundView;

	public int CurrentBackground;
	protected int NbBackgrounds;
	protected int NbLayers;
	protected float[] TextureUnitSize;

	private void Start()
	{
		NbBackgrounds = BackgroundData.ScriptableBackgrounds.Length;
		NbLayers = BackgroundData.ScriptableBackgrounds[0].Layers.Length;

		CurrentBackground = PlayerPrefs.GetInt(PLAYERPREFS_SELECTED_BACKGROUND, 0);

		if (null != BackgroundView.Dropdown)
		{
			for (int i = 0; i < NbBackgrounds; i++)
			{
				BackgroundView.Dropdown.options.Add(new TMP_Dropdown.OptionData() { text = BackgroundData.ScriptableBackgrounds[i].Name });
			}
			BackgroundView.Dropdown.RefreshShownValue();
			BackgroundView.Dropdown.value = CurrentBackground;
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
			BackgroundView.Layers[i].transform.position += Vector3.left * BackgroundData.ScriptableBackgrounds[CurrentBackground].Speeds[i] * Time.deltaTime;
			if (Mathf.Abs(BackgroundView.Layers[i].transform.position.x) >= TextureUnitSize[i])
			{
				float offsetX = BackgroundView.Layers[i].transform.position.x % TextureUnitSize[i];
				BackgroundView.Layers[i].transform.position = new Vector3(offsetX, BackgroundView.Layers[i].transform.position.y);
			}
		}
	}

	protected void LoadBackgrounds()
	{
		TextureUnitSize = new float[NbLayers];
		for (int i = 0; i < NbLayers; i++)
		{
			BackgroundView.Layers[i].sprite = BackgroundData.ScriptableBackgrounds[CurrentBackground].Layers[i];
			Sprite sprite = BackgroundView.Layers[i].sprite;
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
			BackgroundView.Dropdown.value = CurrentBackground;
		}
	}
}
