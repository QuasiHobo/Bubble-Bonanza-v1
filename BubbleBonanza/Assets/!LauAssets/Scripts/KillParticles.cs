using UnityEngine;
using System.Collections;

public class KillParticles : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(KillMe());
	}

	IEnumerator KillMe()
	{
		yield return new WaitForSeconds(5f);
		Destroy(this.gameObject);
	}
}
