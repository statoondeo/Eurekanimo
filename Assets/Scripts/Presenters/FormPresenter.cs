using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using static EventManager;

public class FormPresenter : MonoBehaviour
{
	// Catalogue de formulaires, avec le formulaire sélectionné
	[SerializeField] protected FormsCatalogModel ScriptableFormsCatalog;

	// Données
	[SerializeField] protected GameObject TokensContainer;
	[SerializeField] protected GameObject DropZonesContainer;
	[SerializeField] protected RectTransform InstructionsContainer;
	[SerializeField] protected GameObject InstructionPrefab;
	[SerializeField] protected TextMeshProUGUI TitleText;

	// Comportements visuels
	[SerializeField] protected Color RightTokenColor;
	[SerializeField] protected Color WrongTokenColor;
	[SerializeField] protected float TransitionsDuration;

	protected FormModel CurrentForm;
	protected TokenPresenter[] Tokens;
	protected ContainerPresenter[] Containers;

	private void Awake()
	{
		Tokens = new TokenPresenter[TokensContainer.transform.childCount];
		Containers = DropZonesContainer.GetComponentsInChildren<ContainerPresenter>();

		EventManager.Instance.CreateEventListener(gameObject, Events.OnTokenDragged, OnTokenDraggedCallback);
		EventManager.Instance.CreateEventListener(gameObject, Events.OnTokenDropped, OnTokenDroppedCallback);
	}

	private void Start()
	{
		CurrentForm = ScriptableFormsCatalog.SelectedForm;
		CurrentForm.Reset();

		// On mélange la solution
		TokenModel[] shuffledSolution = CurrentForm.GetSolutionTokens().OrderBy(item => Random.value).ToArray();
		TokenPresenter[] tokens = TokensContainer.transform.GetComponentsInChildren<TokenPresenter>();

		// On attribue un token à chaque dragndroppable
		for (int i = 0, nbItems = FormModel.FORM_SIZE; i < nbItems; i++)
		{
			tokens[i].ScriptableToken = shuffledSolution[i];
		}

		// On affiche les instructions
		for (int i = 0, nbItems = CurrentForm.GetInstructionsCount(); i < nbItems; i++)
		{
			GameObject newGO = Instantiate(InstructionPrefab, InstructionsContainer.transform);
			TextMeshProUGUI instruction = newGO.GetComponent<TextMeshProUGUI>();
			instruction.text = (i + 1).ToString() + ". " + CurrentForm.GetInstruction(i);
		}

		// Titre du formulaire
		TitleText.text = CurrentForm.GetName();

		EventManager.Instance.Raise(Events.OnGridNotFilled);
		EventManager.Instance.Raise(Events.OnGameReady);
	}

	public void CorrectGrid()
	{
		StartCoroutine(CorrectGridRoutine());
	}

	protected IEnumerator CorrectGridRoutine()
	{
		for (int i = 0, nbItems = FormModel.FORM_SIZE; i < nbItems; i++)
		{
			yield return (new WaitForSeconds(TransitionsDuration));
			if (CurrentForm.CheckToken(i))
			{
				SoundManager.Instance.Play(SoundManager.Clips.CorrectSound);
				Containers[i].ColorTo(RightTokenColor);
				Tokens[i].Explode();
			}
			else
			{
				SoundManager.Instance.Play(SoundManager.Clips.WrongSound);
				Containers[i].ColorTo(WrongTokenColor);
			}
		}
	}

	public void CheckGrid()
	{
		SoundManager.Instance.Play(SoundManager.Clips.ClickSound);
		EventManager.Instance.Raise(CurrentForm.CheckForm() ? Events.OnGridCorrectlyFilled : Events.OnGridWronglyFilled);
		EventManager.Instance.Raise(Events.OnGridNotFilled);
	}

	public void OnEmptyForm()
	{
		StartCoroutine(EmptyFormRoutine());
	}

	protected void EmptyContainer(int index)
	{
		if (null != Tokens[index])
		{
			Containers[index].ColorTo(Color.white);
			Tokens[index].MoveTo(Tokens[index].InitialPosition);
			SoundManager.Instance.Play(SoundManager.Clips.UnDropSound);
			CurrentForm.SetToken(index, null);
			Tokens[index] = null;
			EventManager.Instance.Raise(Events.OnContainerEmptied, new OnDropZoneEmptiedScriptableEventArg() { DropZone = Containers[index] });
			EventManager.Instance.Raise(Events.OnGridNotFilled);
		}
	}

	protected void FillContainer(int index, TokenPresenter token)
	{
		SoundManager.Instance.Play(SoundManager.Clips.DropSound);
		CurrentForm.SetToken(index, token.ScriptableToken);
		Tokens[index] = token;
		token.MoveTo(Containers[index].transform.position);
		EventManager.Instance.Raise(Events.OnContainerFilled, new OnDropZoneFilledScriptableEventArg() { DropZone = Containers[index] });
		EventManager.Instance.Raise((CurrentForm.GetTokenPosition(null) == -1 ? Events.OnGridFilled : Events.OnGridNotFilled));
	}

	protected IEnumerator EmptyFormRoutine()
	{
		for (int i = 0, nbItems = Tokens.Length; i < nbItems; i++)
		{
			if (null != Tokens[i])
			{
				EmptyContainer(i);
				yield return (new WaitForSeconds(TransitionsDuration));
			}
		}
	}

	protected void OnTokenDraggedCallback(ModelEventArg eventArg)
	{
		for (int i = 0, nbItems = Tokens.Length; i < nbItems; i++)
		{
			Containers[i].ColorTo(Color.white);
		}

		OnTokenDraggedScriptableEventArg onTokenDraggedEventArg = eventArg as OnTokenDraggedScriptableEventArg;

		int tokenIndex = System.Array.IndexOf(Tokens, onTokenDraggedEventArg.Token);
		if (-1 == tokenIndex)
		{
			SoundManager.Instance.Play(SoundManager.Clips.DragSound);
		}
		else
		{
			SoundManager.Instance.Play(SoundManager.Clips.UnDropSound);
			CurrentForm.SetToken(tokenIndex, null);
			Tokens[tokenIndex] = null;
			EventManager.Instance.Raise(Events.OnContainerEmptied, new OnDropZoneEmptiedScriptableEventArg() { DropZone = Containers[tokenIndex] });
			EventManager.Instance.Raise(Events.OnGridNotFilled);
		}
	}

	protected void OnTokenDroppedCallback(ModelEventArg eventArg)
	{
		OnTokenDroppedScriptableEventArg onTokenDroppedEventArg = eventArg as OnTokenDroppedScriptableEventArg;

		if (null == onTokenDroppedEventArg.DropZone)
		{
			SoundManager.Instance.Play(SoundManager.Clips.UnDragSound);
			onTokenDroppedEventArg.Token.MoveTo(onTokenDroppedEventArg.Token.InitialPosition);
		}
		else
		{
			int containerIndex = System.Array.IndexOf(Containers, onTokenDroppedEventArg.DropZone);
			EmptyContainer(containerIndex);
			FillContainer(containerIndex, onTokenDroppedEventArg.Token);
		}
	}
}
