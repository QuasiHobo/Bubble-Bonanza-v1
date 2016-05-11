using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// ---------Button_FBLogin----------

// USED IN DEMO FOR UNITY 4.6+

// THIS SCRIPT LOGINS THE USER TO HIS/HER FACEBOOK ACC
// BIND THIS TO A BUTTON IN START MENU, ETC.

public class Button_FBLogin : MonoBehaviour
{

		void Start ()
		{
				if (FacebookManager.Instance.LoggedIn) {
						// The following will occur when Facebook is already logged in from previous gaming session:
						// EDITABLE TO SUIT USAGE
						this.GetComponent <Button> ().interactable = false; // Disable login button
				}
		}

		// On pressing the UI button, initiate Facebook login sequence:
		public void FBLoginButtonPressed ()
		{
				FacebookManager.Instance.Login ("public_profile,user_friends,publish_actions",
            	delegate() {
						// The following will occur when when Facebook is logged in successfully:
						// EDITABLE TO SUIT USAGE
						if (FacebookManager.Instance.LoggedIn) {
								this.GetComponent <Button> ().interactable = false; // Disable login button
						}
				},
            	delegate(string reason) {
				});
		}
}