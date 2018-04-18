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

	Transform _tfLastStopDriver;
	Transform _tfLastStopPassenger;

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
	}

	void Start()
	{
		StartCoroutine( StartGameCR() );
	}

	void Update()
	{
		//update the distance travelled
		distanceTravelled += Time.deltaTime * GameManager.inst.tramSpeed;
		tramSpeed = Mathf.Lerp(minTramSpeed,maxTramSpeed,distanceTravelled/maxSpeedDistance);

		if(distanceTravelled >= _nextStopDriver)
		{
			_nextStopDriver += distanceBetweenStops;


//			float yVal = (_tfLastStopDriver == null ? distanceBetweenStops : _tfLastStopDriver.localPosition.y + distanceBetweenStops);

			_shuffledTracks.Shuffle();
			int numStops = UnityEngine.Random.Range(1,4);
			for (int i = 0; i < 4; i++)
			{
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
		//keep a shuffled queue of names
		haltenamen.Shuffle();
		_stopNamesQueue.Clear();
		_stopNamesQueue = new Queue<string>(haltenamen);

		//reset values, stop tram
		reputation = 0;
		distanceTravelled = 0;
		tramSpeed = 0;
		_nextStopDriver = distanceBetweenStops;
		_nextStopPassenger = distanceBetweenStops + passengerStopOffset;

		//wait
		yield return new WaitForSeconds(2.0f);

		//start tram
		tramSpeed = minTramSpeed;
	}
}
