using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestedStop
{
	public string stopName;
	public int urgency;

	public RequestedStop (string stopName, int urgency)
	{
		this.stopName = stopName;
		this.urgency = urgency;
	}
}

[System.Serializable]
public class StopNameManager
{
	public List<string> stopNames;

	Queue<string> _stopNamesQueue;
	List<RequestedStop> _requestedStops;

	public void Setup()
	{
		_stopNamesQueue = new Queue<string>();
		_requestedStops = new List<RequestedStop>();
	}

	public void StartGame()
	{
		//keep a shuffled queue of names
		stopNames.Shuffle();
		_stopNamesQueue.Clear();
		_stopNamesQueue = new Queue<string>(stopNames);
		_requestedStops.Clear();
	}

	public RequestedStop CharArrived()
	{
		RequestedStop requestedStop = new RequestedStop("",UnityEngine.Random.Range(1,4));


		//A chance of 1/6 to get the same stop as another player (if there are other players with stops...)
		bool sameAsOther = (_requestedStops.Count > 0 && UnityEngine.Random.Range(0,6) == 0);
		if(sameAsOther)
		{
			RequestedStop rsSame = _requestedStops[UnityEngine.Random.Range(0,_requestedStops.Count)];
			requestedStop.stopName = rsSame.stopName;
			requestedStop.urgency = rsSame.urgency;
		}
		else
		{
			//increase urgency until it is unique! (no 2 stops with the same urgency)
			bool isUnique = false;
			do
			{
				isUnique = true;
				for (int i = 0; i < _requestedStops.Count; i++)
				{
					if(requestedStop.urgency == _requestedStops[i].urgency)
					{
						++requestedStop.urgency;
						isUnique = false;
					}
				}
			} while (!isUnique);

			requestedStop.stopName = _stopNamesQueue.Dequeue();
			_stopNamesQueue.Enqueue(requestedStop.stopName);
			_requestedStops.Add(requestedStop);
		}
		return requestedStop;
	}

	public List<string> PlaceStops()
	{
		List<string> stopNames = new List<string>();
		for (int i = _requestedStops.Count-1; i >= 0 ; i--)
		{
			_requestedStops[i].urgency--;
			if(_requestedStops[i].urgency == 0)
			{
				//add stop (dont add doubles!!)
				if(!stopNames.Contains(_requestedStops[i].stopName)){ stopNames.Add(_requestedStops[i].stopName); }
				_requestedStops.RemoveAt(i);
			}
		}

		// A chance of 1/2 to add an extra stop (max 3), add extra stop when there are none
		while (stopNames.Count == 0 || stopNames.Count < 3 && UnityEngine.Random.Range(0,2) == 0)
		{
			// A chance of 1/4 to add another requested stop (if there are requested stops...)
			bool earlyStop = _requestedStops.Count > 0 && UnityEngine.Random.Range(0,4) == 0;
			if(earlyStop)
			{
				int index = UnityEngine.Random.Range(0,_requestedStops.Count); //??
				if(!stopNames.Contains(_requestedStops[index].stopName))
				{
					stopNames.Add(_requestedStops[index].stopName);
				}
				else
				{
					earlyStop = false;
				}
			}

			if(!earlyStop)
			{
				string stopName = _stopNamesQueue.Dequeue();
				_stopNamesQueue.Enqueue(stopName);
				stopNames.Add(stopName);
			}
		}
		return stopNames;
	}
}
