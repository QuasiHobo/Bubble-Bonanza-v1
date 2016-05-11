using UnityEngine;
using System.Collections;

// ---------SaveManager----------

// USED IN BOTH DEMOS

// THIS SCRIPT MANAGES THE APP/GAME'S SAVE FILE LOCALLY ON THE DEVICE
// INTEGRATE THIS SCRIPT TO YOUR SAVE MANAGER/SCRIPT

// IN THIS EXAMPLE SCRIPT, 'BEST SCORE' IS USED AS THE SCORE REQUIRED FOR LEADERBOARD
// THIS 'BEST SCORE' IS STORED LOCALLY ON THE DEVICE VIA THIS SCRIPT

public class SaveManager : MonoBehaviour {

	static int bestScore = 0; //The best score the player has reached
	
	//If there is no save data, create it, else, load the existing (locally; not Facebook servers)
	public static void CreateAndLoadData() 
	{
		if (PlayerPrefs.HasKey("Best Score"))
			LoadData();
		else
			CreateData();
	}

	public static void CreateData()
	{
				PlayerPrefs.SetInt ("Best Score", 0);
	}

	static void LoadData()
	{
		bestScore 	= PlayerPrefs.GetInt("Best Score");
	}

	//Return data. ie. high score
	public static int GetBestScore()
	{
		return bestScore;
	}

	//Modifies and saves. ie. overwrite old score with new high score
	public static void SetBestScore(int score)
	{
		bestScore = score;
		PlayerPrefs.SetInt("Best Score", bestScore);
		PlayerPrefs.Save();
	}
}
