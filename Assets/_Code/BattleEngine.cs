using UnityEngine;
using System.Collections;

public class BattleEngine : MonoBehaviour {

	public ProgressBarManager progressBarManager;

	public CategorySelect categorySelect;

	public Sprite[] defaultSizeAsteroids;
	public Asteroids asteroids;
	public Vector3 bigAsteroidScale;
	public Vector3 smallAsteroidScale;
	public Vector2 asteroidSpawnStartRange;

	public Sprite[] defaultCategorySprites;

	private LevelManager levels;
	[HideInInspector]
	public bool canTarget;
	private bool isMouseDown;
	public GameObject AtkTarget;
	private AttackSystem AtkSystem;
	private Characters character;
	private GameObject WinLoseTextAsset;
	private bool isEndGame;
	public bool LastHitMiss;
	public bool isgamewon;
	private Transform gun;
	private Vector3 dir;

	public Font font;
	public Material[] FontMats;

	public static Font font1;
	public static Material[] material1;

	public bool FinishedRound;

	//Related to sound
	GameObject soundmanager;

	public GameObject buttonmanager;

	private LevelInfo levelInfo;

	private XPLevelInfoCollection xpLevelInfoCollection;

	private void Awake()
	{
		levelInfo = GameObject.FindObjectOfType<LevelInfo> ();

		isEndGame = false;
		AtkTarget = (GameObject) GameObject.Find("ATTACK_TARGET");
		WinLoseTextAsset = (GameObject) GameObject.Find("WinLoseText");
		isMouseDown = false;
		canTarget = false;
		categorySelect = (CategorySelect) this.gameObject.AddComponent<CategorySelect>();
		categorySelect.defaultCategorySprites = defaultCategorySprites;

		asteroids = (Asteroids) this.gameObject.AddComponent<Asteroids>();
		asteroids.bigAsteroidScale = bigAsteroidScale;
		asteroids.smallAsteroidScale = smallAsteroidScale;
		asteroids.defaultSizeAsteroids = defaultSizeAsteroids;

		AtkSystem = (AttackSystem) this.gameObject.AddComponent<AttackSystem>();
		character = (Characters) this.gameObject.AddComponent<Characters>();
		levels = (LevelManager) this.gameObject.AddComponent<LevelManager>();
		levels.currentLevel = 0;
		levels.currentINC = 0;
		Invoke("getgun",0.75f);
		font1=font;
		material1=FontMats;
		soundmanager=GameObject.Find("Secondary_SoundManager");

		xpLevelInfoCollection = GameObject.FindObjectOfType<XPLevelInfoCollection> ();
	}

	private IEnumerator Start()
	{	
		int spawnCount = Random.Range (2, 4);
		yield return StartCoroutine(asteroids.SpawnAsteroids(spawnCount));
		
		progressBarManager.UpdateProgressBar (0);
		progressBarManager.StartTimer ();

		//yield return StartCoroutine(asteroids.SpawnAsteroids(levels.GetSpawnCountAutoINC()));
		categorySelect.PlaceCategories(0, true);
		canTarget = true;
		yield return 0; 
	}

	void Update() {
		DebugDamageAsteroid ();
	}

	private void DebugDamageAsteroid() {
		if (Input.GetKeyDown(KeyCode.K)) {
			Asteroids.Asteroid asteroid = asteroids.currentAsteroids[0];

			CategorySelect.ColorTypes color = (CategorySelect.ColorTypes) asteroid.colorType;

			StartCoroutine(AtkSystem.AttackTarget(asteroid.obj.transform.position,asteroids.currentAsteroids, 
			                                      levels, color));

			StartCoroutine(NextRound(true,color));
		}

		if (Input.GetKeyDown(KeyCode.M)) {
			int totalRocks = levelInfo.selectedNodeInfo.totalRocks;
			if(asteroids.asteroidsDestroyed >= totalRocks)
			{
				progressBarManager.StopTimer();
				WinBattle(progressBarManager.GetCompletionTimeSeconds());

				UpdateXPLevel();
			}
		}
	}

	public void SetAttackToNierestEnemy()
	{
		Vector3 pos = asteroids.GetPositionToNierestEnemy();
		Vector3 fixedPos = new Vector3(pos.x,pos.y,-1.9f);
		AtkTarget.transform.position = fixedPos;
	}

