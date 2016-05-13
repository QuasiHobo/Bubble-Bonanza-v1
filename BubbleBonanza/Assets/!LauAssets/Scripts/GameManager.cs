using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour {

	static GameManager _instance;
	
	static public bool isActive 
	{ 
		get 
		{ 
			return _instance != null; 
		} 
	}
	
	static public GameManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType(typeof(GameManager)) as GameManager;
				
				if (_instance == null)
				{
					GameObject go = new GameObject("_gamemanager");
					DontDestroyOnLoad(go);
					_instance = go.AddComponent<GameManager>();
				}
			}
			return _instance;
		}
	}

	public List<GameObject> levelGrid = new List<GameObject>();
	public List<GameObject> bubbleBlue = new List<GameObject>();
	public List<GameObject> bubbleRed = new List<GameObject>();
	public List<GameObject> bubbleGreen = new List<GameObject>();
	public List<GameObject> bubbleBrown = new List<GameObject>();
	public List<GameObject> bubblePurple = new List<GameObject>();
	public List<GameObject> bubbleSpecial = new List<GameObject>();

	public GameObject Bubble_Blue;
	public GameObject Bubble_Red;
	public GameObject Bubble_Green;
	public GameObject Bubble_Brown;
	public GameObject Bubble_Purple;
	public GameObject Bubble_Special;

	public GameObject level1;
	public GameObject level2;
	public GameObject level3;
	public GameObject level4;
	public GameObject level5;
	public GameObject level6;
	public GameObject level7;

	public ParticleSystem penaltySpawnParticles;
	public GameObject penaltyTimeSpawn;

	public ParticleSystem scoreGain;
	public ParticleSystem scorePenalty;

	public int levelChange1 = 2;
	public int levelChange2 = 10;
	public int levelChange3 = 15;
	public int levelChange4 = 25;
	public int levelChange5 = 35;
	public int levelChange6 = 50;

	public int levelRandomizer = 15;

	bool isSpawning = false;
	bool levelCreated = false;

	public float delayTime = 0.01f;
	public float delayTimeCleanUp = 0.1f;

	public bool gamePlaying = false;

	public float playerScore;

	public float baseTime;
	public float timer;
	public float levelTimer;

	public int bubblesLeft;

	public int greenTimeBonus;
	public int purpleTimeBonus;
	public int brownTimeBonus;
	public int blueBubbleTimeBonus;
	public int redTimePunishment;

	public int specialPossibility = 10;
	public int purplePossibility = 10;
	int savedPurplePossibility;

	public int blastRadiusBonus;

	public int startDifficulty;
	public int mainDifficulty;
	int savedStartDif;
	int savedMainDif;

	public int currentLevel;
	public int blueBubblesThisLevel;

	bool isDespawningBlueBubbles = false;
	bool isDespawningGreenBubbles = false;
	bool isDespawningRedBubbles = false;
	bool isDespawningBrownBubbles = false;
	bool isDespawningPurpleBubbles = false;
	bool isDespawningSpecialBubbles = false;

	public bool mouseBeeingHold;
	
	//GUI STUFF
	public Text timerText;
	public Button newGameButton;
	public Button nextLevelButton;
	public Text scoreText;
	public Text timeBonusText;
	public Text levelsCompleted;
	public Button rewardButton;
	public Text currentHighScore;
	public Button newHighScoreGrats;
	public Button tutorialScreen;
	public Text scoreMultiplierText;
	public Button scoreMultiplierGrats;
	public Text scoreMultiplierGratsText;
	public Button speedBonusButton;
	public Text speedBonusText;
	public Button timeGainBonusButton;
	public Text timeGainBonusText;
	public Button tutorialScreen2;
	public Button gameOverButton;

	public int timeGainBonusCollected;
	public int timeBonusAmount;
	public bool scoreMultiplierGratsPressed = false;

	int currentScoreMultiplier;
	public int everyMultiplierGain;

	public ParticleSystem newHighScoreEffect;
	bool newHighScorePressed;

	public int rewardPossibility = 10;
	int firstRunOfGame = 0;
	bool tutorialScreenPressed = false;
	bool tutorialScreen2Pressed = false;

	// LeaderBoard stuff
	public Manager_Game tempManager_Game;
	public Button leaderBoardButton;
	public bool leaderBoardButtonPressed;

	// Use this for initialization
	void Start () 
	{
//		PlayerPrefs.DeleteAll();
		speedBonusButton.gameObject.SetActive(false);
		gameOverButton.gameObject.SetActive(false);

		savedPurplePossibility = purplePossibility;

		scoreMultiplierGrats.gameObject.SetActive(false);
		scoreMultiplierGratsPressed = false;

		currentScoreMultiplier = 1;
		scoreMultiplierText.text = "x"+currentScoreMultiplier;

		tutorialScreen.gameObject.SetActive(false);
		tutorialScreen2.gameObject.SetActive(false);

		newHighScoreGrats.gameObject.SetActive(false);
		newHighScoreEffect.gameObject.SetActive(false);
		newHighScoreEffect.enableEmission = false;

		level1 = GameObject.FindGameObjectWithTag("level1");
		level2 = GameObject.FindGameObjectWithTag("level2");
		level3 = GameObject.FindGameObjectWithTag("level3");
		level4 = GameObject.FindGameObjectWithTag("level4");
		level5 = GameObject.FindGameObjectWithTag("level5");
		level6 = GameObject.FindGameObjectWithTag("level6");
		level7 = GameObject.FindGameObjectWithTag("level7");

		//init start level
		level1.SetActive(true);
		level2.SetActive(false);
		level3.SetActive(false);
		level4.SetActive(false);
		level5.SetActive(false);
		level6.SetActive(false);
		level7.SetActive(false);

		savedMainDif = mainDifficulty;
		savedStartDif = startDifficulty;

		timeGainBonusCollected = PlayerPrefs.GetInt("TimeBonusCollected", 0);
		timeGainBonusText.text = "x"+timeGainBonusCollected;
		if(timeGainBonusCollected > 0)
		{
			timeGainBonusButton.gameObject.GetComponent<Animation>().Play();
		}
		if(timeGainBonusCollected <= 0)
		{
			timeGainBonusCollected = 0;
			timeGainBonusButton.gameObject.GetComponent<Animation>().Stop();
		}

		currentHighScore.text = "Current Highscore: "+PlayerPrefs.GetFloat("highscore", 0);
		PlayerPrefs.Save();

		newHighScorePressed = false;

		penaltyTimeSpawn.gameObject.SetActive(false);
		blueBubblesThisLevel = 0;
		mouseBeeingHold = false;

		Bubble_Blue = Resources.Load("Prefabs/Bubble_Blue", typeof(GameObject)) as GameObject;
		Bubble_Red = Resources.Load("Prefabs/Bubble_Red", typeof(GameObject)) as GameObject;
		Bubble_Green = Resources.Load("Prefabs/Bubble_Green", typeof(GameObject)) as GameObject;
		Bubble_Brown = Resources.Load ("Prefabs/Bubble_Brown", typeof(GameObject)) as GameObject;
		Bubble_Purple = Resources.Load ("Prefabs/Bubble_Purple", typeof(GameObject)) as GameObject;
		Bubble_Special = Resources.Load ("Prefabs/Bubble_Special", typeof(GameObject)) as GameObject;

		penaltySpawnParticles = Resources.Load("ParticleSystems/TimePenaltySpawn", typeof(ParticleSystem)) as ParticleSystem;

		scoreGain = Resources.Load("ParticleSystems/ScoreGain", typeof(ParticleSystem)) as ParticleSystem;
		scorePenalty = Resources.Load("ParticleSystems/ScorePenalty", typeof(ParticleSystem)) as ParticleSystem;

		levelTimer = 0;
		timeBonusText.text = "Time Penalty: "+Mathf.Round(levelTimer);

		nextLevelButton.gameObject.SetActive(false);
		newGameButton.gameObject.SetActive(false);
		rewardButton.gameObject.SetActive(false);

		currentLevel = 0;
		levelsCompleted.text = "Levels Completed: "+currentLevel;

		bubblesLeft = 0;
		playerScore = 0.0f;
		scoreText.text = ""+playerScore;

		timer = baseTime;
		timerText.text = "Time Left: "+timer;

		firstRunOfGame = 0;
		StartCoroutine("InitGridUnits");
	}

	public void TutorialScreenWasPressed()
	{
		tutorialScreen.gameObject.SetActive(false);
		tutorialScreenPressed = true;
	}
	public void Tutorial2ScreenWasPressed()
	{
		tutorialScreen2.gameObject.SetActive(false);
		tutorialScreen2Pressed = true;
		newHighScoreEffect.enableEmission = false;
	}

	IEnumerator InitGridUnits()
	{
		levelGrid.AddRange(GameObject.FindGameObjectsWithTag("GridUnit"));
		yield return null;

		if(PlayerPrefs.GetInt("HintsCheck") == 0)
		{
		if(currentLevel == 0 && firstRunOfGame == 0)
		{
			tutorialScreen.gameObject.SetActive(true);
			newHighScoreEffect.gameObject.SetActive(true);
			newHighScoreEffect.enableEmission = true;
			while(!tutorialScreenPressed)
			{
				yield return null;
			}
			tutorialScreen2.gameObject.SetActive(true);
			while(!tutorialScreen2Pressed)
			{
				yield return null;
			}
			firstRunOfGame += 1;
		}
		}
		if(PlayerPrefs.GetInt("HintsCheck") == 1)
		{
			tutorialScreenPressed = true;
			tutorialScreen2Pressed = true;
		}
	}

	IEnumerator SpawnGridUnit(int index, float seconds)
	{
		yield return new WaitForSeconds (seconds);

		if(index == 0)
		{
			Instantiate(Bubble_Blue, levelGrid[index].transform.position, Quaternion.identity);
		}

		if(index > 0)
		{

		int randomValue;
		randomValue = Random.Range(1, startDifficulty);

		if(randomValue == 1)
		{
			Instantiate(Bubble_Red, levelGrid[index].transform.position, Quaternion.identity);
		}

		if(randomValue >= 2)
		{

			int newValue = Random.Range(1, mainDifficulty);

			if(newValue >= 11)
				Instantiate(Bubble_Blue, levelGrid[index].transform.position, Quaternion.identity);
			if(newValue == 1)
				Instantiate(Bubble_Green, levelGrid[index].transform.position, Quaternion.identity);
			if(newValue == 2)
				Instantiate(Bubble_Green, levelGrid[index].transform.position, Quaternion.identity);
			if(newValue == 3)
				Instantiate(Bubble_Green, levelGrid[index].transform.position, Quaternion.identity);
			if(newValue == 4)
			{
				int newRandom = Random.Range(1, 3);
				if(newRandom == 1)
				Instantiate(Bubble_Brown, levelGrid[index].transform.position, Quaternion.identity);
				if(newRandom >= 2)
				Instantiate(Bubble_Red, levelGrid[index].transform.position, Quaternion.identity);
			}

			if(newValue >= 5 && newValue <= 8)
				Instantiate(Bubble_Brown, levelGrid[index].transform.position, Quaternion.identity);
			if(newValue == 9)
			{
				int newRandom = Random.Range(1, specialPossibility);
				if(newRandom == 1)
				{
					Instantiate(Bubble_Special, levelGrid[index].transform.position, Quaternion.identity);
				}
				if(newRandom > 1)
				Instantiate(Bubble_Brown, levelGrid[index].transform.position, Quaternion.identity);
			}
			if(newValue == 10)
			{
					int newRandom = Random.Range(1, purplePossibility);
					if(newRandom == 1)
					Instantiate(Bubble_Purple, levelGrid[index].transform.position, Quaternion.identity);
					if(newRandom >= 2)
					Instantiate(Bubble_Green, levelGrid[index].transform.position, Quaternion.identity);
			}
		}

		}

		levelGrid.RemoveAt(index);

		isSpawning = false;

		yield return null;
	}

	IEnumerator DespawnBlueBubbles(int indexBlue, float seconds)
	{
		yield return new WaitForSeconds (seconds);
		GameObject tempObj = bubbleBlue[indexBlue].gameObject;

		if(bubbleBlue[indexBlue].gameObject.GetComponent<BubbleController>().bubbleState == "Fresh")
		{		
//			Instantiate(scorePenalty, tempObj.transform.position, Quaternion.identity);
//			playerScore -= 50*currentScoreMultiplier;
		}
		if(bubbleBlue[indexBlue].gameObject.GetComponent<BubbleController>().bubbleState == "popped")
		{	
			Instantiate(scoreGain, tempObj.transform.position, Quaternion.identity);
			playerScore += 50*currentScoreMultiplier;
			scoreText.gameObject.GetComponent<Animation>().Play();
		}

			Destroy(bubbleBlue[indexBlue].gameObject);
			bubbleBlue.RemoveAt(indexBlue);

		isDespawningBlueBubbles = false;

		yield return null;
	}
	IEnumerator DespawnGreenBubbles(int indexGreen, float seconds)
	{
		yield return new WaitForSeconds (seconds);
		GameObject tempObj = bubbleGreen[indexGreen].gameObject;

		if(bubbleGreen[indexGreen].gameObject.GetComponent<BubbleController>().bubbleState == "Fresh")
		{
//			Instantiate(scorePenalty, tempObj.transform.position, Quaternion.identity);
//			playerScore -= 225*currentScoreMultiplier;
		}
		if(bubbleGreen[indexGreen].gameObject.GetComponent<BubbleController>().bubbleState == "Popped")
		{
			Instantiate(scoreGain, tempObj.transform.position, Quaternion.identity);
			playerScore += 100*currentScoreMultiplier;
			scoreText.gameObject.GetComponent<Animation>().Play();
		}
		
		Destroy(bubbleGreen[indexGreen].gameObject);
		bubbleGreen.RemoveAt(indexGreen);
		
		isDespawningGreenBubbles = false;
		
		yield return null;
	}
	IEnumerator DespawnRedBubbles(int indexRed, float seconds)
	{
		yield return new WaitForSeconds (seconds);
		GameObject tempObj = bubbleRed[indexRed].gameObject;

		if(bubbleRed[indexRed].gameObject.GetComponent<BubbleController>().bubbleState == "popped")
		{
//			Instantiate(scorePenalty, tempObj.transform.position, Quaternion.identity);
//			playerScore -= 75*currentScoreMultiplier;
		}
		if(bubbleRed[indexRed].gameObject.GetComponent<BubbleController>().bubbleState == "Fresh")
		{
			Instantiate(scoreGain, tempObj.transform.position, Quaternion.identity);
			playerScore += 50*currentScoreMultiplier;
			scoreText.gameObject.GetComponent<Animation>().Play();
		}

		Destroy(bubbleRed[indexRed].gameObject);
		bubbleRed.RemoveAt(indexRed);
		
		isDespawningRedBubbles = false;
		
		yield return null;
	}

	IEnumerator DespawnBrownBubbles(int indexBrown, float seconds)
	{
		yield return new WaitForSeconds (seconds);
		GameObject tempObj = bubbleBrown[indexBrown].gameObject;
		
		if(bubbleBrown[indexBrown].gameObject.GetComponent<BubbleController>().bubbleState == "popped")
		{
			Instantiate(scoreGain, tempObj.transform.position, Quaternion.identity);
			playerScore += 100*currentScoreMultiplier;
			scoreText.gameObject.GetComponent<Animation>().Play();
		}
		if(bubbleBrown[indexBrown].gameObject.GetComponent<BubbleController>().bubbleState == "Fresh")
		{
			Instantiate(scoreGain, tempObj.transform.position, Quaternion.identity);
			playerScore += 50*currentScoreMultiplier;
			scoreText.gameObject.GetComponent<Animation>().Play();
		}
		
		Destroy(bubbleBrown[indexBrown].gameObject);
		bubbleBrown.RemoveAt(indexBrown);
		
		isDespawningBrownBubbles = false;
		
		yield return null;
	}

	IEnumerator DespawnPurpleBubbles(int indexPurple, float seconds)
	{
		yield return new WaitForSeconds (seconds);
		GameObject tempObj = bubblePurple[indexPurple].gameObject;
		
		if(bubblePurple[indexPurple].gameObject.GetComponent<BubbleController>().bubbleState == "popped")
		{
			Instantiate(scoreGain, tempObj.transform.position, Quaternion.identity);
			playerScore += 1500*currentScoreMultiplier;
			scoreText.gameObject.GetComponent<Animation>().Play();
		}
		if(bubblePurple[indexPurple].gameObject.GetComponent<BubbleController>().bubbleState == "Fresh")
		{
//			Instantiate(scorePenalty, tempObj.transform.position, Quaternion.identity);
//			playerScore -= 250;
		}
		
		Destroy(bubblePurple[indexPurple].gameObject);
		bubblePurple.RemoveAt(indexPurple);
		
		isDespawningPurpleBubbles = false;
		
		yield return null;
	}

	IEnumerator DespawnSpecialBubbles(int indexSpecial, float seconds)
	{
		yield return new WaitForSeconds (seconds);
		GameObject tempObj = bubbleSpecial[indexSpecial].gameObject;
		
		if(bubbleSpecial[indexSpecial].gameObject.GetComponent<BubbleController>().bubbleState == "popped")
		{
			Instantiate(scoreGain, tempObj.transform.position, Quaternion.identity);
			playerScore += 1500*currentScoreMultiplier;
			scoreText.gameObject.GetComponent<Animation>().Play();
		}
		if(bubbleSpecial[indexSpecial].gameObject.GetComponent<BubbleController>().bubbleState == "Fresh")
		{
			Instantiate(scorePenalty, tempObj.transform.position, Quaternion.identity);
			playerScore -= 250;
		}
		
		Destroy(bubbleSpecial[indexSpecial].gameObject);
		bubbleSpecial.RemoveAt(indexSpecial);
		
		isDespawningSpecialBubbles = false;
		
		yield return null;
	}

	IEnumerator IndexBubbles()
	{
		bubbleBlue.AddRange(GameObject.FindGameObjectsWithTag("Bubble_Blue"));
		bubbleRed.AddRange(GameObject.FindGameObjectsWithTag("Bubble_Red"));
		bubbleGreen.AddRange(GameObject.FindGameObjectsWithTag("Bubble_Green"));
		bubbleBrown.AddRange(GameObject.FindGameObjectsWithTag("Bubble_Brown"));
		bubblePurple.AddRange(GameObject.FindGameObjectsWithTag("Bubble_Purple"));
		bubbleSpecial.AddRange(GameObject.FindGameObjectsWithTag("Bubble_Special"));

		bubblesLeft = bubbleBlue.Count;
		StartCoroutine("GamePlayState");

		yield return null;
	}

	public void AddToScore(float points, string bubbleType)
	{
		float transferedPoints = points;
		playerScore += Mathf.Round(transferedPoints)*currentScoreMultiplier;
		scoreText.gameObject.GetComponent<Animation>().Play();

		if(bubbleType == "Bubble_Blue")
		{
			bubblesLeft -= 1;
			blueBubblesThisLevel +=1;
		}

		if(bubbleType == "Bubble_Red")
			timer -= redTimePunishment;
		if(bubbleType == "Bubble_Green")
			timer += greenTimeBonus;
		if(bubbleType == "Bubble_Brown")
			timer += brownTimeBonus;
		if(bubbleType == "Bubble_Purple")
			timer += purpleTimeBonus;

		if(bubbleType == "Bubble_Special")
		{
			Debug.Log("SPECIAL EVENT POPOPO!!!");
		}

		if(blueBubblesThisLevel == 10)
			timer += blueBubbleTimeBonus;
		if(blueBubblesThisLevel == 20)
			timer += blueBubbleTimeBonus;
		if(blueBubblesThisLevel == 30)
			timer += blueBubbleTimeBonus;
		if(blueBubblesThisLevel == 40)
			timer += blueBubbleTimeBonus;
		if(blueBubblesThisLevel == 50)
			timer += blueBubbleTimeBonus;
		if(blueBubblesThisLevel == 60)
			timer += blueBubbleTimeBonus;
		if(blueBubblesThisLevel == 70)
			timer += blueBubbleTimeBonus;
		if(blueBubblesThisLevel == 80)
			timer += blueBubbleTimeBonus;

	}

	IEnumerator GamePlayState()
	{
		gamePlaying = true;
		bool wonLevel = false;

		while(gamePlaying)
		{
			levelTimer += Time.deltaTime;

			if(timer > 0 && mouseBeeingHold)
			{
				levelTimer += Time.deltaTime;
//				timer -= Time.deltaTime;
			}
			if( timer <= 0)
			{
				gamePlaying = false;
				StartCoroutine(GameIsOver(wonLevel));
			}

			if(bubblesLeft == 0)
			{
				Debug.Log("WON LEVEL!");
				wonLevel = true;
				gamePlaying = false;
				StartCoroutine(GameIsOver(wonLevel));
			}

			timerText.text = "Time Left: "+Mathf.Round(timer);

			yield return null;
		}
		yield return null;
	}

	IEnumerator GameIsOver(bool wonLevel)
	{
		bool gameover = true;
		Debug.Log("GAME OVER!!!!");

		while(gameover)
		{
			if(!isDespawningBlueBubbles)
			{
				isDespawningBlueBubbles = true;
				int blueIndex = Random.Range(0, bubbleBlue.Count);

				if(bubbleBlue.Count != 0)
				{
					StartCoroutine(DespawnBlueBubbles(blueIndex, delayTimeCleanUp));
				}
				if(bubbleBlue.Count == 0)
				isDespawningBlueBubbles = false;
				yield return null;
			}
			if(!isDespawningGreenBubbles && !isDespawningBlueBubbles && bubbleBlue.Count == 0)
			{
				isDespawningGreenBubbles = true;
				int greenIndex = Random.Range(0, bubbleGreen.Count);
				
				if(bubbleGreen.Count != 0)
				{
					StartCoroutine(DespawnGreenBubbles(greenIndex, delayTimeCleanUp));
				}
				if(bubbleGreen.Count == 0)
					isDespawningGreenBubbles = false;
				yield return null;
			}
			if(!isDespawningRedBubbles && !isDespawningGreenBubbles && bubbleBlue.Count == 0 && bubbleGreen.Count == 0)
			{
				isDespawningRedBubbles = true;
				int redIndex = Random.Range(0, bubbleRed.Count);
				
				if(bubbleRed.Count != 0)
				{
					StartCoroutine(DespawnRedBubbles(redIndex, delayTimeCleanUp));
				}
				if(bubbleRed.Count == 0)
					isDespawningRedBubbles = false;
				yield return null;
			}
			if(!isDespawningBrownBubbles && !isDespawningRedBubbles && bubbleBlue.Count == 0 && bubbleGreen.Count == 0 && bubbleRed.Count == 0)
			{
				isDespawningBrownBubbles = true;
				int brownIndex = Random.Range(0, bubbleBrown.Count);
				
				if(bubbleBrown.Count != 0)
				{
					StartCoroutine(DespawnBrownBubbles(brownIndex, delayTimeCleanUp));
				}

				if(bubbleBrown.Count == 0)
					isDespawningBrownBubbles = false;
				yield return null;
			}
			if(!isDespawningPurpleBubbles && !isDespawningBrownBubbles && bubbleBlue.Count == 0 && bubbleGreen.Count == 0 && bubbleRed.Count == 0 && bubbleBrown.Count == 0)
			{
				isDespawningPurpleBubbles = true;
				int purpleIndex = Random.Range(0, bubblePurple.Count);
				
				if(bubblePurple.Count != 0)
				{
					StartCoroutine(DespawnPurpleBubbles(purpleIndex, delayTimeCleanUp));
				}
				
				if(bubblePurple.Count == 0)
					isDespawningPurpleBubbles = false;
				yield return null;
			}
			if(!isDespawningSpecialBubbles && !isDespawningPurpleBubbles && bubbleBlue.Count == 0 && bubbleGreen.Count == 0 && bubbleRed.Count == 0 && bubbleBrown.Count == 0 && bubblePurple.Count == 0)
			{
				isDespawningSpecialBubbles = true;
				int specialIndex = Random.Range(0, bubbleSpecial.Count);
				
				if(bubbleSpecial.Count != 0)
				{
					StartCoroutine(DespawnSpecialBubbles(specialIndex, delayTimeCleanUp));
				}
				
				if(bubbleSpecial.Count == 0)
					isDespawningSpecialBubbles = false;
				yield return null;
			}

			if(bubbleBlue.Count == 0 && bubbleGreen.Count == 0 && bubbleRed.Count == 0 && bubbleBrown.Count == 0 && bubblePurple.Count == 0 && bubbleSpecial.Count == 0 && timer <= 0)
			{
				yield return new WaitForSeconds(0.5f);
				gameover = false;
				isDespawningBlueBubbles = false;
				isDespawningRedBubbles = false;
				isDespawningGreenBubbles = false;
				isDespawningBrownBubbles = false;
				isDespawningPurpleBubbles = false;
				isDespawningSpecialBubbles = false;

				StartCoroutine("NewGameChoose");
			}

			if(bubbleBlue.Count == 0 && bubbleGreen.Count == 0 && bubbleRed.Count == 0 && bubbleBrown.Count == 0 && bubblePurple.Count == 0 && bubbleSpecial.Count == 0 && timer >= 0)
			{
				yield return new WaitForSeconds(0.5f);
				gameover = false;
				isDespawningBlueBubbles = false;
				isDespawningRedBubbles = false;
				isDespawningGreenBubbles = false;
				isDespawningBrownBubbles = false;
				isDespawningPurpleBubbles = false;
				isDespawningSpecialBubbles = false;

				StartCoroutine("NextLevelChoose");
			}
			yield return null;
		}
		yield return null;
	}

		public void ShowRewardedAd()
		{
			rewardButton.gameObject.SetActive(false);
			if (Advertisement.IsReady("rewardedVideo"))
			{
				var options = new ShowOptions { resultCallback = HandleShowResult };
				Advertisement.Show("rewardedVideo", options);
			}
		}
		
		private void HandleShowResult(ShowResult result)
		{
			switch (result)
			{
			case ShowResult.Finished:
				Debug.Log("The ad was successfully shown.");
				timer += 10f;
				break;
			case ShowResult.Skipped:
				Debug.Log("The ad was skipped before reaching the end.");
				break;
			case ShowResult.Failed:
				Debug.LogError("The ad failed to be shown.");
				break;
			}
		}


	IEnumerator NextLevelChoose()
	{
		float oldHighScore = PlayerPrefs.GetFloat("highscore", 0);
		if(playerScore > oldHighScore)
			PlayerPrefs.SetFloat("highscore", Mathf.Round(playerScore));
		PlayerPrefs.Save(); 

		bool stillWon = false;

		//SpeedBonus stuff
		float speedBonus = 0;
		float speedPenaltyNew = levelTimer;
		speedBonusButton.gameObject.SetActive(true);
		float speedBonusMultiplier = 0;
		float speedPenaltyMultiplier = 0;

		if(level1.activeSelf == true)
		{
			speedBonusMultiplier = 1f;
			speedPenaltyMultiplier = 2;
		}
		if(level2.activeSelf == true)
		{
			speedBonusMultiplier = 2.5f;
			speedPenaltyMultiplier = 3;
		}
		if(level3.activeSelf == true)
		{
			speedBonusMultiplier = 4.5f;
			speedPenaltyMultiplier = 4;
		}
		if(level4.activeSelf == true)
		{
			speedBonusMultiplier = 7f;
			speedPenaltyMultiplier = 5;
		}
		if(level5.activeSelf == true)
		{
			speedBonusMultiplier = 8f;
			speedPenaltyMultiplier = 6;
		}
		if(level6.activeSelf == true)
		{
			speedBonusMultiplier = 10f;
			speedPenaltyMultiplier = 6;
		}
		if(level7.activeSelf == true)
		{
			speedBonusMultiplier = 10f;
			speedPenaltyMultiplier = 6;
		}

		speedBonus = 1000*speedBonusMultiplier;
		speedBonusText.text = ""+speedBonus;

		yield return new WaitForSeconds(.1f);

			while(speedPenaltyNew > 0 && speedBonus > 0)
			{
				speedPenaltyNew -= Time.deltaTime*10;
				speedBonus -= 10*speedPenaltyMultiplier;
				speedBonusText.text = ""+speedBonus;
				speedBonusText.gameObject.GetComponent<Animation>().Play();
				yield return null;
			}
		if(speedBonus < 0)
		{
			speedBonus = 0;
			speedBonusText.text = ""+speedBonus;
		}
		yield return new WaitForSeconds(.25f);
		speedBonus *= currentScoreMultiplier;
		speedBonusText.text = ""+speedBonus;

		if(currentScoreMultiplier > 1)
		speedBonusText.gameObject.GetComponent<Animation>().Play();

		yield return new WaitForSeconds(.25f);

		scoreText.gameObject.GetComponent<Animation>().Play();
		playerScore += speedBonus;
		speedBonusButton.gameObject.SetActive(false);

		Instantiate(penaltySpawnParticles, penaltyTimeSpawn.gameObject.transform.position, Quaternion.identity);
		yield return new WaitForSeconds(0.3f);
		timerText.gameObject.GetComponent<Animation>().Play();
		timer -= levelTimer;

		levelTimer = 0;

		if(timer > 0)
			stillWon = true;
		if(timer <= 0)
			stillWon = false;

		if(stillWon == true && currentLevel == levelChange1)
		{
			level1.SetActive(false);
			level2.SetActive(true);
			level3.SetActive(false);
			level4.SetActive(false);
			level5.SetActive(false);
			level6.SetActive(false);
			level7.SetActive(false);
		}
		if(stillWon == true && currentLevel == levelChange2)
		{
			level1.SetActive(false);
			level2.SetActive(false);
			level3.SetActive(true);
			level4.SetActive(false);
			level5.SetActive(false);
			level6.SetActive(false);
			level7.SetActive(false);
		}
		if(stillWon == true && currentLevel == levelChange3)
		{
			level1.SetActive(false);
			level2.SetActive(false);
			level3.SetActive(false);
			level4.SetActive(true);
			level5.SetActive(false);
			level6.SetActive(false);
			level7.SetActive(false);
		}
		if(stillWon == true && currentLevel >= levelChange4 && currentLevel < levelChange5)
		{
			level1.SetActive(false);
			level2.SetActive(false);
			level3.SetActive(false);
			level4.SetActive(false);
			level5.SetActive(true);
			level6.SetActive(false);
			level7.SetActive(false);
		}
		if(stillWon == true && currentLevel >= levelChange5 && currentLevel < levelChange6)
		{
			level1.SetActive(false);
			level2.SetActive(false);
			level3.SetActive(false);
			level4.SetActive(false);
			level5.SetActive(false);
			level6.SetActive(true);
			level7.SetActive(false);
		}
		if(stillWon == true && currentLevel >= levelChange6)
		{
			level1.SetActive(false);
			level2.SetActive(false);
			level3.SetActive(false);
			level4.SetActive(false);
			level5.SetActive(false);
			level6.SetActive(false);
			level7.SetActive(true);
		}

		if(currentLevel >= levelChange4)
		{
			int randomValue2 = Random.Range(1, levelRandomizer);
			if(randomValue2 == 1)
			{
				level1.SetActive(true);
				level2.SetActive(false);
				level3.SetActive(false);
				level4.SetActive(false);
				level5.SetActive(false);
				level6.SetActive(false);
				level7.SetActive(false);
			}
			if(randomValue2 == 2)
			{
				level1.SetActive(false);
				level2.SetActive(true);
				level3.SetActive(false);
				level4.SetActive(false);
				level5.SetActive(false);
				level6.SetActive(false);
				level7.SetActive(false);
			}
			if(randomValue2 == 3)
			{
				level1.SetActive(false);
				level2.SetActive(false);
				level3.SetActive(true);
				level4.SetActive(false);
				level5.SetActive(false);
				level6.SetActive(false);
				level7.SetActive(false);
			}
			if(randomValue2 == 4)
			{
				level1.SetActive(false);
				level2.SetActive(false);
				level3.SetActive(false);
				level4.SetActive(true);
				level5.SetActive(false);
				level6.SetActive(false);
				level7.SetActive(false);
			}
			if(randomValue2 == 5)
			{
				level1.SetActive(false);
				level2.SetActive(false);
				level3.SetActive(false);
				level4.SetActive(false);
				level5.SetActive(true);
				level6.SetActive(false);
				level7.SetActive(false);
			}
			if(randomValue2 == 6)
			{
				level1.SetActive(false);
				level2.SetActive(false);
				level3.SetActive(false);
				level4.SetActive(false);
				level5.SetActive(false);
				level6.SetActive(true);
				level7.SetActive(false);
			}
		}

		if(timer > 0)
			stillWon = true;
		if(timer <= 0)
			stillWon = false;

		if(stillWon == true)
		{
			if((currentLevel + 1) % everyMultiplierGain == 0)
			{
				yield return new WaitForSeconds(.6f);
				currentScoreMultiplier += 1;
				scoreMultiplierText.text = "x"+currentScoreMultiplier;

				scoreMultiplierGrats.gameObject.SetActive(true);
				scoreMultiplierGratsText.text = "Congratulations! \r\n You completed "+everyMultiplierGain+" levels!";
				while(scoreMultiplierGratsPressed == false)
				{
					yield return null;
				}
				scoreMultiplierGratsPressed = false;

			}

			if(timer <= 30)
			{
				int randomValue = Random.Range(1, rewardPossibility);
				if(randomValue == 1)
				{
					rewardButton.gameObject.SetActive(true);
				}
				if(timer <= 8)
				{
					int randomValueAgain = Random.Range(1, 3);
					if(randomValueAgain == 1)
						rewardButton.gameObject.SetActive(true);
				}
			}


			nextLevelButton.gameObject.SetActive(true);
		}
		if(stillWon == false)
		StartCoroutine("NewGameChoose");

		yield return null;
	}

	public void NewHighScoreButtonPressed()
	{
		newHighScoreGrats.gameObject.SetActive(false);
		newHighScorePressed = true;
	}

	public void ScoreMultiplierGratsWasPressed()
	{
		scoreMultiplierGrats.gameObject.SetActive(false);
		scoreMultiplierGratsPressed = true;
	}
	
	IEnumerator NewGameChoose()
	{
		gameOverButton.gameObject.SetActive(true);
		gameOverButton.gameObject.GetComponent<Animation>().Play();

		yield return new WaitForSeconds (2f);

		int aRandomVal = Random.Range (0, 11);

		if (aRandomVal == 1) {
			Advertisement.Show ();
			while (Advertisement.isShowing) {
				yield return null;
			}
		}

		gameOverButton.gameObject.GetComponent<Animation>().Rewind();

		gameOverButton.gameObject.SetActive(false);

		rewardButton.gameObject.SetActive(false);

		if(timer <= -1)
		{
			timer = 0;
		}

		float oldHighScore = PlayerPrefs.GetFloat("highscore");
		yield return new WaitForSeconds(.1f);
		if(playerScore >= oldHighScore)
		{
			Debug.Log("HIGHSCORRRREEEED!!!");
			PlayerPrefs.SetFloat("highscore", Mathf.Round(playerScore));
			PlayerPrefs.Save(); 
			currentHighScore.text = "Current Highscore: "+PlayerPrefs.GetFloat("highscore");
			newHighScoreGrats.gameObject.SetActive(true);
			newHighScoreEffect.gameObject.SetActive(true);
			newHighScoreEffect.enableEmission = true;
			newHighScorePressed = false;

			yield return new WaitForSeconds(1f);
			
			while(!newHighScorePressed && newHighScoreGrats.gameObject.activeSelf == true)
			{
				yield return null;
			}
			
			newHighScoreEffect.enableEmission= false;
			newHighScorePressed = false;

			if (FacebookManager.Instance.LoggedIn) 
			{
			tempManager_Game.Die();
			newGameButton.gameObject.SetActive(false);
			leaderBoardButton.gameObject.SetActive(false);
			}
		}

		newHighScoreEffect.enableEmission= false;
		newHighScorePressed = false;

		//init start level again
		level1.SetActive(true);
		level2.SetActive(false);
		level3.SetActive(false);
		level4.SetActive(false);
		level5.SetActive(false);
		level6.SetActive(false);
		level7.SetActive(false);

		int randomValue = Random.Range(1, 10);

		if(randomValue == 1 && newHighScoreGrats.gameObject.activeSelf == false)
		{
				Advertisement.Show();
					while(Advertisement.isShowing)
					{
						yield return null;
					}
		}

		currentScoreMultiplier = 1;
		scoreMultiplierText.text = "x"+currentScoreMultiplier;

		levelTimer = 0;
		//Leaderboard stuff
		leaderBoardButton.gameObject.SetActive(true);
		newGameButton.gameObject.SetActive(true);
		yield return null;
	}

	public void LeaderBoardQuit()
	{
		Application.LoadLevel(1);
	}
	public void LeaderBoardPressed()
	{
		tempManager_Game.Die();
	}

	public void InititateNextLevel()
	{
		rewardButton.gameObject.SetActive(false);
		nextLevelButton.gameObject.SetActive(false);
		currentLevel += 1;
		levelsCompleted.gameObject.GetComponent<Animation>().Play();
		blueBubblesThisLevel = 0;
		purplePossibility = savedPurplePossibility;

		if(currentLevel >= 0 && currentLevel <= 3)
		{
			startDifficulty = Random.Range(4, 9);
			mainDifficulty = Random.Range(20, 32);
		}
		if(currentLevel >= 4 && currentLevel <= 10)
		{
			startDifficulty = Random.Range(4, 8);
			mainDifficulty = Random.Range(25, 38);
		}
		if(currentLevel >= 11 && currentLevel <= 20)
		{
			startDifficulty = Random.Range(3, 9);
			mainDifficulty = Random.Range(33, 43);
		}
		if(currentLevel >= 21 && currentLevel <= 30)
		{
			startDifficulty = Random.Range(3, 8);
			mainDifficulty = Random.Range(35, 51);
		}
		if(currentLevel >= 31 && currentLevel <= 40)
		{
			startDifficulty = Random.Range(3, 7);
			mainDifficulty = Random.Range(35, 53);
		}
		if(currentLevel >= 41 && currentLevel <= 60)
		{
			startDifficulty = Random.Range(2, 7);
			mainDifficulty = Random.Range(35, 54);
		}
		if(currentLevel >= 61 && currentLevel < 100)
		{
			startDifficulty = Random.Range(2, 6);
			mainDifficulty = Random.Range(35, 65);
		}
		if(currentLevel > 50)
		{
			purplePossibility = 12;
		}

		// Bonus levels:
		if(currentLevel >= 45)
		{
			int newRandom = Random.Range(1, 19);
			if(newRandom == 1)
			{
				purplePossibility = 15;
				startDifficulty = Random.Range(4, 14);
				mainDifficulty = Random.Range(18, 55);
			}
		}

		levelsCompleted.text = "Levels Completed: "+currentLevel;
		levelCreated = false;
		StartCoroutine("InitGridUnits");
	}

	public void InititateNewGame()
	{
		leaderBoardButton.gameObject.SetActive(false);
		purplePossibility = savedPurplePossibility;
		playerScore = 0;
		currentLevel = 0;
		blueBubblesThisLevel = 0;
		startDifficulty = savedStartDif;
		mainDifficulty = savedMainDif;

		levelsCompleted.text = "Levels Completed: "+currentLevel;
		newGameButton.gameObject.SetActive(false);
		timer = baseTime;
		levelCreated = false;
		StartCoroutine("InitGridUnits");
	}

	public void TimeBonusUsed()
	{
		if(gamePlaying)
		{
			if(timeGainBonusCollected > 0)
			{
				timer += timeBonusAmount;
				timeGainBonusCollected -= 1;
			}

			if(timeGainBonusCollected <= 0)
			{
				timeGainBonusButton.gameObject.GetComponent<Animation>().Stop();
				timeGainBonusCollected = 0;
			}

			PlayerPrefs.SetInt("TimeBonusCollected", timeGainBonusCollected);
			PlayerPrefs.Save(); 
			timeGainBonusText.text = "x"+timeGainBonusCollected;
		}
	}
	public IEnumerator SpecialBubblePopped(string rewardType)
	{
		if(rewardType == "TimeBonus")
		{
			timeGainBonusCollected += 1;
			PlayerPrefs.SetInt("TimeBonusCollected", timeGainBonusCollected);
			timeGainBonusText.text = "x"+timeGainBonusCollected;
			timeGainBonusButton.gameObject.GetComponent<Animation>().Play();
		}

		if(rewardType == "ScoreMultiplier")
		{
			yield return new WaitForSeconds(.25f);
			currentScoreMultiplier += 1;
			scoreMultiplierText.text = "x"+currentScoreMultiplier;
		}

		yield return null;
	}

	// Update is called once per frame
	void Update () 
	{
		if(tutorialScreenPressed && tutorialScreen2Pressed)
		{
		if(playerScore <= 0)
			playerScore = 0;

		scoreText.text = " "+Mathf.Round(playerScore);

		timerText.text = "Time Left: "+Mathf.Round(timer);
		timeBonusText.text = "Time Penalty: "+Mathf.Round(levelTimer);

		if(!isSpawning && !levelCreated && !gamePlaying)
		{
			isSpawning = true;
			int gridIndex = Random.Range(0, levelGrid.Count);

			if(levelGrid.Count != 0)
				StartCoroutine(SpawnGridUnit(gridIndex, delayTime));

			if(levelGrid.Count == 0)
			{
				levelCreated = true;
				Debug.Log("LEVEL CREATED");
				StartCoroutine("IndexBubbles");
				isSpawning = false;
			}
		}
		}
		//Difficulty Controller
//		if(currentLevel == 3)
//		{
//			startDifficulty -=
//		}

	}
}
