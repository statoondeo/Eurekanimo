using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Composant permettant d'�couter/observer un �venement en particulier.
/// Lorsque ce dernier est �mis, alors chaque callback enregistr�e est sollicit�e, si le listener est actif
/// </summary>
public class EventListener : MonoBehaviour
{

	// Ev�nement observ�
	[SerializeField] protected EventModel GameEvent;

	// Callback (sous forme de UnityEvent pour les enregistrer dans l'�diteur
	[SerializeField] protected UnityEvent<ModelEventArg> Response;

	// Faut-il r�pondre � l'�venement
	protected bool IsActive;

	#region Message Unity

	// Activation automatique
	protected void OnEnable()
	{
		Resume();
		GameEvent?.RegisterListener(this);
	}

	// D�sactivation automatique
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

	// On arr�te temporairement d'�couter l'�v�nement
	public void Pause() => IsActive = false;

	// On reprend l'�coute
	public void Resume() => IsActive = true;

	// L'�v�nement est survenu, on d�clenche les callbacks
	public void OnEventRaised(ModelEventArg scriptableEventArg)
	{
		if (IsActive) Response.Invoke(scriptableEventArg);
	}
}