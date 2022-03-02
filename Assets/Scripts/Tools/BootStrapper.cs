using UnityEngine;

public class BootStrapper
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void OnRuntimeMethodLoad()
	{
		(new GameObject("GameManager")).AddComponent<GameManager>();
	}
}
