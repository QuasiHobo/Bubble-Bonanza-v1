using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManagerLau : MonoBehaviour {

	public Button hintEnableButton;
	public Button hintDisableButton;
	public Button rulesButton;
	public GameObject leaderBoard;

	public int hintsEnabled = 0;

	// Use this for initialization
	void Start () 
	{
		hintsEnabled = PlayerPrefs.GetInt("HintsCheck");
		leaderBoard.gameObject.SetActive (false);
		if(hintsEnabled == 0)
		{
			hintEnableButton.gameObject.SetActive(true);
			hintDisableButton.gameObject.SetActive(false);
		}
		if(hintsEnabled == 1)
		{
			hintEnableButton.gameObject.SetActive(false);
			hintDisableButton.gameObject.SetActive(true);
		}
	}

	public void StartGame()
	{
		Debug.Log("Start new game");
		Application.LoadLevel(1);
	}

	public void HintsEnabled()
	{
			hintsEnabled = 1;
			PlayerPrefs.SetInt("HintsCheck", hintsEnabled);
			PlayerPrefs.Save();
			hintEnableButton.gameObject.SetActive(false);
			hintDisableButton.gameObject.SetActive(true);
	}

	public void HintsDisabled()
	{
		Debug.Log("!!!!! WTF");
		hintsEnabled = 0;
		PlayerPrefs.SetInt("HintsCheck", hintsEnabled);
		PlayerPrefs.Save();
		hintEnableButton.gameObject.SetActive(true);
		hintDisableButton.gameObject.SetActive(false);
	}

	public void RulesButtonPressed()
	{
		Application.LoadLevel(2);
	}

	public void LeaderBoardButtonPressed()
	{
		leaderBoard.gameObject.SetActive (true);
	}

	public void LeaderBoardQuit()
	{
		leaderBoard.gameObject.SetActive (false);
	}

}

