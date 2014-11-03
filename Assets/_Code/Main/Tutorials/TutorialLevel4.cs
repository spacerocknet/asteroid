using UnityEngine;
using System.Collections;

public class TutorialLevel4 : TutorialBase {

	private GameObject stage1;
	private GameObject stage2;
	private GameObject stage3;
	private GameObject stage4;
	private GameObject stage5;

	public Vector3 textboxPosition;

	public GameObject bombPowerUps;
	public GameObject doubleBlastRadiusPowerUps;
	public GameObject reverseTimePowerUps;
	public GameObject changeCategoryPowerUps;

	public string sortingLayerName;
	public int sortingOrder;

	private bool started;

	private bool doneStage1;
	private bool doneStage2;
	private bool doneStage3;
	private bool doneStage4;
	private bool doneStage5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isRunning) {
			if (stage1 == null && !doneStage1) {
				StartStage1();
			}

			if (stage1 != null) {
				if (Input.GetMouseButtonDown(0)) {
					GameObject.Destroy(stage1);

					doneStage1 = true;
				}
			}

			if (stage2 == null && doneStage1 && !doneStage2) {
				StartStage2();
				return;
			}

			Vector3 touchposition = Input.mousePosition;

			if (stage2 != null) {
				if (Input.GetMouseButtonDown(0)) {
					GameObject.DestroyObject(stage2);

					doneStage2 = true;
				}
			}

			if (stage3 == null && doneStage2 && !doneStage3) {
				StartStage3();

				return;
			}

			if (stage3 != null) {
				if (Input.GetMouseButtonDown(0)) {
					GameObject.Destroy(stage3);

					doneStage3 = true;
				}
			}

			if (stage4 == null && doneStage3 && !doneStage4) {
				StartStage4();

				return;
			}

			if (stage4 != null) {
				if (Input.GetMouseButtonDown(0)) {
					GameObject.Destroy(stage4);

					doneStage4 = true;
				}
			}

			if (stage5 == null && doneStage4 && !doneStage5) {
				StartStage5();

				return;
			}

			if (stage5 != null) {
				if (Input.GetMouseButtonDown(0)) {
					GameObject.Destroy(stage5);

					doneStage5 = true;

					End();
				}
			}
		}
	}

	public override void Begin (int currentLevel) {
		base.Begin (currentLevel);

		started = false;

		doneStage1 = false;
		doneStage2 = false;
		doneStage3 = false;
		doneStage4 = false;
		doneStage5 = false;

		bombPowerUps.SetActive (false);
		doubleBlastRadiusPowerUps.SetActive (false);
		reverseTimePowerUps.SetActive (false);
		changeCategoryPowerUps.SetActive (false);
	}

	public override void End () {
		base.End ();

		if (stage1 != null) {
			GameObject.Destroy(stage1);
		}

		if (stage2 != null) {
			GameObject.Destroy(stage2);
		}

		if (stage3 != null) {
			GameObject.Destroy(stage3);
		}

		if (stage4 != null) {
			GameObject.Destroy(stage4);
		}

		if (stage5 != null) {
			GameObject.Destroy(stage5);
		}

		bombPowerUps.SetActive (true);
		bombPowerUps.transform.FindChild ("button_minus_bomb").gameObject.SetActive (true);
		bombPowerUps.transform.FindChild ("button_plus_bomb").gameObject.SetActive (true);

		doubleBlastRadiusPowerUps.SetActive (true);
		doubleBlastRadiusPowerUps.transform.FindChild ("button_minus_doubleblast").gameObject.SetActive (true);
		doubleBlastRadiusPowerUps.transform.FindChild ("button_plus_doubleblast").gameObject.SetActive (true);

		reverseTimePowerUps.SetActive (true);
		reverseTimePowerUps.transform.FindChild ("button_minus_reversetime").gameObject.SetActive (true);
		reverseTimePowerUps.transform.FindChild ("button_plus_reversetime").gameObject.SetActive (true);

		changeCategoryPowerUps.SetActive (true);
		changeCategoryPowerUps.transform.FindChild ("button_minus_changequestioncategory").gameObject.SetActive (true);
		changeCategoryPowerUps.transform.FindChild ("button_plus_changequestioncategory").gameObject.SetActive (true);
	}

	private void StartStage1() {
		stage1 = new GameObject();

		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		//instruction.renderer.sortingLayerName = sortingLayerName;
		//instruction.renderer.sortingOrder = sortingOrder;

		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("You can use a power up if you get stuck. We've started you off with 2 of each kind. Tap the screen to continue.", 40);

		instruction.transform.position = textboxPosition;
		instruction.transform.parent = stage1.transform;
	}

	private void StartStage2() {
		stage2 = new GameObject ();

		bombPowerUps.SetActive (true);
		bombPowerUps.transform.FindChild ("button_minus_bomb").gameObject.SetActive (false);
		bombPowerUps.transform.FindChild ("button_plus_bomb").gameObject.SetActive (false);

		Vector3 pointerOffset = new Vector3 (0, bombPowerUps.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = bombPowerUps.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);

		pointer.transform.parent = stage2.transform;

		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("This power-up doubles your attack damage.", 40);

		instruction.transform.position = textboxPosition;
		instruction.transform.parent = stage2.transform;
	}

	private void StartStage3() {
		stage3 = new GameObject ();

		doubleBlastRadiusPowerUps.SetActive (true);
		doubleBlastRadiusPowerUps.transform.FindChild ("button_minus_doubleblast").gameObject.SetActive (false);
		doubleBlastRadiusPowerUps.transform.FindChild ("button_plus_doubleblast").gameObject.SetActive (false);
		
		Vector3 pointerOffset = new Vector3 (0, doubleBlastRadiusPowerUps.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = doubleBlastRadiusPowerUps.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage3.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("This one doubles your attack radius.", 40);

		instruction.transform.position = textboxPosition;
		instruction.transform.parent = stage3.transform;
	}

	private void StartStage4() {
		stage4 = new GameObject ();
		
		reverseTimePowerUps.SetActive (true);
		reverseTimePowerUps.transform.FindChild ("button_minus_reversetime").gameObject.SetActive (false);
		reverseTimePowerUps.transform.FindChild ("button_plus_reversetime").gameObject.SetActive (false);
		
		Vector3 pointerOffset = new Vector3 (0, reverseTimePowerUps.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = reverseTimePowerUps.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage4.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("Use this one to send asteroids back two steps.", 40);

		instruction.transform.position = textboxPosition;
		instruction.transform.parent = stage4.transform;
	}

	private void StartStage5() {
		stage5 = new GameObject ();
		
		changeCategoryPowerUps.SetActive (true);
		changeCategoryPowerUps.transform.FindChild ("button_minus_changequestioncategory").gameObject.SetActive (false);
		changeCategoryPowerUps.transform.FindChild ("button_plus_changequestioncategory").gameObject.SetActive (false);
	
		Vector3 pointerOffset = new Vector3 (0, changeCategoryPowerUps.renderer.bounds.extents.y * 2f, 0);
		Vector3 pointerPostition = changeCategoryPowerUps.transform.position - pointerOffset;
		GameObject pointer = (GameObject)GameObject.Instantiate (pointerPrefab, pointerPostition, Quaternion.identity);
		
		pointer.transform.parent = stage5.transform;
		
		GameObject instruction = (GameObject)GameObject.Instantiate (instructionTextPrefab);
		GameObject text = instruction.transform.FindChild ("Instruction Text").gameObject;
		text.GetComponent<TextMesh> ().text = ResolveTextSize ("This one will change category colors.", 40);

		instruction.transform.position = textboxPosition;
		instruction.transform.parent = stage5.transform;
	}
}
