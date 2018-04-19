﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
	public static event Action<int> OnReputationChanged = delegate {};
	public static void ReputationChangedEvent(int newAmount)
	{
		OnReputationChanged(newAmount);
	}

	public static event Action<int> OnEnterTramComplete = delegate {};
	public static void EnterTramCompleteEvent(int passengerIndex)
	{
		OnEnterTramComplete(passengerIndex);
	}

	public static event Action<int> OnPassengerTimeout = delegate {};
	public static void PassengerTimeoutEvent(int passengerIndex)
	{
		OnPassengerTimeout(passengerIndex);
	}
}
