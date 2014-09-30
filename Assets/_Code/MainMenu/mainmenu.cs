using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook.MiniJSON;

public class mainmenu : MonoBehaviour {

	private Vector2 touchposition;
	private RaycastHit2D hit;

	public LevelNodeInfoCollection levelNodeInfos;
	public LevelNodeInfoManager levelNodeInfoManager;

	public enum state{settings,store,facebook,mainmenu,powerups,leaderboard,buylives};
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
	public GameObject newlifetimer2;
	public static bool timerstarted;
	public static int totaltimefornewlife=1800;
	public int totaltimrfornewlife_onresume;
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


	//Related to sounds
	public AudioSource[] soundsources;
	public GameObject mainbg;

	public AudioClip buttonclicksound;

	public GameObject buymorelivesgameobject;

	//Quit Popup
	public GameObject quitpopup;


	//From where did buy button clicked
	int fromwhere;


	//Date time 
	DateTime oldtime;
	DateTime currenttime;


	//Run counter;
	public static int runcounter;
	private int fromthis;


	//State for facebook and sound
	public GameObject facebook_sprite;
	public GameObject sound_sprite;
	public static int facebook;
	public static int sound;
	GameObject temp;
	public GameObject audiolistener;

	private String lastTouchedButtonName;

	//For FB
	private GameObject fbPreLogin;
	private GameObject fbPostLogin;

	// level node
	private LevelNodeInfoCollection.LevelNodeInfo selectedLevelNodeInfo;
	
	void Awake()
	{
		fromthis=0;
		launchcount=PlayerPrefs.GetInt("launchcount",0);
		gamestate=state.mainmenu;
	}

	void Start()
	{
		
		//facebook=PlayerPrefs.GetInt("facebook",0);
		sound=PlayerPrefs.GetInt("sound",1);


		//if(facebook==1)
		//{
		//	facebook_sprite.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;		
		//}
		//else
		//{
		//	facebook_sprite.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
		//}

		if(sound==1)
		{
			sound_sprite.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;
			unmuteallaudiosourcesinscene();
		}
		else
		{
			sound_sprite.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
			muteallaudiosourcesinscene();
		}

		//PlayerPrefs.DeleteAll();
		runcounter=PlayerPrefs.GetInt("runcounter",0);
		fromwhere=0;
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
			int gamequit=PlayerPrefs.GetInt("gamequit",0);
			if(gamequit==0)
			{
			   managetimerfornewlife(true);
			}
			else if(gamequit==1)
			{
			   runcodeonresume();
			}
			launchcount++;
		}
	
		
		totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
		//PowerUps Count
	

		GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[0].GetComponent<TextMesh>().text=bombpowerup_count_cachevalue.ToString();
		GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[1].GetComponent<TextMesh>().text=doubleblastradiuspowerupcount_cachevalue.ToString();
		GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[2].GetComponent<TextMesh>().text=reversetimepowerupcount_cachevalue.ToString();
		GameObject.Find("Main Camera").GetComponent<sortlayerforpoweruptextmesh>().textmeshes[3].GetComponent<TextMesh>().text=changequestioncategorypowercount_cachevalue.ToString();

		Invoke("thememusicplay",0.12f);

		lastTouchedButtonName = "";

		//For Facebook
		FB.Init(SetInit, OnHideUnity);
		fbPostLogin = GameObject.Find("button_fb_post_login");
		fbPreLogin = GameObject.Find("button_facebook");

