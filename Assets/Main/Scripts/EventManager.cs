using System.Collections;
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

	public static event Action<int> OnScoreChanged = delegate {};
	public static void ScoreChangedEvent(int newAmount)
	{
		OnScoreChanged(newAmount);
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

	public static event Action<int,string> OnPassengerHitStop = delegate {};
	public static void PassengerHitStopEvent(int playerIndex, string haltenaam)
	{
		OnPassengerHitStop(playerIndex, haltenaam);
	}

	public static event Action<int> OnPassengerHitRails = delegate {};
	public static void PassengerHitRailsEvent(int playerIndex)
	{
		OnPassengerHitRails(playerIndex);
	}

	public static event Action OnGameOver = delegate {};
	public static void GameOverEvent()
	{
		OnGameOver();
	}

	public static event Action OnResetGame = delegate {};
	public static void ResetGameEvent()
	{
		OnResetGame();
	}

	public static event Action OnStartTram = delegate {};
	public static void StartTramEvent()
	{
		OnStartTram();
	}
}
