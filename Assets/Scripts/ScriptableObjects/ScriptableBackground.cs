using UnityEngine;

[CreateAssetMenu(menuName = "New Background", fileName = "New Background")]
public class ScriptableBackground : ScriptableObject
{
	public string Name;
	public Sprite[] Layers;
	public float[] Speeds;
}
