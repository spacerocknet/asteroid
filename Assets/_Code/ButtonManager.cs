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



	//Public static GameObjects to access on the reduction
	public static GameObject hitpowerup_static_gameobject;
	public static GameObject doubleblastradius_static_gameobject;
	public static GameObject reversetime_static_gameobject;
	public static GameObject changequestioncategory_Static_gameobject;

	public static bool canmovemarker;
	GameObject character;

	//SoundMnager
	private GameObject soundmanager;

	private GameObject soundmanager1;


	public static bool gameover;

	//Rewards
	public GameObject rewardscreen;

	//Lose Screen
	public GameObject losescreen;

	//Lives
	public GameObject buymorelives;

	//New Life Timer
	public GameObject livestimer;

	//Basic Button Click
	public GameObject basic_button_click;

	//Quit Popup
	public GameObject quit_popup;

	//Date
	DateTime currenttime;
	DateTime oldtime;

	public int totaltimrfornewlife_onresume;

	private LevelInfo levelInfo;
	private PlayerData playerDataManager;

	int runcounter;

	int newlifetimer_minutes;
	int newlifetimer_seconds;
	

	private int fromthis;

	GameObject temp;

	void Awake()
	{
		levelInfo = GameObject.FindObjectOfType<LevelInfo> ();

		fromthis=0;
		gameover=false;
		canmovemarker=true;
		hitpowerup_static_gameobject=GameObject.Find("button_power_up_02");
		doubleblastradius_static_gameobject=GameObject.Find("button_power_up_03");
		reversetime_static_gameobject=GameObject.Find("button_power_up_04");
		changequestioncategory_Static_gameobject=GameObject.Find("button_power_up_05");

		powerupselectedcolor=new Color(139f,142f,142f,0.5f);
		powerupnormalcolor=new Color(255f,255f,255f,1f);
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
		//Invoke("removetopwallcollider",1.65f);
		Invoke("getcharacter",0.10f);
		soundmanager=GameObject.Find("Powerup_SoundManager");
		soundmanager1=GameObject.Find("Secondary_SoundManager");

		playerDataManager = GameObject.FindObjectOfType<PlayerData> ();
	}

	void Start()
	{
		if(mainmenu.sound==1)
		{
			unmuteallaudiosourcesinscene();
		}
		else if(mainmenu.sound==0)
		{
			muteallaudiosourcesinscene();
		}
		Invoke("thememusicplay",0.12f);
	}

	
	void Update () {

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) 
		{
			touchposition=Input.GetTouch(0).position;
			touchended();
		}

#if UNITY_EDITOR
		if (Input.GetMouseButtonUp(0)) {
			touchposition = Input.mousePosition;
			touchended();
		}
