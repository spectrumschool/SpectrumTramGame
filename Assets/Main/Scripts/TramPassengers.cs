using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramPassengers : MonoBehaviour
{
	public float tredInterval = 1.0f;
	public float tredDelta = 1.0f;
	float _tredTimer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		_tredTimer += Time.deltaTime * GameManager.inst.tramSpeed;
		if(_tredTimer >= tredInterval)
		{
			_tredTimer -= tredInterval;
			tredDelta *= -1.0f;
			transform.Translate(0,tredDelta, 0);
		}
	}
}
