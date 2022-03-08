using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Forms Catalog", fileName = "New Forms Catalog")]
public class FormsCatalogModel : ScriptableObject
{
	[HideInInspector, NonSerialized] public FormModel SelectedForm;
	[HideInInspector, NonSerialized] public CategoryModel SelectedCategory;
	[SerializeField] protected CategoryModel[] Categories;

	public int GetCategoriesCount()
	{
		return (Categories.Length);
	}

	public CategoryModel GetCategoryModel(int position)
	{
		return (Categories[position]);
	}
}

