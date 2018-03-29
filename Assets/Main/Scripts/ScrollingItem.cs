using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameScreen
{
	Driver,
	Passenger
}

public class ScrollingItem : MonoBehaviour, IPoolObject
{
	public float relativeSpeed = 1.0f;
	public GameScreen gameScreen;

	public GameObject Prefab { get; set; }

	public void Init ()
	{
		//reset code...
	}

	void Update ()
    {
		if(gameScreen == GameScreen.Driver)
		{
			float yTranslation = Time.deltaTime * -relativeSpeed * GameManager.inst.tramSpeed;
			transform.Translate(0, yTranslation, 0);
		}
		else
		{
			float xTranslation = Time.deltaTime * -relativeSpeed * GameManager.inst.tramSpeed;
			transform.Translate(xTranslation, 0, 0);
		}
	}
}