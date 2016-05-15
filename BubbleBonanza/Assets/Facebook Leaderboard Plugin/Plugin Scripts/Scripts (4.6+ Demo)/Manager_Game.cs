using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// ---------Manager_Game----------

// USED IN DEMO FOR UNITY 4.6+

// THIS SCRIPT IS THE MASTER 'GAMEPLAY CONTROLLER' FOR THIS DEMO
// INTEGRATE THIS SCRIPT TO YOUR GAME MANAGER/SCRIPT

public class Manager_Game : MonoBehaviour
{
		// DATA
		public int score = 0;				// Initial score
		public int FBUserscore;				// Facebook: for offline score sync; when user logged in later
		List<Facebook.LeaderboardEntry> mLeaderboard = null;
		Facebook.LeaderboardEntry currentNext = null;			

		// UI
		public GameObject ScoreCount;		// UI: score counters and dynamic rank display
		public GameObject Leaderboard;		// UI: Leaderboard
		public Text ScoreText;				// Main score counter (player)
		public Text nextPlayerText;			// Score display for next higher scored friend (Dynamic Rank)
		public Image nextPlayer;			// Profile picture of next higher scored friend (Dynamic Rank)



		// START GAME
		void Start ()
		{
				SaveManager.CreateAndLoadData ();
		
				if (FacebookManager.Instance.LoggedIn) {

						// Facebook: To check offline score from previous offline sessions
						// Used to sync score with Facebook later
						FacebookManager.Instance.RequestScores (
						delegate(List<Facebook.LeaderboardEntry> leaderboard) {
								foreach (Facebook.LeaderboardEntry entry in leaderboard) {
										if (entry.Id == FacebookManager.Instance.UserID) {
												FBUserscore = entry.Score;
										}
								}
								mLeaderboard = leaderboard;

								// Facebook: Dynamic next higher ranked friend display
								// Trigger the method/function to display player's friend who scored higher than current score
//								ClosestOpponent ();
						},
						delegate(string reason) {
						}
						);
				} else {
						// If not logged in, disable dynamic ranking
//						HideNextPlayer ();
				}
		}


		// SCORE TRIGGER
		public void AddOnePoint ()
		{
				// Mock 'gameplay': add score when screen is tapped. For illustration purposes only.
				// Replace this with whatever your App/Game trigger scorings. ie. when jumped over fire, etc.
				score++;
				ScoreText.text = "" + score;

				// Facebook: Dynamic next higher ranked friend display
				ClosestOpponent ();
		}

		// Facebook: Dynamic next higher ranked friend display
		// NO EDITING REQUIRED
		void ClosestOpponent ()
		{
				if (!FacebookManager.Instance.LoggedIn || mLeaderboard == null) {
						HideNextPlayer ();
						return;
				}
				ShowNextPlayer ();
				Facebook.LeaderboardEntry foundEntry = null;
				foreach (Facebook.LeaderboardEntry lbEntry in mLeaderboard) {
						if (lbEntry.Score > score) {
								if (foundEntry != null) {
										if (lbEntry.Score < foundEntry.Score) {
												foundEntry = lbEntry;
										}
								} else {
										foundEntry = lbEntry;
								}
						}
				}
				if (foundEntry == null) {
						HideNextPlayer ();
						return;
				}
				if (foundEntry != currentNext) {
						nextPlayerText.text = foundEntry.Score.ToString ();
						FacebookManager.Instance.RequestPicture (foundEntry.Id, 64, 64, false,
                		delegate(Texture2D texture) {
								nextPlayer.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
						},
                		delegate(string reason) {
						});
						currentNext = foundEntry;
				}
		}

	void Update()
	{
		if (SceneManager.GetActiveScene ().buildIndex == 1) 
		{
			score = Mathf.RoundToInt (GameManager.instance.playerScore);
		}
	}

		// Continue from above:
		// Facebook: Dynamic next higher ranked friend display
		void HideNextPlayer ()
		{
				nextPlayer.gameObject.SetActive (false);
				nextPlayerText.gameObject.SetActive (false);
		}

		// Continue from above:
		// Facebook: Dynamic next higher ranked friend display
		void ShowNextPlayer ()
		{
				nextPlayer.gameObject.SetActive (true);
				nextPlayerText.gameObject.SetActive (true);
		}


		// END GAME
		public void Die ()
		{
//			float tempScore = PlayerPrefs.GetFloat("highscore");
//			score = Mathf.RoundToInt(tempScore);

			ScoreCount.SetActive (false);			// Disable ScoreCount UI
			Leaderboard.SetActive (true);			// Call leaderboard
		}
}

