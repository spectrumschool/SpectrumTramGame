using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramDriver : MonoBehaviour
{
    public Transform[] tracks;

	private int _currentTrack;
    private Vector3 original;

    void Start()
	{
        //we start at track 0
        _currentTrack = 2;
		ChangeTrack();
	}

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow) && _currentTrack > 0)
		{
            _currentTrack = _currentTrack - 1;
            ChangeTrack();

        }
		if(Input.GetKeyDown(KeyCode.RightArrow) && _currentTrack < 3)
		{
            //TODO: ChangeTrack(???);
            _currentTrack = _currentTrack + 1;
            ChangeTrack();
		}
	}

	void ChangeTrack()
	{
        transform.localPosition = new Vector3(tracks[_currentTrack].localPosition.x, -3.8f, 0f);
	}
}
