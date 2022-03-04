using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Form", fileName = "New Form")]
public class ScriptableForm : ScriptableObject
{
	public string Name;
	public string[] Instructions;
	public ScriptableToken[] Solution;
}

