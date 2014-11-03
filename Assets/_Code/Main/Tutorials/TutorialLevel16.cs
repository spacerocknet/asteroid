using UnityEngine;
using System.Collections;

public class TutorialLevel16 : TutorialBase {

	private BattleEngine battleEngine;

	private GameObject stage1;
	private GameObject stage1a;
	private GameObject stage2;

	private bool started;

	private Asteroids.Asteroid targetAsteroid;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isRunning) {
			if (stage1 == null && !started) {
				started = true;
					StartStage1();
			}

			Asteroids.Asteroid bigRedAsteroid = battleEngine.asteroids.currentAsteroids.Find(
				x => x.colorType == Asteroids.AsteroidColorTypes.Red && x.obj.transform.localScale.x > battleEngine.asteroids.smallAsteroidScale.x);


			if (stage1 != null) {
				if (Input.GetMouseButtonDown(0)) {
					GameObject.Destroy(stage1);

					if (bigRedAsteroid != null) {
						StartStage1A(bigRedAsteroid);

						return;
					}
				}
			}

			if (stage1a != null) {
				if (Input.GetMouseButtonDown(0)) {
					GameObject.Destroy(stage1a);
				}
			}

			if (bigRedAsteroid != null) {
				Vector3 asteroidPostion = bigRedAsteroid.obj.transform.position;
				Bounds asteroidBounds = bigRedAsteroid.obj.GetComponent<SpriteRenderer>().bounds;
				
				Rect asteroidRect = new Rect(asteroidPostion.x - asteroidBounds.extents.x, asteroidPostion.y - asteroidBounds.extents.y,
				                             asteroidBounds.extents.x * 2, asteroidBounds.extents.y * 2);

				GameObject attackTarget = battleEngine.AtkTarget;

				if (targetAsteroid == null && stage2 == null) {
					Sprite asteroidSprite = bigRedAsteroid.obj.GetComponent<SpriteRenderer>().sprite;
					if (asteroidRect.Contains(attackTarget.transform.position)) {
						targetAsteroid = bigRedAsteroid;
					}
				}
			}

			if (stage2 == null && targetAsteroid == bigRedAsteroid) {
				StartStage2();

				return;
			}

			if (stage2 != null && !battleEngine.canTarget) {
				GameObject.Destroy(stage2);

				End();
			}
		}
	}

	public override void Begin (int currentLevel) {
		base.Begin (currentLevel);

		battleEngine = GameObject.FindObjectOfType<BattleEngine> ();

		started = false;
	}

	public override void End () {
		base.End ();
	}

	private void StartStage1() {
		stage1 = new GameObject ();

		GameObject rockPaperScissors = GameObject.Find ("button_power_up_01");

		Vector3 pointerOffset = new Vector3 (0, rockPaperScissors.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = rockPaperScissors.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage1.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("Certain categories do extra damage depending on the color match. Tap the screen to continue.", 40);
		
		instruction.transform.parent = stage1.transform;
	}	

	private void StartStage1A(Asteroids.Asteroid bigRedAsteroid) {
		stage1a = new GameObject ();

		Vector3 pointerOffset = new Vector3 (0, bigRedAsteroid.obj.renderer.bounds.extents.y * 1.6f, 0);
		Vector3 pointerPostition = bigRedAsteroid.obj.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage1a.transform;

		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("Target the red rock", 40);
		
		instruction.transform.parent = stage1a.transform;
	}

	private void StartStage2() {
		stage2 = new GameObject ();

		Categories categories = GameObject.FindObjectOfType<Categories>();
		GameObject middleCat = categories.transform.Find("1").gameObject;

		Vector3 pointerOffset = new Vector3 (0, -middleCat.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = middleCat.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		pointer.transform.RotateAround(pointerPostition, Vector3.forward, 180);
		
		pointer.transform.parent = stage2.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("Blue questions are strong against red asteroids.", 40);
		
		instruction.transform.parent = stage2.transform;
	}
}
