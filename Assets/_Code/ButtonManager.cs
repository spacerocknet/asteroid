using UnityEngine;
using System.Collections;
using System;

public class ButtonManager : MonoBehaviour {

	Vector2 touchposition;
	RaycastHit2D hit;
	public LayerMask layermask;


	//Data from the old scene



	void Update () {
		if(Input.touchCount>0)
		{
			foreach(Touch touch in Input.touches)
			{
				if(touch.phase==TouchPhase.Began)
				{	
					touchposition=touch.position;
					touchended();
				}
			}
		}
		
		//For testing with the mouse
		if(Input.GetMouseButtonUp(0))
		{
			touchposition=Input.mousePosition;
			touchended();
		}
		
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			Application.LoadLevel(0);
		}

		if(mainmenu.timerstarted==true)
		{
			mainmenu.cachetotaltimefornewlife -=Time.deltaTime;
			mainmenu.ts=TimeSpan.FromSeconds(mainmenu.cachetotaltimefornewlife);
		}
	}

	
	
	void touchended()
	{
			hit=Physics2D.Raycast(camera.ScreenToWorldPoint(new Vector3(touchposition.x,touchposition.y,0)),Vector2.zero,Mathf.Infinity,layermask);
			if(hit.collider!=null)
					{
					if(hit.collider.gameObject.name=="button_power_up_01")
					{
					Debug.Log("Power Up 1 Works");
					}
					else if(hit.collider.gameObject.name=="button_power_up_02")
					{
					Debug.Log("Power Up 2 Works");
					}
					else if(hit.collider.gameObject.name=="button_power_up_03")
					{
					Debug.Log("Power Up 3 Works");
					}
					else if(hit.collider.gameObject.name=="button_power_up_04")
					{
					Debug.Log("Power Up 4 Works");	
					}
					else if(hit.collider.gameObject.name=="button_power_up_05")
					{
					Debug.Log("Power Up 5 Works");	
					}
				}

	}
}