using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// ---------Button_FBInvite----------

// USED IN DEMO FOR UNITY 4.6+

// THIS SCRIPT CALLS AN INVITE FRIEND DIALOG
// BIND THIS TO A BUTTON

public class Button_FBInvite : MonoBehaviour {

//	void Start ()
//	{
//		if (!FacebookManager.Instance.LoggedIn) {
//			// The following will occur when Facebook is not logged in
//			// EDITABLE TO SUIT USAGE
//			this.GetComponent <Button> ().interactable = false; // Disable button
//		}
//	}

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

	// TEXT EDITABLE
    public void FBInvitePressed()
    {
		FacebookManager.Instance.SendInvitations("Can you beat my highscore! Download for free!","Select friends to invite."); 
    }
}