#endif
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
			//Application.LoadLevel(0);
			if(gameover==false)
			{
			StartCoroutine("showquitpopup");
			}
			else
			{
				GameObject.Destroy(levelInfo.gameObject);
				playerDataManager.PostPlayerData();

				Application.LoadLevel(0);
			}
		}

		if(mainmenu.timerstarted==true)
		{
			mainmenu.cachetotaltimefornewlife -= Time.deltaTime;
			mainmenu.ts=TimeSpan.FromSeconds(mainmenu.cachetotaltimefornewlife);
			livestimer.GetComponent<TextMesh>().text=mainmenu.ts.ToString().Substring(3,5);
			Invoke("calculateminutesandseconds",0.15f);
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
					//mainmenu.managetimerfornewlife(true);
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
			if(gameover==false)
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
							if(PlayerPrefs.GetInt(PlayerData.BombPowerUpsKey)>0)
							{
								if(powerupselected=="double_blast_radius")
									{
									attack_target.transform.localScale=new Vector3(0.6f,0.6f,0f);
									double_blastpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
									}
								powerupselected="bomb";
								//ToggleHere	
								hit.collider.gameObject.GetComponent<SpriteRenderer>().color=powerupselectedcolor;
								soundmanager1.GetComponent<SoundManager>().powerupcategory_select_soundplay();	
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
						if(PlayerPrefs.GetInt(PlayerData.DoubleBlastRadiusPowerUpsKey)>0)
							{
							powerupselected="double_blast_radius";
							attack_target.transform.localScale=new Vector3(1.2f,1.2f,0f);
							//ToggleHere
							hit.collider.gameObject.GetComponent<SpriteRenderer>().color=powerupselectedcolor;
							double_hitpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
							soundmanager1.GetComponent<SoundManager>().powerupcategory_select_soundplay();
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
					if(PlayerPrefs.GetInt(PlayerData.ReverseTimePowerUpsKey)>0)
						{
							if(powerupselected=="double_blast_radius")
							{
							attack_target.transform.localScale=new Vector3(0.6f,0.6f,0f);
							double_blastpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
							}
							double_hitpowerup_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
							powerupselected="reverse_time";
							soundmanager.audio.Play();
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
							if(PlayerPrefs.GetInt(PlayerData.ChageQuestionCategoryPowerUpsKey)>0)
							{
								if(powerupselected=="double_blast_radius")
								{
								attack_target.transform.localScale=new Vector3(1.2f,1.2f,0f);
								}
								soundmanager1.GetComponent<SoundManager>().powerupcategory_select_soundplay();	
								character.GetComponent<SpriteRenderer>().enabled=false;
								character.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled=false;
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

					//Activated if Button_Okay_Rewards_Screen

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
					soundmanager.audio.Play();
					MAIN.GetComponent<BattleEngine>().categorySelect.PlaceCategoriesByCategoryChangePowerup("blue");
					}

					else if(hit.collider.gameObject.name=="button_greencolor")
					{
					fadebgalphaandtriggerdisable();	
					StartCoroutine("hidequestionchangecatwindow");
					reducepowerupcount(powerupselected);
				soundmanager.audio.Play();
					MAIN.GetComponent<BattleEngine>().categorySelect.PlaceCategoriesByCategoryChangePowerup("green");
					}

					else if(hit.collider.gameObject.name=="button_redcolor")
					{
					fadebgalphaandtriggerdisable();
					StartCoroutine("hidequestionchangecatwindow");
					reducepowerupcount(powerupselected);
					soundmanager.audio.Play();
					MAIN.GetComponent<BattleEngine>().categorySelect.PlaceCategoriesByCategoryChangePowerup("red");
					}

					//For the quit game button
					else if (hit.collider.gameObject.name=="button_continue")
					{
					basic_button_click.audio.Play();
					StartCoroutine("hidequitpopup");
					}

					else if (hit.collider.gameObject.name=="button_quit")
					{
					basic_button_click.audio.Play();
					mainmenu.totallives--;
					PlayerPrefs.SetInt("totallives",mainmenu.totallives);
				

					if(mainmenu.totallives<5)
					{
						if(mainmenu.timerstarted==false)
						{
							mainmenu.resettimerfornewlife();
							mainmenu.managetimerfornewlife(true);
						}
					}

						GameObject.Destroy(levelInfo.gameObject);
						playerDataManager.PostPlayerData();
						Application.LoadLevel(0);
					}

				}
		else
				{
				canmovemarker=true;
				}

		}

		//Only works if gameover is true
		else if(gameover==true)
		{
			hit=Physics2D.Raycast(camera.ScreenToWorldPoint(new Vector3(touchposition.x,touchposition.y,0)),Vector2.zero,Mathf.Infinity,layermask);
			if(hit.collider!=null)
			{
				if(hit.collider.gameObject.name=="button_okay_rewards")
				{
					basic_button_click.audio.Play();
					button_okay_rewardsscreen_clicked();
				}

				else if(hit.collider.gameObject.name=="retry")
				{
					 if(mainmenu.totallives!=0)
					{
					 basic_button_click.audio.Play();
					 reloadscene();
					}
					else
					{
					//Show the popup for buy lives and hide the current popup.
					 basic_button_click.audio.Play();
					 StartCoroutine("hidelosescreenandshowbuylives");
					}
				}

				else if(hit.collider.gameObject.name=="home")
				{
					GameObject.Destroy(levelInfo.gameObject);
					playerDataManager.PostPlayerData();

					Application.LoadLevel(0);
				}

				else if(hit.collider.gameObject.name=="button_buy")
				{
					if(mainmenu.totalgold>=200)
					{
						basic_button_click.audio.Play();
						mainmenu.totallives++;
						mainmenu.totalgold -=200;
						PlayerPrefs.SetInt("totallives",mainmenu.totallives);
						PlayerPrefs.SetInt("totalgold",mainmenu.totalgold);

						if(mainmenu.totallives>=5)
						{
								mainmenu.managetimerfornewlife(false);
								livestimer.GetComponent<TextMesh>().text="Lives Full";
						}
						reloadscene();
					}
				}

				else if(hit.collider.gameObject.name=="button_close")
				{
					playerDataManager.PostPlayerData();

					if(mainmenu.totallives==0)
					{
						basic_button_click.audio.Play();
						GameObject.Destroy(levelInfo.gameObject);
						Application.LoadLevel(0);
					}
					else
					{
						basic_button_click.audio.Play();
						Application.LoadLevel(Application.loadedLevel);
					}
				}
			}
		}
	}

	public static void reducepowerupcount(string powerup)
	{
			if(powerup=="bomb")
			{
			int bombPowerUpCount = PlayerPrefs.GetInt(PlayerData.BombPowerUpsKey);
			bombPowerUpCount--;
			ButtonManager.powerupselected=String.Empty;
			bombtextmesh.GetComponent<TextMesh>().text = bombPowerUpCount.ToString();
			PlayerPrefs.SetInt(PlayerData.BombPowerUpsKey, bombPowerUpCount);
			hitpowerup_static_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
			}
			else if(powerup=="double_blast_radius")
			{
			attack_target.transform.localScale=new Vector3(0.6f,0.6f,0f);

			int doubleBlastPowerUpCount = PlayerPrefs.GetInt(PlayerData.DoubleBlastRadiusPowerUpsKey);

			doubleBlastPowerUpCount--;
			ButtonManager.powerupselected=String.Empty;
			doubleblastradiustextmesh.GetComponent<TextMesh>().text=doubleBlastPowerUpCount.ToString();
			PlayerPrefs.SetInt(PlayerData.DoubleBlastRadiusPowerUpsKey, doubleBlastPowerUpCount);
			doubleblastradius_static_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
			}
			else if(powerup=="reverse_time")
			{
			ButtonManager.powerupselected=String.Empty;

			int reversTimePowerUpCount = PlayerPrefs.GetInt(PlayerData.ReverseTimePowerUpsKey);

			reversTimePowerUpCount--;
			reversetimetextmesh.GetComponent<TextMesh>().text=reversTimePowerUpCount.ToString();
			PlayerPrefs.SetInt(PlayerData.ReverseTimePowerUpsKey, reversTimePowerUpCount);
			reversetime_static_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
			}
			else if(powerup=="change_question_category")
			{
			ButtonManager.powerupselected=String.Empty;

			int changeCatPowerUpsCount = PlayerPrefs.GetInt(PlayerData.ChageQuestionCategoryPowerUpsKey);
			changeCatPowerUpsCount--;
			changequestioncategoriestextmesh.GetComponent<TextMesh>().text=changeCatPowerUpsCount.ToString();
			PlayerPrefs.SetInt(PlayerData.ChageQuestionCategoryPowerUpsKey, changeCatPowerUpsCount);
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
			float newposition=Mathf.Lerp(oldposition.x,0f,0.25f);
			changequestioncategorycolor.transform.position=new Vector3(newposition,changequestioncategorycolor.transform.position.y,changequestioncategorycolor.transform.position.z);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
	}
		
	IEnumerator hidequestionchangecatwindow()
	{
		character.GetComponent<SpriteRenderer>().enabled=true;
		character.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled=true;
		for(int i=0;i<40;i++)
		{
			Vector3 oldposition=changequestioncategorycolor.transform.position;
			float newposition=Mathf.Lerp(oldposition.x,7f,0.25f);
			changequestioncategorycolor.transform.position=new Vector3(newposition,changequestioncategorycolor.transform.position.y,changequestioncategorycolor.transform.position.z);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		changequestioncategory_gameobject.GetComponent<SpriteRenderer>().color=powerupnormalcolor;
	}

	
	void fadebgalphaandtriggerdisable()
	{
		MAIN.GetComponent<BattleEngine>().categorySelect.enablequestioncollidersandtriggers();
		fadebg.GetComponent<BoxCollider2D>().enabled=false;
		fadebgcolor=fadebg.GetComponent<SpriteRenderer>().color;
		fadebgcolor.a=0;
		fadebg.GetComponent<SpriteRenderer>().color=fadebgcolor;
	}

	private void getcharacter()
	{
		character=GameObject.Find("Character(Clone)");
	}


	//show the rewards screen 
	public IEnumerator showrewardsscreen(int score)
	{
		GameObject goldRewardText = rewardscreen.transform.FindChild ("gold_reward_text").gameObject;
		goldRewardText.GetComponent<TextMesh> ().text = levelInfo.selectedNodeRewardInfo.goldPayout.ToString ();

		int currentTotalGold = PlayerPrefs.GetInt (PlayerData.TotalGoldKey, 0);
		PlayerPrefs.SetInt(PlayerData.TotalGoldKey, currentTotalGold + levelInfo.selectedNodeRewardInfo.goldPayout);
		
		GameObject xpRewardText = rewardscreen.transform.FindChild ("xp_reward_text").gameObject;
		xpRewardText.GetComponent<TextMesh> ().text = levelInfo.selectedNodeRewardInfo.xpPayout.ToString ();

		ulong currentTotalXP = ulong.Parse(PlayerPrefs.GetString (PlayerData.TotalXPKey, "0"));
		ulong cumulativeXP = currentTotalXP + (ulong) levelInfo.selectedNodeRewardInfo.xpPayout;
		PlayerPrefs.SetString (PlayerData.TotalXPKey, cumulativeXP.ToString());

		GameObject scoreText = rewardscreen.transform.FindChild ("score_text").gameObject;
		scoreText.GetComponent<TextMesh> ().text = score.ToString();

		int currentTotalScore = PlayerPrefs.GetInt (PlayerData.TotalScoreKey, 0);
		PlayerPrefs.SetInt (PlayerData.TotalScoreKey, currentTotalScore + score);

		for(int i=0;i<20;i++)
		{
			Vector3 oldposition=rewardscreen.transform.position;
			float newposition=Mathf.Lerp(oldposition.x,0f,0.25f);
			rewardscreen.transform.position=new Vector3(newposition,rewardscreen.transform.position.y,rewardscreen.transform.position.z);
			//rewardscreen.transform.position=new Vector3(0f,0f,0f);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator showlosescreen()
	{
		GameObject levelFailedText = losescreen.transform.FindChild ("Level Failed").gameObject;
		levelFailedText.GetComponent<TextMesh> ().text = levelInfo.selectedNodeInfo.level.ToString();

		for(int i=0;i<20;i++)
		{
			Vector3 oldposition=losescreen.transform.position;
			float newposition=Mathf.Lerp(oldposition.x,-0.04f,0.25f);
			losescreen.transform.position=new Vector3(newposition,losescreen.transform.position.y,losescreen.transform.position.z);
			//rewardscreen.transform.position=new Vector3(0f,0f,0f);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator hidelosescreenandshowbuylives()
	{
		for(int i=0;i<12;i++)
		{
			
			Vector3 oldposition=losescreen.transform.position;
			float newposition=Mathf.Lerp(oldposition.x,-7f,0.15f);
			losescreen.transform.position=new Vector3(newposition,losescreen.transform.position.y,losescreen.transform.position.z);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		StartCoroutine("buylives");
	}

	public IEnumerator buylives()
	{
		for(int i=0;i<20;i++)
		{
			Vector3 oldposition=buymorelives.transform.position;
			float newposition=Mathf.Lerp(oldposition.x,0f,0.25f);
			buymorelives.transform.position=new Vector3(newposition,buymorelives.transform.position.y,buymorelives.transform.position.z);
			//rewardscreen.transform.position=new Vector3(0f,0f,0f);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator showquitpopup()
	{
		StartCoroutine("hidequestionchangecatwindow");
		for(int i=0;i<20;i++)
		{
			Vector3 oldposition=quit_popup.transform.position;
			float newposition=Mathf.Lerp(oldposition.x,-0.04f,0.25f);
			quit_popup.transform.position=new Vector3(newposition,quit_popup.transform.position.y,quit_popup.transform.position.z);
			//rewardscreen.transform.position=new Vector3(0f,0f,0f);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator hidequitpopup()
	{
		for(int i=0;i<20;i++)
		{
			Vector3 oldposition=quit_popup.transform.position;
			float newposition=Mathf.Lerp(oldposition.x,9f,0.25f);
			quit_popup.transform.position=new Vector3(newposition,quit_popup.transform.position.y,quit_popup.transform.position.z);
			//rewardscreen.transform.position=new Vector3(0f,0f,0f);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
	}

	void button_okay_rewardsscreen_clicked()
	{
		GameObject.Destroy (levelInfo.gameObject);
		playerDataManager.PostPlayerData();

		Application.LoadLevel(0);
	}

	void reloadscene()
	{
		playerDataManager.PostPlayerData();
		Application.LoadLevel(Application.loadedLevel);
	}


	//For background timer manager
	void OnApplicationPause (bool pause)
	{

		
		if(pause)
		{
			fromthis=1;
			runcounter++;
			PlayerPrefs.SetInt("runcounter",runcounter);
			PlayerPrefs.SetString("oldtime",DateTime.Now.ToBinary().ToString());
			int previouselapsedminutes=newlifetimer_minutes;
			int previouselapsedseconds=newlifetimer_seconds;
			int totalelapsedseconds=(((1800-(previouselapsedminutes*60))-((00-previouselapsedseconds)*-1)));
			PlayerPrefs.SetInt("totalelapsedseconds",totalelapsedseconds);
		}
		else
		{
			//Resume
			if(mainmenu.totallives<5 && runcounter>0 &&fromthis==1)
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
					mainmenu.totallives +=1;
					totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-3600);
				}
				
				else if(finaldifferenceseconds>=3600 && finaldifferenceseconds< 5400)
				{
					//Increase 2 Life
					mainmenu.totallives+=2;
					totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-5400);
				}
				
				else if(finaldifferenceseconds>=5400  && finaldifferenceseconds<7200)
				{
					//Increase 3 Life
					mainmenu.totallives+=3;
					totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-7200);
				}                                                                                         
				else if(finaldifferenceseconds>=7200 && finaldifferenceseconds<9000)
				{
					//Increase 4 Life
					mainmenu.totallives+=4;
					totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-9000);
				}
				else if(finaldifferenceseconds>=9000)
				{
					//Increase 5 Life 
					mainmenu.totallives+=5;
				}
				else if(finaldifferenceseconds<1800)
				{
					totaltimrfornewlife_onresume=Mathf.Abs(finaldifferenceseconds-1800);
				}
				
				if(mainmenu.totallives>=5)
				{
					//No need for timer as lives becomes full.
					mainmenu.totallives=5;
					mainmenu.managetimerfornewlife(false);
	
				}
				else
				{
					//Set the new required timer
					mainmenu.resettimerfornewlifeonresume(totaltimrfornewlife_onresume);
					mainmenu.managetimerfornewlife(true);
				}
			} 
		}
	}
	
	void calculateminutesandseconds()
	{
		string temp1=mainmenu.ts.ToString().Substring(3,2);
		string temp2=mainmenu.ts.ToString().Substring(6,2);
		newlifetimer_minutes=int.Parse(temp1);
		newlifetimer_seconds=int.Parse(temp2);
	}

	//Sound management
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

	void thememusicplay()
	{
		this.audio.Play();
		this.audio.loop=true;
	}
}