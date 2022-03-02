using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Composant permettant d'écouter/observer un évenement en particulier.
/// Lorsque ce dernier est émis, alors chaque callback enregistrée est sollicitée, si le listener est actif
/// </summary>
public class ScriptableEventListener : MonoBehaviour
{
    public ScriptableEventListener() { }

    /// <summary>
    /// Constructeur pour la gestion sans éditeur
    /// </summary>
    /// <param name="gameEvent"></param>
    /// <param name="response"></param>
    public ScriptableEventListener(ScriptableEvent gameEvent, UnityEvent<ScriptableEventArg> response)
    {
        GameEvent = gameEvent;
        Response = response;
    }
    
    // Evènement observé
    [SerializeField] protected ScriptableEvent GameEvent;

    // Callback (sous forme de UnityEvent pour les enregistrer dans l'éditeur
    [SerializeField] protected UnityEvent<ScriptableEventArg> Response;

    // Faut-il répondre à l'évenement
    protected bool IsActive;

	#region Message Unity

    // Activation automatique
	protected void OnEnable()
    {
        Resume();
        GameEvent.RegisterListener(this);
    }

    // Désactivation automatique
    protected void OnDisable()
    {
        Pause();
        GameEvent.UnregisterListener(this);
    }

	#endregion

    // On arrête temporairement d'écouter l'évènement
	public void Pause() => IsActive = false;
	
    // On reprend l'écoute
    public void Resume() => IsActive = true;

    // L'évènement est survenu, on déclenche les callbacks
    public void OnEventRaised(ScriptableEventArg scriptableEventArg)
    {
        if (IsActive) Response.Invoke(scriptableEventArg);
    }
}