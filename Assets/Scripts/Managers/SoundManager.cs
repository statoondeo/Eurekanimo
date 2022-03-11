using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	protected static readonly string PLAYERPREFS_MUSIC_VOLUME = "MusicVolume";
	protected static readonly string PLAYERPREFS_SOUND_VOLUME = "SoundVolume";
	private static readonly string PREFAB_PATH = "Prefabs/SoundManager";

	public static GameObject Instantiate()
	{
		return (GameObject.Instantiate(Resources.Load<GameObject>(PREFAB_PATH)));
	}

	public enum Clips
	{
		MainTheme,
		VictoryTheme,
		DefeatTheme,
		ClickSound,
		DragSound,
		UnDragSound,
		DropSound,
		UnDropSound,
		CorrectSound,
		WrongSound
	}

	[Header("Sources pour jouer les sons et musiques")]
	[SerializeField] protected AudioSource MainMusicSource;
	[SerializeField] protected AudioSource SecondaryMusicSource;
	[SerializeField] protected AudioSource EffectSource;

	[Header("Musiques")]
	[SerializeField] protected AudioClip MainTheme;
	[SerializeField] protected AudioClip VictoryTheme;
	[SerializeField] protected AudioClip DefeatTheme;

	[Header("Sons")]
	[SerializeField] protected AudioClip ClickSound;
	[SerializeField] protected AudioClip UnDragSound;
	[SerializeField] protected AudioClip DragSound;
	[SerializeField] protected AudioClip UnDropSound;
	[SerializeField] protected AudioClip DropSound;
	[SerializeField] protected AudioClip CorrectSound;
	[SerializeField] protected AudioClip WrongSound;

	protected readonly Dictionary<Clips, Tuple<AudioClip, AudioSource>> SoundsAtlas = new Dictionary<Clips, Tuple<AudioClip, AudioSource>>();

	protected override void Awake()
	{
		base.Awake();
		Debug.Assert(null != MainMusicSource, gameObject.name + "/MainMusic not set!");
		Debug.Assert(null != SecondaryMusicSource, gameObject.name + "/SecondaryMusic not set!");
		Debug.Assert(null != EffectSource, gameObject.name + "/EffectSource not set!");

		Debug.Assert(null != MainTheme, gameObject.name + "/MainTheme not set!");
		Debug.Assert(null != VictoryTheme, gameObject.name + "/VictoryTheme not set!");
		Debug.Assert(null != DefeatTheme, gameObject.name + "/DefeatTheme not set!");

		Debug.Assert(null != ClickSound, gameObject.name + "/ClickSound not set!");
		Debug.Assert(null != DragSound, gameObject.name + "/DragSound not set!");
		Debug.Assert(null != UnDragSound, gameObject.name + "/UnDragSound not set!");
		Debug.Assert(null != DropSound, gameObject.name + "/DropSound not set!");
		Debug.Assert(null != UnDropSound, gameObject.name + "/UnDropSound not set!");
		Debug.Assert(null != CorrectSound, gameObject.name + "/CorrectSound not set!");
		Debug.Assert(null != WrongSound, gameObject.name + "/WrongSound not set!");

		MainMusicSource.volume = PlayerPrefs.GetFloat(PLAYERPREFS_MUSIC_VOLUME, 0.25f);
		SecondaryMusicSource.volume = PlayerPrefs.GetFloat(PLAYERPREFS_SOUND_VOLUME, 0.5f);
		EffectSource.volume = SecondaryMusicSource.volume;

		Load();
	}

	protected void Load()
	{
		SoundsAtlas.Add(Clips.MainTheme, new Tuple<AudioClip, AudioSource>(MainTheme, MainMusicSource));
		SoundsAtlas.Add(Clips.VictoryTheme, new Tuple<AudioClip, AudioSource>(VictoryTheme, SecondaryMusicSource));
		SoundsAtlas.Add(Clips.DefeatTheme, new Tuple<AudioClip, AudioSource>(DefeatTheme, SecondaryMusicSource));
		SoundsAtlas.Add(Clips.ClickSound, new Tuple<AudioClip, AudioSource>(ClickSound, EffectSource));
		SoundsAtlas.Add(Clips.CorrectSound, new Tuple<AudioClip, AudioSource>(CorrectSound, EffectSource));
		SoundsAtlas.Add(Clips.DragSound, new Tuple<AudioClip, AudioSource>(DragSound, EffectSource));
		SoundsAtlas.Add(Clips.UnDragSound, new Tuple<AudioClip, AudioSource>(UnDragSound, EffectSource));
		SoundsAtlas.Add(Clips.DropSound, new Tuple<AudioClip, AudioSource>(DropSound, EffectSource));
		SoundsAtlas.Add(Clips.UnDropSound, new Tuple<AudioClip, AudioSource>(UnDropSound, EffectSource));
		SoundsAtlas.Add(Clips.WrongSound, new Tuple<AudioClip, AudioSource>(WrongSound, EffectSource));
	}

	public float MusicVolume
	{
		get => MainMusicSource.volume;
		set
		{
			MainMusicSource.volume = value;
			PlayerPrefs.SetFloat(PLAYERPREFS_MUSIC_VOLUME, value);
		}
	}
	public float SoundVolume
	{
		get => EffectSource.volume;
		set
		{
			SecondaryMusicSource.volume = value;
			EffectSource.volume = value;
			PlayerPrefs.SetFloat(PLAYERPREFS_SOUND_VOLUME, value);
		}
	}

	public bool IsPlaying(Clips clip)
	{
		return (SoundsAtlas[clip].Item2.isPlaying);
	}

	public void Play(Clips clip)
	{
		Tuple<AudioClip, AudioSource> tuple = SoundsAtlas[clip];
		if (EffectSource == tuple.Item2)
		{
			tuple.Item2.PlayOneShot(tuple.Item1);
		}
		else
		{
			tuple.Item2.clip = tuple.Item1;
			tuple.Item2.Play();
		}
	}
}
