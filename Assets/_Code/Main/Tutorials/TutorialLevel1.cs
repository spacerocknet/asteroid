using UnityEngine;
using System.Collections;

public class TutorialLevel1 : TutorialBase {
	private BattleEngine battleEngine;

	private GameObject pointer;
	private Asteroids.Asteroid targetAsteroid;
	private Categories categories;

	private GameObject stage1;
	private GameObject stage2;
	private bool lastStage;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (isRunning) {
			Asteroids.Asteroid asteroid = battleEngine.asteroids.currentAsteroids[0];

			Vector3 asteroidPostion = asteroid.obj.transform.position;
			Bounds asteroidBounds = asteroid.obj.GetComponent<SpriteRenderer>().bounds;
			
			Rect asteroidRect = new Rect(asteroidPostion.x - asteroidBounds.extents.x, asteroidPostion.y - asteroidBounds.extents.y,
			                             asteroidBounds.extents.x * 2, asteroidBounds.extents.y * 2);

			GameObject attackTarget = battleEngine.AtkTarget;

			if (targetAsteroid == null) {
				if (stage1 == null || !lastStage) {
					ShowStage1(asteroid);
				}
			}

			if (targetAsteroid == null && stage2 == null) {
				Sprite asteroidSprite = asteroid.obj.GetComponent<SpriteRenderer>().sprite;
				if (asteroidRect.Contains(attackTarget.transform.position)) {
					targetAsteroid = asteroid;
					CategoriesSetActive(true);

					lastStage = true;

					GameObject.Destroy (stage1);
					ShowStage2();
				}
			}

			if (targetAsteroid != null && stage2 != null) {
				if (!asteroidRect.Contains(attackTarget.transform.position)) {
					targetAsteroid = null;
					CategoriesSetActive(false);

					GameObject.Destroy(stage2);

					lastStage = false;
				}
			}

			if (stage2 != null && battleEngine.FinishedRound) {
				End();
			}
		}
	}

	public override void Begin (int currentLevel) {
		base.Begin (currentLevel);

		categories = GameObject.FindObjectOfType<Categories> ();
		CategoriesSetActive (false);

		battleEngine = GameObject.FindObjectOfType<BattleEngine> ();
		lastStage = false;
	}

	public override void End () {
		base.End ();

		CategoriesSetActive (true);
		GameObject.Destroy (stage1);
		GameObject.Destroy (stage2);
	}
	
	private void ShowStage1 (Asteroids.Asteroid asteroid) {
		if (stage1 == null) {
			stage1 = new GameObject();

			Vector3 pointerOffset = new Vector3 (0, asteroid.obj.renderer.bounds.extents.y * 2f, 0);
			Vector3 pointerPostition = asteroid.obj.transform.position - pointerOffset;
			pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);

			pointer.transform.parent = stage1.transform;

			GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
			GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
			text.GetComponent<TextMesh> ().text = ResolveTextSize ("Select this asteroid for targeting.", 40);

			instruction.transform.parent = stage1.transform;
		}
	}

	private void ShowStage2() {
		if (stage2 == null) {
			stage2 = new GameObject();

			Categories categories = GameObject.FindObjectOfType<Categories>();
			GameObject middleCat = categories.transform.Find("1").gameObject;

			Vector3 pointerOffset = new Vector3 (0, -middleCat.renderer.bounds.extents.y * 2f, 0);
			Vector3 pointerPostition = middleCat.transform.position - pointerOffset;
			pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
			pointer.transform.RotateAround(pointerPostition, Vector3.forward, 180);

			pointer.transform.parent = stage2.transform;

			GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
			GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
			text.GetComponent<TextMesh> ().text = ResolveTextSize ("Pick a question category.", 40);
			
			instruction.transform.parent = stage2.transform;
		}
	}

	private void CategoriesSetActive (bool active) {
		categories.gameObject.SetActive (active);
	}
}
