using UnityEngine;
using System.Collections;

public class TutorialLevel16 : TutorialBase {

	private BattleEngine battleEngine;

	private GameObject stage1;
	private GameObject stage2;

	private bool started;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isRunning) {
			if (stage1 == null && !started) {
				started = true;

				Asteroids.Asteroid bigRedAsteroid = battleEngine.asteroids.currentAsteroids.Find(
					x => x.colorType == Asteroids.AsteroidColorTypes.Red && x.obj.transform.localScale.x > battleEngine.asteroids.smallAsteroidScale.x);

				if (bigRedAsteroid != null) {
					StartStage1(bigRedAsteroid);
				}
			}

			if (stage1 != null) {
				if (Input.GetMouseButtonDown(0)) {
					GameObject.Destroy(stage1);

					StartStage2();
				}
			}

			if (stage2 != null && battleEngine.FinishedRound) {
				GameObject.Destroy(stage2);
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

	private void StartStage1(Asteroids.Asteroid bigRedAsteroid) {
		stage1 = new GameObject ();

		Vector3 pointerOffset = new Vector3 (0, bigRedAsteroid.obj.renderer.bounds.extents.y * 1.6f, 0);
		Vector3 pointerPostition = bigRedAsteroid.obj.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage1.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("Certain categories do extra damage depending on the color match. Tap the screen to continue.", 40);
		
		instruction.transform.parent = stage1.transform;
	}	

	private void StartStage2() {
		stage2 = new GameObject ();

		GameObject rockPaperScissorsIcon = GameObject.Find ("button_power_up_01");

		Vector3 pointerOffset = new Vector3 (0, rockPaperScissorsIcon.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = rockPaperScissorsIcon.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage2.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("Blue questions are strong against red asteroids.", 40);
		
		instruction.transform.parent = stage2.transform;
	}
}
