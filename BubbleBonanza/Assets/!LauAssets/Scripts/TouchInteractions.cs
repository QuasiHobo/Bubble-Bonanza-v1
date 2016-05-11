using UnityEngine;
using System.Collections;

public class TouchInteractions : MonoBehaviour 
{

	void Update () 
	{
		if(GameManager.instance.gamePlaying == true)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);

					if(hitInfo.transform.gameObject.tag == "Bubble_Blue" || hitInfo.transform.gameObject.tag == "Bubble_Green" || hitInfo.transform.gameObject.tag == "Bubble_Red" || hitInfo.transform.gameObject.tag == "Bubble_Brown" || hitInfo.transform.gameObject.tag == "Bubble_Purple" || hitInfo.transform.gameObject.tag == "Bubble_Special")
					{
						GameManager.instance.levelTimer += 1f;
					}
//					GameManager.instance.levelTimer += 1f;
			}

			if (Input.GetMouseButton (0)) 
			{
				GameManager.instance.mouseBeeingHold = true;

				Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);

				if(hitInfo)
				{
					Debug.Log( hitInfo.transform.gameObject.name );

					if(hitInfo.transform.gameObject.tag == "Bubble_Blue")
					{
						hitInfo.transform.gameObject.GetComponent<BubbleController>().StartCoroutine("BubblePopped");

					}
					if(hitInfo.transform.gameObject.tag == "Bubble_Green")
					{
						hitInfo.transform.gameObject.GetComponent<BubbleController>().StartCoroutine("BubblePopped");
						
					}
					if(hitInfo.transform.gameObject.tag == "Bubble_Red")
					{
						hitInfo.transform.gameObject.GetComponent<BubbleController>().StartCoroutine("BubblePopped");
					
					}
					if(hitInfo.transform.gameObject.tag == "Bubble_Brown")
					{
						hitInfo.transform.gameObject.GetComponent<BubbleController>().StartCoroutine("BrownBubblePopped");
						
					}
					if(hitInfo.transform.gameObject.tag == "Bubble_Purple")
					{
						hitInfo.transform.gameObject.GetComponent<BubbleController>().StartCoroutine("BubblePopped");
						
					}
					if(hitInfo.transform.gameObject.tag == "Bubble_Special")
					{
						hitInfo.transform.gameObject.GetComponent<BubbleController>().StartCoroutine("SpecialBubblePopped");
						
					}
				}

			}
			else
			{
				GameManager.instance.mouseBeeingHold = false;
			}

		}
	}

}
