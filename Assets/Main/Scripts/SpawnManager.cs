using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
	public ScrollingItem prefab;
	public int poolSize;

	List<GameObject> _spawnedItems;

	public GameObject SpawnItem(Vector2 pos)
	{
		GameObject item = ObjectPool.Instance.PopFromPool(prefab.gameObject,false,false,transform);
		item.transform.localPosition = pos;
		_spawnedItems.Add(item);
		return item;
	}

	void Awake()
	{
		_spawnedItems = new List<GameObject>();
		ObjectPool.Instance.AddToPool(prefab.gameObject,poolSize,transform);
	}

	void Start()
	{
		
	}

	void Update()
	{
		
		if(prefab.gameScreen == GameScreen.Driver)
		{
			for (int i = _spawnedItems.Count-1; i >= 0; --i)
			{
				if(_spawnedItems[i].transform.localPosition.y < -16.0f)
				{
					var item = _spawnedItems[i];
					ObjectPool.Instance.PushToPool(ref item);
					_spawnedItems.RemoveAt(i);
				}
			}
		}
		else
		{
			for (int i = _spawnedItems.Count-1; i >= 0; --i)
			{
				if(_spawnedItems[i].transform.localPosition.x < -24.0f)
				{
					var item = _spawnedItems[i];
					ObjectPool.Instance.PushToPool(ref item);
					_spawnedItems.RemoveAt(i);
				}
			}
		}
	}
}
