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
	public GameObject changequestioncategorycolor;
	public GameObject fadebg;
	Color fadebgcolor;

	//For toggling the color of the powerups
	public static Color powerupselectedcolor;
	public static Color powerupnormalcolor;
	public GameObject double_hitpowerup_gameobject;
	public GameObject double_blastpowerup_gameobject;
	public GameObject changequestioncategory_gameobject;
	public static GameObject rewardsscreen;


	//Public static GameObjects to access on the reduction
	public static GameObject hitpowerup_static_gameobject;
	public static GameObject doubleblastradius_static_gameobject;
	public static GameObject reversetime_static_gameobject;
	public static GameObject changequestioncategory_Static_gameobject;

	public static bool canmovemarker;
	
	void Awake()
	{
		canmovemarker=true;
		hitpowerup_static_gameobject=GameObject.Find("button_power_up_02");
		doubleblastradius_static_gameobject=GameObject.Find("button_power_up_03");
		reversetime_static_gameobject=GameObject.Find("button_power_up_04");
		changequestioncategory_Static_gameobject=GameObject.Find("button_power_up_05");

		powerupselectedcolor=new Color(139f,142f,142f,0.5f);
		powerupnormalcolor=new Color(255f,255f,255f,1f);
		bombtextmesh=GameObject.Find("bomb_textmesh");
		rewardsscreen=GameObject.Find("rewardsscreen");
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
		//Invoke("removetopwallcollider",1.65f);
	}

	void Update () {

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
		{
			touchposition=Input.GetTouch(0).position;
			touchended();
		}

		//For testing with the mouse
		if(Input.GetMouseButtonUp(0))
		{
		//	touchposition=Input.mousePosition;
		//	touchended();
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
					canmovemarker=false;
					if(hit.collider.gameObject.name=="button_power_up_01")
					{
					//Rock Paper Scissors Logic to be placed here
					}
					else if(hit.collider.gameObject.name=="button_power_up_02")
					{
					if(powerupselected!="bomb")
						{
							if(mainmenu.bombpowerupcount>0)
							{
								if(powerupselected=="double_blast_radius")
									{
									attack_target.transform.localScale=new Vector3(0.6f,0.6f,0f);
									double_blastpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
									}
								powerupselected="bomb";
								//ToggleHere	
								hit.collider.gameObject.GetComponent<SpriteRenderer>().color=powerupselectedcolor;
							}
							else
								{
								Debug.Log("Bomb powerup is finished");
								}
							}
						else
						{
						//This will toggle if it already selected
						double_hitpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
						powerupselected=string.Empty;
						}
					}
					else if(hit.collider.gameObject.name=="button_power_up_03")
					{
					if(powerupselected!="double_blast_radius")
						{
						if(mainmenu.doublebastradiuspowerupcount>0)
							{
							powerupselected="double_blast_radius";
							attack_target.transform.localScale=new Vector3(1.2f,1.2f,0f);
							//ToggleHere
							hit.collider.gameObject.GetComponent<SpriteRenderer>().color=powerupselectedcolor;
							double_hitpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
							}
						else
							{
							Debug.Log("Double Blast radius powerup is finished");
							}
						}
					else 
						{
						//This will toggle if already selected
						double_blastpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
						powerupselected=string.Empty;
						attack_target.transform.localScale=new Vector3(0.6f,0.6f,0f);
						}
					}
					else if(hit.collider.gameObject.name=="button_power_up_04")
					{
						if(mainmenu.reversetimepowerupcount>0)
						{
							if(powerupselected=="double_blast_radius")
							{
							attack_target.transform.localScale=new Vector3(0.6f,0.6f,0f);
							double_blastpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
							}
							double_hitpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
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
						if(powerupselected!="change_question_category")
						{
							if(mainmenu.changequestioncategorypowerupcount>0)
							{
								if(powerupselected=="double_blast_radius")
								{
								attack_target.transform.localScale=new Vector3(1.2f,1.2f,0f);
								}
								MAIN.GetComponent<BattleEngine>().categorySelect.disablequestioncollidersandtriggers();
								powerupselected="change_question_category";
								fadebg.GetComponent<BoxCollider2D>().enabled=true;
								StartCoroutine("showquestionchangecatwindow");
								fadebgcolor=fadebg.GetComponent<SpriteRenderer>().color;
								fadebgcolor.a=1;
								fadebg.GetComponent<SpriteRenderer>().color=fadebgcolor;
								//state=gamestate.changequestioncategory;
								double_hitpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
								double_blastpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
								hit.collider.gameObject.GetComponent<SpriteRenderer>().color=powerupselectedcolor;
										
					}
							else
							{
								Debug.Log("Change question powerup is finished");
							}
						}
						else
						{
						//It is already toggled so hideitup
						fadebgalphaandtriggerdisable();
						StartCoroutine("hidequestionchangecatwindow");
						powerupselected=string.Empty;
						}
						
					}
					else if(hit.collider.gameObject.name=="FadeBG")
					{
					Debug.Log("working");
					fadebgalphaandtriggerdisable();
					StartCoroutine("hidequestionchangecatwindow");
					}
					else if(hit.collider.gameObject.name=="button_bluecolor")
					{	
					fadebgalphaandtriggerdisable();
					StartCoroutine("hidequestionchangecatwindow");	
					reducepowerupcount(powerupselected);
					MAIN.GetComponent<BattleEngine>().categorySelect.PlaceCategoriesByCategoryChangePowerup("blue");
					}
					else if(hit.collider.gameObject.name=="button_greencolor")
					{
					fadebgalphaandtriggerdisable();	
					StartCoroutine("hidequestionchangecatwindow");
					reducepowerupcount(powerupselected);
					MAIN.GetComponent<BattleEngine>().categorySelect.PlaceCategoriesByCategoryChangePowerup("green");
					}
					else if(hit.collider.gameObject.name=="button_redcolor")
					{
					fadebgalphaandtriggerdisable();
					StartCoroutine("hidequestionchangecatwindow");
					reducepowerupcount(powerupselected);
					MAIN.GetComponent<BattleEngine>().categorySelect.PlaceCategoriesByCategoryChangePowerup("red");
					}
				}
		else
				{
				canmovemarker=true;
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
			hitpowerup_static_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
			}
			else if(powerup=="double_blast_radius")
			{
			attack_target.transform.localScale=new Vector3(0.6f,0.6f,0f);
			mainmenu.doublebastradiuspowerupcount--;
			ButtonManager.powerupselected=String.Empty;
			doubleblastradiustextmesh.GetComponent<TextMesh>().text=mainmenu.doublebastradiuspowerupcount.ToString();
			PlayerPrefs.SetInt("doubleblastradiuspowerupcount",mainmenu.doublebastradiuspowerupcount);
			doubleblastradius_static_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
			}
			else if(powerup=="reverse_time")
			{
			ButtonManager.powerupselected=String.Empty;
			mainmenu.reversetimepowerupcount--;
			reversetimetextmesh.GetComponent<TextMesh>().text=mainmenu.reversetimepowerupcount.ToString();
			PlayerPrefs.SetInt("reversetimepowerupcount",mainmenu.reversetimepowerupcount);
			reversetime_static_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
			}
			else if(powerup=="change_question_category")
			{
			ButtonManager.powerupselected=String.Empty;
			mainmenu.changequestioncategorypowerupcount--;
			changequestioncategoriestextmesh.GetComponent<TextMesh>().text=mainmenu.changequestioncategorypowerupcount.ToString();
			PlayerPrefs.SetInt("changequestioncategorypowerupcount",mainmenu.changequestioncategorypowerupcount);
			//Change Question Cat disable
			changequestioncategory_Static_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
			}
	}

	private void removetopwallcollider()
	{
		topwall.GetComponent<BoxCollider2D>().enabled=false;
	}


	IEnumerator showquestionchangecatwindow()
	{	
		for(int i=0;i<40;i++)
		{
			Vector3 oldposition=changequestioncategorycolor.transform.position;
			float newposition=Mathf.Lerp(oldposition.x,-0.10f,0.15f);
			changequestioncategorycolor.transform.position=new Vector3(newposition,changequestioncategorycolor.transform.position.y,changequestioncategorycolor.transform.position.z);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
	}
		
	IEnumerator hidequestionchangecatwindow()
	{

		for(int i=0;i<40;i++)
		{
			Vector3 oldposition=changequestioncategorycolor.transform.position;
			float newposition=Mathf.Lerp(oldposition.x,5.21f,0.15f);
			changequestioncategorycolor.transform.position=new Vector3(newposition,changequestioncategorycolor.transform.position.y,changequestioncategorycolor.transform.position.z);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		changequestioncategory_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;

	}

	public static IEnumerator showrewardsscreen()
	{
		yield return null;
	}


	void fadebgalphaandtriggerdisable()
	{
		MAIN.GetComponent<BattleEngine>().categorySelect.enablequestioncollidersandtriggers();
		fadebg.GetComponent<BoxCollider2D>().enabled=false;
		fadebgcolor=fadebg.GetComponent<SpriteRenderer>().color;
		fadebgcolor.a=0;
		fadebg.GetComponent<SpriteRenderer>().color=fadebgcolor;
	}
}