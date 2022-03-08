using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Category", fileName = "New Category")]
public class CategoryModel : ScriptableObject
{
	[SerializeField] protected string Name;
	[SerializeField] protected Sprite Sprite;
	[SerializeField] protected FormModel[] Forms;

	public string GetName()
	{
		return (Name);
	}

	public Sprite GetSprite()
	{
		return (Sprite);
	}

	public int GetFormsCount()
	{
		return (Forms.Length);
	}

	public FormModel GetForm(int position)
	{
		return (Forms[position]);
	}
}

