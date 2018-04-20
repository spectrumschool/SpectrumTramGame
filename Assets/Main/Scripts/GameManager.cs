using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public float minTramSpeed = 1;
	public float maxTramSpeed = 10;
	public float distanceBetweenStops = 8;
	public float passengerStopOffset = 4;
	public float maxSpeedDistance = 100.0f;
	public List<string> haltenamen;
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

	Transform _tfLastStopDriver;
	Transform _tfLastStopPassenger;

	System.Action _updateHandler = delegate {};

	Queue<string> _stopNamesQueue;
	List<int> _shuffledTracks;
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

	void Awake()
	{
		_stopNamesQueue = new Queue<string>();
		_nextStopNames = new string[4];
		_shuffledTracks = new List<int>(){0,1,2,3};
		_playerInput = new string[]{"P1","P2","P3","P4"};
	}

	void Start()
	{
		Cursor.visible = false;
		StartCoroutine( StartGameCR() );
	}

	void OnEnable()
	{
		EventManager.OnEnterTramComplete += OnEnterTramComplete;
	}

	void OnDisable()
	{
		EventManager.OnEnterTramComplete -= OnEnterTramComplete;
	}

	void OnEnterTramComplete (int playerIndex)
	{
		if(_boardingCount < 4)
		{
			++_boardingCount;
			passengerInfo[playerIndex].Show( UnityEngine.Random.Range(20,40) ,_stopNamesQueue.Dequeue());
			if(_boardingCount == 4)
			{
				tramSpeed = minTramSpeed;
				_updateHandler = UpdateMovingTram;
			}
		}
		else
		{

		}
	}

	void Update()
	{
		_updateHandler();
	}
		
	void UpdateMovingTram()
	{
		//update the distance travelled
		if(tramSpeed > 0)
		{
			distanceTravelled += Time.deltaTime * tramSpeed;
			tramSpeed = Mathf.Lerp(minTramSpeed,maxTramSpeed,distanceTravelled/maxSpeedDistance);
		}

		if(distanceTravelled >= _nextStopDriver)
		{
			_nextStopDriver += distanceBetweenStops;


//			float yVal = (_tfLastStopDriver == null ? distanceBetweenStops : _tfLastStopDriver.localPosition.y + distanceBetweenStops);

			_shuffledTracks.Shuffle();
			int numStops = UnityEngine.Random.Range(1,4);
			for (int i = 0; i < 4; i++)
			{
//				string halteNaam = "";
//				if(


				if(!string.IsNullOrEmpty(_nextStopNames[_shuffledTracks[i]]))
				{
					_stopNamesQueue.Enqueue(_nextStopNames[_shuffledTracks[i]]);
				}

				if(i < numStops)
				{
					//TODO: assign halte naam
					_nextStopNames[_shuffledTracks[i]] = _stopNamesQueue.Dequeue();



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

			//halte naam based on track

			//if there is a name
			if(!string.IsNullOrEmpty(_nextStopNames[tramDriver.currentTrack]))
			{
//				float xVal = (_tfLastStopPassenger == null ? distanceBetweenStops : _tfLastStopPassenger.localPosition.x + distanceBetweenStops);
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
			if(!passengers[i].onBoard && Input.GetButtonDown(_playerInput[i]))
			{
				passengers[i].EnterTram();
			}
		}
	}
		


	IEnumerator StartGameCR()
	{


		//keep a shuffled queue of names
		haltenamen.Shuffle();
		_stopNamesQueue.Clear();
		_stopNamesQueue = new Queue<string>(haltenamen);

		//reset values, stop tram
		reputation = 2;
		distanceTravelled = 0;
		tramSpeed = 0;
		_nextStopDriver = distanceBetweenStops;
		_nextStopPassenger = distanceBetweenStops + passengerStopOffset;


		_updateHandler = UpdateBoarding;

		//wait
		yield return new WaitForSeconds(2.0f);

		//todo: do stuff

	}
}
