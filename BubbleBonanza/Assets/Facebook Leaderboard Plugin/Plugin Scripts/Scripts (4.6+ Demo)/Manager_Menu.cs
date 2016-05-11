using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// ---------Manager_Menu----------

// USED IN DEMO FOR UNITY 4.6+

// THIS SCRIPT INITIALISES FACEBOOK DURING APP/GAME STARTUP
// INTEGRATE THIS SCRIPT TO YOUR MAIN MENU MANAGER/SCRIPT

public class Manager_Menu : MonoBehaviour
{
		// Initialise Facebook, and check if Facebook is logged in from previous session
		void Start ()
		{
				FacebookManager fbMgr = FacebookManager.Instance;

				if (!fbMgr.InitCalled) {
						fbMgr.Initialize (
                delegate() {
								// The following will occur when Facebook is logged in:
								// EDITABLE TO SUIT USAGE
								if (fbMgr.LoggedIn) {
										// Add what you want to happen when your App/Game detects Facebook logged in
								}
						});
				}
		}
}