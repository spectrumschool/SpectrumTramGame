using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingManager : MonoBehaviour
{
	public SpawnManager spawnManager;
	public float spacing;
	public Vector2 startingPos;

	Transform _lastItem;
	float _nextPos;

	void Start ()
	{
		FillScreen();
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
		StartCoroutine(ResetGameCR());
	}

	IEnumerator ResetGameCR()
	{
		yield return null;
		FillScreen();
	}

	void LateUpdate ()
	{
		if(spawnManager.prefab.gameScreen == GameScreen.Driver)
		{
			if(_lastItem.localPosition.y <=  8.0f)
			{
				_lastItem = spawnManager.SpawnItem(new Vector2(startingPos.x,_lastItem.localPosition.y + spacing)).transform;
			}
		}
		else
		{
			if(_lastItem.localPosition.x <= 12.0f)
			{
				_lastItem = spawnManager.SpawnItem(new Vector2(_lastItem.localPosition.x + spacing,startingPos.y)).transform;
			}
		}


	}

	void FillScreen()
	{
		if(spawnManager.prefab.gameScreen == GameScreen.Driver)
		{
			float currPos = startingPos.y;
			while (currPos < 8.0f)
			{
				_lastItem = spawnManager.SpawnItem(new Vector2(startingPos.x,currPos)).transform;
				currPos += spacing;
			}
//			_nextPos = currPos - 2 * spacing;
		}
		else
		{
			float currPos = startingPos.x;
			while (currPos < 12.0f)
			{
				_lastItem = spawnManager.SpawnItem(new Vector2(currPos, startingPos.y)).transform;
				currPos += spacing;
			}
//			_nextPos = currPos - 2 * spacing;
		}
	}
}
