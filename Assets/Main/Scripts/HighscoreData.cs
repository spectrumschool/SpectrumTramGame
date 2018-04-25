using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HighscoreEntryData : JSONData<HighscoreEntryData>, IComparable<HighscoreEntryData>
{
	public int score;
	public string sDateTime;

	public HighscoreEntryData (int score, DateTime dateTime)
	{
		this.score = score;
		this.sDateTime = dateTime.ToString();
	}

	public int CompareTo (HighscoreEntryData other)
	{
		return other.score.CompareTo(score);
	}
}

[Serializable]
public class HighscoreListData : JSONData<HighscoreListData>
{
	public List<HighscoreEntryData> highscores;

	public HighscoreListData ()
	{
		highscores = new List<HighscoreEntryData>();
		highscores.Add(new HighscoreEntryData(10,DateTime.Now));
		highscores.Add(new HighscoreEntryData(20,DateTime.Today));
	}
}
