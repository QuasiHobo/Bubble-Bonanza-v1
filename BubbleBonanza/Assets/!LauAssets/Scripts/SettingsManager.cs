using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class SettingsManager : MonoBehaviour {

	public Button areYouSureYes;
	public Button areYouSureNO; 
	public Button startGameButton;
	public Button rulesButton;

	// Use this for initialization
	void Start () 
	{
		if(startGameButton != null)
			startGameButton.gameObject.SetActive(true);
		if(rulesButton != null)
			rulesButton.gameObject.SetActive(true);

		areYouSureYes.gameObject.SetActive(false);
		areYouSureNO.gameObject.SetActive(false);
	}

	public void quitPressed()
	{
		areYouSureYes.gameObject.SetActive(true);
		areYouSureNO.gameObject.SetActive(true);

		if(startGameButton != null)
		{
		startGameButton.gameObject.SetActive(false);
			rulesButton.gameObject.SetActive(false);
		}
		if(rulesButton != null)
			rulesButton.gameObject.SetActive(false);
	}

	public void PlayerIsNotSure()
	{
		areYouSureYes.gameObject.SetActive(false);
		areYouSureNO.gameObject.SetActive(false);

		if(startGameButton != null)
		startGameButton.gameObject.SetActive(true);
		if(rulesButton != null)
			rulesButton.gameObject.SetActive(true);
	}

	public void PlayerIsSure()
	{
		PlayerPrefs.Save(); 
		//StartCoroutine(ShowAdWhenReady());

		if(Application.loadedLevel == 0)
			Application.Quit();
		if(Application.loadedLevel == 1)
			Application.LoadLevel(0);
		if(Application.loadedLevel == 2)
			Application.LoadLevel(0);
	}

	IEnumerator ShowAdWhenReady()
	{
		int randomValue = Random.Range(1, 10);
		if(randomValue == 1 && Application.loadedLevel != 0)
		{
			while(!Advertisement.IsReady())
				yield return null;
			
			Advertisement.Show();
		}
		
		yield return null;
	}	

}
