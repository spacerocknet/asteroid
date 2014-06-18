using UnityEngine;
using System.Collections;
using System;

public class ButtonManager : MonoBehaviour {

	Vector2 touchposition;
	RaycastHit2D hit;
	public LayerMask layermask;
	public static GameObject attack_target;
	public GameObject MAIN;
	//Data from the old scene

	public static string powerupselected;

	public static GameObject bombtextmesh;
	public static GameObject doubleblastradiustextmesh;
	public static GameObject reversetimetextmesh;
	public static GameObject changequestioncategoriestextmesh;

	public GameObject topwall;

	void Awake()
	{
		bombtextmesh=GameObject.Find("bomb_textmesh");
		doubleblastradiustextmesh=GameObject.Find("doubleblastradius_textmesh");
		reversetimetextmesh=GameObject.Find("reversetime_textmesh");
		changequestioncategoriestextmesh=GameObject.Find("changecategory_textmesh");
		attack_target=GameObject.Find("ATTACK_TARGET");
		powerupselected=String.Empty;
		bombtextmesh.GetComponent<MeshRenderer>().sortingLayerID=1;
		doubleblastradiustextmesh.GetComponent<MeshRenderer>().sortingLayerID=1;
		reversetimetextmesh.GetComponent<MeshRenderer>().sortingLayerID=1;
		changequestioncategoriestextmesh.GetComponent<MeshRenderer>().sortingLayerID=1;
		bombtextmesh.GetComponent<TextMesh>().text=mainmenu.bombpowerupcount.ToString();
		doubleblastradiustextmesh.GetComponent<TextMesh>().text=mainmenu.doublebastradiuspowerupcount.ToString();
		reversetimetextmesh.GetComponent<TextMesh>().text=mainmenu.reversetimepowerupcount.ToString();
		changequestioncategoriestextmesh.GetComponent<TextMesh>().text=mainmenu.changequestioncategorypowerupcount.ToString();
		Invoke("removetopwallcollider",1.65f);
	}

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

		if(mainmenu.ts.TotalSeconds<=0)
		{	
			if(mainmenu.timerstarted==true)
			{
				mainmenu.resettimerfornewlife();
				mainmenu.totallives++;
				mainmenu.managetimerfornewlife(false);
				PlayerPrefs.SetInt("totallives",mainmenu.totallives);
				Debug.Log("Calling this");
				//resettimer();
				if(mainmenu.totallives<5)
				{
					mainmenu.managetimerfornewlife(true);
				}
				else
				{
					mainmenu.managetimerfornewlife(false);
				}
			}
		}

	}

	
	
	void touchended()
	{
			hit=Physics2D.Raycast(camera.ScreenToWorldPoint(new Vector3(touchposition.x,touchposition.y,0)),Vector2.zero,Mathf.Infinity,layermask);
			if(hit.collider!=null)
					{
					if(hit.collider.gameObject.name=="button_power_up_01")
					{
					//Rock Paper Scissors Logic to be placed here
					}
					else if(hit.collider.gameObject.name=="button_power_up_02")
					{
						if(mainmenu.bombpowerupcount>0)
						{
							if(powerupselected=="double_blast_radius")
								{
								attack_target.transform.localScale=new Vector3(1.2f,1.2f,0f);
								}
							powerupselected="bomb";
							}
						else
							{
							Debug.Log("Bomb powerup is finished");
							}
						}
					else if(hit.collider.gameObject.name=="button_power_up_03")
					{
						if(mainmenu.doublebastradiuspowerupcount>0)
							{
							powerupselected="double_blast_radius";
							attack_target.transform.localScale=new Vector3(2.4f,2.4f,0f);
							}
						else
							{
							Debug.Log("Double Blast radius powerup is finished");
							}
					}
					else if(hit.collider.gameObject.name=="button_power_up_04")
					{
						if(mainmenu.reversetimepowerupcount>0)
						{
							if(powerupselected=="double_blast_radius")
							{
							attack_target.transform.localScale=new Vector3(1.2f,1.2f,0f);
							}
							powerupselected="reverse_time";
							MAIN.GetComponent<BattleEngine>().asteroids.StartCoroutine("ReverseTimePowerUp");
							reducepowerupcount(powerupselected);
						}
						else
							{
							Debug.Log("ReverseTimePowerUp is Finsihed");
							}
					}
					else if(hit.collider.gameObject.name=="button_power_up_05")
					{
						if(mainmenu.changequestioncategorypowerupcount>0)
						{
							if(powerupselected=="double_blast_radius")
							{
							attack_target.transform.localScale=new Vector3(1.2f,1.2f,0f);
							}
							powerupselected="change_question_category";
						}
						else
						{
							Debug.Log("Change question powerup is finished");
						}

					}
				}
	}

	public static void reducepowerupcount(string powerup)
	{
			if(powerup=="bomb")
			{
			mainmenu.bombpowerupcount--;
			ButtonManager.powerupselected=String.Empty;
			bombtextmesh.GetComponent<TextMesh>().text=mainmenu.bombpowerupcount.ToString();
			PlayerPrefs.SetInt("bombpowerupcount",mainmenu.bombpowerupcount);
			}
			else if(powerup=="double_blast_radius")
			{
			mainmenu.doublebastradiuspowerupcount--;
			ButtonManager.powerupselected=String.Empty;
			doubleblastradiustextmesh.GetComponent<TextMesh>().text=mainmenu.doublebastradiuspowerupcount.ToString();
			PlayerPrefs.SetInt("doubleblastradiuspowerupcount",mainmenu.doublebastradiuspowerupcount);
			}
			else if(powerup=="reverse_time")
			{
			ButtonManager.powerupselected=String.Empty;
			mainmenu.reversetimepowerupcount--;
			reversetimetextmesh.GetComponent<TextMesh>().text=mainmenu.reversetimepowerupcount.ToString();
			PlayerPrefs.SetInt("reversetimepowerupcount",mainmenu.reversetimepowerupcount);
			}
			else if(powerup=="change_question_category")
			{
			ButtonManager.powerupselected=String.Empty;
			mainmenu.changequestioncategorypowerupcount--;
			changequestioncategoriestextmesh.GetComponent<TextMesh>().text=mainmenu.changequestioncategorypowerupcount.ToString();
			PlayerPrefs.SetInt("changequestioncategorypowerupcount",mainmenu.changequestioncategorypowerupcount);
			}
	}

	private void removetopwallcollider()
	{
		topwall.GetComponent<BoxCollider2D>().enabled=false;
	}
}