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

	void Awake()
	{
		_targetPosition = transform.localPosition;
		_tram = transform.parent;
		onBoard = false;
		//offscreen
		transform.position = new Vector3(0,50,0);
		_button = "P"+(playerIndex+1).ToString();
		_sprite = GetComponent<SpriteRenderer>();
	}

	void OnEnable()
	{
		EventManager.OnPassengerTimeout += OnPassengerTimeout;
	}

	void OnDisable()
	{
		EventManager.OnPassengerTimeout -= OnPassengerTimeout;
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
		this.enabled = true;
		_onTheFloor = false;
		_inTram = false;
		_sprite.sortingOrder = 2;
		transform.parent = _tram;
		transform.localPosition = new Vector3(-0.24f, _targetPosition.y,0);
		transform.ZKlocalPositionTo(_targetPosition,1.0f).setEaseType(EaseType.QuadInOut).setCompletionHandler(OnEnterTramComplete).start();

	}

	public void JumpOut()
	{
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
				AudioManager.inst.PlayScream();
			}
		}
	}

	void OnEnterTramComplete(ITween<Vector3> tween)
	{
		EventManager.EnterTramCompleteEvent(playerIndex);
		onBoard = true;
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
			float xTranslation = Time.deltaTime * GameManager.inst.tramSpeed * -4.0f;
			transform.Translate(xTranslation, 0, 0);

			if(transform.position.x < -20.0f)
			{
				this.enabled = false;
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
