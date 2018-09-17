using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public float musicVolume = .5f;
	public float musicFadeDuration = 1.0f;
	public AudioClip acDoef;
	public AudioClip acBackground;
	public AudioClip acMusic;
	public AudioClip acMenuMusic;
	public AudioClip acScream;
	public AudioClip acHopOn;
	public AudioClip acPositive;
	public AudioClip acNegative;
	public AudioClip acBell;

	static string BELL = "Bell";

	void Start()
	{
		SoundKit.instance.playBackgroundMusic(acMenuMusic,musicVolume);
		//start bg music
//		SoundKit.instance.playSoundLooped(acMusic);

		//start bg sound tram

//		SoundKit.instance.playSoundLooped(acBackground);


	}

	void OnEnable()
	{
		EventManager.OnStartTram += OnStartTram;
		EventManager.OnGameOver += OnGameOver;
	}

	void OnDisable()
	{
		EventManager.OnStartTram -= OnStartTram;
		EventManager.OnGameOver -= OnGameOver;
	}

	public void OnGameOver()
	{
//		SoundKit.instance.playBackgroundMusic(acMenuMusic,musicVolume);
		SoundKit.instance.backgroundSound.fadeOutAndStop(musicFadeDuration, ()=>{SoundKit.instance.playBackgroundMusic(acMenuMusic,musicVolume);});
	}

	public void OnStartTram()
	{
//		SoundKit.instance.playBackgroundMusic(acMusic,musicVolume);
		SoundKit.instance.backgroundSound.fadeOutAndStop(musicFadeDuration, ()=>{SoundKit.instance.playBackgroundMusic(acMusic,musicVolume);});
	}

	public void PlayDoef()
	{
		SoundKit.instance.playOneShot (acDoef);
	}

	public void PlayScream()
	{
		SoundKit.instance.playOneShot (acScream);
	}

	void Update()
	{
		if(Input.GetButtonDown(BELL))
		{
			SoundKit.instance.playOneShot (acBell);
		}
	}
}
