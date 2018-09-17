using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.ZestKit;
using System;

public class Passenger : MonoBehaviour
{
	public int playerIndex;
	Transform _tram;
	Vector3 _targetPosition;
	bool _onTheFloor = false;
	bool _inTram = false;
	string _button;
	SpriteRenderer _sprite;

	public bool onBoard {get; private set; }
	public bool inputEnabled {get; private set; }

	void Awake()
	{
		_targetPosition = transform.localPosition;
		_tram = transform.parent;
		_button = "P"+(playerIndex+1).ToString();
		_sprite = GetComponent<SpriteRenderer>();
		OnResetGame();
	}

	void OnEnable()
	{
		EventManager.OnPassengerTimeout += OnPassengerTimeout;
		EventManager.OnResetGame += OnResetGame;
		EventManager.OnGameOver += OnGameOver;
	}

	void OnGameOver ()
	{
		ZestKit.instance.stopAllTweensWithTarget(transform);
	}

	void OnDisable()
	{
		EventManager.OnPassengerTimeout -= OnPassengerTimeout;
		EventManager.OnResetGame -= OnResetGame;
		EventManager.OnGameOver -= OnGameOver;
	}

	void OnResetGame()
	{
		ZestKit.instance.stopAllTweensWithTarget(transform);
		_onTheFloor = false;
		_inTram = false;
		onBoard = false;
		inputEnabled = true;
		//offscreen
		transform.position = new Vector3(0,50,0);
		this.enabled = false;
	}

	void OnPassengerTimeout (int index)
	{
		if(index == playerIndex)
		{
			JumpOut();
		}
	}

	public void EnterTram()
	{
		Debug.Log("EnterTram");
		inputEnabled = false;
		SoundKit.instance.playOneShot(AudioManager.inst.acHopOn);
		this.enabled = true;
		_onTheFloor = false;
		_inTram = false;
		_sprite.sortingOrder = 2;
		transform.parent = _tram;
		transform.localPosition = new Vector3(-0.24f, _targetPosition.y,0);
		float duration = (GameManager.inst.tramSpeed == 0) ? 1.0f : 1.0f/GameManager.inst.tramSpeed;
		transform.ZKlocalPositionTo(_targetPosition,duration).setEaseType(EaseType.QuadInOut).setCompletionHandler(OnEnterTramComplete).start();

	}

	public void JumpOut()
	{
		inputEnabled = false;
		transform.SetParent(null);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10.0f);
		if(hit.collider != null)
		{
			Vector3 target = new Vector3(transform.position.x, hit.point.y,0);
			_onTheFloor = true;
			_inTram = false;
			transform.position = target;
			_sprite.sortingOrder = 7;

			Halte halte = hit.collider.transform.parent.GetComponent<Halte>();
			if(halte != null)
			{
				EventManager.PassengerHitStopEvent(playerIndex,halte.haltenaam.text);
			}
			else
			{
				EventManager.PassengerHitRailsEvent(playerIndex);
			}
		}
	}

	void OnEnterTramComplete(ITween<Vector3> tween)
	{
		Debug.Log("OnEnterTramComplete");
		EventManager.EnterTramCompleteEvent(playerIndex);
		onBoard = true;
		inputEnabled = true;
		_inTram = true;
	}

//	void OnHitFloor(ITween<Vector3> tween)
//	{
//		_onTheFloor = true;
//	}

	void Update ()
	{
		if(_onTheFloor)
		{
			float xTranslation = Time.deltaTime * GameManager.inst.tramSpeed * -GameManager.inst.passengerViewSpeedMult;
			transform.Translate(xTranslation, 0, 0);

			if(transform.position.x < -20.0f)
			{
				this.enabled = false;
				inputEnabled = true;
			}
		}
		else
		{
			if(_inTram && GameManager.inst.tramSpeed > 0 && Input.GetButtonDown(_button))
			{
				JumpOut();
			}
		}
	}
}
