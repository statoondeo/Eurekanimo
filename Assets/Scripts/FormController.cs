using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class FormController : MonoBehaviour
{
	[SerializeField] protected ScriptableFormsCatalog ScriptableFormsCatalog;
	[SerializeField] protected ScriptableGrid EmptyGrid;

	[SerializeField] protected Dropdown Dropdown;
	[SerializeField] protected Sprite DropdownSprite;
	[SerializeField] protected Text InstructionsPool;
	[SerializeField] protected Transform SlotsPool;
	[SerializeField] protected Transform TokensPool;
	[SerializeField] protected Animator RightResultPanel;
	[SerializeField] protected Animator WrongResultPanel;

	[SerializeField] protected ScriptableEvent OnGridCompleted;
    [SerializeField] protected ScriptableEvent OnGridNotCompleted;

	protected ScriptableForm SelectedScriptableForm;
	protected Slot[] Slots;
	protected Token[] Tokens;

	private void Start()
	{
		InitGame();
		ResetGame(0);
	}

	protected void InitGame()
	{
		InitDropDown();
		InitSlots();
		InitTokens();
	}

	protected void ResetGame(int formIndex)
	{
		SelectedScriptableForm = ScriptableFormsCatalog.Forms[formIndex];
		ResetInstructions();
		ResetTable();
		OnGridNotCompleted.Raise();
	}

	protected void InitDropDown()
	{
		for (int i = 0, nbItem = ScriptableFormsCatalog.Forms.Length; i < nbItem; i++)
		{
			Dropdown.options.Add(new OptionData(ScriptableFormsCatalog.Forms[i].Name, DropdownSprite));
		}
		Dropdown.value = 0;
		Dropdown.RefreshShownValue();
	}

	protected void ResetInstructions()
	{
		InstructionsPool.text = string.Empty;
		for (int i = 0, nbItem = SelectedScriptableForm.Instructions.Length; i < nbItem; i++)
		{
			InstructionsPool.text += SelectedScriptableForm.Instructions[i] + System.Environment.NewLine + System.Environment.NewLine;
		}
	}

	protected void InitSlots()
	{
		Slots = SlotsPool.GetComponentsInChildren<Slot>();
	}

	protected void InitTokens()
	{
		Tokens = TokensPool.GetComponentsInChildren<Token>();
	}

	protected void ResetTable()
	{
		List<int> availableRoom = new List<int>();
		for (int i = 0, nbItem = Tokens.Length; i < nbItem; i++)
		{
			availableRoom.Add(i);
		}

		for (int i = 0, nbItem = Tokens.Length; i < nbItem; i++)
		{
			int roomIndex = Random.Range(0, availableRoom.Count);
			Tokens[i].ScriptableToken = SelectedScriptableForm.Solution.ScriptableTokens[availableRoom[roomIndex]];
			Tokens[i].ReturnToInitialPosition();
			Slots[i].ResetColor();
			Tokens[i].Container = null;
			Slots[i].ContainedToken = null;
			availableRoom.RemoveAt(roomIndex);
		}
	}

	public void OnGridChangedCallBack()
	{
		bool completed = true;
		ScriptableEvent completionEvent = OnGridCompleted;
		for (int i = 0, nbSlot = EmptyGrid.ScriptableTokens.Length; i < nbSlot; i++)
		{
			completed &= EmptyGrid.ScriptableTokens[i] != null;
			if (!completed)
			{
				completionEvent = OnGridNotCompleted;
				break;
			}
		}
		completionEvent.Raise();
	}

	protected bool CheckGridResult()
	{
		for (int i = 0, nbItem = Slots.Length; i < nbItem; i++)
		{
			if (EmptyGrid.ScriptableTokens[i] != SelectedScriptableForm.Solution.ScriptableTokens[i])
			{
				return (false);
			}
		}
		return (true);
	}

	protected IEnumerator ShowGridResults()
	{
		for (int i = 0, nbItem = Slots.Length; i < nbItem; i++)
		{
			if (EmptyGrid.ScriptableTokens[i] == SelectedScriptableForm.Solution.ScriptableTokens[i])
			{
				Slots[i].SetRightColor();
			}
			else
			{
				Slots[i].SetWrongColor();
			}
			yield return (new WaitForSeconds(0.25f));
		}
	}

	public void OnValidationClick()
	{
		(CheckGridResult() ? RightResultPanel : WrongResultPanel).SetTrigger("ShowPanel");
		OnGridNotCompleted.Raise();
	}

	public void OnRightResultContinueClick()
	{
		RightResultPanel.SetTrigger("HidePanel");
	}

	public void OnWrongResultContinueClick()
	{
		WrongResultPanel.SetTrigger("HidePanel");
	}

	public void OnWrongResultShowErrorsClick()
	{
		WrongResultPanel.SetTrigger("HidePanel");
		StartCoroutine(ShowGridResults());
	}

	public void OnApplicationQuitCallback()
	{
		Application.Quit();
	}

	public void OnDropdownValueChangedCallback()
	{
		ResetGame(Dropdown.value);
	}
}
