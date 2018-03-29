using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public int minTramSpeed = 1;
	public int maxTramSpeed = 10;
	public string[] haltenamen;
	public Transform[] tracks;
	public Transform driverView;
	public Transform passengersView;

	[HideInInspector]
	public int tramSpeed = 0;

	private int _reputation = 0;
	public int reputation
	{
		get { return _reputation; }
		set
		{
			_reputation = value;
			EventManager.ReputationChangedEvent(_reputation);
		}
	}

	void Start()
	{
		StartCoroutine( StartGameCR() );
	}

	void Update()
	{
		//TEST
		if(Input.GetButtonDown("P1"))
		{
			reputation += 1;
		}
		if(Input.GetButtonDown("P2"))
		{
			reputation -= 1;
		}
	}

	IEnumerator StartGameCR()
	{
		_reputation = 0;
		EventManager.ReputationChangedEvent(_reputation);
		tramSpeed = 0;

		yield return new WaitForSeconds(2.0f);

		tramSpeed = minTramSpeed;
	}
}
