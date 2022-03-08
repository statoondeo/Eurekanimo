using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe permettant de créer un évènement à observer et de lui donner
/// de la substance en le matérialisant sous forme d'asset.
/// </summary>
[CreateAssetMenu(menuName = "New Event", fileName = "New Event")]
public class EventModel : ScriptableObject
{
    // Liste des observers de l'évènement
    // (Hashset pour éviter les doublons)
    protected readonly HashSet<EventListener> listeners = new HashSet<EventListener>();

    // Emission de l'évènement
    public void Raise(ModelEventArg scriptableEventArg)
    {
        // On notifie tous les observes
        foreach (EventListener listener in listeners) listener.OnEventRaised(scriptableEventArg);
    }

    // On ajoute un listener
    public void RegisterListener(EventListener listener) => listeners.Add(listener);

    // On supprime un listener
    public void UnregisterListener(EventListener listener) => listeners.Remove(listener);
}
