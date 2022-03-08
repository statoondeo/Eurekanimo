using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Token", fileName = "New Token")]
public class TokenModel : ScriptableObject
{
	[SerializeField] protected Sprite Sprite;

	public Sprite GetSprite()
	{
		return (Sprite);
	}
}