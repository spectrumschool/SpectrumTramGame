using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
	float DEBUGTIME = 0;
	float DEBUGTIME_STOPS = 0;
	float DEBUGTIME_SPAWN_ARRIVE = 0;

	public float minTramSpeed = 1;
	public float maxTramSpeed = 10;
	public float passengerViewSpeedMult = 4;
	public float distanceBetweenStops = 8;
	public float passengerStopOffset = 4;
	public float maxSpeedDistance = 100.0f;
	public int timeBetweenStops = 30;
	public int timeBetweenStopsMargin = 5;
	public StopNameManager stopNameManager;
	public Transform[] tracks;
	public PassengerInfo[] passengerInfo;
	public Passenger[] passengers;
	public Transform driverView;
	public Transform passengersView;
	public TramDriver tramDriver;
	public SpawnManager spawnHalteDriver;
	public SpawnManager spawnHaltePassenger;

	public float tramSpeed = 0;

	public float distanceTravelled {get; private set; }
	float _nextStopDriver;
	float _nextStopPassenger;
	string[] _nextStopNames;

	string[] _playerInput;
	int _boardingCount;
	int _stopsPassed;

	Transform _tfLastStopDriver;
	Transform _tfLastStopPassenger;

	System.Action _updateHandler = delegate {};
	List<int> _shuffledTracks;
	private int _reputation = 0;
	public int reputation
	{
		get { return _reputation; }
		set
		{
			if(value != _reputation)
			{
				if(value > _reputation) SoundKit.instance.playOneShot(AudioManager.inst.acPositive);
				else SoundKit.instance.playOneShot(AudioManager.inst.acNegative);

				if(value == 0)
				{
					EventManager.GameOverEvent();
				}
			}

			_reputation = Mathf.Clamp(value,0,6);
			EventManager.ReputationChangedEvent(_reputation);


		}
	}

	private int _score = 0;
	public int score
	{
		get { return _score; }
		set
		{
			_score = value;
			EventManager.ScoreChangedEvent(_score);
		}
	}

	void Awake()
	{
		_nextStopNames = new string[4];
		_shuffledTracks = new List<int>(){0,1,2,3};
		_playerInput = new string[]{"P1","P2","P3","P4"};
		stopNameManager.Setup();
	}

	void Start()
	{
		Cursor.visible = false;
		StartCoroutine( StartGameCR() );
	}

	void OnEnable()
	{
		EventManager.OnEnterTramComplete += OnEnterTramComplete;
		EventManager.OnGameOver += OnGameOver;
		EventManager.OnResetGame += OnResetGame;
	}

	void OnDisable()
	{
		EventManager.OnEnterTramComplete -= OnEnterTramComplete;
		EventManager.OnGameOver -= OnGameOver;
		EventManager.OnResetGame -= OnResetGame;
	}

	void OnGameOver()
	{
		_updateHandler = delegate {};
		tramSpeed = 0;
	}

	void OnResetGame()
	{
		StartCoroutine( StartGameCR() );
	}

	void OnEnterTramComplete (int playerIndex)
	{
//		Debug.Log("DEBUGTIME_SPAWN_ARRIVE: "+DEBUGTIME_SPAWN_ARRIVE);

		RequestedStop rs = stopNameManager.CharArrived();
		passengerInfo[playerIndex].Show( rs.urgency * timeBetweenStops + timeBetweenStopsMargin, rs.stopName);

		if(_boardingCount < 4)
		{
			++_boardingCount;

			//all aboard, start moving!
			if(_boardingCount == 4)
			{
				tramSpeed = minTramSpeed;
				_updateHandler = UpdateMovingTram;
				EventManager.StartTramEvent();
			}
		}
	}

	void Update()
	{
		DEBUGTIME += Time.deltaTime * tramSpeed;
		DEBUGTIME_STOPS += Time.deltaTime * tramSpeed;
		DEBUGTIME_SPAWN_ARRIVE += Time.deltaTime * tramSpeed;

		_updateHandler();
	}
		
	void UpdateMovingTram()
	{
		//update the distance travelled & tramspeed
		if(tramSpeed > 0)
		{
			distanceTravelled += Time.deltaTime * tramSpeed;
//			tramSpeed = Mathf.Lerp(minTramSpeed,maxTramSpeed,distanceTravelled/maxSpeedDistance);
		}

		//place stops & refill passengers
		if(distanceTravelled >= _nextStopDriver)
		{
//			Debug.Log(_stopsPassed);
			if(_stopsPassed > 0 && _stopsPassed < 3) tramSpeed += .5f;
			else if(_stopsPassed != 0) tramSpeed += 0.2f;
			tramSpeed = Mathf.Min(tramSpeed,maxTramSpeed);
			++_stopsPassed;

//			Debug.Log("DEBUGTIME_STOPS: "+ DEBUGTIME_STOPS);
			DEBUGTIME_STOPS = 0;

			//refill passengers
			for (int i = 0; i < 4; i++)
			{
				if(!passengers[i].enabled)
				{
					passengers[i].EnterTram();
					DEBUGTIME_SPAWN_ARRIVE = 0;
				}
			}

//			Debug.Log(DEBUGTIME.ToString("F"));

			_nextStopDriver += distanceBetweenStops;
			_shuffledTracks.Shuffle();

			List<string> stopNames = stopNameManager.PlaceStops();
			if(stopNames.Count == 0)
			{
				Debug.LogWarning("NO STOPNAMES!");
			}

			for (int i = 0; i < 4; i++)
			{

				if(i < stopNames.Count)
				{
					//TODO: assign halte naam
					_nextStopNames[_shuffledTracks[i]] = stopNames[i];

					_tfLastStopDriver = spawnHalteDriver.SpawnItem(new Vector2(tracks[_shuffledTracks[i]].localPosition.x, 5.0f)).transform;
					_tfLastStopDriver.GetComponent<Halte>().haltenaam.text = _nextStopNames[_shuffledTracks[i]];
				}
				else
				{
					_nextStopNames[_shuffledTracks[i]] = "";
				}
			}
		}

		if(distanceTravelled >= _nextStopPassenger)
		{
			_nextStopPassenger += distanceBetweenStops;

			//halte naam based on track, no name => no stop!
			if(!string.IsNullOrEmpty(_nextStopNames[tramDriver.currentTrack]))
			{
				//spawn halte Passenger
				_tfLastStopPassenger = spawnHaltePassenger.SpawnItem(new Vector2(8.9f, 0)).transform;
				_tfLastStopPassenger.GetComponent<Halte>().haltenaam.text = _nextStopNames[tramDriver.currentTrack];
			}
		}
	}

	void UpdateBoarding()
	{
		for (int i = 0; i < 4; i++)
		{
			if(!passengers[i].onBoard && passengers[i].inputEnabled && Input.GetButtonDown(_playerInput[i]))
			{
				passengers[i].EnterTram();
			}
		}
	}

	IEnumerator StartGameCR()
	{
		stopNameManager.StartGame();

		//reset values, stop tram
		reputation = 3;
		score = 0;
		distanceTravelled = 0;
		tramSpeed = 0;
		_boardingCount = 0;
		_stopsPassed = 0;
		_nextStopDriver = 0;
		_nextStopPassenger = passengerStopOffset;

		_updateHandler = UpdateBoarding;

		//wait
		yield return new WaitForSeconds(2.0f);

		//todo: do stuff

	}

//	void OnGUI()
//	{
//		GUIStyle style = new GUIStyle();
//		style.fontSize = 120;
//		GUILayout.Label(DEBUGTIME.ToString("F"), style);
//	}
}
