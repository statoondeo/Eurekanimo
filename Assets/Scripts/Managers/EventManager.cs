using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
	private static readonly string PREFAB_PATH = "Prefabs/EventManager";

	public static GameObject Instantiate()
	{
		return (GameObject.Instantiate(Resources.Load(PREFAB_PATH)) as GameObject);
	}

	public enum Events
	{
		// Changement de scènes
		OnSceneRequested,
		OnSceneTransitionRequested,

		// Sélection d'une fiche
		OnFormSelected,
		OnFormNotSelected,

		// Gestion du formulaire
		OnGridFilled,
		OnGridNotFilled,
		OnGridCorrectlyFilled,
		OnGridWronglyFilled,

		// Gestion des containers
		OnContainerEmptied,
		OnContainerFilled,
		OnContainerHovered,

		// Gestion des tokens
		OnTokenDragged,
		OnTokenDropped,

		// Gestion du gameplay
		OnGameReady,
		OnGameNotReady
	}

	[SerializeField] protected EventModel OnSceneRequested;
	[SerializeField] protected EventModel OnSceneTransitionRequested;
	[SerializeField] protected EventModel OnFormSelected;
	[SerializeField] protected EventModel OnFormNotSelected;
	[SerializeField] protected EventModel OnGridFilled;
	[SerializeField] protected EventModel OnGridNotFilled;
	[SerializeField] protected EventModel OnGridCorrectlyFilled;
	[SerializeField] protected EventModel OnGridWronglyFilled;
	[SerializeField] protected EventModel OnContainerEmptied;
	[SerializeField] protected EventModel OnContainerFilled;
	[SerializeField] protected EventModel OnContainerHovered;
	[SerializeField] protected EventModel OnTokenDragged;
	[SerializeField] protected EventModel OnTokenDropped;
	[SerializeField] protected EventModel OnGameReady;
	[SerializeField] protected EventModel OnGameNotReady;

	protected readonly Dictionary<Events, EventModel> EventsAtlas = new Dictionary<Events, EventModel>();

	protected override void Awake()
	{
		base.Awake();

		Load();
	}

	protected void Load()
	{
		EventsAtlas.Add(Events.OnSceneRequested, OnSceneRequested);
		EventsAtlas.Add(Events.OnSceneTransitionRequested, OnSceneTransitionRequested);
		EventsAtlas.Add(Events.OnFormSelected, OnFormSelected);
		EventsAtlas.Add(Events.OnFormNotSelected, OnFormNotSelected);
		EventsAtlas.Add(Events.OnGridFilled, OnGridFilled);
		EventsAtlas.Add(Events.OnGridNotFilled, OnGridNotFilled);
		EventsAtlas.Add(Events.OnGridCorrectlyFilled, OnGridCorrectlyFilled);
		EventsAtlas.Add(Events.OnGridWronglyFilled, OnGridWronglyFilled);
		EventsAtlas.Add(Events.OnContainerEmptied, OnContainerEmptied);
		EventsAtlas.Add(Events.OnContainerFilled, OnContainerFilled);
		EventsAtlas.Add(Events.OnContainerHovered, OnContainerHovered);
		EventsAtlas.Add(Events.OnTokenDragged, OnTokenDragged);
		EventsAtlas.Add(Events.OnTokenDropped, OnTokenDropped);
		EventsAtlas.Add(Events.OnGameReady, OnGameReady);
		EventsAtlas.Add(Events.OnGameNotReady, OnGameNotReady);
	}

	protected EventModel GetEvent(Events eventName)
	{
		return (EventsAtlas[eventName]);
	}

	public void Raise(Events eventName)
	{
		Raise(eventName, ModelEventArg.Empty);
	}

	public void Raise(Events eventName, ModelEventArg eventArg)
	{
		GetEvent(eventName).Raise(eventArg);
	}

	/// <summary>
	/// Fonction utilitaire permettant de créer un observateur d'évènement
	/// </summary>
	/// <param name="eventAssetPath">ScriptableEvent asset à charger</param>
	/// <param name="callback">Fonction callback</param>
	/// <returns>Listener</returns>
	public EventListener CreateEventListener(GameObject gameObject, Events eventToListen, UnityAction<ModelEventArg> callback)
	{
		// Création du listener
		EventListener eventListener = gameObject.AddComponent<EventListener>();

		// Chargement de l'asset associée
		EventModel scriptableEvent = eventListener.SetEventModel(GetEvent(eventToListen));

		// Chargement de la callback
		UnityEvent<ModelEventArg> unityEvent = eventListener.SetEventCallback(new UnityEvent<ModelEventArg>());
		unityEvent.AddListener(callback);

		// Liaison entre le listener et l'événement
		scriptableEvent.RegisterListener(eventListener);

		return (eventListener);
	}
}
