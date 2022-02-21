using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Token", fileName = "New Token")]
public class ScriptableToken : ScriptableObject
{
	public ScriptableColor Color;
	public ScriptableAnimal Animal;
	public Sprite Sprite;
}
