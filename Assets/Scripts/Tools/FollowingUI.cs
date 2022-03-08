using UnityEngine;

public class FollowingUI : MonoBehaviour
{
	[SerializeField] protected RectTransform UIElementToFollow;

	protected Camera Camera;

	private void Awake()
	{
		Debug.Assert(null != UIElementToFollow, gameObject.name + "/UIElementToFollow not set!");
	}

	private void Start()
	{
		Camera = Camera.main;
		UpdatePosition();
	}

	private void Update()
	{
		UpdatePosition();
	}

	protected void UpdatePosition()
	{
		transform.position = Camera.ScreenToWorldPoint(UIElementToFollow.transform.position);
	}
}
