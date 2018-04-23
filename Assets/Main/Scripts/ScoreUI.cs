using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
	public Color clrNeutral;
	public Color clrHighlight;

	public Text txtScore;

	void OnEnable()
	{
		EventManager.OnScoreChanged += OnScoreChanged;
	}

	void OnDisable()
	{
		EventManager.OnScoreChanged -= OnScoreChanged;
	}

	void OnScoreChanged (int newAmount)
	{
		txtScore.text = newAmount.ToString();
		StartCoroutine(ScoreChangedCR());
	}

	IEnumerator ScoreChangedCR()
	{
		txtScore.color = clrHighlight;
		yield return new WaitForSeconds(0.12f);
		txtScore.color = clrNeutral;
	}
}
