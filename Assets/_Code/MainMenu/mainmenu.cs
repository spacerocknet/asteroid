using UnityEngine;
using System.Collections;
using System;

public class mainmenu : MonoBehaviour {

			
	private Vector2 touchposition;
	private RaycastHit2D hit;
	
	public enum state{settings,store,facebook,mainmenu,powerups};
	public state gamestate;
	public GameObject coinstore;
	public GameObject settings;
	public GameObject powerups;
	public GameObject fadebg;
	public Sprite button_off;
	public Sprite button_on;
	public GameObject lives_textmesh;
	
	public static int totalgold;
	public static int levelselected;
	public static int playcount;


	//Related to life time management

	public GameObject newlifetimer;
	public static bool timerstarted;
	public static int totaltimefornewlife=20;
	public static float cachetotaltimefornewlife=20;
	public static TimeSpan ts;
	public static int totallives;

	public static int launchcount;


	//Related to powerups
	public static int bombpowerupcount;
	public static int doublebastradiuspowerupcount;
	public static int reversetimepowerupcount;
	public static int changequestioncategorypowerupcount;

	void Start()
	{
		levelselected=0;
		gamestate=state.mainmenu;
		totalgold=PlayerPrefs.GetInt("totalgold");
		totallives=PlayerPrefs.GetInt("totallives",5);
		bombpowerupcount=PlayerPrefs.GetInt("bombpowerupcount",10);
		doublebastradiuspowerupcount=PlayerPrefs.GetInt("doubleblastradiuspowerupcount",10);
		reversetimepowerupcount=PlayerPrefs.GetInt("reversetimepowerupcount",10);
		changequestioncategorypowerupcount=PlayerPrefs.GetInt("changequestioncategorypowerupcount",10);
		lives_textmesh.GetComponent<TextMesh>().text=totallives.ToString();

		if(launchcount==0 && totallives<5)
		{
			managetimerfornewlife(true);
		}

		//PowerUps Count

		launchcount++;
	}

	void Update()
			{
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
				PlayerPrefs.SetInt("totallives",totallives);
				Application.Quit();
				}

			if(timerstarted)
			{
			cachetotaltimefornewlife -=Time.deltaTime;
			ts=TimeSpan.FromSeconds(cachetotaltimefornewlife);
			newlifetimer.GetComponent<TextMesh>().text=ts.ToString().Substring(3,5);
			}
			
