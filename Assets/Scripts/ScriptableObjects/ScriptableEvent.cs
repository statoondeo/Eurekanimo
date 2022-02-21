using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Eurekanimo/New Event", fileName = "New Event")]
public class ScriptableEvent : ScriptableObject
{
    protected readonly HashSet<EventListener> listeners = new HashSet<EventListener>();

    public void Raise()
    {
        foreach (EventListener listener in listeners) listener.OnEventRaised();
    }

    public void RegisterListener(EventListener listener) => listeners.Add(listener);

    public void UnregisterListener(EventListener listener) => listeners.Remove(listener);
}