		if (FB.IsLoggedIn) {
			fbPreLogin.transform.Translate(7, 0, 0);
			fbPreLogin.renderer.enabled = false;
			fbPostLogin.transform.Translate (-7, 0, 0);
			fbPostLogin.renderer.enabled = true;
		} else {
			fbPreLogin.renderer.enabled = true;
			fbPostLogin.renderer.enabled = false;
		}

	}
	
	void Update()
	{
		
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
		{
			touchposition=Input.GetTouch(0).position;
			touchended();
		}
		
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(gamestate==state.mainmenu)
			{
				if(totallives<5)
				{
				   savecurrenttime();
				}
				PlayerPrefs.SetInt("gamequit",1);
				PlayerPrefs.SetInt("totalgold",totalgold);
				PlayerPrefs.SetInt("totallives",totallives);
				Application.Quit();
			}
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


	
		if(Input.GetMouseButtonUp(0))
	    {
		   touchposition=Input.mousePosition;
		   touchended();
		}
	
	
		
			//if(totallives>=5)
		//	{
		///		newlifetimer.GetComponent<TextMesh>().text="Full";
		//	}
			
		//	lives_textmesh.GetComponent<TextMesh>().text=totallives.ToString();
			

		if(timerstarted)
		{
			cachetotaltimefornewlife -=Time.deltaTime;
			ts=TimeSpan.FromSeconds(cachetotaltimefornewlife);
			newlifetimer.GetComponent<TextMesh>().text=ts.ToString().Substring(3,5);
			newlifetimer2.GetComponent<TextMesh>().text=ts.ToString().Substring(3,5);
			Invoke("calculateminutesandseconds",0.2f);
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
					newlifetimer2.GetComponent<MeshRenderer>().enabled=false;
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
				
	        if(lastTouchedButtonName.Equals(hit.collider.gameObject.name))
		        return;  //don't process the same button more than one time

			LevelNode levelNode = hit.collider.gameObject.GetComponent<LevelNode>();
			if (levelNode != null) {
				Debug.Log("Level " + levelNode.level + " selected.");

				LevelNodeInfoCollection.LevelNodeInfo levelNodeInfo = levelNodeInfos.GetLevelNodeInfo(levelNode.level);
				selectedLevelNodeInfo = levelNodeInfo;
					
				StartCoroutine(showpowerupswindow());

				levelselected = levelNode.level;

				fadebg.renderer.enabled = false;

				buttonclickeffect();
			}

					
			if(gamestate==state.mainmenu)
					{
						if(hit.collider.gameObject.name=="button_settings")
						{
							//fadebg.renderer.enabled=true;
							StartCoroutine("showsettings");
							buttonclickeffect();
						}
						
				else if(hit.collider.gameObject.name=="button_store")
						{
							//fadebg.renderer.enabled=false;
							StartCoroutine("showcoinstore");
							buttonclickeffect();
						}
						
				else if(hit.collider.gameObject.name=="button_facebook")
						{
							//if(facebook==1)
							//{
						    Debug.Log("facebook works");
						       //fadebg.renderer.enabled=true;
						
						    FB.Login("email,publish_actions", LoginCallback);
						    buttonclickeffect();
							//}	
				        } 
				else if(hit.collider.gameObject.name =="button_fb_post_login")
				        {
					       Debug.Log ("fbpostlogin works!!!");
					       StartCoroutine("showleaderboardui");
					       buttonclickeffect();
				        }
				        else if(hit.collider.gameObject.name=="coins_bar_empty")
				        {
					       fadebg.renderer.enabled=false;
					       StartCoroutine("showcoinstore");
					       buttonclickeffect();
				        }
//				        else if(hit.collider.gameObject.name=="Location_01_Level1")
//					    {
//							Debug.Log("Location 1 works");	
//							StartCoroutine("showpowerupswindow");
//							levelselected=1;
//							fadebg.renderer.enabled=false;
//							buttonclickeffect();
//						}
//						else if(hit.collider.gameObject.name=="Location_01_Level2")
//						{
//							StartCoroutine("showpowerupswindow");
//							StartCoroutine("showpowerupswindow");
//							levelselected=2;
//							fadebg.renderer.enabled=false;
//							buttonclickeffect();
//						}
//						else if(hit.collider.gameObject.name=="Location_01_Level3")
//						{
//							StartCoroutine("showpowerupswindow");
//							levelselected=3;
//							fadebg.renderer.enabled=false;
//							buttonclickeffect();
//						}
//						else if(hit.collider.gameObject.name=="Location_01_Level4")
//									{
//										StartCoroutine("showpowerupswindow");
//										levelselected=4;
//										fadebg.renderer.enabled=false;
//										buttonclickeffect();
//								}
						else if(hit.collider.gameObject.name=="lives_bar_empty")
								{
							//Show buy lives popup
										if(totallives==0)
										{
										fromwhere=1;
										buttonclickeffect();
										StartCoroutine("buymorelives");
										}
								}
						}

					else if(gamestate==state.buylives)
						{
						if(hit.collider.gameObject.name=="button_close_buylives")
							{
								Debug.Log("close_button_working");
								if(fromwhere==1)
								{
								buttonclickeffect();
								StartCoroutine("hidemorelives");
								}
								else if(fromwhere==2)
								{
								buttonclickeffect();
								if(totallives>0)
									{
									Application.LoadLevel(1);
									}
								else
									{
									StartCoroutine("hidemorelives");
									}
								}
							}
						else if(hit.collider.gameObject.name=="button_okay_buylives")
							{
							if(totalgold>=200)
								{
								buttonclickeffect();
								totalgold-=200;
								totallives++;
								
								if(totallives>=5)
								{
								managetimerfornewlife(false);
								lives_textmesh.GetComponent<TextMesh>().text="Full";
								}

								updatetotalgoldmesh();
								lives_textmesh.GetComponent<TextMesh>().text=totallives.ToString();
								PlayerPrefs.SetInt("totalgold",totalgold);
								PlayerPrefs.SetInt("totallives",totallives);
								StartCoroutine("hidemorelives");
								}
								else if(totalgold<200 && totallives<5)
								{	
								StartCoroutine("hidemorelives");
								StartCoroutine("showcoinstore");
								}
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
								
							buttonclickeffect();
							totalgold+=tempvaluetogetbackgold;
							updatetotalgoldmesh();
							fadebg.renderer.enabled=false;
						}
						else if(hit.collider.gameObject.name=="button_play")
						{
							buttonclickeffect();
							if(totallives != 0)
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
								
								if (selectedLevelNodeInfo != null) {
									LevelInfo levelInfo = levelNodeInfoManager.gameObject.AddComponent<LevelInfo>();
									levelInfo.selectedLevelNodeInfo = selectedLevelNodeInfo; 
								}

								//loadnewlevel(levelselected);
								Application.LoadLevel("MainScene");
							}
							else
							{
							StartCoroutine("hidepowerupwindow");
							fromwhere=2;
							StartCoroutine("buymorelives");
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
								buttonclickeffect();
							}
						else if(hit.collider.gameObject.name=="button_plus_doubleblast")
						{
							buttonclickeffect();
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
							buttonclickeffect();
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
							buttonclickeffect();
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
							buttonclickeffect();
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
							buttonclickeffect();
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
							buttonclickeffect();
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
							buttonclickeffect();		
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
							buttonclickeffect();
						}
					}

					else if(gamestate==state.store)
					{
						if(hit.collider.gameObject.name=="button_buy_pileofgold")
							{
							Debug.Log("works");
							totalgold +=200;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
							buttonclickeffect();
							}
						else if(hit.collider.gameObject.name=="button_buy_boxofgold")
							{
							Debug.Log("works");
							totalgold +=11500;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
							buttonclickeffect();
							}
						else if(hit.collider.gameObject.name=="button_buy_chestofgold")
							{
							Debug.Log("works");
							totalgold +=24000;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
							buttonclickeffect();
							}
						else if(hit.collider.gameObject.name=="button_buy_bagofgold")
							{
							Debug.Log("works");
							totalgold +=1050;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
							buttonclickeffect();
							}
						else if(hit.collider.gameObject.name=="button_buy_sackofgold")
							{
							Debug.Log("works");
							totalgold +=2200;
							PlayerPrefs.SetInt("totalgold",totalgold);
							totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
							buttonclickeffect();
							}
						else if(hit.collider.gameObject.name=="button_hidestore")
							{
							StartCoroutine("hidecoinstore");
							fadebg.renderer.enabled=false;
							buttonclickeffect();
							}
						else if(hit.collider.gameObject.name=="Play_button")
							{
								buttonclickeffect();
								loadnewlevel(levelselected);
							}
						}
			
					else if(gamestate==state.settings)
					{
						if(hit.collider.gameObject.name=="button_hidesettings")
						{
							StartCoroutine("hidesettings");
							fadebg.renderer.enabled=false;
							buttonclickeffect();
						}
						else if(hit.collider.gameObject.name=="buttononstatefacebook")
						{
							if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_off")
							{
							hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;
							PlayerPrefs.SetInt("facebook",1);
							facebook=PlayerPrefs.GetInt("facebook",1);
							}
							else if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_on")
					        {
							hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
							PlayerPrefs.SetInt("facebook",0);
							facebook=PlayerPrefs.GetInt("facebook",1);
							}
							buttonclickeffect();
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
							buttonclickeffect();
						}
						else if(hit.collider.gameObject.name=="button_help_feedback")
						{
							Debug.Log("Works");
							buttonclickeffect();
						}
						else if(hit.collider.gameObject.name=="buttonstatemusic")
						{
							if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_off")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;
								unmuteallaudiosourcesinscene();	
								//unmuteaudiolistener();
								PlayerPrefs.SetInt("sound",1);
								sound=PlayerPrefs.GetInt("sound",1);
							}
							else if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_on")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
								muteallaudiosourcesinscene();
								//muteaudiolistener();
								PlayerPrefs.SetInt("sound",0);
								sound=PlayerPrefs.GetInt("sound",1);
							}
							buttonclickeffect();
						}
						else if(hit.collider.gameObject.name=="button_about")
						{
							Debug.Log("Works");
							buttonclickeffect();
						}
						else if(hit.collider.gameObject.name=="button_service")
						{
							Debug.Log("Works");
							buttonclickeffect();
						}
					}
						
					else if(gamestate==state.leaderboard)
						{
							if(hit.collider.gameObject.name=="button_close")
							{
							StartCoroutine("hideleaderboardui");
							fadebg.renderer.enabled=false;
							buttonclickeffect();
							}

							else if(hit.collider.gameObject.name=="button_invite_friends")
							{
								Debug.Log("Button Invite Friends");
								buttonclickeffect();
							}
							
							else if(hit.collider.gameObject.name=="button_ok")
							{
							Debug.Log("Button Okay Works");
							StartCoroutine("hideleaderboardui");
							fadebg.renderer.enabled=false;
								buttonclickeffect();
							}

							else if(hit.collider.gameObject.name=="button_friends")
							{
								if(LeaderBoard.GetComponent<SpriteRenderer>().sprite.name=="global")
								{
								LeaderBoard.GetComponent<SpriteRenderer>().sprite=friends;
								data.GetComponent<dataforleaderboard>().changecat("friends");
								}
								buttonclickeffect();
							}

							else if(hit.collider.gameObject.name=="button_global")
							{
								if(LeaderBoard.GetComponent<SpriteRenderer>().sprite.name=="friend")
								{
								LeaderBoard.GetComponent<SpriteRenderer>().sprite=global;	
								data.GetComponent<dataforleaderboard>().changecat("global");
								}
								buttonclickeffect();
							}
						}

			            lastTouchedButtonName = hit.collider.gameObject.name;
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
	
		public static void resettimerfornewlifeonresume(int time)
		{
		cachetotaltimefornewlife=time;
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

		IEnumerator buymorelives()
		{
			gamestate=state.buylives;
			for(int i=0;i<20;i++)
			{
				Vector3 oldposition=buymorelivesgameobject.transform.position;
				float newposition=Mathf.Lerp(oldposition.x,0f,0.25f);
				buymorelivesgameobject.transform.position=new Vector3(newposition,buymorelivesgameobject.transform.position.y,buymorelivesgameobject.transform.position.z);
				//rewardscreen.transform.position=new Vector3(0f,0f,0f);
				yield return null;
			}
			yield return new WaitForEndOfFrame();
		}

		IEnumerator hidemorelives()
		{
			gamestate=state.mainmenu;
			for(int i=0;i<20;i++)
			{
				Vector3 oldposition=buymorelivesgameobject.transform.position;
				float newposition=Mathf.Lerp(oldposition.x,7f,0.25f);
				buymorelivesgameobject.transform.position=new Vector3(newposition,buymorelivesgameobject.transform.position.y,buymorelivesgameobject.transform.position.z);
				//rewardscreen.transform.position=new Vector3(0f,0f,0f);
				yield return null;
			}
			yield return new WaitForEndOfFrame();
		}

		void updatetotalgoldmesh()
		{
		totalgoldtextmesh.GetComponent<TextMesh>().text=totalgold.ToString();
		}

		void buttonclickeffect()
		{
		mainbg.audio.clip=buttonclicksound;
		mainbg.audio.Play();
		}

	void OnApplicationPause (bool pause)
	{
		if(pause)
		{
			fromthis=1;
			runcounter++;
			PlayerPrefs.SetInt("runcounter",runcounter);
			PlayerPrefs.SetString("oldtime",DateTime.Now.ToBinary().ToString());
			int previouselapsedminutes=int.Parse(newlifetimer.GetComponent<TextMesh>().text.Substring(0,2));
			int previouselapsedseconds=int.Parse(newlifetimer.GetComponent<TextMesh>().text.Substring(3,2));
			int totalelapsedseconds=(((1800-(previouselapsedminutes*60))-((00-previouselapsedseconds)*-1)));
			PlayerPrefs.SetInt("totalelapsedseconds",totalelapsedseconds);
		}
		else
		{
			runcodeonresume();
		}
	}

	void OnApplicationQuit() {
			savecurrenttime();
		}


	void savecurrenttime()
	{
		PlayerPrefs.SetString("oldtime",DateTime.Now.ToBinary().ToString());
		int previouselapsedminutes=int.Parse(newlifetimer.GetComponent<TextMesh>().text.Substring(0,2));
		int previouselapsedseconds=int.Parse(newlifetimer.GetComponent<TextMesh>().text.Substring(3,2));
		int totalelapsedseconds=(((1800-(previouselapsedminutes*60))-((00-previouselapsedseconds)*-1)));
		PlayerPrefs.SetInt("totalelapsedseconds",totalelapsedseconds);
	}

	void runcodeonresume()
	{
	int wasgamequit=PlayerPrefs.GetInt("gamequit");
	if((totallives<5 && runcounter>0 && fromthis==1) || (wasgamequit==1 && totallives<5))
	{
			fromthis=0;
			currenttime=DateTime.Now;
			long temp=Convert.ToInt64(PlayerPrefs.GetString("oldtime"));
			
			oldtime=DateTime.FromBinary(temp);
			
			TimeSpan difference = currenttime.Subtract(oldtime);
			int totaldifferenceseconds=(int)difference.TotalSeconds;
			
			//Previous pause elapsed seconds
			int previouslyelapsedseconds=PlayerPrefs.GetInt("totalelapsedseconds");
			
			//Final difference seconds computed by adding differenceseconds + previously already elapsed seconds for new life 
			int finaldifferenceseconds=totaldifferenceseconds+previouslyelapsedseconds;
			
			//Increase the life on the difference
			if(finaldifferenceseconds>=1800  && finaldifferenceseconds< 3600)
			{
				//Increase 1 life
				totallives +=1;
				totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-3600);
			}
			
			else if(finaldifferenceseconds>=3600 && finaldifferenceseconds< 5400)
			{
				//Increase 2 Life
				totallives+=2;
				totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-5400);
			}
			
			else if(finaldifferenceseconds>=5400  && finaldifferenceseconds<7200)
			{
				//Increase 3 Life
				totallives+=3;
				totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-7200);
			}                                                                                         
			else if(finaldifferenceseconds>=7200 && finaldifferenceseconds<9000)
			{
				//Increase 4 Life
				totallives+=4;
				totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-9000);
			}
			else if(finaldifferenceseconds>=9000)
			{
				//Increase 5 Life 
				totallives+=5;
			}
			else if(finaldifferenceseconds<1800)
			{
				totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-1800);
			}
			
			if(totallives>=5)
			{
				//No need for timer as lives becomes full.
				totallives=5;
				lives_textmesh.GetComponent<TextMesh>().text=totallives.ToString();
				managetimerfornewlife(false);
				newlifetimer.GetComponent<TextMesh>().text="Full";
			}		
			else
			{
				//Set the new required timer
				resettimerfornewlifeonresume(totaltimrfornewlife_onresume);
				managetimerfornewlife(true);
			}
		}
		PlayerPrefs.SetInt("gamequit",0);
	}


	void muteallaudiosourcesinscene()
	{
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj)
		{
			temp=(GameObject) o;
			if(temp.GetComponent<AudioSource>()!=null)
			{
				temp.GetComponent<AudioSource>().mute=true;
			}
		}
	}


	
	void unmuteallaudiosourcesinscene()
	{
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj)
		{
			temp=(GameObject) o;
			if(temp.GetComponent<AudioSource>()!=null)
			{
				temp.GetComponent<AudioSource>().mute=false;
			}
		}
	}


	/*
	void muteaudiolistener()
	{
		this.gameObject.GetComponent<AudioListener>().enabled=false;
	}

	void unmuteaudiolistener()
	{
		this.gameObject.GetComponent<AudioListener>().enabled=true;
	}
	*/


	void thememusicplay()
	{
		this.audio.Play();
		this.audio.loop=true;
	}

	private void SetInit()
	{
		Debug.Log("Facebook SetInit");
		//enabled = true; // "enabled" is a property inherited from MonoBehaviour
		if (FB.IsLoggedIn)
		{
			Debug.Log("Facebook Already logged in");
			//OnLoggedIn();
		}
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Facebook OnHideUnity");
		//if (!isGameShown)
		//{
		// pause the game - we will need to hide
		//  Time.timeScale = 0;
		//}
		//else
		//{
		// start the game back up - we're getting focus again
		//  Time.timeScale = 1;
		//}
	}
	
	void LoginCallback(FBResult result)
	{
		Debug.Log("LoginCallback");
		
		if (FB.IsLoggedIn)
		{
			OnLoggedIn();
			
		}
	}
	
	void OnLoggedIn()
	{
		Debug.Log("Logged in. ID: " + FB.UserId);
		
		// Reqest player info and profile picture
		FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
		//LoadPicture(Util.GetPictureURL("me", 128, 128),MyPictureCallback);
		
	}

	void APICallback(FBResult result)
	{
		Dictionary<string, string>   profile         = null;
		Debug.Log("APICallback");
		if (result.Error != null)
		{
			Debug.LogError(result.Error);
			// Let's just try again
			FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
			return;
		}
		
		profile = Util.DeserializeJSONProfile(result.Text);
		Debug.Log ("Facebook info: " + result.Text);
		Debug.Log("Facebook " + profile["first_name"]);
		//friends = Util.DeserializeJSONFriends(result.Text);
		StartCoroutine("showleaderboardui");
		StartCoroutine ("flipFacebookButtons");
	}

	
	IEnumerator flipFacebookButtons()
	{
		fbPreLogin.transform.Translate(7, 0, 0);
		fbPreLogin.renderer.enabled = false;
		fbPostLogin.transform.Translate (-7, 0, 0);
		fbPostLogin.renderer.enabled = true;
		yield return new WaitForEndOfFrame();
	}

	
	
}




