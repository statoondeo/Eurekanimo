using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] protected ScriptableEvent GameEvent;
    [SerializeField] protected UnityEvent Response;

    protected void OnEnable() => GameEvent.RegisterListener(this);

    protected void OnDisable() => GameEvent.UnregisterListener(this);

    public void OnEventRaised() => Response?.Invoke();
}