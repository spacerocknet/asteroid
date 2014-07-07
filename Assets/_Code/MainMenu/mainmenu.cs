using UnityEngine;
using System.Collections;
using System;

public class mainmenu : MonoBehaviour {

			
	private Vector2 touchposition;
	private RaycastHit2D hit;
	
	public enum state{settings,store,facebook,mainmenu,powerups,leaderboard};
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
	public static int totaltimefornewlife=1800;
	public static float cachetotaltimefornewlife=1800;
	public static TimeSpan ts;
	public static int totallives;

	public static int launchcount;


	//Related to powerups
	public static int bombpowerupcount;
	public static int doublebastradiuspowerupcount;
	public static int reversetimepowerupcount;
	public static int changequestioncategorypowerupcount;


	//Cache values of powerups
	public static int bombpowerup_count_cachevalue;
	public static int doubleblastradiuspowerupcount_cachevalue;
	public static int reversetimepowerupcount_cachevalue;
	public static int changequestioncategorypowercount_cachevalue;

	//Gold textmesh
	public GameObject totalgoldtextmesh;


	//Related to leaderboard
	public GameObject LeaderBoard;
	public Sprite global;
	public Sprite friends;
	public GameObject data;


	
	void Start()
	{
		//PlayerPrefs.DeleteAll();
		levelselected=0;
		gamestate=state.mainmenu;
		totalgold=PlayerPrefs.GetInt("totalgold",2500);
		totallives=PlayerPrefs.GetInt("totallives",5);
		bombpowerupcount=PlayerPrefs.GetInt("bombpowerupcount",10);
		doublebastradiuspowerupcount=PlayerPrefs.GetInt("doubleblastradiuspowerupcount",10);
		reversetimepowerupcount=PlayerPrefs.GetInt("reversetimepowerupcount",10);
		changequestioncategorypowerupcount=PlayerPrefs.GetInt("changequestioncategorypowerupcount",10);
		lives_textmesh.GetComponent<TextMesh>().text=totallives.ToString();

		bombpowerup_count_cachevalue=0;
		doubleblastradiuspowerupcount_cachevalue=0;
		reversetimepowerupcount_cachevalue=0;
		changequestioncategorypowercount_cachevalue=0;

		if(launchcount==0 && totallives<5)
		{
			managetimerfornewlife(true);
		}

		totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
		//PowerUps Count
		launchcount++;

		GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[0].GetComponent<TextMesh>().text=bombpowerup_count_cachevalue.ToString();
		GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[1].GetComponent<TextMesh>().text=doubleblastradiuspowerupcount_cachevalue.ToString();
		GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[2].GetComponent<TextMesh>().text=reversetimepowerupcount_cachevalue.ToString();
		GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[3].GetComponent<TextMesh>().text=changequestioncategorypowercount_cachevalue.ToString();
	}

	void Update()
			{

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
		{
			touchposition=Input.GetTouch(0).position;
			touchended();
		}

/*
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
		*/	
			
			//For testing with the mouse

	/*
		if(Input.GetMouseButtonUp(0))
			    {
				touchposition=Input.mousePosition;
				touchended();
				}
	*/
	
			if(Input.GetKeyUp(KeyCode.Escape))
			{
				if(gamestate==state.mainmenu)
				{
				PlayerPrefs.SetInt("totalgold",totalgold);
				PlayerPrefs.SetInt("totallives",totallives);
				Application.Quit();
				}
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
							fadebg.renderer.enabled=false;
							StartCoroutine("showcoinstore");
									}
						else if(hit.collider.gameObject.name=="button_facebook")
									{
							Debug.Log("works");
							fadebg.renderer.enabled=true;
							StartCoroutine("showleaderboardui");
									}
						else if(hit.collider.gameObject.name=="Location_01_Level1")
									{
										Debug.Log("Location 1 works");	
										StartCoroutine("showpowerupswindow");
										levelselected=1;
										fadebg.renderer.enabled=false;
									}
						else if(hit.collider.gameObject.name=="Location_01_Level2")
									{
										StartCoroutine("showpowerupswindow");
										StartCoroutine("showpowerupswindow");
										levelselected=2;
										fadebg.renderer.enabled=false;
									}
						else if(hit.collider.gameObject.name=="Location_01_Level3")
									{
										StartCoroutine("showpowerupswindow");
										levelselected=3;
										fadebg.renderer.enabled=false;
									}
						else if(hit.collider.gameObject.name=="Location_01_Level4")
									{
										StartCoroutine("showpowerupswindow");
										levelselected=4;
										fadebg.renderer.enabled=false;
								}
							}

