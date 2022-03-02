using TMPro;
using UnityEngine;

/// <summary>
/// Controller permettant de g�rer la sc�ne de menu
/// </summary>
public class MenuManager : MonoBehaviour
{
	[Tooltip("Param�trage du menu")] [SerializeField] protected ScriptableMenu ScriptableMenu;
	[Tooltip("Vue du menu")] [SerializeField] protected MenuView MenuView;

	/// <summary>
	/// Contr�le des donn�es manipul�es
	/// </summary>
	private void Awake()
	{
		Debug.Assert(null != ScriptableMenu, gameObject.name + "/ScriptableMenu not set!");
		Debug.Assert(null != MenuView, gameObject.name + "/MenuView not set!");
	}

	private void Start()
	{
		ScriptableMenu.ScriptableFormsCatalog.SelectedForm = null;
		SoundManager.Instance.PlayMainMusic(ScriptableMenu.Music);

		for (int i = 0, nbItems = ScriptableMenu.ScriptableFormsCatalog.Forms.Length; i < nbItems; i++)
		{
			MenuView.Dropdown.options.Add(new TMP_Dropdown.OptionData() { text = ScriptableMenu.ScriptableFormsCatalog.Forms[i].Name });
		}
		MenuView.Dropdown.RefreshShownValue();
		MenuView.Dropdown.value = 0;
		ScriptableMenu.ScriptableFormsCatalog.SelectedForm = ScriptableMenu.ScriptableFormsCatalog.Forms[0];
	}

	public void OnChangeForm(int form)
	{
		// On applique le background demand�
		if (ScriptableMenu.ScriptableFormsCatalog.Forms[form] != ScriptableMenu.ScriptableFormsCatalog.SelectedForm)
		{
			ScriptableMenu.ScriptableFormsCatalog.SelectedForm = ScriptableMenu.ScriptableFormsCatalog.Forms[form];
			MenuView.Dropdown.value = form;
		}
	}

	public void OnGameplayClick()
	{
		ScriptableMenu.OnGameplaySceneRequested.Raise(ScriptableEventArg.Empty);
	}
}
