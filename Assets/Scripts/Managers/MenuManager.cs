using TMPro;
using UnityEngine;

/// <summary>
/// Controller permettant de gérer la scène de menu
/// </summary>
public class MenuManager : MonoBehaviour
{
	[Tooltip("Paramétrage du menu")] [SerializeField] protected ScriptableMenu ScriptableMenu;
	[Tooltip("Vue du menu")] [SerializeField] protected MenuView MenuView;

	/// <summary>
	/// Contrôle des données manipulées
	/// </summary>
	private void Awake()
	{
		Debug.Assert(null != ScriptableMenu, gameObject.name + "/ScriptableMenu not set!");
		Debug.Assert(null != MenuView, gameObject.name + "/MenuView not set!");
	}

	private void Start()
	{
		if (!SoundManager.Instance.IsPlaying(ScriptableMenu.Music))
		{
			SoundManager.Instance.Play(ScriptableMenu.Music);
		}

		FillCategories();
	}

	protected void FillCategories()
	{
		ScriptableMenu.ScriptableFormsCatalog.SelectedCategory = null;
		MenuView.CategoriesDropdown.ClearOptions();
		int nbItems = ScriptableMenu.ScriptableFormsCatalog.Categories.Length;
		if (nbItems > 0)
		{
			for (int i = 0; i < nbItems; i++)
			{
				MenuView.CategoriesDropdown.options.Add(new TMP_Dropdown.OptionData() { text = ScriptableMenu.ScriptableFormsCatalog.Categories[i].Name, image = ScriptableMenu.ScriptableFormsCatalog.Categories[i].Sprite });
			}
			MenuView.CategoriesDropdown.RefreshShownValue();
			OnChangeCategory(0);
		}
		else
		{
			MenuView.CategoriesDropdown.interactable = false;
			ScriptableMenu.OnFormNotSelected.Raise(ScriptableEventArg.Empty);
		}
	}

	protected void FillForms()
	{
		ScriptableMenu.ScriptableFormsCatalog.SelectedForm = null;
		MenuView.FormsDropdown.ClearOptions();
		int nbItems = ScriptableMenu.ScriptableFormsCatalog.SelectedCategory.Forms.Length;
		if (nbItems > 0)
		{
			for (int i = 0; i < nbItems; i++)
			{
				MenuView.FormsDropdown.options.Add(new TMP_Dropdown.OptionData() { text = ScriptableMenu.ScriptableFormsCatalog.SelectedCategory.Forms[i].Name });
			}
			MenuView.FormsDropdown.RefreshShownValue();
			MenuView.FormsDropdown.interactable = true;
			OnChangeForm(0);
		}
		else
		{
			MenuView.FormsDropdown.interactable = false;
			ScriptableMenu.OnFormNotSelected.Raise(ScriptableEventArg.Empty);
		}
	}

	public void OnChangeCategory(int category)
	{
		if (ScriptableMenu.ScriptableFormsCatalog.Categories[category] != ScriptableMenu.ScriptableFormsCatalog.SelectedCategory)
		{
			ScriptableMenu.ScriptableFormsCatalog.SelectedCategory = ScriptableMenu.ScriptableFormsCatalog.Categories[category];
			MenuView.CategoriesDropdown.value = category;
			FillForms();
		}
	}

	public void OnChangeForm(int form)
	{
		if (ScriptableMenu.ScriptableFormsCatalog.SelectedCategory.Forms[form] != ScriptableMenu.ScriptableFormsCatalog.SelectedForm)
		{
			ScriptableMenu.ScriptableFormsCatalog.SelectedForm = ScriptableMenu.ScriptableFormsCatalog.SelectedCategory.Forms[form];
			MenuView.FormsDropdown.value = form;
			ScriptableMenu.OnFormSelected.Raise(ScriptableEventArg.Empty);
		}
	}

	public void OnGameplayClick()
	{
		SoundManager.Instance.Play(SoundManager.Clips.ClickSound);
		ScriptableMenu.OnSceneRequested.Raise(new OnSceneTransitionRequestedEventArg() { Scene = SceneNames.Gameplay });
	}
}
