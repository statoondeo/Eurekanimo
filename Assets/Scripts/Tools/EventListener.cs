using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Composant permettant d'écouter/observer un évenement en particulier.
/// Lorsque ce dernier est émis, alors chaque callback enregistrée est sollicitée, si le listener est actif
/// </summary>
public class EventListener : MonoBehaviour
{

	// Evènement observé
	[SerializeField] protected EventModel GameEvent;

	// Callback (sous forme de UnityEvent pour les enregistrer dans l'éditeur
	[SerializeField] protected UnityEvent<ModelEventArg> Response;

	// Faut-il répondre à l'évenement
	protected bool IsActive;

	#region Message Unity

	// Activation automatique
	protected void OnEnable()
	{
		Resume();
		GameEvent?.RegisterListener(this);
	}

	// Désactivation automatique
	protected void OnDisable()
	{
		Pause();
		GameEvent?.UnregisterListener(this);
	}

	#endregion

	public EventModel SetEventModel(EventModel eventModel)
	{
		GameEvent = eventModel;
		return (GameEvent);
	}
	public UnityEvent<ModelEventArg> SetEventCallback(UnityEvent<ModelEventArg> callback)
	{
		Response = callback;
		return (Response);
	}

	// On arrête temporairement d'écouter l'évènement
	public void Pause() => IsActive = false;

	// On reprend l'écoute
	public void Resume() => IsActive = true;

	// L'évènement est survenu, on déclenche les callbacks
	public void OnEventRaised(ModelEventArg scriptableEventArg)
	{
		if (IsActive) Response.Invoke(scriptableEventArg);
	}
}