using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Forms Catalog", fileName = "New Forms Catalog")]
public class ScriptableFormsCatalog : ScriptableObject
{
	[HideInInspector, NonSerialized] public ScriptableForm SelectedForm;
	public ScriptableForm[] Forms;
}

