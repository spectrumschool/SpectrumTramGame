using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioClip acDoef;
	public AudioClip acBackground;
	public AudioClip acMusic;
	float _nextSample;
	float _currentTime;

	void Start()
	{
		_nextSample = GameManager.inst.tramSpeed;

		//start bg music

		SoundKit.instance.playSoundLooped(acMusic);

		//start bg sound tram

		SoundKit.instance.playSoundLooped(acBackground);
	}

	void Update()
	{
		if (GameManager.inst.tramSpeed > 0)
		{
			_nextSample = Mathf.Lerp(0.01f,1f, GameManager.inst.maxTramSpeed/GameManager.inst.tramSpeed);

			_currentTime += Time.deltaTime;
			if (_currentTime > _nextSample) {
				_currentTime = 0;
				SoundKit.instance.playOneShot (acDoef);
			}
		}
			 
	}
}
