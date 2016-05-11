using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MockGiveResults : MonoBehaviour
{
	// Mock game
	public string Scene;
	public MockGameController GC;
	public TextMesh Score;
	public TextMesh ScoreBest;

	// ** Facebook
    public GameObject bragButton;
    public GameObject facebookButton;
	public GameObject EnableFB;

	// Restart mock game
	public void Restart ()
	{
		Application.LoadLevel (Scene);
	}

	// Check if Facebook is logged in when Leaderboard is called
    public void OnActivate()
    {
        if (FacebookManager.Instance.LoggedIn)
        {
            facebookButton.SetActive(false);	// EDITABLE TO SUIT USAGE
        }
        else
        {
            bragButton.SetActive(false);		// Hide 'Brag' button if FB is not logged in
        }
    }

	// 
	public void Fill ()
	{
				Score.text = GC.ScoreWhite.text;

				// Compare high score and current score
				if (SaveManager.GetBestScore () < int.Parse (Score.text)) {
						ScoreBest.text = Score.text;
						SaveManager.SetBestScore (int.Parse (ScoreBest.text));
				} else {
						ScoreBest.text = SaveManager.GetBestScore ().ToString ();
				}

				// Offline high scores are now synced when user logs in at later time
				if (GC.FBUserscore < int.Parse (ScoreBest.text)) {
						FacebookManager.Instance.PostScore (int.Parse (ScoreBest.text),
			                                    delegate() {
						},
						delegate(string reason) {
						});
				}
				EnableFB.SetActive (true);
		}

	// Move Leaderboard into main camera
	// Mock game only. In your real game, you can call up the leaderboard whichever way you choose:
		// GUI, NextGUI, etc
	void Update ()
	{
		transform.position = Vector3.Lerp (transform.position, Vector3.zero, Time.deltaTime * 5);
	}

}
