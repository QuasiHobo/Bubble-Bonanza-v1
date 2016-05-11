using UnityEngine;
using System.Collections;

public class OrientationFixScript : MonoBehaviour 
{
	void Awake()
	{
		Screen.orientation = ScreenOrientation.Landscape;
		Screen.orientation = ScreenOrientation.Portrait;
	}
}
