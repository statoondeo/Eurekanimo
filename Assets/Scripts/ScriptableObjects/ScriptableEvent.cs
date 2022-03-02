using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe permettant de cr�er un �v�nement � observer et de lui donner
/// de la substance en le mat�rialisant sous forme d'asset.
/// </summary>
[CreateAssetMenu(menuName = "New Event", fileName = "New Event")]
public class ScriptableEvent : ScriptableObject
{
    // Liste des observers de l'�v�nement
    // (Hashset pour �viter les doublons)
    protected readonly HashSet<ScriptableEventListener> listeners = new HashSet<ScriptableEventListener>();

    // Emission de l'�v�nement
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
