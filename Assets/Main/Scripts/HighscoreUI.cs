using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

public class HighscoreUI : MonoBehaviour
{
	public HighscoreManager highscoreManager;
	public Text[] txtDaily;
	public Text[] txtAllTime;
	public Color clrNeutral = Color.white;
	public Color clrBack = Color.gray;
	public Color clrFill = Color.cyan;
	public Color clrHighlight = Color.magenta;
	public Color clrAllTime = Color.yellow;

	void Start()
	{
	}

	void OnEnable()
	{
		EventManager.OnGameOver -= OnGameOver;
		EventManager.OnGameOver += OnGameOver;
	}

	void OnDisable()
	{
		EventManager.OnGameOver -= OnGameOver;
	}

	void OnGameOver()
	{
		for (int i = 0; i < txtDaily.Length; i++) txtDaily[i].enabled = true;
		for (int i = 0; i < txtAllTime.Length; i++) txtAllTime[i].enabled = true;
		StartCoroutine(OnGameOverCR());
	}

	IEnumerator OnGameOverCR()
	{
		//daily
		yield return null;

		StringBuilder sb = new StringBuilder("");
		Append(sb,clrNeutral,"Highscores");
		Append(sb,clrFill,"  "+DateTime.Today.ToString("dd/MM/yyyy")+"\n\n");

		for (int i = 0; i < txtDaily.Length; i++) txtDaily[i].text = sb.ToString();
		for (int i = 0; i < highscoreManager.highscoreListDaily.highscores.Count; i++)
		{
			yield return new WaitForSeconds(.1f);

			int index = (i == highscoreManager.maxEntries) ? 0 : i+1;
			Color clrEntry = (i == highscoreManager.lastAddedIndexDaily) ? clrHighlight : clrNeutral;
			DateTime dt = DateTime.Parse(highscoreManager.highscoreListDaily.highscores[i].sDateTime);

			Append(sb,clrFill,	"[");
			Append(sb,clrEntry,	index.ToString("D2"));
			Append(sb,clrFill,	"] - ");
			Append(sb,clrEntry,	highscoreManager.highscoreListDaily.highscores[i].score.ToString("D4"));
			Append(sb,clrFill,	" - [");
			Append(sb,clrBack,	dt.ToString("HH:mm:ss"));
			Append(sb,clrFill,	"]\n");

			for (int j = 0; j < txtDaily.Length; j++) txtDaily[j].text = sb.ToString();
		}

		//all time
		yield return new WaitForSeconds(.1f);
		sb = new StringBuilder("");
		Append(sb,clrNeutral,"Highscores");
		Append(sb,clrAllTime,"  All-time\n\n");

		for (int i = 0; i < txtAllTime.Length; i++) txtAllTime[i].text = sb.ToString();
		for (int i = 0; i < highscoreManager.highscoreListMain.highscores.Count; i++)
		{
			yield return new WaitForSeconds(.1f);

			int index = (i == highscoreManager.maxEntries) ? 0 : i+1;
			Color clrEntry = (i == highscoreManager.lastAddedIndexMain) ? clrHighlight : clrNeutral;
			DateTime dt = DateTime.Parse(highscoreManager.highscoreListMain.highscores[i].sDateTime);

			Append(sb,clrFill,	"[");
			Append(sb,clrEntry,	index.ToString("D2"));
			Append(sb,clrFill,	"] - ");
			Append(sb,clrEntry,	highscoreManager.highscoreListMain.highscores[i].score.ToString("D4"));
			Append(sb,clrFill,	" - [");
			Append(sb,clrBack,	dt.ToString("dd/MM/yyyy"));
			Append(sb,clrFill,	"]\n");

			//-----------
//			if(i < highscoreManager.maxEntries)
//			{
//				sb.Append("<color=cyan>[</color>"+(i+1).ToString("D2")+"<color=cyan>] --- </color>");
//			}
//			else
//			{
//				sb.Append("<color=cyan>score --- </color>");
//			}
//			if(i == highscoreManager.lastAddedIndexMain) sb.Append("<color=magenta>");
//			sb.Append(highscoreManager.highscoreListMain.highscores[i].score.ToString("D4"));
//			if(i == highscoreManager.lastAddedIndexMain) sb.Append("</color>");
//			DateTime dt = DateTime.Parse(highscoreManager.highscoreListMain.highscores[i].sDateTime);
//			sb.Append("<color=cyan> --- [</color><color=grey>"+dt.ToString("dd/MM/yyyy")+"</color><color=cyan>]</color>");
//			sb.Append("\n");

			for (int j = 0; j < txtAllTime.Length; j++) txtAllTime[j].text = sb.ToString();
		}

		highscoreManager.SaveHighscores();
	}

	public void Hide()
	{
		for (int i = 0; i < txtDaily.Length; i++) txtDaily[i].enabled = false;
		for (int i = 0; i < txtAllTime.Length; i++) txtAllTime[i].enabled = false;
	}

	public void Append(StringBuilder sb, Color color, string text)
	{
		sb.Append("<color=#"+ColorUtility.ToHtmlStringRGBA(color)+">"+text+"</color>");
	}
}
