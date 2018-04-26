using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.ZestKit;

public class TramDriver : MonoBehaviour
{
    public float speed = 1.0f;
    public float yPos = -2.0f;
	public float bounceAnimationDuration = 0.03f;
	public float bounceDistance = 0.5f;
	public SpriteRenderer sprtTramWhite;

	[HideInInspector]
	public int currentTrack;

    Vector3 _target;
	bool _inAnimation;

    void Start()
	{
		OnResetGame();
	}

	void OnEnable()
	{
		EventManager.OnResetGame += OnResetGame;
	}

	void OnDisable()
	{
		EventManager.OnResetGame -= OnResetGame;
	}

	void OnResetGame()
	{
		_inAnimation = false;
		sprtTramWhite.enabled = false;
		//move to starting track
		currentTrack = 2;
		ChangeTrack();
	}

	void Update ()
	{
		if (!_inAnimation && Input.GetKeyDown(KeyCode.LeftArrow) && currentTrack > 0)
        {
            //3 tests voorkan, midden , achter

            RaycastHit2D hitVoor = Physics2D.Raycast(transform.position + new Vector3(0, 1.5f, 0), Vector3.left, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, 2, 0), transform.position + new Vector3(0, 2, 0) + Vector3.left * 4, Color.magenta, 0.1f);
            bool bVoor = hitVoor.collider != null;

            RaycastHit2D hitMid = Physics2D.Raycast(transform.position + new Vector3(0, 0, 0), Vector3.left, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, 0, 0), transform.position + new Vector3(0, 0, 0) + Vector3.left * 4, Color.magenta, 0.1f);
            bool bMid = hitMid.collider != null;

            RaycastHit2D hitAchter = Physics2D.Raycast(transform.position + new Vector3(0, -1, 0), Vector3.left, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, -2, 0), transform.position + new Vector3(0, -2, 0) + Vector3.left * 4, Color.magenta, 0.1f);
            bool bAchter = hitAchter.collider != null;

            if (!bVoor && !bMid && !bAchter)
            {
                currentTrack = currentTrack - 1;
                ChangeTrack();
            }
            else
            {
				StartBounce(transform.localPosition + new Vector3(-bounceDistance, 0.0f, 0.0f));
			}
        }

       
        
		if(!_inAnimation && Input.GetKeyDown(KeyCode.RightArrow) && currentTrack < 3)
		{
            RaycastHit2D hitVoor = Physics2D.Raycast(transform.position + new Vector3(0, 1.5f, 0), Vector3.right, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, 2, 0), transform.position + new Vector3(0, 2, 0) + Vector3.right * 4, Color.magenta, 0.1f);
            bool bVoor = hitVoor.collider != null;

            RaycastHit2D hitMid = Physics2D.Raycast(transform.position + new Vector3(0, 0, 0), Vector3.right, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, 0, 0), transform.position + new Vector3(0, 0, 0) + Vector3.right * 4, Color.magenta, 0.1f);
            bool bMid = hitMid.collider != null;

            RaycastHit2D hitAchter = Physics2D.Raycast(transform.position + new Vector3(0, -1, 0), Vector3.right, 4);
            //Debug.DrawLine(transform.position + new Vector3(0, -2, 0), transform.position + new Vector3(0, -2, 0) + Vector3.right * 4, Color.magenta, 0.1f);
            bool bAchter = hitAchter.collider != null;

            if (!bVoor && !bMid && !bAchter)
            {
                currentTrack = currentTrack + 1;
                ChangeTrack();
            }
            else
            {
				StartBounce(transform.localPosition + new Vector3(bounceDistance, 0.0f, 0.0f));
            }
        }

		if(!_inAnimation)
		{
        	float step = speed * Time.deltaTime;
        	transform.localPosition = Vector3.MoveTowards(transform.localPosition, _target, step);
		}


    }

	void StartBounce(Vector3 bounceTarget)
	{
		StartCoroutine(BounceCR(bounceTarget));
		//_inAnimation = true;
		//sprtTramWhite.enabled = true;
		//transform.ZKlocalPositionTo(bounceTarget,bounceAnimationDuration).setLoops(LoopType.PingPong).setLoopCompletionHandler(OnBounceComplete).setEaseType(EaseType.Linear).start();
	}

	void OnBounceComplete(ITween<Vector3> tween)
	{
		_inAnimation = false;
		sprtTramWhite.enabled = false;
	}

	void ChangeTrack()
	{
		_target = new Vector3(GameManager.inst.tracks[currentTrack].localPosition.x, yPos, 0f);
        
	}

	IEnumerator BounceCR(Vector3 bounceTarget)
	{
		_inAnimation = true;
		sprtTramWhite.enabled = true;
		transform.localPosition = bounceTarget;
		yield return new WaitForSeconds(bounceAnimationDuration);
		_inAnimation = false;
		sprtTramWhite.enabled = false;
		transform.localPosition = _target;

		//transform.ZKlocalPositionTo(bounceTarget,bounceAnimationDuration).setLoops(LoopType.PingPong).setLoopCompletionHandler(OnBounceComplete).setEaseType(EaseType.Linear).start();
	}
}
