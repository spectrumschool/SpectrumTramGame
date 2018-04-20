using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public AudioClip acDoef;
	public AudioClip acBackground;
	public AudioClip acMusic;
	public AudioClip acScream;

	void Start()
	{

		//start bg music
		SoundKit.instance.playSoundLooped(acMusic);

		//start bg sound tram

		SoundKit.instance.playSoundLooped(acBackground);
	}

	public void PlayDoef()
	{
		SoundKit.instance.playOneShot (acDoef);
	}

	public void PlayScream()
	{
		SoundKit.instance.playOneShot (acScream);
	}
}
