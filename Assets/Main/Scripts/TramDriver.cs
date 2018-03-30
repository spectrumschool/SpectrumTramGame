using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramDriver : MonoBehaviour
{
    public Transform[] tracks;
    public float speed = 1.0f;
    public float yPos = -2.0f;

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
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _currentTrack > 0)
        {
            //3 tests voorkan, midden , achter

            RaycastHit2D hitVoor = Physics2D.Raycast(transform.position + new Vector3(0, 2, 0), Vector3.left, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, 2, 0), transform.position + new Vector3(0, 2, 0) + Vector3.left * 4, Color.magenta, 0.1f);
            bool bVoor = hitVoor.collider != null;

            RaycastHit2D hitMid = Physics2D.Raycast(transform.position + new Vector3(0, 0, 0), Vector3.left, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, 0, 0), transform.position + new Vector3(0, 0, 0) + Vector3.left * 4, Color.magenta, 0.1f);
            bool bMid = hitMid.collider != null;

            RaycastHit2D hitAchter = Physics2D.Raycast(transform.position + new Vector3(0, -2, 0), Vector3.left, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, -2, 0), transform.position + new Vector3(0, -2, 0) + Vector3.left * 4, Color.magenta, 0.1f);
            bool bAchter = hitAchter.collider != null;

            if (!bVoor && !bMid && !bAchter)
            {
                _currentTrack = _currentTrack - 1;
                ChangeTrack();
            }
            else
            {
                transform.localPosition += new Vector3(-2.0f, 0.0f, 0.0f);
            }
        }

       
        
		if(Input.GetKeyDown(KeyCode.RightArrow) && _currentTrack < 3)
		{
            RaycastHit2D hitVoor = Physics2D.Raycast(transform.position + new Vector3(0, 2, 0), Vector3.right, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, 2, 0), transform.position + new Vector3(0, 2, 0) + Vector3.right * 4, Color.magenta, 0.1f);
            bool bVoor = hitVoor.collider != null;

            RaycastHit2D hitMid = Physics2D.Raycast(transform.position + new Vector3(0, 0, 0), Vector3.right, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, 0, 0), transform.position + new Vector3(0, 0, 0) + Vector3.right * 4, Color.magenta, 0.1f);
            bool bMid = hitMid.collider != null;

            RaycastHit2D hitAchter = Physics2D.Raycast(transform.position + new Vector3(0, -2, 0), Vector3.right, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, -2, 0), transform.position + new Vector3(0, -2, 0) + Vector3.right * 4, Color.magenta, 0.1f);
            bool bAchter = hitAchter.collider != null;

            if (!bVoor && !bMid && !bAchter)
            {
                _currentTrack = _currentTrack + 1;
                ChangeTrack();
            }
            else
            {
                transform.localPosition += new Vector3(+2.0f, 0.0f, 0.0f);
            }
        }

        float step = speed * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, _target, step);


    }

	void ChangeTrack()
	{
        _target = new Vector3(tracks[_currentTrack].localPosition.x, yPos, 0f);
        
	}
}
