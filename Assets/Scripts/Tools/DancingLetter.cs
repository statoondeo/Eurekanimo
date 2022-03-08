using System.Collections;
using UnityEngine;

public class DancingLetter : MonoBehaviour
{
	[SerializeField] protected Vector2 Offset;
	[SerializeField] protected float MoveDuration;
	[SerializeField] protected float StartDelay;

	protected RectTransform RectTransform;
	protected Vector2 InitialPosition;

	private void Awake()
	{
		RectTransform = GetComponent<RectTransform>();

		Debug.Assert(null != RectTransform, gameObject.name + "/OfRectTransformfset not set!");
		Debug.Assert(Vector2.zero != Offset, gameObject.name + "/Offset not set!");
		Debug.Assert(0.0f != MoveDuration, gameObject.name + "/MoveDuration not set!");
		Debug.Assert(0.0f != StartDelay, gameObject.name + "/StartDelay not set!");
	}

	private IEnumerator Start()
	{
		InitialPosition = RectTransform.position;
		yield return (new WaitForSeconds(StartDelay));
		yield return (DanceUp());
	}

	protected IEnumerator DanceUp()
	{
		yield return (StartCoroutine(Tweening.MoveToRoutine(RectTransform.transform, InitialPosition * Offset, MoveDuration, Tweening.SinInOut)));
		yield return (DanceDown());
	}

	protected IEnumerator DanceDown()
	{
		yield return (StartCoroutine(Tweening.MoveToRoutine(RectTransform.transform, InitialPosition / Offset, MoveDuration, Tweening.SinInOut)));
		yield return (DanceUp());
	}
}