					else if(gamestate==state.powerups)
					{
						if(hit.collider.gameObject.name=="button_hidepowerups")
						{
							StartCoroutine("hidepowerupwindow");
							int tempvaluetogetbackgold=0;
							if(bombpowerup_count_cachevalue!=0)
							{
							tempvaluetogetbackgold +=bombpowerup_count_cachevalue*250;
							bombpowerup_count_cachevalue=0;
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[0].GetComponent<TextMesh>().text=bombpowerup_count_cachevalue.ToString();
							}
							if(doubleblastradiuspowerupcount_cachevalue!=0)
							{
							tempvaluetogetbackgold +=doubleblastradiuspowerupcount_cachevalue*250;
							doubleblastradiuspowerupcount_cachevalue=0;
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[1].GetComponent<TextMesh>().text=bombpowerup_count_cachevalue.ToString();
							}
							if(changequestioncategorypowercount_cachevalue!=0)
					   		{
							tempvaluetogetbackgold +=changequestioncategorypowerupcount*250;
							changequestioncategorypowercount_cachevalue=0;
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[3].GetComponent<TextMesh>().text=bombpowerup_count_cachevalue.ToString();
							}
							if(reversetimepowerupcount_cachevalue!=0)
							{
							tempvaluetogetbackgold +=reversetimepowerupcount_cachevalue*250;
							reversetimepowerupcount_cachevalue=0;
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[2].GetComponent<TextMesh>().text=bombpowerup_count_cachevalue.ToString();
							}
							
							totalgold+=tempvaluetogetbackgold;
							updatetotalgoldmesh();
							fadebg.renderer.enabled=false;
						}
						else if(hit.collider.gameObject.name=="button_play")
						{
							if(totallives!=0)
							{
							gamestate=state.mainmenu;
							bombpowerupcount +=bombpowerup_count_cachevalue;
							doublebastradiuspowerupcount += doubleblastradiuspowerupcount_cachevalue;
							reversetimepowerupcount +=reversetimepowerupcount_cachevalue;
							changequestioncategorypowerupcount +=changequestioncategorypowercount_cachevalue;
							PlayerPrefs.SetInt("bombpowerupcount",bombpowerupcount);
							PlayerPrefs.SetInt("doubleblastradiuspowerupcount",doublebastradiuspowerupcount);
							PlayerPrefs.SetInt("reversetimepowerupcount",reversetimepowerupcount);
							PlayerPrefs.SetInt("changequestioncategorypowerupcount",changequestioncategorypowerupcount);
							PlayerPrefs.SetInt("totalgold",totalgold);
							loadnewlevel(levelselected);
							}
							else
							{
							//Buy more lives
							}
					}
						else if(hit.collider.gameObject.name=="button_plus_bomb")
						{
								if(totalgold>=250)
								{
								bombpowerup_count_cachevalue++;
								GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[0].GetComponent<TextMesh>().text=bombpowerup_count_cachevalue.ToString();
								totalgold -=250;
								updatetotalgoldmesh();
								}
								else
								{
								//Fail
								}
							}
						else if(hit.collider.gameObject.name=="button_plus_doubleblast")
						{
							if(totalgold>=250)
							{
							doubleblastradiuspowerupcount_cachevalue++;
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[1].GetComponent<TextMesh>().text=doubleblastradiuspowerupcount_cachevalue.ToString();
							totalgold -=250;
							updatetotalgoldmesh();
							}
							else
							{
							//Fail
							}
						}
						else if(hit.collider.gameObject.name=="button_plus_reversetime")
						{
							if(totalgold>=250)
							{
							reversetimepowerupcount_cachevalue++;
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[2].GetComponent<TextMesh>().text=reversetimepowerupcount_cachevalue.ToString();
							totalgold -=250;
							updatetotalgoldmesh();
							}
							else
							{
							//Fail
							}
						}
						else if(hit.collider.gameObject.name=="button_plus_changequestioncategory")
						{
							if(totalgold>=250)
							{
							changequestioncategorypowercount_cachevalue++;
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[3].GetComponent<TextMesh>().text=changequestioncategorypowercount_cachevalue.ToString();
							totalgold -=250;
							updatetotalgoldmesh();
							}
							else
							{	
							//Fail
							}
						}
						
						//Minus values
						else if(hit.collider.gameObject.name=="button_minus_bomb")
						{
							if(bombpowerup_count_cachevalue!=0)
							{
							bombpowerup_count_cachevalue--;
							totalgold+=250;
							updatetotalgoldmesh();
							}
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[0].GetComponent<TextMesh>().text=bombpowerup_count_cachevalue.ToString();
							
						}
						else if(hit.collider.gameObject.name=="button_minus_doubleblast")
						{
							if(doubleblastradiuspowerupcount_cachevalue!=0)
							{
							doubleblastradiuspowerupcount_cachevalue--;
							totalgold+=250;
							updatetotalgoldmesh();
							}
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[1].GetComponent<TextMesh>().text=doubleblastradiuspowerupcount_cachevalue.ToString();
						}

						else if(hit.collider.gameObject.name=="button_minus_reversetime")
						{
							if(reversetimepowerupcount_cachevalue!=0)
							{
							reversetimepowerupcount_cachevalue--;
							totalgold+=250;
							updatetotalgoldmesh();
							}
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[2].GetComponent<TextMesh>().text=reversetimepowerupcount_cachevalue.ToString();
						}

						else if(hit.collider.gameObject.name=="button_minus_changequestioncategory")
						{
							if(changequestioncategorypowercount_cachevalue!=0)
							{
							changequestioncategorypowercount_cachevalue--;
							totalgold+=250;
							updatetotalgoldmesh();
							}
							GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[3].GetComponent<TextMesh>().text=changequestioncategorypowercount_cachevalue.ToString();
						}
					}

