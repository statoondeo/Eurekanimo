using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	[SerializeField] protected AudioSource MainMusicSource;
	[SerializeField] protected AudioSource SecondaryMusicSource;
	[SerializeField] protected AudioSource EffectSource;

	protected override void Awake()
	{
		base.Awake();
		Debug.Assert(null != MainMusicSource, gameObject.name + "/MainMusic not set!");
		Debug.Assert(null != SecondaryMusicSource, gameObject.name + "/SecondaryMusic not set!");
		Debug.Assert(null != EffectSource, gameObject.name + "/EffectSource not set!");
	}

	public void PlayMainMusic(AudioClip clip, bool forceRestart = false)
	{
		if (!MainMusicSource.isPlaying || forceRestart)
		{
			MainMusicSource.clip = clip;
			MainMusicSource.Play();
		}
	}

	public void PlayOneShot(AudioClip clip)
	{
		EffectSource.PlayOneShot(clip);
	}
}
