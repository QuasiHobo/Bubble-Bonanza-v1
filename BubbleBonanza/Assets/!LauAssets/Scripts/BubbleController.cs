using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleController : MonoBehaviour {

	public Sprite freshBubble;
	public Sprite poppedBubble;

	AudioSource myAudioSource;

	AudioClip blueBubblePopped;
	AudioClip blueBubblePopped_2;

	AudioClip redBubblePopped;
	AudioClip greenBubblePopped;

	public ParticleSystem timeGain;
	public ParticleSystem timePenalty;

	public ParticleSystem bluePopped;
	public ParticleSystem redPopped;
	public ParticleSystem greenPopped;
	public ParticleSystem brownPopped;
	public ParticleSystem purplePopped;
	public ParticleSystem specialPopped;

	ParticleSystem bubbleGlow;

	private SpriteRenderer mySpriteRenderer;

	public string bubbleState;

	public string bubbleType;

	float bluePoints = 10;
	float greenPoints = 30;
	float redPoints = -500;
	float brownPoints = 15;
	float purplePoints = 250;
	float specialPoints = 500;

	public GameObject brownBlastRadius;

	float bubblePoints;
	AudioClip clipAudio;

	void Start () 
	{
		myAudioSource = this.gameObject.GetComponent<AudioSource>();

		if(this.gameObject.GetComponentInChildren<ParticleSystem>() != null)
		{
			bubbleGlow = this.gameObject.GetComponentInChildren<ParticleSystem>() as ParticleSystem;
		}

		timeGain = Resources.Load("ParticleSystems/TimeGain", typeof(ParticleSystem)) as ParticleSystem;
		timePenalty = Resources.Load("ParticleSystems/TimePenalty", typeof(ParticleSystem)) as ParticleSystem;

		bluePopped = Resources.Load("ParticleSystems/BubblePopped_Blue", typeof(ParticleSystem)) as ParticleSystem;
		redPopped = Resources.Load("ParticleSystems/BubblePopped_Red", typeof(ParticleSystem)) as ParticleSystem;
		greenPopped = Resources.Load("ParticleSystems/BubblePopped_Green", typeof(ParticleSystem)) as ParticleSystem;
		brownPopped = Resources.Load("ParticleSystems/BubblePopped_Brown", typeof(ParticleSystem)) as ParticleSystem;
		purplePopped = Resources.Load("ParticleSystems/BubblePopped_Purple", typeof(ParticleSystem)) as ParticleSystem;
		specialPopped = Resources.Load("ParticleSystems/BubblePopped_Special", typeof(ParticleSystem)) as ParticleSystem;

		brownBlastRadius = Resources.Load("Prefabs/BlastRadius", typeof(GameObject)) as GameObject;

		blueBubblePopped = Resources.Load("Sound/blueBubble_pop", typeof(AudioClip)) as AudioClip;
		blueBubblePopped_2 = Resources.Load("Sound/blueBubble_pop_2", typeof(AudioClip)) as AudioClip;

		bluePopped = Resources.Load("ParticleSystems/BubblePopped_Blue", typeof(ParticleSystem)) as ParticleSystem;

		bubbleType = this.gameObject.tag;
		bubbleState = "Fresh";
		mySpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
		mySpriteRenderer.sprite = freshBubble;

		if(this.gameObject.tag == "Bubble_Blue")
		{
			bubblePoints = bluePoints;
		}
		if(this.gameObject.tag == "Bubble_Green")
		{
			bubblePoints = greenPoints;
		}
		if(this.gameObject.tag == "Bubble_Red")
		{
			bubblePoints = redPoints;
		}
		if(this.gameObject.tag == "Bubble_Brown")
		{
			bubblePoints = brownPoints;
		}
		if(this.gameObject.tag == "Bubble_Purple")
		{
			bubblePoints = purplePoints;
		}
		if(this.gameObject.tag == "Bubble_Special")
		{
			bubblePoints = specialPoints;
		}
	}


	public IEnumerator BubblePopped()
	{
			if(bubbleState == "Fresh")
			{
				int randomValue = Random.Range (1, 3);
				Debug.Log("Random: "+randomValue);	
			          	
				if(randomValue == 1)
				clipAudio = blueBubblePopped;

				if(randomValue >= 2)
				clipAudio = blueBubblePopped_2;

			if(bubbleGlow != null)
			Destroy(bubbleGlow.gameObject);

			if(bubbleType == "Bubble_Blue")
				Instantiate(bluePopped, this.transform.position, Quaternion.identity);
			if(bubbleType == "Bubble_Red")
				Instantiate(redPopped, this.transform.position, Quaternion.identity);
			if(bubbleType == "Bubble_Green")
				Instantiate(greenPopped, this.transform.position, Quaternion.identity);
			if(bubbleType == "Bubble_Purple")
				Instantiate(purplePopped, this.transform.position, Quaternion.identity);
				
				myAudioSource.clip = clipAudio;
				myAudioSource.Play();

			if(bubbleType == "Bubble_Green")
			{
				Instantiate(timeGain, this.gameObject.transform.position, Quaternion.identity);
			}
			if(bubbleType == "Bubble_Red")
			{
				Instantiate(timePenalty, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+0.2f), Quaternion.identity);
			}
			if(bubbleType == "Bubble_Purple")
			{
				Instantiate(timeGain, this.gameObject.transform.position, Quaternion.identity);
			}

				mySpriteRenderer.sprite = poppedBubble;
				bubbleState = "popped";

				GameManager.instance.AddToScore(bubblePoints, bubbleType);
			}
		yield return null;
	}

	public IEnumerator SpecialBubblePopped()
	{
		if(bubbleState == "Fresh")
		{
		Instantiate(specialPopped, this.transform.position, Quaternion.identity);

		clipAudio = blueBubblePopped;
		myAudioSource.clip = clipAudio;
		myAudioSource.Play();

		mySpriteRenderer.sprite = poppedBubble;
		bubbleState = "popped";

		string rewardType = "";
		int randomNr = Random.Range(1,11);
		if(randomNr > 1)
		{
			rewardType = "TimeBonus";
		}
		if(randomNr == 1)
		{
			rewardType = "ScoreMultiplier";
		}

		GameManager.instance.StartCoroutine("SpecialBubblePopped", rewardType);
		}

		GameManager.instance.AddToScore(bubblePoints, bubbleType);

		yield return null;
	}

	public IEnumerator BrownBubblePopped()
	{
		if(bubbleState == "Fresh")
		{

			Instantiate(brownPopped, this.transform.position, Quaternion.identity);

			clipAudio = blueBubblePopped;
			myAudioSource.clip = clipAudio;
			myAudioSource.Play();

			mySpriteRenderer.sprite = poppedBubble;
			bubbleState = "popped";

			Instantiate(brownBlastRadius, this.gameObject.transform.position, Quaternion.identity);

			GameManager.instance.AddToScore(bubblePoints, bubbleType);
		}

		yield return null;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log("TRIGGERED!!!!!");
		if(bubbleType == "Bubble_Red")
			bubblePoints = bluePoints;

		bubblePoints *= GameManager.instance.blastRadiusBonus;

		if(other.gameObject.tag == "BrownBlast")
		{
			if(bubbleType == "Bubble_Blue" && bubbleState == "Fresh")
				StartCoroutine(BubblePopped());

			if(bubbleType == "Bubble_Red" && bubbleState == "Fresh")
				StartCoroutine(BubblePopped());

			if(bubbleType == "Bubble_Green" && bubbleState == "Fresh")
				StartCoroutine(BubblePopped());

			if(bubbleType == "Bubble_Purple" && bubbleState == "Fresh")
				StartCoroutine(BubblePopped());

			if(bubbleType == "Bubble_Special" && bubbleState == "Fresh")
				StartCoroutine(SpecialBubblePopped());

			if(bubbleType == "Bubble_Brown" && bubbleState == "Fresh")
				StartCoroutine(BrownBubblePopped());

		}
	}
	                               

}
