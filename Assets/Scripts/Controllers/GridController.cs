using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class GridController : MonoBehaviour
{
	[SerializeField] protected ScriptableFormsCatalog ScriptableFormsCatalog;

	[SerializeField] protected ScriptableEvent OnGridFilled;
	[SerializeField] protected ScriptableEvent OnGridNotFilled;
	[SerializeField] protected ScriptableEvent OnGridCorrectlyFilled;
	[SerializeField] protected ScriptableEvent OnGridWronglyFilled;
	[SerializeField] protected ScriptableEvent OnDropZoneEmptied;
	[SerializeField] protected ScriptableEvent OnDropZoneFilled;

	[SerializeField] protected Color RightTokenColor;
	[SerializeField] protected Color WrongTokenColor;
	[SerializeField] protected GameObject TokensContainer;
	[SerializeField] protected GameObject DropZonesContainer;
	[SerializeField] protected GameObject InstructionsContainer;
	[SerializeField] protected TextMeshProUGUI TitleText;
	[SerializeField] protected float TransitionsDuration;

	protected DragNDroppable[] Tokens;
	protected DropZone[] DropZones;
	protected TextMeshProUGUI[] Instructions;

	private void Awake()
	{
		Tokens = new DragNDroppable[TokensContainer.transform.childCount];
		DropZones = DropZonesContainer.GetComponentsInChildren<DropZone>();
		Instructions = InstructionsContainer.GetComponentsInChildren<TextMeshProUGUI>();
	}

	private void Start()
	{
		// On mélange la solution
		ScriptableToken[] shuffledSolution = ScriptableFormsCatalog.SelectedForm.Solution.OrderBy(item => Random.value).ToArray();
		DragNDroppable[] tokens = TokensContainer.transform.GetComponentsInChildren<DragNDroppable>();

		// On attribue un token à chaque dragndroppable
		int nbInstructions = ScriptableFormsCatalog.SelectedForm.Instructions.Length;
		for (int i = 0, nbItems = shuffledSolution.Length; i < nbItems; i++)
		{
			tokens[i].ScriptableToken = shuffledSolution[i];
			Instructions[i].text = i < nbInstructions ? ScriptableFormsCatalog.SelectedForm.Instructions[i] : string.Empty;
		}
		TitleText.text = ScriptableFormsCatalog.SelectedForm.Name;
	}

	public void CorrectGrid()
	{
		StartCoroutine(CorrectGridRoutine());
	}

	protected IEnumerator CorrectGridRoutine()
	{
		yield return (new WaitForSeconds(TransitionsDuration));
		for (int i = 0, nbItems = Tokens.Length; i < nbItems; i++)
		{
			if (Tokens[i].ScriptableToken == ScriptableFormsCatalog.SelectedForm.Solution[i])
			{
				SoundManager.Instance.Play(SoundManager.Clips.CorrectSound);
				DropZones[i].ColorTo(RightTokenColor);
				Tokens[i].Explode();
			}
			else
			{
				SoundManager.Instance.Play(SoundManager.Clips.WrongSound);
				DropZones[i].ColorTo(WrongTokenColor);
			}
			yield return (new WaitForSeconds(TransitionsDuration));
		}
	}

	public void CheckGrid()
	{
		SoundManager.Instance.Play(SoundManager.Clips.ClickSound);
		for (int i = 0, nbItems = Tokens.Length; i < nbItems; i++)
		{
			if (Tokens[i].ScriptableToken != ScriptableFormsCatalog.SelectedForm.Solution[i])
			{
				OnGridNotFilled.Raise(ScriptableEventArg.Empty);
				OnGridWronglyFilled.Raise(ScriptableEventArg.Empty);
				return;
			}
		}
		OnGridNotFilled.Raise(ScriptableEventArg.Empty);
		OnGridCorrectlyFilled.Raise(ScriptableEventArg.Empty);

	}

	public void OnTokenDraggedCallback(ScriptableEventArg eventArg)
	{
		for (int i = 0, nbItems = Tokens.Length; i < nbItems; i++)
		{
			DropZones[i].ColorTo(Color.white);
		}

		OnTokenDraggedScriptableEventArg onTokenDraggedEventArg = eventArg as OnTokenDraggedScriptableEventArg;

		int tokenIndex = System.Array.IndexOf(Tokens, onTokenDraggedEventArg.Token);
		if (-1 != tokenIndex)
		{
			Tokens[tokenIndex] = null;
			OnDropZoneEmptied.Raise(new OnDropZoneEmptiedScriptableEventArg() { DropZone = DropZones[tokenIndex] });
			OnGridNotFilled.Raise(ScriptableEventArg.Empty);
		}
	}

	public void OnTokenDroppedCallback(ScriptableEventArg eventArg)
	{
		OnTokenDroppedScriptableEventArg onTokenDroppedEventArg = eventArg as OnTokenDroppedScriptableEventArg;
		
		if (null == onTokenDroppedEventArg.DropZone)
		{
			onTokenDroppedEventArg.Token.MoveTo(onTokenDroppedEventArg.Token.InitialPosition);
			OnGridNotFilled.Raise(ScriptableEventArg.Empty);
		}
		else
		{
			int containerIndex = System.Array.IndexOf(DropZones, onTokenDroppedEventArg.DropZone);
			if (null != Tokens[containerIndex])
			{
				Tokens[containerIndex].MoveTo(Tokens[containerIndex].InitialPosition);
			}
			Tokens[containerIndex] = onTokenDroppedEventArg.Token;
			onTokenDroppedEventArg.Token.MoveTo(onTokenDroppedEventArg.DropZone.transform.position);
			OnDropZoneFilled.Raise(new OnDropZoneFilledScriptableEventArg() { DropZone = DropZones[containerIndex] });

			(System.Array.IndexOf(Tokens, null) == -1 ? OnGridFilled : OnGridNotFilled).Raise(ScriptableEventArg.Empty);
		}
	}
}