					else if(gamestate==state.store)
					{
						if(hit.collider.gameObject.name=="button_buy_pileofgold")
							{
							Debug.Log("works");
							totalgold +=100;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
							}
						else if(hit.collider.gameObject.name=="button_buy_boxofgold")
							{
							Debug.Log("works");
							totalgold +=800;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
							}
						else if(hit.collider.gameObject.name=="button_buy_chestofgold")
							{
							Debug.Log("works");
							totalgold +=1000;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
							}
						else if(hit.collider.gameObject.name=="button_buy_bagofgold")
							{
							Debug.Log("works");
							totalgold +=250;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
							}
						else if(hit.collider.gameObject.name=="button_buy_sackofgold")
							{
							Debug.Log("works");
							totalgold +=500;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
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
						
					else if(gamestate==state.leaderboard)
						{
							if(hit.collider.gameObject.name=="button_close")
							{
							StartCoroutine("hideleaderboardui");
							fadebg.renderer.enabled=false;
							}

							else if(hit.collider.gameObject.name=="button_invite_friends")
							{
								Debug.Log("Button Invite Friends");
							}
							
							else if(hit.collider.gameObject.name=="button_ok")
							{
							Debug.Log("Button Okay Works");
							StartCoroutine("hideleaderboardui");
							fadebg.renderer.enabled=false;
							}

							else if(hit.collider.gameObject.name=="button_friends")
							{
								if(LeaderBoard.GetComponent<SpriteRenderer>().sprite.name=="global")
								{
								LeaderBoard.GetComponent<SpriteRenderer>().sprite=friends;
								data.GetComponent<dataforleaderboard>().changecat("friends");
								}
							}

							else if(hit.collider.gameObject.name=="button_global")
							{
								if(LeaderBoard.GetComponent<SpriteRenderer>().sprite.name=="friend")
								{
								LeaderBoard.GetComponent<SpriteRenderer>().sprite=global;	
								data.GetComponent<dataforleaderboard>().changecat("global");
								}
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
			gamestate=state.powerups;
			for(int i=0;i<20;i++)
				{
				Vector3 oldposition=powerups.transform.position;
				float newposition=Mathf.Lerp(oldposition.y,0f,0.15f);
				powerups.transform.position=new Vector3(powerups.transform.position.x,newposition,0f);
				yield return null;
				}
			yield return new WaitForEndOfFrame();
			}
	
			IEnumerator hidepowerupwindow()
				{
				gamestate=state.mainmenu;
				for(int i=0;i<20;i++)
				{
					Vector3 oldposition=powerups.transform.position;
					float newposition=Mathf.Lerp(oldposition.y,9.50314f,0.15f);
					powerups.transform.position=new Vector3(powerups.transform.position.x,newposition,0f);
					yield return null;
				}
			yield return new WaitForEndOfFrame();
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
	
		IEnumerator showleaderboardui()
		{
			gamestate=state.leaderboard;
			for(int i=0;i<20;i++)
			{
				Vector3 oldposition=LeaderBoard.transform.position;
				float newposition=Mathf.Lerp(oldposition.y,0f,0.15f);
				LeaderBoard.transform.position=new Vector3(LeaderBoard.transform.position.x,newposition,0f);
				yield return null;
			}
			yield return new WaitForEndOfFrame();
		}

		IEnumerator hideleaderboardui()
		{
			gamestate=state.mainmenu;
			for(int i=0;i<20;i++)
			{
				Vector3 oldposition=LeaderBoard.transform.position;
				float newposition=Mathf.Lerp(oldposition.y,-9f,0.15f);
				LeaderBoard.transform.position=new Vector3(LeaderBoard.transform.position.x,newposition,0f);
				yield return null;
			}
			yield return new WaitForEndOfFrame();
			//gamestate=state.mainmenu;
		}

		void updatetotalgoldmesh()
		{
		totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
		}
}



