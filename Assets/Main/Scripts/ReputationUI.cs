using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationUI : MonoBehaviour
{
	public Image[] bars;
	public Color clrActive;
	public Color clrDisabled;

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
		for (int i = 0; i < bars.Length; i++)
		{
			Color color = clrDisabled;
			if (i < newAmount)
				color = clrActive;
			
			bars[i].color = color;
		}
	}
}
