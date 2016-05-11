using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// ---------Button_FBPost----------

// USED IN DEMO FOR UNITY 4.6+

// THIS SCRIPT CALLS FB'S POSTTOWALL COMMAND
// BIND THIS TO A BUTTON
// TO EDIT THE TEXT AND IMAGE IN THE FB POST, GO TO FacebookManager.cs
// SEARCH Posttowall METHOD

public class Button_FBPost : MonoBehaviour {

	public Manager_Game gameController;

	void Start ()
	{
		StartCoroutine("WaitForLogin");
	}

	IEnumerator WaitForLogin()
	{
		if (!FacebookManager.Instance.LoggedIn) 
		{
			// The following will occur when Facebook is not logged in
			// EDITABLE TO SUIT USAGE
			this.GetComponent <Button> ().interactable = false; // Disable button
		}
		while(!FacebookManager.Instance.LoggedIn)
		{
			yield return null;
		}
		if (FacebookManager.Instance.LoggedIn) 
		{
			// The following will occur when Facebook is not logged in
			// EDITABLE TO SUIT USAGE
			this.GetComponent <Button> ().interactable = true; // Disable button
		}
	}

    public void FBPostButtonPressed()
    {
        FacebookManager.Instance.PostToWall(Mathf.RoundToInt(PlayerPrefs.GetFloat("highscore"))); 
    }

}
