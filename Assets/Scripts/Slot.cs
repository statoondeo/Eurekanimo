using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{

	[HideInInspector] public Token ContainedToken
	{
		get => mContainedToken;
		set
		{
			mContainedToken = value;
			ScriptableGrid.ScriptableTokens[GridIndex] = mContainedToken?.ScriptableToken;
			ResetColor();
			OnGridChanged.Raise();
		}
	}
	[SerializeField] protected ScriptableGrid ScriptableGrid;
	[SerializeField] protected int GridIndex;
	[SerializeField] protected float ColorChangeDuration;
	[SerializeField] protected ScriptableEvent OnGridChanged;
	[SerializeField] protected Color RightColor;
	[SerializeField] protected Color WrongColor;

	protected Vector2 ContainerPosition;
	protected Image Image;
	protected Token mContainedToken;
	protected Coroutine ColorChangeCoroutine;

	private void Start()
	{
		ContainerPosition = GetComponent<RectTransform>().anchoredPosition;
		Image = GetComponentInChildren<Image>();
		ContainedToken = null;
	}

	public void OnDrop(PointerEventData eventData)
	{
		GameObject dropped = eventData.pointerDrag;
		if (null != dropped)
		{
			if (null != ContainedToken)
			{
				ContainedToken.ReturnToInitialPosition();
			}
			ContainedToken = dropped.GetComponent<Token>();
			ContainedToken.Container = this;
			ContainedToken.GotoPosition(ContainerPosition);
		}
	}

	protected void SetColor(Color color)
	{
		if (null != ColorChangeCoroutine) StopCoroutine(ColorChangeCoroutine);
		ColorChangeCoroutine = StartCoroutine(SetColorRoutine(color, ColorChangeDuration));
	}

	public void ResetColor()
	{
		SetColor(Color.white);
	}

	public void SetWrongColor()
	{
		SetColor(WrongColor);
	}

	public void SetRightColor()
	{
		SetColor(RightColor);
	}

	protected IEnumerator SetColorRoutine(Color targetColor, float duration)
	{
		float ttl = 0.0f;
		if (null != Image)
		{
			Color originalColor = Image.color;
			while (ttl <= duration)
			{
				ttl += Time.deltaTime;
				Image.color = Color.Lerp(originalColor, targetColor, Tweening.QuintOut(ttl / duration));
				yield return (null);
			}
			Image.color = targetColor;
		}
	}
}
