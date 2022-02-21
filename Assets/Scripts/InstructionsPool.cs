using UnityEngine;
using UnityEngine.UI;

public class InstructionsPool : MonoBehaviour
{
    [SerializeField] protected ScriptableForm ScriptableForm;

    protected Text[] Instructions;

	private void Start()
	{
		Instructions =  new Text[transform.childCount];
		for (int i = 0, nbText = Instructions.Length; i < nbText; i++)
		{
			Instructions[i] = transform.GetChild(i).GetComponent<Text>();
			Instructions[i].text = ScriptableForm.Instructions[i];
		}
	}
}
