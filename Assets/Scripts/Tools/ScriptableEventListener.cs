using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Composant permettant d'�couter/observer un �venement en particulier.
/// Lorsque ce dernier est �mis, alors chaque callback enregistr�e est sollicit�e, si le listener est actif
/// </summary>
public class ScriptableEventListener : MonoBehaviour
{
    public ScriptableEventListener() { }

    /// <summary>
    /// Constructeur pour la gestion sans �diteur
    /// </summary>
    /// <param name="gameEvent"></param>
    /// <param name="response"></param>
    public ScriptableEventListener(ScriptableEvent gameEvent, UnityEvent<ScriptableEventArg> response)
    {
        GameEvent = gameEvent;
        Response = response;
    }
    
    // Ev�nement observ�
    [SerializeField] protected ScriptableEvent GameEvent;

    // Callback (sous forme de UnityEvent pour les enregistrer dans l'�diteur
    [SerializeField] protected UnityEvent<ScriptableEventArg> Response;

    // Faut-il r�pondre � l'�venement
    protected bool IsActive;

	#region Message Unity

    // Activation automatique
	protected void OnEnable()
    {
        Resume();
        GameEvent.RegisterListener(this);
    }

    // D�sactivation automatique
    protected void OnDisable()
    {
        Pause();
        GameEvent.UnregisterListener(this);
    }

	#endregion

    // On arr�te temporairement d'�couter l'�v�nement
	public void Pause() => IsActive = false;
	
    // On reprend l'�coute
    public void Resume() => IsActive = true;

    // L'�v�nement est survenu, on d�clenche les callbacks
    public void OnEventRaised(ScriptableEventArg scriptableEventArg)
    {
        if (IsActive) Response.Invoke(scriptableEventArg);
    }
}