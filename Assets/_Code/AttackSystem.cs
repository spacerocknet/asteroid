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
		BATTLE_ENGINE.levelInfo.selectedNodeInfo.hitPointsDone = 0;
		progressBarManager.UpdateProgressBar (BATTLE_ENGINE.levelInfo.selectedNodeInfo.hitPointsDone);
	}

	public void AttackTarget(float damageMultiplier, GameObject target, List<Asteroids.Asteroid> currentAsteroids, LevelManager levelMRef, CategorySelect.ColorTypes currentCategoryColorType)
	{		
		BATTLE_ENGINE.LastHitMiss = false;
		
//		Debug.Log("Attack!");

		List<int> destroyIndex = new List<int>();

		GameObject expT = (GameObject) Instantiate(weapons.AllWeapons[currentWeapon].obj,
		                                           target.transform.position, Quaternion.identity);
		Destroy(expT,0.67f);

		for(int i=0;i<currentAsteroids.Count;i++)
		{
			float asteroidExtent = target.transform.localScale.x * 0.85f;

			Vector3 asteroidPosition = currentAsteroids[i].obj.transform.position;
			asteroidPosition.z = target.transform.position.z;

			float dist = Vector3.Distance(target.transform.position, asteroidPosition);

			if(dist < asteroidExtent)
			{
				GameObject exp = (GameObject) Instantiate(weapons.AllWeapons[currentWeapon].obj, asteroidPosition, Quaternion.identity);
				Destroy(exp,0.67f);
				destroyIndex.Add(i);
			}
		}
		
		for(int i=destroyIndex.Count-1;i>=0;i--)
		{
			asteroidDamage = 2.0f;
			asteroidDamage *= damageMultiplier;
			asteroidDamage = ApplyRockPaperScissorsDamage(asteroidDamage, currentCategoryColorType,currentAsteroids[destroyIndex[i]].colorType);

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

		if (ButtonManager.powerupselected == "double_blast_radius") {
			ButtonManager.reducepowerupcount(ButtonManager.powerupselected);
			soundmanager.audio.Play();
			ButtonManager.attack_target.transform.localScale=ButtonManager.targetStartScale;

		}
	}



	private float ApplyRockPaperScissorsDamage(float baseDamage, CategorySelect.ColorTypes c1, Asteroids.AsteroidColorTypes c2)
	{
		int cDistance = ColorDistance((int)c1,(int)c2);
		if(cDistance==0)
		{
			return baseDamage;
		}

		else if(cDistance==1)
		{
			return baseDamage + baseDamage * 0.5f;
		}

		else if(cDistance==2)
		{
			return baseDamage - baseDamage * 0.5f;
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
