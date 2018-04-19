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

	public bool onBoard {get; private set; }

	void Awake()
	{
		_targetPosition = transform.localPosition;
		_tram = transform.parent;
		onBoard = false;
		//offscreen
		transform.position = new Vector3(0,50,0);
	}

	public void EnterTram()
	{
		onBoard = true;
		transform.parent = _tram;
		transform.localPosition = new Vector3(-0.24f, _targetPosition.y,0);
		transform.ZKlocalPositionTo(_targetPosition,1.0f).setEaseType(EaseType.QuadInOut).setCompletionHandler(OnEnterTramComplete).start();

	}

	void OnEnterTramComplete(ITween<Vector3> tween)
	{
		EventManager.EnterTramCompleteEvent(playerIndex);
	}

	void Update ()
	{
		
	}
}
