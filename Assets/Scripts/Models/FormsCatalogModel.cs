using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Forms Catalog", fileName = "New Forms Catalog")]
public class FormsCatalogModel : ScriptableObject
{
	[HideInInspector, NonSerialized] public FormModel SelectedForm;
	[HideInInspector, NonSerialized] public CategoryModel SelectedCategory;
	[SerializeField] protected CategoryModel[] Categories;
	[SerializeField] protected bool Prototype;

	public bool IsPrototype()
	{
		return (Prototype);
	}

	public int GetCategoriesCount()
	{
		return (Categories.Length);
	}

	public CategoryModel GetCategoryModel(int position)
	{
		return (Categories[position]);
	}

	public int GetCategoryPosition(CategoryModel category)
	{
		return (Array.IndexOf(Categories, category));
	}
}

