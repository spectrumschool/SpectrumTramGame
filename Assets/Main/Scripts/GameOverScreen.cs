using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prime31.ZestKit;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
	public Image imgFade;
	public Text txtTitle;
	string _originalText;

	void Awake()
	{
		_originalText = txtTitle.text;
	}

	void Start ()
	{
		imgFade.enabled = false;
		txtTitle.enabled = false;
	}

	void OnEnable()
	{
		EventManager.OnGameOver += OnGameOver;
	}

	void OnDisable()
	{
		EventManager.OnGameOver -= OnGameOver;
	}

	void OnGameOver()
	{
		StartCoroutine(GameOverCR());
	}

	IEnumerator GameOverCR()
	{
		txtTitle.text = "";

		imgFade.enabled = true;
		txtTitle.enabled = true;
		imgFade.color = new Color(imgFade.color.r,imgFade.color.g,imgFade.color.b,0);
		imgFade.ZKalphaTo(1,1.0f).start();

		yield return new WaitForSeconds(.5f);

		for (int i = 0; i < _originalText.Length; i++)
		{
			txtTitle.text = _originalText.Substring(0,i+1);
			yield return new WaitForSeconds(.12f);
		}

		yield return new WaitForSeconds(3.0f);

//		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
