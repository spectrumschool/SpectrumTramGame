using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramDriver : MonoBehaviour
{
    public Transform[] tracks;
    public float speed = 1.0f;

    private int _currentTrack;
    private Vector3 _target;

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

        float step = speed * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, _target, step);


    }

	void ChangeTrack()
	{
        _target = new Vector3(tracks[_currentTrack].localPosition.x, -3.8f, 0f);
        
	}
}
