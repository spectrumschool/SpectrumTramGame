using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationUI : MonoBehaviour
{
	void OnEnable()
	{
		EventManager.OnReputationChanged += OnReputationChanged;
	}

	void OnDisable()
	{
		EventManager.OnReputationChanged -= OnReputationChanged;
	}

	void OnReputationChanged (int newAmount)
	{
		Debug.Log("ReputationUI: "+newAmount);
	}
}
