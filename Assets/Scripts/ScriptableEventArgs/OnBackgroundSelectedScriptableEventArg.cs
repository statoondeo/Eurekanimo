using UnityEngine;

public class OnBackgroundSelectedScriptableEventArg : ScriptableEventArg
{
	public int Index { get; protected set; }

	public OnBackgroundSelectedScriptableEventArg(int index) : base()
	{
		Index = index;
	}
}
