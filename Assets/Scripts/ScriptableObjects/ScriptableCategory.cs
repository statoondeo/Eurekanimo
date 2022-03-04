using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Category", fileName = "New Category")]
public class ScriptableCategory : ScriptableObject
{
	public string Name;
	public Sprite Sprite;
	public ScriptableForm[] Forms;
}

