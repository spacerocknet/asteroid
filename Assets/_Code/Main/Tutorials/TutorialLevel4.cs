using UnityEngine;
using System.Collections;

public class TutorialLevel4 : TutorialBase {

	private GameObject stage1;
	private GameObject stage2;
	private GameObject stage3;
	private GameObject stage4;
	private GameObject stage5;

	private GameObject bombPowerUps;
	private GameObject doubleBlastRadiusPowerUps;
	private GameObject reverseTimePowerUps;
	private GameObject changeCategoryPowerUps;

	private bool started;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isRunning) {
			if (stage1 == null && !started) {
				StartStage1();

				started = true;
			}

			int bombPowerUpCount = PlayerPrefs.GetInt(PlayerData.BombPowerUpsKey);
			if (stage1 != null && bombPowerUpCount <= 0) {
				if (Input.GetMouseButtonDown(0)) {
					PlayerPrefs.SetInt(PlayerData.BombPowerUpsKey, 2);
					GameObject.Destroy(stage1);
				}
			}

			if (stage2 == null) {
				if (bombPowerUpCount >= 2) {
					StartStage2();
				}
			}

			if (stage2 != null) {
				if (bombPowerUpCount < 2) {
					PlayerPrefs.SetInt(PlayerData.DoubleBlastRadiusPowerUpsKey, 2);
					GameObject.DestroyObject(stage2);
				}
			}

			int doubleRadiusPowerUpCount = PlayerPrefs.GetInt(PlayerData.DoubleBlastRadiusPowerUpsKey);
			if (stage3 == null) {
				if (doubleRadiusPowerUpCount >= 2) {
					StartStage3();
				}
			}

			if (stage3 != null) {
				if (doubleRadiusPowerUpCount < 2) {
					PlayerPrefs.SetInt(PlayerData.ReverseTimePowerUpsKey, 2);
					GameObject.Destroy(stage3);
				}
			}

			int reverseTimePowerUpCount = PlayerPrefs.GetInt(PlayerData.ReverseTimePowerUpsKey);
			if (stage4 == null) {
				if (reverseTimePowerUpCount >= 2) {
					StartStage4();
				}
			}

			if (stage4 != null) {
				if (reverseTimePowerUpCount < 2) {
					PlayerPrefs.SetInt(PlayerData.ChageQuestionCategoryPowerUpsKey, 2);
					GameObject.Destroy(stage4);
				}
			}

			int changeCategoryPowerUpCount = PlayerPrefs.GetInt(PlayerData.ChageQuestionCategoryPowerUpsKey);
			if (stage5 == null) {
				if (changeCategoryPowerUpCount >= 2) {
					StartStage5();
				}
			}

			if (stage5 != null) {
				if (changeCategoryPowerUpCount < 2) {
					GameObject.Destroy(stage5);

					End();
				}
			}
		}
	}

	public override void Begin (int currentLevel) {
		base.Begin (currentLevel);

		started = false;

		bombPowerUps = GameObject.Find ("Power Up 1");
		doubleBlastRadiusPowerUps = GameObject.Find ("Power Up 2");
		reverseTimePowerUps = GameObject.Find ("Power Up 3");
		changeCategoryPowerUps = GameObject.Find ("Power Up 4");

		bombPowerUps.SetActive (false);
		doubleBlastRadiusPowerUps.SetActive (false);
		reverseTimePowerUps.SetActive (false);
		changeCategoryPowerUps.SetActive (false);

		PlayerPrefs.SetInt (PlayerData.BombPowerUpsKey, 0);
		PlayerPrefs.SetInt (PlayerData.DoubleBlastRadiusPowerUpsKey, 0);
		PlayerPrefs.SetInt (PlayerData.ReverseTimePowerUpsKey, 0);
		PlayerPrefs.SetInt (PlayerData.ChageQuestionCategoryPowerUpsKey, 0);
	}

	public override void End () {
		base.End ();
	}

	private void StartStage1() {
		stage1 = new GameObject();

		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("You can use a power up if you get stuck. We've started you off with 2 of each kind. Tap the screen to continue.", 40);
		
		instruction.transform.parent = stage1.transform;
	}

	private void StartStage2() {
		stage2 = new GameObject ();

		bombPowerUps.SetActive (true);

		GameObject button = bombPowerUps.transform.FindChild("button_power_up_02").gameObject;

		GameObject bombText = bombPowerUps.transform.FindChild ("bomb_textmesh").gameObject;
		bombText.GetComponent<TextMesh> ().text = PlayerPrefs.GetInt (PlayerData.BombPowerUpsKey).ToString();

		Vector3 pointerOffset = new Vector3 (0, button.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = button.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);

		pointer.transform.parent = stage2.transform;

		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("This power-up doubles your attack damage.", 40);

		instruction.transform.parent = stage2.transform;
	}

	private void StartStage3() {
		stage3 = new GameObject ();

		doubleBlastRadiusPowerUps.SetActive (true);

		GameObject button = doubleBlastRadiusPowerUps.transform.FindChild("button_power_up_03").gameObject;

		GameObject bombText = doubleBlastRadiusPowerUps.transform.FindChild ("doubleblastradius_textmesh").gameObject;
		bombText.GetComponent<TextMesh> ().text = PlayerPrefs.GetInt (PlayerData.DoubleBlastRadiusPowerUpsKey).ToString();
		
		Vector3 pointerOffset = new Vector3 (0, button.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = button.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage3.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("This one doubles your attack radius.", 40);
		
		instruction.transform.parent = stage3.transform;
	}

	private void StartStage4() {
		stage4 = new GameObject ();
		
		reverseTimePowerUps.SetActive (true);

		GameObject button = reverseTimePowerUps.transform.FindChild("button_power_up_04").gameObject;

		GameObject bombText = reverseTimePowerUps.transform.FindChild ("reversetime_textmesh").gameObject;
		bombText.GetComponent<TextMesh> ().text = PlayerPrefs.GetInt (PlayerData.ReverseTimePowerUpsKey).ToString();
		
		Vector3 pointerOffset = new Vector3 (0, button.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = button.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage4.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("Use this one to send asteroids back two steps.", 40);
		
		instruction.transform.parent = stage4.transform;
	}

	private void StartStage5() {
		stage5 = new GameObject ();
		
		changeCategoryPowerUps.SetActive (true);

		GameObject button = changeCategoryPowerUps.transform.FindChild("button_power_up_05").gameObject;

		GameObject bombText = changeCategoryPowerUps.transform.FindChild ("changecategory_textmesh").gameObject;
		bombText.GetComponent<TextMesh> ().text = PlayerPrefs.GetInt (PlayerData.ChageQuestionCategoryPowerUpsKey).ToString();
		
		Vector3 pointerOffset = new Vector3 (0, button.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = button.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage5.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("Use this one to send asteroids back two steps.", 40);
		
		instruction.transform.parent = stage5.transform;
	}
}
