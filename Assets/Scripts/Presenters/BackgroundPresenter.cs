using System.Collections;
using TMPro;
using UnityEngine;

public class BackgroundPresenter : MonoBehaviour
{
	[SerializeField] protected BackgroundsCatalogModel BackgroundData;
	[SerializeField] protected TMP_Dropdown Dropdown;
	[SerializeField] protected SpriteRenderer[] Layers;

	protected float[] TextureUnitSize;

	protected void Start()
	{
		if (null != Dropdown)
		{
			for (int i = 0; i < BackgroundData.BackgroundsCount; i++)
			{
				Dropdown.options.Add(new TMP_Dropdown.OptionData() { text = BackgroundData.GetName(i) });
			}
			Dropdown.RefreshShownValue();
			Dropdown.value = BackgroundData.CurrentIndex;
		}

		// On applique le background trouvé
		LoadBackgrounds();

		// On déplace les backgrounds
		for (int i = 0; i < BackgroundData.LayersCount; i++)
		{
			StartCoroutine(MoveBackgroundRoutine(i));
		}
	}

	protected IEnumerator MoveBackgroundRoutine(int layerIndex)
	{
		Transform transform = Layers[layerIndex].transform;
		float speed = BackgroundData.GetCurrentSpeed(layerIndex);
		float textureUnitSize = TextureUnitSize[layerIndex];
		while (true)
		{
			transform.position += Vector3.left * speed * Time.deltaTime;
			if (Mathf.Abs(transform.position.x) >= textureUnitSize)
			{
				float offsetX = transform.position.x % textureUnitSize;
				transform.position = new Vector3(offsetX, transform.position.y);
			}
			yield return (null);
		}
	}

	protected void LoadBackgrounds()
	{
		TextureUnitSize = new float[BackgroundData.LayersCount];
		for (int i = 0; i < BackgroundData.LayersCount; i++)
		{
			Layers[i].sprite = BackgroundData.GetCurrentSprite(i);
			Sprite sprite = Layers[i].sprite;
			Texture2D texture = sprite.texture;
			TextureUnitSize[i] = texture.width / sprite.pixelsPerUnit;
		}
	}

	public void OnChangeBackground(int newBackground)
	{
		// On applique le background demandé
		if (newBackground != BackgroundData.CurrentIndex)
		{
			BackgroundData.CurrentIndex = newBackground;
			LoadBackgrounds();
			Dropdown.value = BackgroundData.CurrentIndex;
		}
	}
}
