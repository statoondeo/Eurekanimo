using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EventManager;

/// <summary>
/// Controller permettant de gérer la scène de menu
/// </summary>
public class MenuPresenter : MonoBehaviour
{
	// Models
	[SerializeField] protected FormsCatalogModel FormsCatalogModel;
	[SerializeField] protected SoundManager.Clips Music;

	// Views
	[SerializeField] protected TMP_Dropdown CategoriesDropdown;
	[SerializeField] protected TMP_Dropdown FormsDropdown;

	/// <summary>
	/// Contrôle des données manipulées
	/// </summary>
	private void Awake()
	{
		Debug.Assert(null != FormsCatalogModel, gameObject.name + "/ScriptableFormsCatalog not set!");
		Debug.Assert(null != CategoriesDropdown, gameObject.name + "/CategoriesDropdown not set!");
		Debug.Assert(null != FormsDropdown, gameObject.name + "/FormsDropdown not set!");
	}

	private void Start()
	{
		if (!SoundManager.Instance.IsPlaying(Music))
		{
			SoundManager.Instance.Play(Music);
		}

		FillCategories();
	}

	protected void FillCategories()
	{
		FormsCatalogModel.SelectedCategory = null;
		CategoriesDropdown.ClearOptions();
		int nbItems = FormsCatalogModel.GetCategoriesCount();
		if (nbItems > 0)
		{
			for (int i = 0; i < nbItems; i++)
			{
				CategoryModel category = FormsCatalogModel.GetCategoryModel(i);
				CategoriesDropdown.options.Add(new TMP_Dropdown.OptionData() { text = category.GetName(), image = category.GetSprite() });
			}
			CategoriesDropdown.RefreshShownValue();
			OnChangeCategory(0);
		}
		else
		{
			CategoriesDropdown.interactable = false;
			EventManager.Instance.Raise(Events.OnFormNotSelected);
		}
	}

	protected void FillForms()
	{
		FormsCatalogModel.SelectedForm = null;
		FormsDropdown.ClearOptions();
		int nbItems = FormsCatalogModel.SelectedCategory.GetFormsCount();
		if (nbItems > 0)
		{
			for (int i = 0; i < nbItems; i++)
			{
				FormsDropdown.options.Add(new TMP_Dropdown.OptionData() { text = FormsCatalogModel.SelectedCategory.GetForm(i).GetName() });
			}
			FormsDropdown.RefreshShownValue();
			FormsDropdown.interactable = true;
			OnChangeForm(0);
		}
		else
		{
			FormsDropdown.interactable = false;
			EventManager.Instance.Raise(Events.OnFormNotSelected);
		}
	}

	public void OnChangeCategory(int index)
	{
		CategoryModel category = FormsCatalogModel.GetCategoryModel(index);
		if (category != FormsCatalogModel.SelectedCategory)
		{
			FormsCatalogModel.SelectedCategory = category;
			CategoriesDropdown.value = index;
			FillForms();
		}
	}

	public void OnChangeForm(int index)
	{
		FormModel form = FormsCatalogModel.SelectedCategory.GetForm(index);
		if (form != FormsCatalogModel.SelectedForm)
		{
			FormsCatalogModel.SelectedForm = form;
			FormsDropdown.value = index;
			EventManager.Instance.Raise(Events.OnFormSelected);
		}
	}

	public void OnGameplayClick()
	{
		SoundManager.Instance.Play(SoundManager.Clips.ClickSound);
		EventManager.Instance.Raise(Events.OnSceneTransitionRequested, new OnSceneTransitionRequestedEventArg() { Scene = SceneNames.Gameplay });
	}
}
