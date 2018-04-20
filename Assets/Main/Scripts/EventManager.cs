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

	public static event Action<int> OnEnterTramComplete;
	public static void EnterTramCompleteEvent(int passengerIndex)
	{
		OnEnterTramComplete(passengerIndex);
	}

	public static event Action<int> OnPassengerTimeout;
	public static void PassengerTimeoutEvent(int passengerIndex)
	{
		OnPassengerTimeout(passengerIndex);
	}
}
