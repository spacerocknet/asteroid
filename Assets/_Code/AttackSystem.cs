using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSystem : MonoBehaviour {

	public float asteroidDamage = 2.0f;

	private Weapons weapons;
	private int currentWeapon;
	private GameObject label_ref;
	private BattleEngine BATTLE_ENGINE;

	private GameObject soundmanager;

	private ProgressBarManager progressBarManager;

	private void Awake()
	{
		soundmanager=GameObject.Find("Powerup_SoundManager");
		weapons = (Weapons) this.gameObject.AddComponent<Weapons>();
		label_ref = (GameObject) GameObject.Find("REFERENCES/label_ref");
		BATTLE_ENGINE = (BattleEngine) GameObject.Find("MAIN").GetComponent<BattleEngine>();

		progressBarManager = GameObject.FindObjectOfType<ProgressBarManager> ();

		currentWeapon = 0;
	}

	void Start() {
		progressBarManager.UpdateProgressBar (BATTLE_ENGINE.levelInfo.selectedNodeInfo.hitPointsDone);
		
	}

	public IEnumerator AttackTarget(Vector3 target, List<Asteroids.Asteroid> currentAsteroids, LevelManager levelMRef, CategorySelect.ColorTypes currentCategoryColorType)
	{		
		BATTLE_ENGINE.LastHitMiss = false;
		
//		Debug.Log("Attack!");

		List<int> destroyIndex = new List<int>();

		GameObject expT = (GameObject) Instantiate(weapons.AllWeapons[currentWeapon].obj,target,Quaternion.identity);
		Destroy(expT,0.67f);

		for(int i=0;i<currentAsteroids.Count;i++)
		{
			//Normal Distance without powerup
			float normalpowerupdistance=1.0f;

			//Double up the distance
			float powerupdoubleblast=normalpowerupdistance*2.0f;

			Vector3 p1 = target;
			Vector3 p2 = new Vector3(currentAsteroids[i].obj.transform.position.x,currentAsteroids[i].obj.transform.position.y,p1.z);
			float dist = Vector3.Distance(p1,p2);

			if(ButtonManager.powerupselected=="double_blast_radius")
			{
				if(dist<powerupdoubleblast)
				{
					ButtonManager.reducepowerupcount(ButtonManager.powerupselected);
					soundmanager.audio.Play();
					ButtonManager.attack_target.transform.localScale=ButtonManager.targetStartScale;
					GameObject exp = (GameObject) Instantiate(weapons.AllWeapons[currentWeapon].obj,p2,Quaternion.identity);
					Destroy(exp,0.67f);
					destroyIndex.Add(i);
				}
			}
			else
				{
				if(dist<normalpowerupdistance)
					{
						GameObject exp = (GameObject) Instantiate(weapons.AllWeapons[currentWeapon].obj,p2,Quaternion.identity);
						Destroy(exp,0.67f);
						destroyIndex.Add(i);
					}
				}
			}
		
		for(int i=destroyIndex.Count-1;i>=0;i--)
		{
	//		Debug.Log(currentCategoryColorType+":"+currentAsteroids[destroyIndex[i]].colorType+":"+)
			asteroidDamage = 2.0f;

			if ((int) currentCategoryColorType == (int) currentAsteroids[destroyIndex[i]].colorType) {
				//asteroidDamage += 0.5f * asteroidDamage;
			}

			asteroidDamage += DamageCalcByColor(currentCategoryColorType,currentAsteroids[destroyIndex[i]].colorType);

			if (asteroidDamage > currentAsteroids[destroyIndex[i]].life) {
				asteroidDamage = currentAsteroids[destroyIndex[i]].life;
			}

			StartCoroutine(currentAsteroids[destroyIndex[i]].DoDamage(asteroidDamage));

			BATTLE_ENGINE.levelInfo.selectedNodeInfo.hitPointsDone += asteroidDamage;

			progressBarManager.UpdateProgressBar(BATTLE_ENGINE.levelInfo.selectedNodeInfo.hitPointsDone);

			if(currentAsteroids[destroyIndex[i]].isDead)
			{
				Destroy(currentAsteroids[destroyIndex[i]].obj,0);
				currentAsteroids.RemoveAt(destroyIndex[i]);

				//New Changes ***
				//GameObject.Find("MAIN").GetComponent<LevelManager>().StartCoroutine("UpdateLevelProgressBarForAsteroidsDestroyed");
				//
			}
		}
		yield return 0;
	}



	private float DamageCalcByColor(CategorySelect.ColorTypes c1, Asteroids.AsteroidColorTypes c2)
	{
		int cDistance = ColorDistance((int)c1,(int)c2);
		if(cDistance==0)
		{
			if(ButtonManager.powerupselected=="bomb")
			{
				
				ButtonManager.reducepowerupcount(ButtonManager.powerupselected);
				soundmanager.audio.Play();
				return 2+2;
			}
			else
			{
				return 2 + 1;
			}
		}

		else if(cDistance==1)
		{
			if(ButtonManager.powerupselected=="bomb")
			{
				ButtonManager.reducepowerupcount(ButtonManager.powerupselected);
				soundmanager.audio.Play();
				return 3+3;
			}
			else
			{
				return 3 + 1.5f;
			}
		}

		else if(cDistance==2)
		{
			if(ButtonManager.powerupselected=="bomb")
			{
				ButtonManager.reducepowerupcount(ButtonManager.powerupselected);
				soundmanager.audio.Play();
				return 1+1; 
			}
			else
			{
				return 1 + 0.5f;
			}
		}

		return 0;
	}

	private int ColorDistance(int c1, int c2)
	{
		int dist = c1-c2;

		if(dist.Equals(-1))
		{
			dist = 2;
		}
		else if(dist.Equals(-2))
		{
			dist = 1;
		}

		return dist;
	}

	public IEnumerator MissTarget()
	{
		BATTLE_ENGINE.LastHitMiss = true;

		GameObject missInit = (GameObject) Instantiate(label_ref,new Vector3(0f,0f,-1.45f),label_ref.transform.rotation);
		TextMesh tm = (TextMesh) missInit.GetComponent<TextMesh>();
		tm.text = "Miss!";
		missInit.transform.localScale = new Vector3(0,0,1);

		yield return new WaitForSeconds(0.2f);

		for(int i=0;i<12;i++)
		{
			missInit.transform.localScale += new Vector3(0.1f,0.1f,0);

			yield return 0;
		}

		Color cl = tm.color;

		for(int i=0;i<20;i++)
		{
			missInit.transform.position += new Vector3(0,0.04f,0);

			if(i>10)
			{
				cl.a -= 0.1f;
				tm.color = cl;
			}

			yield return 0;
		}
		Destroy(missInit);
//		Debug.Log("Miss!");

		yield return 0;
	}

	public void SelectWeapon(int id)
	{
		currentWeapon = id;
	}

	private float GetAsteroidHitPointsRemaining() {
		float remaining = 0;

		remaining += BATTLE_ENGINE.levelInfo.selectedNodeInfo.smallRocks * 0.75f * BATTLE_ENGINE.levelInfo.selectedNodeInfo.multiplier;
		remaining += BATTLE_ENGINE.levelInfo.selectedNodeInfo.bigRocks * 1.3125f * BATTLE_ENGINE.levelInfo.selectedNodeInfo.multiplier;

		return remaining;
	}
}
