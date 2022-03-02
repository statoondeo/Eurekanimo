using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe permettant de créer un évènement à observer et de lui donner
/// de la substance en le matérialisant sous forme d'asset.
/// </summary>
[CreateAssetMenu(menuName = "New Event", fileName = "New Event")]
public class ScriptableEvent : ScriptableObject
{
    // Liste des observers de l'évènement
    // (Hashset pour éviter les doublons)
    protected readonly HashSet<ScriptableEventListener> listeners = new HashSet<ScriptableEventListener>();

    // Emission de l'évènement
    public void Raise(ScriptableEventArg scriptableEventArg)
    {
        // On notifie tous les observes
        foreach (ScriptableEventListener listener in listeners) listener.OnEventRaised(scriptableEventArg);
    }

    // On ajoute un listener
    public void RegisterListener(ScriptableEventListener listener) => listeners.Add(listener);

    // On supprime un listener
    public void UnregisterListener(ScriptableEventListener listener) => listeners.Remove(listener);
}
