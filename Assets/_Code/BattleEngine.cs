using UnityEngine;
using System.Collections;

public class BattleEngine : MonoBehaviour {

	private CategorySelect categorySelect;
	public Asteroids asteroids;
	private LevelManager levels;
	[HideInInspector]
	public bool canTarget;
	private bool isMouseDown;
	private GameObject AtkTarget;
	private AttackSystem AtkSystem;
	private Characters character;
	private GameObject WinLoseTextAsset;
	private bool isEndGame;
	public bool LastHitMiss;
	public bool isgamewon;

	private void Awake()
	{
		isEndGame = false;
		AtkTarget = (GameObject) GameObject.Find("ATTACK_TARGET");
		WinLoseTextAsset = (GameObject) GameObject.Find("WinLoseText");
		isMouseDown = false;
		canTarget = false;
		categorySelect = (CategorySelect) this.gameObject.AddComponent<CategorySelect>();
		asteroids = (Asteroids) this.gameObject.AddComponent<Asteroids>();
		AtkSystem = (AttackSystem) this.gameObject.AddComponent<AttackSystem>();
		character = (Characters) this.gameObject.AddComponent<Characters>();
		levels = (LevelManager) this.gameObject.AddComponent<LevelManager>();

		levels.currentLevel = 0;
		levels.currentINC = 0;

	}

	private IEnumerator Start()
	{	
		yield return StartCoroutine(asteroids.SpawnAsteroids(levels.GetSpawnCountAutoINC(),1.0f));
		categorySelect.PlaceCategories(0);
		canTarget = true;
		yield return 0;
	}

	public void SetAttackToNierestEnemy()
	{
		Vector3 pos = asteroids.GetPositionToNierestEnemy();
		Vector3 fixedPos = new Vector3(pos.x,pos.y,-1.9f);
		AtkTarget.transform.position = fixedPos;
	}

	public IEnumerator NextRound(bool isHitTarget, CategorySelect.ColorTypes currentColorType)
	{
		categorySelect.animationIsPlaying = true;
		Vector3 target = AtkTarget.transform.position;
		categorySelect.targetSelect = false;
		AtkTarget.transform.position = new Vector3(-100,0,0);
		categorySelect.PlaceCategories(0);
		canTarget = true;
		isMouseDown = false;

		GA.API.Business.NewEvent("New Round", "round", 1);

		if(isHitTarget)
		{
			yield return StartCoroutine(AtkSystem.AttackTarget(target,asteroids.currentAsteroids,levels,currentColorType));
		}
		else
		{
			yield return StartCoroutine(AtkSystem.MissTarget());
		}

		bool isAnyLeft = asteroids.CheckIfAnyExists();
		bool isLevelProgressFull = levels.CheckIfProgressIfFull();

		//New Changes ***
		if(isLevelProgressFull)
		{
			WinBattle();
		}
		//
		else
		{
			yield return StartCoroutine(asteroids.MoveAsteroids());

			yield return new WaitForSeconds(0.1f);
			
			int spawnINC = levels.GetSpawnCountAutoINC();
			StartCoroutine(asteroids.SpawnAsteroids(spawnINC,1.0f));

			if(!LastHitMiss&&spawnINC!=0)
			{
				//New
				StartCoroutine(levels.UpdateLevelProgressBarForAsteriodsSpawned(1));
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

	private void WinBattle()
	{
		isEndGame = true;
		StartCoroutine(categorySelect.QE.ShowFadeBG(false,true));
		isgamewon=true;
		StartCoroutine(ShowEndBattleText("YOU WIN!",true));
	}

	private void LoseBattle()
	{
		isgamewon=false;
		if(mainmenu.totallives!=0)
		{
		mainmenu.totallives--;
		}
		PlayerPrefs.SetInt("totallives",mainmenu.totallives);
		isEndGame = true;
		StartCoroutine(categorySelect.QE.ShowFadeBG(false,true));
		StartCoroutine(ShowEndBattleText("YOU LOSE!",false));
		mainmenu.resettimerfornewlife();


		if(mainmenu.totallives<5)
		{
			Debug.Log("true");
			mainmenu.managetimerfornewlife(true);
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
			WinLoseTextAsset.transform.localScale += new Vector3(0.1f,0.1f,0.1f);
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
					if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2-25,200,50),"Restart You still have "+mainmenu.totallives.ToString()+" Lives"))
					{
						Application.LoadLevel("MainScene");
					}
					if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2+50,200,50),"Go To Main Menu"))
					{
						Application.LoadLevel("MenuScene");
					}
				}
				else
				{
					if(GUI.Button(new Rect(Screen.width/2-100+200,Screen.height/2-25,200,50),"You have lost all your lives, Please buy more lives or proceed to main menu"))
					{
						Application.LoadLevel("MainScene");
					}

					if(GUI.Button(new Rect(Screen.width/2-100+100,Screen.height/2+50,200,50),"Buy More Lives"))
					{
						Debug.Log("Does nothing");
					}
				}
			}
			else if(isgamewon==true)
			{
				//Temperory Data
				if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2-25,200,50),"Rewards Section"))
				   {

				   }
				if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2+50,200,50),"Go To Main Menu"))
				{
					Application.LoadLevel("MenuScene");
				}
			}
		}
	}

	private void Update()
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
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if(hit.collider)
				{
					if(hit.collider.gameObject.name.Equals("BackgroundLayer"))
					{
						categorySelect.targetSelect = true;
						AtkTarget.transform.position = hit.point;
						AtkTarget.transform.position += new Vector3(0,0,-1.9f);

						Vector3 relative = transform.InverseTransformPoint(AtkTarget.transform.position);
						float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
						character.currentChar.transform.localEulerAngles = new Vector3(0,0,angle);
					}
				}
			}
		}
	}

	private void GoToMainMenu()
	{
		Application.LoadLevel(0);
	}
}