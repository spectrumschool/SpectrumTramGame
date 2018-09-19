using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class HighscoreUI : MonoBehaviour
{
	static string BELL = "Bell";
	
	public HighscoreManager highscoreManager;
	public Text[] txtDaily;
	public Text[] txtAllTime;
	public Text[] txtPressAny;
	public Color clrNeutral = Color.white;
	public Color clrBack = Color.gray;
	public Color clrFill = Color.cyan;
	public Color clrHighlight = Color.magenta;
	public Color clrAllTime = Color.yellow;

	bool _canResetGame;

	void Start()
	{
		_canResetGame = false;
	}

	void OnEnable()
	{
		EventManager.OnGameOver += OnGameOver;
		EventManager.OnResetGame += OnResetGame;
	}

	void OnDisable()
	{
		EventManager.OnGameOver -= OnGameOver;
		EventManager.OnResetGame -= OnResetGame;
	}

	void Update()
	{
		if(_canResetGame && Input.GetButtonDown(BELL))
		{
			_canResetGame = false;
			ObjectPool.Instance.ReleasePool();
			ObjectPool.Instance.Reset();
			SceneManager.LoadScene(0);
//			EventManager.ResetGameEvent();
		}
	}

	void OnGameOver()
	{
		for (int i = 0; i < txtDaily.Length; i++) txtDaily[i].enabled = true;
		for (int i = 0; i < txtAllTime.Length; i++) txtAllTime[i].enabled = true;
		StartCoroutine(OnGameOverCR());
	}

	void OnResetGame()
	{
		for (int i = 0; i < txtDaily.Length; i++) { txtDaily[i].enabled = false; txtDaily[i].text = "";}
		for (int i = 0; i < txtAllTime.Length; i++) { txtAllTime[i].enabled = false; txtAllTime[i].text = "";}
		for (int i = 0; i < txtPressAny.Length; i++) { txtPressAny[i].enabled = false;}
	}

	IEnumerator OnGameOverCR()
	{
		//daily
		yield return new WaitForSeconds(1.0f);

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

			for (int j = 0; j < txtAllTime.Length; j++) txtAllTime[j].text = sb.ToString();
		}

		highscoreManager.SaveHighscores();

		yield return new WaitForSeconds(3.0f);
		for (int i = 0; i < txtPressAny.Length; i++) txtPressAny[i].enabled = true;
		_canResetGame = true;
	}

	public void Append(StringBuilder sb, Color color, string text)
	{
		sb.Append("<color=#"+ColorUtility.ToHtmlStringRGBA(color)+">"+text+"</color>");
	}
}
