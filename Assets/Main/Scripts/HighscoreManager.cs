using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HighscoreManager : MonoBehaviour
{
	public int maxEntries = 10;
	public Environment.SpecialFolder specialFolder = Environment.SpecialFolder.MyDocuments;
	public string folderName = "SpecTram_Highscores";

	[HideInInspector]
	public HighscoreListData highscoreListMain;
	[HideInInspector]
	public HighscoreListData highscoreListDaily;

	[HideInInspector]
	public int lastAddedIndexMain;
	[HideInInspector]
	public int lastAddedIndexDaily;

	string _filePathMain;
	string _filePathToday;

	void OnEnable()
	{
		EventManager.OnGameOver += OnGameOver;
	}

	void OnDisable()
	{
		EventManager.OnGameOver -= OnGameOver;
	}

	void Start ()
	{
		LoadHighscores();
	}

	void LoadHighscores()
	{
		string folderPath = FileHelper.FolderPath(specialFolder, folderName);
		FileHelper.CreateFolder(folderPath);

		//main
		_filePathMain = FileHelper.FilePath(folderPath,"highscores_main","json");
		highscoreListMain = new HighscoreListData();
		if(FileHelper.FileExists(_filePathMain))
		{
			highscoreListMain.Load(FileHelper.ReadFile(_filePathMain));
		}
		else
		{
			FileHelper.WriteFile(_filePathMain, highscoreListMain.SaveToString());
		}

		//today
		_filePathToday = FileHelper.FilePath(folderPath,"highscores_"+DateTime.Today.ToString("yyyyMMdd"),"json");
		highscoreListDaily = new HighscoreListData();
		if(FileHelper.FileExists(_filePathToday))
		{
			highscoreListDaily.Load(FileHelper.ReadFile(_filePathToday));
		}
		else
		{
			FileHelper.WriteFile(_filePathToday, highscoreListDaily.SaveToString());
		}
	}

	void OnGameOver()
	{
		AddScore(new HighscoreEntryData(GameManager.inst.score,DateTime.Now));
	}

	public void AddScore(HighscoreEntryData entry)
	{
		TrimLists();
		highscoreListMain.highscores.Add(entry);
		highscoreListMain.highscores.Sort();
		lastAddedIndexMain = highscoreListMain.highscores.IndexOf(entry);
		if(lastAddedIndexMain != maxEntries) TrimList(highscoreListMain.highscores);

		highscoreListDaily.highscores.Add(entry);
		highscoreListDaily.highscores.Sort();
		lastAddedIndexDaily = highscoreListDaily.highscores.IndexOf(entry);
		if(lastAddedIndexDaily != maxEntries) TrimList(highscoreListDaily.highscores);
	}

	public void SaveHighscores()
	{
		TrimLists();
		FileHelper.WriteFile(_filePathMain, highscoreListMain.SaveToString());
		FileHelper.WriteFile(_filePathToday, highscoreListDaily.SaveToString());
	}

	void TrimLists()
	{
		TrimList(highscoreListMain.highscores);
		TrimList(highscoreListDaily.highscores);

//		while (highscoreListMain.highscores.Count > maxEntries)
//		{
//			highscoreListMain.highscores.RemoveAt(highscoreListMain.highscores.Count-1);
//		}
//		while (highscoreListDaily.highscores.Count > maxEntries)
//		{
//			highscoreListDaily.highscores.RemoveAt(highscoreListDaily.highscores.Count-1);
//		}
	}

	void TrimList(List<HighscoreEntryData> list)
	{
		while (list.Count > maxEntries)
		{
			list.RemoveAt(list.Count-1);
		}
	}
}
