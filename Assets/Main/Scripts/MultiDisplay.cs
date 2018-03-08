﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDisplay : MonoBehaviour
{
	void Awake()
	{
		Debug.Log("displays connected: " + Display.displays.Length);

		// Display.displays[0] is the primary, default display and is always ON.
		// Check if additional displays are available and activate each.
		for (int i = 1; i < Display.displays.Length; i++)
		{
			Display.displays[i].Activate();
		}
	}
}