	public IEnumerator NextRound(bool isHitTarget, CategorySelect.ColorTypes currentColorType)
	{
		FinishedRound = true;
		if(ButtonManager.powerupselected=="double_blast_radius")
		{
			ButtonManager.reducepowerupcount("double_blast_radius");
		}
		else if(ButtonManager.powerupselected=="bomb")
		{
			ButtonManager.reducepowerupcount("bomb");
		}
	

		categorySelect.animationIsPlaying = true;
		Vector3 target = AtkTarget.transform.position;
		categorySelect.targetSelect = false;
		AtkTarget.transform.position = new Vector3(-100,0,0);
		categorySelect.PlaceCategories(0, false);
		canTarget = true;
		isMouseDown = false;

		GA.API.Business.NewEvent("New Round", "round", 1);

		if(isHitTarget)
		{
			yield return StartCoroutine(AtkSystem.AttackTarget(target,asteroids.currentAsteroids,levels,currentColorType));
			soundmanager.GetComponent<SoundManager>().weapon_attack_soundplay();
		}
		else
		{
			yield return StartCoroutine(AtkSystem.MissTarget());
		}

//		bool isAnyLeft = asteroids.CheckIfAnyExists();
//		bool isLevelProgressFull = levels.CheckIfProgressIfFull();
		int totalRocks = levelInfo.selectedNodeInfo.totalRocks;

		//New Changes ***
		//if(isLevelProgressFull)
		if(asteroids.asteroidsDestroyed >= totalRocks)
		{
			progressBarManager.StopTimer();
			WinBattle(progressBarManager.GetCompletionTimeSeconds());

			UpdateXPLevel ();
		}
		//
		else
		{
			yield return StartCoroutine(asteroids.MoveAsteroids());

			yield return new WaitForSeconds(0.1f);

			int spawnCount = Random.Range(2, 4);
			StartCoroutine(asteroids.SpawnAsteroids(spawnCount));

//			int spawnINC = levels.GetSpawnCountAutoINC();
//			StartCoroutine(asteroids.SpawnAsteroids(spawnINC));

			if(!LastHitMiss&&spawnCount!=0)
			{
				//New
//				StartCoroutine(levels.UpdateLevelProgressBarForAsteriodsSpawned(1));
				//
			}

			bool isAnyCrossingTheLine = asteroids.CheckIfAnyCrossesTheLine();

			if(isAnyCrossingTheLine)
			{
				LoseBattle();
			}
		}

		categorySelect.animationIsPlaying = false;

		yield return 0;
	}

	public void OnAsteroidDestroyed() {
		asteroids.asteroidsDestroyed++;
		progressBarManager.UpdateProgressBar (asteroids.asteroidsDestroyed);
	}

	private void WinBattle(int score)
	{
		isEndGame = true;
		soundmanager.GetComponent<SoundManager>().mutemaintheme_sound();
		StartCoroutine(buttonmanager.GetComponent<ButtonManager>().showrewardsscreen(score));
		StartCoroutine(categorySelect.QE.ShowFadeBG(false,true));
		isgamewon=true;
		ButtonManager.gameover=true;
		StartCoroutine(soundmanager.GetComponent<SoundManager>().winbattle_soundplay());
		StartCoroutine(categorySelect.HideCategories());
		StartCoroutine(ShowEndBattleText("YOU WIN!",true));

		int currentLevel = PlayerPrefs.GetInt (PlayerData.CurrentLevelKey, 1);
		currentLevel += 1;

		LevelCompleteInfo levelCompleteInfo = GameObject.FindObjectOfType<LevelCompleteInfo> ();
		if (levelCompleteInfo != null) {
			levelCompleteInfo.levelUnlocked = currentLevel;
		}
	}

	private void LoseBattle()
	{
		isgamewon=false;
		if(mainmenu.totallives!=0)
		{
		mainmenu.totallives--;
		}

		soundmanager.GetComponent<SoundManager>().mutemaintheme_sound();

		StartCoroutine(buttonmanager.GetComponent<ButtonManager>().showlosescreen());
	
		PlayerPrefs.SetInt("totallives",mainmenu.totallives);
		ButtonManager.gameover=true;
		isEndGame = true;
		StartCoroutine(categorySelect.QE.ShowFadeBG(false,true));
		StartCoroutine(categorySelect.HideCategories());
		StartCoroutine(soundmanager.GetComponent<SoundManager>().lostbattle_soundplay());
		StartCoroutine(ShowEndBattleText("YOU LOSE!",false));


		if(mainmenu.totallives<5)
		{
			Debug.Log("true");
			if(mainmenu.timerstarted==false)
			{
			Debug.Log("going here");
			mainmenu.resettimerfornewlife();
			mainmenu.managetimerfornewlife(true);
			}
		}
	}

