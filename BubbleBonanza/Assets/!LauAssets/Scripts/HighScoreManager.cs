using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour {

	public Text highScoreText;
	public float currentHighScore;

	// Use this for initialization
	void Start () 
	{
		currentHighScore = PlayerPrefs.GetFloat("highscore", 0);

		if(Application.loadedLevel == 0)
		highScoreText.text = ""+Mathf.Round(currentHighScore);

		PlayerPrefs.Save();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
