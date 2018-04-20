using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prime31.ZestKit;

public class PassengerInfo : MonoBehaviour
{
	public int playerIndex;
	public Text txtHalteNaam;
	public Text txtTimeLeft;

	Vector3 _upTarget;
	Vector3 _downTarget;
	int _timeLeft;
	float _timer;

	void Awake()
	{
		_upTarget = new Vector3(transform.position.x,12.0f,0);
		_downTarget = new Vector3(transform.position.x,0,0);
		transform.position = _upTarget;
		this.enabled = false;
	}

	void Start()
	{
	}

	void OnEnable()
	{
		EventManager.OnPassengerHitStop += OnPassengerHitStop;
	}

	void OnPassengerHitStop (int index, string haltenaam)
	{
		if(index == playerIndex)
		{
			if(haltenaam.Equals(txtHalteNaam.text))
			{
				//TODO: score add
			}
			else
			{
				AudioManager.inst.PlayScream();
			}
		}
	}

	void OnDisable()
	{
		EventManager.OnPassengerHitStop -= OnPassengerHitStop;
	}

	public void Show(int time, string haltenaam)
	{
		_timeLeft = time;
		txtTimeLeft.text = _timeLeft.ToString();
		txtTimeLeft.color = Color.black;

		txtHalteNaam.text = haltenaam;

		_timer = 0;
		transform.position = _upTarget;
		transform.ZKpositionTo(_downTarget,0.7f).setEaseType(EaseType.BounceOut).start();
		this.enabled = true;
	}

	public void Hide()
	{
		transform.ZKpositionTo(_upTarget).start();
	}

	void Update()
	{
		if(GameManager.inst.tramSpeed == 0) return;
		_timer += Time.deltaTime;
		if(_timer >= 1.0f)
		{
			_timer -= 1.0f;
			_timeLeft--;
			txtTimeLeft.text = _timeLeft.ToString();
			if(_timeLeft == 0)
			{
				EventManager.PassengerTimeoutEvent(playerIndex);
				this.enabled = false;
				StartCoroutine(OutOfTimeCR());
			}
		}
	}

	IEnumerator OutOfTimeCR()
	{
		
		bool toggle = true;
		for (int i = 0; i < 8; i++)
		{
			toggle = !toggle;
			txtTimeLeft.color = toggle ? Color.red : Color.black;
			yield return new WaitForSeconds(.12f);
		}
		yield return new WaitForSeconds(0.7f);
		Hide();

	}
}