	void UpdateXPLevel ()
	{
		ulong totalXP = ulong.Parse (PlayerPrefs.GetString (PlayerData.TotalXPKey, "0"));
		XPLevelInfo currentXPLevelInfo = xpLevelInfoCollection.GetCurrentLevelInfo (totalXP);
		if (currentXPLevelInfo != null) {
			PlayerPrefs.SetInt (PlayerData.CurrentXPLevel, currentXPLevelInfo.level);
		}
	}

	private IEnumerator ShowEndBattleText(string str, bool isItWin)
	{
		WinLoseTextAsset.GetComponent<TextMesh>().text = str;
		
		if(isItWin)
		{
			WinLoseTextAsset.GetComponent<TextMesh>().color = Color.green;
		}
		else
		{
			WinLoseTextAsset.GetComponent<TextMesh>().color = Color.red;
		}

		for(int i=0;i<10;i++)
		{
			WinLoseTextAsset.transform.localScale += new Vector3(0.05f,0.05f,0.05f);
			yield return 0;
		}

		yield return 0;
	}

	private void OnGUI()
	{
		if(isEndGame)
		{
			if(isgamewon==false)
			{
				if(mainmenu.totallives!=0)
				{
					/*if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2-25,200,50),"Restart You still have "+mainmenu.totallives.ToString()+" Lives"))
					{
						Application.LoadLevel("MainScene");
					}
					if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2+50,200,50),"Go To Main Menu"))
					{
						Application.LoadLevel("MenuScene");
					}
					*/
				}
				else
				{
					//if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2-25,200,50),"You have lost all your lives, Please buy more lives or proceed to main menu"))
					//{
					//	Application.LoadLevel("MenuScene");
					//}

					//if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2+50,200,50),"Buy More Lives"))
					//{
					////Debug.Log("Does nothing");
					//}
				}
			}
			else if(isgamewon==true)
			{
				/*
				//Temperory Data
				if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2-25,200,50),"Rewards Section"))
				   {

				   }
				if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2+50,200,50),"Go To Main Menu"))
				{
					Application.LoadLevel("MenuScene");
				}
				*/
			}
		}
	}

	private void LateUpdate()
	{
		if(canTarget)
		{
			if(Input.GetMouseButtonDown(0))
			{
				isMouseDown = true;
			}

			if(Input.GetMouseButtonUp(0))
			{
				isMouseDown = false;
			}

			if(isMouseDown)
			{
				if(ButtonManager.canmovemarker)
				{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if(hit.collider)
				{
					if(hit.collider.gameObject.name.Equals("BackgroundLayer"))
					{
						categorySelect.targetSelect = true;
						AtkTarget.transform.position = hit.point;
						AtkTarget.transform.position += new Vector3(0,0,-1.9f);

						//New Code for the character aiming
						if(Input.mousePosition.x<Screen.width/2)
							{
								leftsidetouched();
							}
						else if(Input.mousePosition.x>Screen.width/2)
							{
								rightsidetouched();
							}
					
						//The old logic, for the character programming is ignored and new logic is considered for the programming which has
						//two methods leftsidetouched and rightsidetouched that manage the rotation of the player and the rotation of the gun

						/*
						Vector3 relative = transform.InverseTransformPoint(AtkTarget.transform.position);
						float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
						character.currentChar.transform.localEulerAngles = new Vector3(0,0,angle);
						*/
					}
				}
			}
				}
		}
	}

	private void GoToMainMenu()
	{
		GameObject.Destroy(levelInfo.gameObject);
		Application.LoadLevel(0);
	}

	private void getgun()
	{
		gun=character.currentChar.transform.GetChild(0);
	}

	private void leftsidetouched()
	{
		character.currentChar.transform.eulerAngles=new Vector3(0f,180f,0f);
		gun.transform.localScale=new Vector3(gun.transform.localScale.x,-1f,gun.transform.localScale.z);
		dir=AtkTarget.transform.position-gun.transform.position;
		gun.transform.rotation = Quaternion.Euler (new Vector3 (0,0,Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg));
	}

	private void rightsidetouched()
	{
		character.currentChar.transform.eulerAngles=new Vector3(0f,360f,0f);
		gun.transform.localScale=new Vector3(gun.transform.localScale.x,1f,gun.transform.localScale.z);
		dir=AtkTarget.transform.position-gun.transform.position;
		gun.transform.rotation = Quaternion.Euler (new Vector3 (0,0,Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg));
	}
}