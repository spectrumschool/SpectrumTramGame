using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prime31.ZestKit;

public class PassengerInfo : MonoBehaviour
{
	public int playerIndex;
	public Text txtHalteNaam;
	public Text txtTimeLeft;

	Vector3 _upTarget;
	Vector3 _downTarget;
	int _timeLeft;
	float _timer;

	void Awake()
	{
		_upTarget = new Vector3(transform.position.x,12.0f,0);
		_downTarget = new Vector3(transform.position.x,0,0);

		OnResetGame();
	}

	void Start()
	{
	}

	void OnEnable()
	{
		EventManager.OnPassengerHitStop += OnPassengerHitStop;
		EventManager.OnPassengerHitRails += OnPassengerHitRails;
		EventManager.OnGameOver += OnGameOver;
		EventManager.OnResetGame += OnResetGame;
	}

	void OnDisable()
	{
		EventManager.OnPassengerHitStop -= OnPassengerHitStop;
		EventManager.OnPassengerHitRails -= OnPassengerHitRails;
		EventManager.OnGameOver -= OnGameOver;
		EventManager.OnResetGame -= OnResetGame;
	}

	void OnResetGame()
	{
		transform.position = _upTarget;
		this.enabled = false;
	}

	void OnGameOver()
	{
		Hide();
	}

	void OnPassengerHitStop (int index, string haltenaam)
	{
		if(index == playerIndex)
		{
			if(haltenaam.Equals(txtHalteNaam.text))
			{
				GameManager.inst.reputation += 1;
				GameManager.inst.score += GameManager.inst.reputation;
				Hide();
			}
			else
			{
				AudioManager.inst.PlayScream();
				GameManager.inst.reputation -= 1;
				Hide();
			}
		}
	}

	void OnPassengerHitRails (int index)
	{
		if(index == playerIndex)
		{
			AudioManager.inst.PlayScream();
			GameManager.inst.reputation -= 1;
			Hide();
		}
	}

	public void Show(int time, string haltenaam)
	{
		_timeLeft = time;
		txtTimeLeft.text = _timeLeft.ToString();
		txtTimeLeft.color = Color.black;

		txtHalteNaam.text = haltenaam;

		_timer = 0;
		transform.position = _upTarget;
		ZestKit.instance.stopAllTweensWithTarget(transform);
		transform.ZKpositionTo(_downTarget,0.12f).setEaseType(EaseType.BounceOut).start();
		this.enabled = true;
	}

	public void Hide()
	{
		this.enabled = false;
		ZestKit.instance.stopAllTweensWithTarget(transform);
		transform.ZKpositionTo(_upTarget,0.12f).start();
	}

	void Update()
	{
		if(GameManager.inst.tramSpeed == 0) return;
		_timer += Time.deltaTime * GameManager.inst.tramSpeed;
		if(_timer >= 1.0f)
		{
			_timer -= 1.0f;
			_timeLeft--;
			txtTimeLeft.text = _timeLeft.ToString();
			if(_timeLeft == 0)
			{
				EventManager.PassengerTimeoutEvent(playerIndex);
				txtTimeLeft.color = Color.red;
				this.enabled = false;
			}
		}
	}
}
