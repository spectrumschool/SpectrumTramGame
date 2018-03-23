using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
	public static event Action<int> OnReputationChanged;
	public static void ReputationChangedEvent(int newAmount)
	{
		OnReputationChanged(newAmount);
	}
}
