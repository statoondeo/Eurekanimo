using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Form", fileName = "New Form")]
public class FormModel : ScriptableObject
{
	public static readonly int FORM_SIZE = 9;

	[SerializeField] protected string Name;
	[SerializeField] protected string[] Instructions;
	[SerializeField] protected TokenModel[] Solution;
	[NonSerialized] protected TokenModel[] CurrentConfiguration;

	//private void Awake()
	//{
	//	Debug.Assert(!string.IsNullOrWhiteSpace(Name), "Name manquant.");
	//	Debug.Assert(null != Instructions && 0 != Instructions.Length, Name + "/Instructions manquant.");
	//	Debug.Assert(null != Solution && 0 != Solution.Length, Name + "/Solution manquant.");
	//	Debug.Assert(FORM_SIZE != Solution.Length, Name + "/Solution mal dimensionné.");
	//	for (int i = 0, nbItems = Solution.Length; i < nbItems - 1; i++)
	//	{
	//		Debug.Assert(i == Array.LastIndexOf(Solution, Solution[i]), Name + "/Doublon dans la solution.");
	//	}
	//}

	#region Solution

	public TokenModel[] GetSolutionTokens()
	{
		return (Solution);
	}

	#endregion

	#region Name

	public string GetName()
	{
		return (Name);
	}

	#endregion

	#region Instructions

	public int GetInstructionsCount()
	{
		return (Instructions.Length);
	}

	public string GetInstruction(int position)
	{
		return (Instructions[position]);
	}

	#endregion

	#region Configuration saisie

	public void Reset()
	{
		CurrentConfiguration = new TokenModel[Solution.Length];
	}

	public int GetTokenPosition(TokenModel token)
{
		return (Array.IndexOf(CurrentConfiguration, token));
	}

	public TokenModel GetToken(int position)
	{
		return (CurrentConfiguration[position]);
	}

	public void SetToken(int position, TokenModel token)
	{
		CurrentConfiguration[position] = token;
	}

	public bool CheckToken(int position)
	{
		return (CurrentConfiguration[position] == Solution[position]);
	}

	public bool CheckForm()
	{
		for (int i = 0, nbItems = CurrentConfiguration.Length; i < nbItems; i++)
{
			if (!CheckToken(i)) return (false);
		}
		return (true);
	}

	#endregion
}