			if(ts.TotalSeconds<=0)
			{	
				if(timerstarted==true)
				{
				totallives++;
				managetimerfornewlife(false);
				lives_textmesh.GetComponent<TextMesh>().text=totallives.ToString();
				PlayerPrefs.SetInt("totalives",totallives);
				Debug.Log("Calling this");
				//resettimer();
					if(totallives<5)
					{
					managetimerfornewlife(true);
					}
					else
					{
					managetimerfornewlife(false);
					newlifetimer.GetComponent<MeshRenderer>().enabled=false;
					}
				}
				resettimerfornewlife();
			}
	}

	void touchended()
			{
			hit=Physics2D.Raycast(camera.ScreenToWorldPoint(new Vector3(touchposition.x,touchposition.y,0)),Vector2.zero);
				if(hit.collider!=null)
					{
					if(gamestate==state.mainmenu)
					{
						if(hit.collider.gameObject.name=="button_settings")
									{
							fadebg.renderer.enabled=true;
							StartCoroutine("showsettings");
									}
						else if(hit.collider.gameObject.name=="button_store")
									{
							fadebg.renderer.enabled=true;
							StartCoroutine("showcoinstore");
									}
						else if(hit.collider.gameObject.name=="button_facebook")
									{
							Debug.Log("works");
									}
						else if(hit.collider.gameObject.name=="Location_01_Level1")
									{
										Debug.Log("Location 1 works");		
										StartCoroutine("showpowerupswindow");
										levelselected=1;
										fadebg.renderer.enabled=true;
									}
						else if(hit.collider.gameObject.name=="Location_01_Level2")
									{
										StartCoroutine("showpowerupswindow");
										StartCoroutine("showpowerupswindow");
										levelselected=2;
										fadebg.renderer.enabled=true;
									}
						else if(hit.collider.gameObject.name=="Location_01_Level3")
									{
										StartCoroutine("showpowerupswindow");
										levelselected=3;
										fadebg.renderer.enabled=true;
									}
						else if(hit.collider.gameObject.name=="Location_01_Level4")
									{
										StartCoroutine("showpowerupswindow");
										levelselected=4;
										fadebg.renderer.enabled=true;
									}

						else if(hit.collider.gameObject.name=="button_hidepowerups")
									{
										StartCoroutine("hidepowerupwindow");
										fadebg.renderer.enabled=false;
									}
						else if(hit.collider.gameObject.name=="button_play")
									{
										loadnewlevel(levelselected);
									}
						else if(hit.collider.gameObject.name=="button_plus_bomb")
									{
									bombpowerupcount++;
									PlayerPrefs.SetInt("bombpowerupcount",bombpowerupcount);
									}
						else if(hit.collider.gameObject.name=="button_plus_doubleblast")
									{
									doublebastradiuspowerupcount++;
									PlayerPrefs.SetInt("doubleblastradiuspowerupcount",doublebastradiuspowerupcount);
									}
						else if(hit.collider.gameObject.name=="button_plus_reversetime")
									{
									reversetimepowerupcount++;
									PlayerPrefs.SetInt("reversetimepowerupcount",reversetimepowerupcount);
									}
						else if(hit.collider.gameObject.name=="button_plus_changequestioncategory")
								{
									Debug.Log("bomb3");
									changequestioncategorypowerupcount++;
									PlayerPrefs.SetInt("changequestioncategorypowerupcount",changequestioncategorypowerupcount);
								}
							}
					else if(gamestate==state.store)
					{
						if(hit.collider.gameObject.name=="button_buy_pileofgold")
							{
							Debug.Log("works");
							totalgold +=100;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_buy_boxofgold")
							{
							Debug.Log("works");
							totalgold +=800;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_buy_chestofgold")
							{
							Debug.Log("works");
							totalgold +=1000;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_buy_bagofgold")
							{
							Debug.Log("works");
							totalgold +=250;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_buy_sackofgold")
							{
							Debug.Log("works");
							totalgold +=500;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_hidestore")
							{
							StartCoroutine("hidecoinstore");
							fadebg.renderer.enabled=false;
							}
						else if(hit.collider.gameObject.name=="Play_button")
							{
							loadnewlevel(levelselected);
							}
						}
			
					else if(gamestate==state.settings)
					{
						if(hit.collider.gameObject.name=="button_hidesettings")
						{
							StartCoroutine("hidesettings");
							fadebg.renderer.enabled=false;
						}
						else if(hit.collider.gameObject.name=="buttononstatefacebook")
						{
							if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_off")
							{
							hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;
							}
							else if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_on")
					        {
							hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
							}
						}
						else if(hit.collider.gameObject.name=="buttononstategoogle")
						{
							Debug.Log("Working");
							if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_off")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;
							}
							else if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_on")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
							}
						}
						else if(hit.collider.gameObject.name=="button_help_feedback")
						{
							Debug.Log("Works");
						}
						else if(hit.collider.gameObject.name=="buttonstatemusic")
						{
							if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_off")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;
							}
							else if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_on")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
							}
						}
						else if(hit.collider.gameObject.name=="button_about")
						{
							Debug.Log("Works");
						}
						else if(hit.collider.gameObject.name=="button_service")
						{
							Debug.Log("Works");
						}
					}
				}
			}


			IEnumerator showcoinstore()
			{
				for(int i=0;i<20;i++)
				{
				Vector3 oldposition=coinstore.transform.position;
				float newposition=Mathf.Lerp(oldposition.x,-0.538f,0.25f);
				coinstore.transform.position=new Vector3(newposition,coinstore.transform.position.y,0f);
				yield return null;
				}
				yield return new WaitForEndOfFrame();
				gamestate=state.store;
			}

			IEnumerator hidecoinstore()
			{
				for(int i=0;i<20;i++)
				{
				Vector3 oldposition=coinstore.transform.position;
				float newposition=Mathf.Lerp(oldposition.x,-7.55f,0.25f);
				coinstore.transform.position=new Vector3(newposition,coinstore.transform.position.y,0f);
				yield return null;
				}
			yield return new WaitForEndOfFrame();
			gamestate=state.mainmenu;
			}
		
			IEnumerator showsettings()
			{
				for(int i=0;i<20;i++)
				{
				Vector3 oldposition=settings.transform.position;
				float newposition=Mathf.Lerp(oldposition.x,-0.04f,0.25f);
				settings.transform.position=new Vector3(newposition,settings.transform.position.y,0f);
				yield return null;
				}
			yield return new WaitForEndOfFrame();
			gamestate=state.settings;
			}
		
			IEnumerator hidesettings()
			{
				for(int i=0;i<20;i++)
				{
					Vector3 oldposition=settings.transform.position;
					float newposition=Mathf.Lerp(oldposition.x,7.231156f,0.25f);
					settings.transform.position=new Vector3(newposition,settings.transform.position.y,0f);
					yield return null;
				}
				yield return new WaitForEndOfFrame();
				gamestate=state.mainmenu;
			}

			IEnumerator showpowerupswindow()
			{
			for(int i=0;i<20;i++)
				{
				Vector3 oldposition=powerups.transform.position;
				float newposition=Mathf.Lerp(oldposition.y,0f,0.15f);
				powerups.transform.position=new Vector3(powerups.transform.position.x,newposition,0f);
				yield return null;
				}
			yield return new WaitForEndOfFrame();
			gamestate=state.mainmenu;
			}
	
			IEnumerator hidepowerupwindow()
				{
				for(int i=0;i<20;i++)
				{
					Vector3 oldposition=powerups.transform.position;
					float newposition=Mathf.Lerp(oldposition.y,9.50314f,0.15f);
					powerups.transform.position=new Vector3(powerups.transform.position.x,newposition,0f);
					yield return null;
				}
			yield return new WaitForEndOfFrame();
			gamestate=state.mainmenu;
			}

		void loadnewlevel(int loadlevelindex)
		{
		Application.LoadLevel(loadlevelindex);
		}
	
		public static void managetimerfornewlife(bool start)
		{
		timerstarted=start;	
		}

		public static void resettimerfornewlife()
		{
		cachetotaltimefornewlife=totaltimefornewlife;
		}
		
}



