using UnityEngine;
using System.Collections;

// ---------Button_LoadScene----------

// USED IN DEMO FOR UNITY 4.6+

// THIS SCRIPT LOADS THE NEXT SCENE (LEVEL/MENU)
// BIND THIS TO A BUTTON IN START MENU, ETC.

public class Button_LoadScene : MonoBehaviour {

	// In the Unity Editor, change the string to the scene name. ie. Scene_MainMenu
	public string Scene;

	public void LoadNextScene() {
		Application.LoadLevel (Scene); 
	}
}
