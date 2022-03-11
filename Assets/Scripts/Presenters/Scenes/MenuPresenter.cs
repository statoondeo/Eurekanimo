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
	[SerializeField] protected TextMeshProUGUI PrototypeWarning;
	[SerializeField] protected Slider MusicSlider;
	[SerializeField] protected Slider SoundSlider;
	[SerializeField] protected TMP_Dropdown CategoriesDropdown;
	[SerializeField] protected TMP_Dropdown FormsDropdown;
	[SerializeField] protected RectTransform OptionsPanel;
	[SerializeField] protected float TransitionsDuration;

	protected Vector2 OptionsPanelInitialPosition;

	/// <summary>
	/// Contrôle des données manipulées
	/// </summary>
	private void Awake()
	{
		Debug.Assert(null != FormsCatalogModel, gameObject.name + "/ScriptableFormsCatalog not set!");
		Debug.Assert(null != PrototypeWarning, gameObject.name + "/PrototypeWarning not set!");
		Debug.Assert(null != MusicSlider, gameObject.name + "/MusicSlider not set!");
		Debug.Assert(null != SoundSlider, gameObject.name + "/SoundSlider not set!");
		Debug.Assert(null != CategoriesDropdown, gameObject.name + "/CategoriesDropdown not set!");
		Debug.Assert(null != FormsDropdown, gameObject.name + "/FormsDropdown not set!");
		Debug.Assert(null != OptionsPanel, gameObject.name + "/OptionsPanel not set!");
		Debug.Assert(0.0f != TransitionsDuration, gameObject.name + "/TransitionsDuration not set!");
	}

	private void Start()
	{
		OptionsPanelInitialPosition = OptionsPanel.anchoredPosition;
		MusicSlider.value = SoundManager.Instance.MusicVolume;
		SoundSlider.value = SoundManager.Instance.SoundVolume;
		PrototypeWarning.gameObject.SetActive(FormsCatalogModel.IsPrototype());

		if (!SoundManager.Instance.IsPlaying(Music))
		{
			SoundManager.Instance.Play(Music);
		}

		FillCategories();
	}

	protected void FillCategories()
	{
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
			int index = FormsCatalogModel.GetCategoryPosition(FormsCatalogModel.SelectedCategory);
			OnChangeCategory(-1 == index ? 0 : index);
		}
		else
		{
			CategoriesDropdown.interactable = false;
			EventManager.Instance.Raise(Events.OnFormNotSelected);
		}
	}

	protected void FillForms()
	{
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
			int index = FormsCatalogModel.SelectedCategory.GetFormPosition(FormsCatalogModel.SelectedForm);
			OnChangeForm(-1 == index ? 0 : index);
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
		FormsCatalogModel.SelectedCategory = category;
		CategoriesDropdown.value = index;
		FillForms();
	}

	public void OnChangeForm(int index)
	{
		FormModel form = FormsCatalogModel.SelectedCategory.GetForm(index);
		FormsCatalogModel.SelectedForm = form;
		FormsDropdown.value = index;
		EventManager.Instance.Raise(Events.OnFormSelected);
	}

	public void OnGameplayClick()
	{
		EventManager.Instance.Raise(Events.OnSceneTransitionRequested, new OnSceneTransitionRequestedEventArg() { Scene = SceneNames.Gameplay });
	}

	public void OnExitClick()
	{
		EventManager.Instance.Raise(Events.OnExitRequested);
	}

	public void OnOptionsClick()
	{
		ToggleOptionsPanel();
	}

	public void OnMusicVolumeChanged(float volume)
	{
		SoundManager.Instance.MusicVolume = volume;
	}

	public void OnSoundVolumeChanged(float volume)
	{
		SoundManager.Instance.SoundVolume = volume;
	}

	protected void ToggleOptionsPanel()
	{
		if (OptionsPanel.anchoredPosition == Vector2.zero)
		{
			OptionsPanel.MoveTo(OptionsPanelInitialPosition, TransitionsDuration, Tweening.QuintIn);
		}
		else
		{
			OptionsPanel.MoveTo(Vector2.zero, TransitionsDuration, Tweening.QuintOut);
		}
	}
}
