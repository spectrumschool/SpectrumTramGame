using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

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
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Update()
	{
		//TEST
		if(Input.GetButtonDown("P1"))
		{
			reputation += 1;
		}
	}
}
