using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RateMePromtController : MonoBehaviour {

	public Button customRatePrompt;

	void OnEnable()
	{
		UniRate.Instance.OnPromptedForRating += new UniRate.OnPromptedForRatingDelegate(ShowRateMe);
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	public void ShowRateMe()
	{
		Debug.Log("RateMe");
		customRatePrompt.gameObject.SetActive(true);
	}

	public void RateNow()
	{
		UniRate.Instance.SendMessage("UniRateUserWantToRate");
		customRatePrompt.gameObject.SetActive(false);
	}
	public void RemindLater()
	{
		UniRate.Instance.SendMessage("UniRateUserWantRemind");
		customRatePrompt.gameObject.SetActive(false);
	}
	public void NoThanks()
	{
		UniRate.Instance.SendMessage("UniRateUserDeclinedPrompt");
		customRatePrompt.gameObject.SetActive(false);
	}
}
