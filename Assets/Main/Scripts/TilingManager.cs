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

	void Update ()
	{
		if(spawnManager.prefab.gameScreen == GameScreen.Driver)
		{
			if(_lastItem.localPosition.y <= _nextPos)
			{
				_lastItem = spawnManager.SpawnItem(new Vector2(startingPos.x,_nextPos + spacing)).transform;
			}
		}
		else
		{
			if(_lastItem.localPosition.x <= _nextPos)
			{
				_lastItem = spawnManager.SpawnItem(new Vector2(_nextPos + spacing,startingPos.y)).transform;
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
			_nextPos = currPos - 2 * spacing;
		}
		else
		{
			float currPos = startingPos.x;
			while (currPos < 12.0f)
			{
				_lastItem = spawnManager.SpawnItem(new Vector2(currPos, startingPos.y)).transform;
				currPos += spacing;
			}
			_nextPos = currPos - 2 * spacing;
		}
	}
}
