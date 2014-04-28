using UnityEngine;
using System.Collections;

public class BattleEngine : MonoBehaviour {

	private CategorySelect categorySelect;
	private Asteroids asteroids;
	private LevelManager levels;
	[HideInInspector]
	public bool canTarget;
	private bool isMouseDown;
	private GameObject AtkTarget;
	private AttackSystem AtkSystem;
	private Characters character;
	private GameObject WinLoseTextAsset;
	private bool isEndGame;

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

	public IEnumerator NextRound(bool isHitTarget)
	{
		Vector3 target = AtkTarget.transform.position;
		categorySelect.targetSelect = false;
		AtkTarget.transform.position = new Vector3(-100,0,0);
		categorySelect.PlaceCategories(0);
		canTarget = true;
		isMouseDown = false;
		
		if(isHitTarget)
		{
			yield return StartCoroutine(AtkSystem.AttackTarget(target,asteroids.currentAsteroids,levels));
		}
		else
		{
			yield return StartCoroutine(AtkSystem.MissTarget());
		}

		bool isAnyLeft = asteroids.CheckIfAnyExists();
		bool isLevelProgressFull = levels.CheckIfProgressIfFull();

		if(!isAnyLeft||isLevelProgressFull)
		{
			WinBattle();
		}
		else
		{
			yield return StartCoroutine(asteroids.MoveAsteroids());

			yield return new WaitForSeconds(0.1f);
			
			StartCoroutine(asteroids.SpawnAsteroids(levels.GetSpawnCountAutoINC(),1.0f));

			StartCoroutine(levels.UpdateLevelProgressBar());

			bool isAnyCrossingTheLine = asteroids.CheckIfAnyCrossesTheLine();

			if(isAnyCrossingTheLine)
			{
				LoseBattle();
			}
		}

		yield return 0;
	}

	private void WinBattle()
	{
		isEndGame = true;
		StartCoroutine(categorySelect.QE.ShowFadeBG(false,true));
		StartCoroutine(ShowEndBattleText("YOU WIN!",true));
	}

	private void LoseBattle()
	{
		isEndGame = true;
		StartCoroutine(categorySelect.QE.ShowFadeBG(false,true));
		StartCoroutine(ShowEndBattleText("YOU LOSE!",false));
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
			if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2,200,50),"Restart"))
			{
				Application.LoadLevel("MainScene");
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
}