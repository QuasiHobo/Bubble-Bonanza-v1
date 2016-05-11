using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;


public class AdsController : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
//		Advertisement.Initialize ("1019169", true);
		StartCoroutine(ShowAdWhenReady());
	}
	
	IEnumerator ShowAdWhenReady()
	{
		int randomValue = Random.Range(1, 4);
		if(randomValue == 1)
		{
			while(!Advertisement.IsReady())
				yield return null;

			Advertisement.Show();
		}

		yield return null;
	}	
}
