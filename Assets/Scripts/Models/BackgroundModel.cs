using UnityEngine;

[CreateAssetMenu(menuName = "New Background", fileName = "New Background")]
public class BackgroundModel : ScriptableObject
{
	[SerializeField] protected string Name;
	[SerializeField] protected Sprite[] Layers;
	[SerializeField] protected float[] Speeds;

	protected int layersCount = -1;

	public string GetName()
	{
		return (Name);
	}

	public Sprite GetSprite(int layerIndex)
	{
		return (Layers[layerIndex]);
	}

	public float GetSpeed(int layerIndex)
	{
		return (Speeds[layerIndex]);
	}

	public int LayersCount
	{
		get
		{
			if (-1 == layersCount) layersCount = Layers.Length;
			return (layersCount);
		}
	}
}
