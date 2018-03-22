using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramDriver : MonoBehaviour
{
	
	private int currentTrack;

	void Start()
	{
		//we start at track 1
		ChangeTrack(1);
	}

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			//TODO: ChangeTrack(???);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			//TODO: ChangeTrack(???);
		}
	}

	void ChangeTrack(int newTrack)
	{
		//TODO: change position to track position...
		//transform.position = new Vector3(?,?,?);
	}
}
