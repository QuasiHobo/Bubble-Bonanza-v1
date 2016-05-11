using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// ---------Manager_Leaderboard----------

// USED IN DEMO FOR UNITY 4.6+

// THIS SCRIPT IS THE MASTER LEADERBOARD CONTROLLER
// SETUP THIS SCRIPT AS PER PROVIDED SAMPLE SCENE IN Assets/Demo (For Unity 4.6+)/Scene_Demo
// MOST OF THE TIME YOU WILL NOT NEED TO EDIT THIS SCRIPT

public class Manager_Leaderboard : MonoBehaviour
{
		// DATA
		public Manager_Game GC;
		public Text Score;				// Current score (stored locally at device)
		public Text ScoreBest;			// Best score (stored locally at device)
		List<Facebook.LeaderboardEntry> mLeaderboard = null;
		GameObject instantiatedEntry;
		private int entrycount = 0;
		public Text waitForLeaderBoard;
		// UI
		public GameObject prefabEntry;	// Drag empty prefab leaderboard entry here
		public Text pleaseLoginFB;

		// INITIALISE
		void Start ()
		{
				waitForLeaderBoard.gameObject.SetActive(false);
				pleaseLoginFB.gameObject.SetActive(false);
				Score.text = GC.ScoreText.text;
		
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

				// Activates scroll view with rankings
//				CallLeaderboard ();
				StartCoroutine("CallLeaderboard");
		}

		// DOWNLOAD FACEBOOK LEADERBOARD DATAS
		IEnumerator CallLeaderboard ()
		{

				if(!FacebookManager.Instance.LoggedIn) 
				{
					pleaseLoginFB.gameObject.SetActive(true);
						while(!FacebookManager.Instance.LoggedIn)
						{
							yield return null;
						}
					pleaseLoginFB.gameObject.SetActive(false);
				}

				yield return new WaitForSeconds(.8f);
				// Downloads data from Facebook servers
				// NO EDITING REQUIRED
				if (FacebookManager.Instance.LoggedIn) {
						FacebookManager.Instance.RequestScores (
						delegate(List<Facebook.LeaderboardEntry> leaderboard) {
								foreach (Facebook.LeaderboardEntry entry in leaderboard) {
										entry.RankLoc = entrycount;
										entrycount += 1;
										if (entry.Id == FacebookManager.Instance.UserID) {
												if (entry.Score < int.Parse (ScoreBest.text)) {
														entry.Score = int.Parse (ScoreBest.text);
												}
										}
								}
								mLeaderboard = leaderboard;
								foreach (Facebook.LeaderboardEntry entry in mLeaderboard) {
										Facebook.LeaderboardEntry lbEntry = entry;
										FacebookManager.Instance.RequestPicture (entry.Id, 64, 64, false,
					                                        delegate(Texture2D texture) {
												lbEntry.Picture = texture;
										},
						delegate(string reason) {

										}
										);
								}
						},
						delegate(string reason) {
						}
						);
						StartCoroutine ("LoadEntries");
				}
		}

		// PLOT FACEBOOK LEADERBOARD ON UI
		private IEnumerator LoadEntries ()
		{
				waitForLeaderBoard.gameObject.SetActive(true);
				yield return new WaitForSeconds (4.5f); // waits x seconds for data to be fully downloaded
				waitForLeaderBoard.gameObject.SetActive(false);
				// Assign and plot downloaded data on prefab LB entries
				if (mLeaderboard != null) {
						int rank = 0;
			
						foreach (Facebook.LeaderboardEntry entry in mLeaderboard) {
								rank += 1;

								// Create empty entries
								instantiatedEntry = Instantiate (prefabEntry, transform.position, transform.rotation) as GameObject; 
								instantiatedEntry.transform.SetParent (GameObject.Find ("Panel_ScrollView").transform, false);  
								Text[] EntryText = instantiatedEntry.GetComponentsInChildren<Text>();
								Image imgs = instantiatedEntry.GetComponentInChildren<Image>();
								
								// Draw texture:profile picture
								imgs.sprite = Sprite.Create (entry.Picture, new Rect (0, 0, entry.Picture.width, entry.Picture.height), new Vector2 (0.5f, 0.5f));
								
								// Update text:rank on created entry
								EntryText[0].text = "" + rank.ToString();

								// Update text:profile name on created entry
								EntryText[1].text = "" + entry.Name;

								// Update text:score on created entry
								EntryText[2].text = "" + entry.Score.ToString();

								// Rename each created entry
								instantiatedEntry.name = "Entry"+rank;
						}
				} else {
						Debug.LogError ("leaderboard still not loaded from FB servers. Edit WaitForSeconds above to longer period.");
				}
		}

}
