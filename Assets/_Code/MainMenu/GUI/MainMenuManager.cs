using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour {
	public GameObject currentLevelOverlay;

	public Sprite background1;
	public Sprite background2;

	public Vector3 pageAnimationSpeed;
	public Vector3 pageScrollSpeed;
	public float dragSpeedModifier;
	
	public Vector3 deceleration;

	public GameObject subPage1;
	public GameObject mainMenu;
	public GameObject subPage2;

	public GameObject level1Curve;
	public GameObject level8Curve;

	public int[] fontSizes;
	public Vector3[] textMeshOffsets;

	private GameObject sub1Background;
	private GameObject mainBackground;
	private GameObject sub2Background;

	private GameObject subPage1Nodes;
	private GameObject mainPageNodes;
	private GameObject subPage2Nodes;

	private GameObject subButtons1;
	private GameObject mainButtons;
	private GameObject subButtons2;

	private int levelsUnlocked;

	private Vector3 menuBoundsUpper;
	private Vector3 menuBoundsLower;

	private Bounds menuBounds;
	private Vector3 startDragPosition;
	private int pageIndex;
	private int levelNodesPerPage = 8;
	private int maxLevels = 200;
	private int maxPages;

	public bool draggingPage;

	private bool animatingPage;
	private int nextPageIndex;

	private bool performingUnlock;

	private int currentLevel;

	private ScreenSizeManager screenSizeManager;
	private mainmenu main;

	void Start () {
		main = GameObject.FindObjectOfType<mainmenu> ();

		screenSizeManager = GameObject.FindObjectOfType<ScreenSizeManager> ();
		float scaleX = screenSizeManager.scaleX;
		float scaleY = screenSizeManager.scaleY;

		mainBackground = mainMenu.transform.FindChild ("Background").gameObject;
		mainButtons = mainMenu.transform.FindChild ("Buttons").gameObject;

		sub1Background = subPage1.transform.FindChild ("Background").gameObject;
		subButtons1 = subPage1.transform.FindChild ("Buttons").gameObject;

		sub2Background = subPage2.transform.FindChild ("Background").gameObject;
		subButtons2 = subPage2.transform.FindChild ("Buttons").gameObject;

		Bounds spriteBounds = mainBackground.GetComponent<SpriteRenderer>().sprite.bounds;
		Vector3 menuBounds = new Vector3 (spriteBounds.extents.x * 2 * scaleY, spriteBounds.extents.y * 2 * scaleY, 0);
		menuBoundsUpper = menuBounds;
		menuBoundsLower = mainMenu.transform.position - menuBounds;

		//PlayerPrefs.SetInt (PlayerData.CurrentLevelKey, 1);
		currentLevel = PlayerPrefs.GetInt (PlayerData.CurrentLevelKey, 1);

		maxPages = (int) ((float) maxLevels / (float) levelNodesPerPage);
		pageIndex = (currentLevel - 1) / levelNodesPerPage;
		UpdateLevelNodes(pageIndex);

		LevelCompleteInfo levelCompleteInfo = GameObject.FindObjectOfType<LevelCompleteInfo> ();
		if (levelCompleteInfo != null) {
			if (levelCompleteInfo.levelUnlocked > currentLevel) {
				performingUnlock = true;

				int unlockedLevel = levelCompleteInfo.levelUnlocked;
				pageIndex = (unlockedLevel - 1) / levelNodesPerPage;
				UpdateLevelNodes(pageIndex);

				StartCoroutine (CheckForLevelUnlock (unlockedLevel));
			}

			GameObject.Destroy(levelCompleteInfo.gameObject);
		}
	}
	
	void Update () {
		Vector3 touchPosition = Input.mousePosition;
		Camera camera = Camera.main;

		RaycastHit2D hit=Physics2D.Raycast(camera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 0)), Vector2.zero);
		if (hit.collider != null) {
			if (hit.collider.name == "Background") {
				if (Input.GetMouseButtonDown (0) && !draggingPage && !animatingPage && !performingUnlock) {
					startDragPosition = Input.mousePosition;
					draggingPage = true;
				}
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			draggingPage = false;
		}

		if (!animatingPage && !performingUnlock && main.gamestate == mainmenu.state.mainmenu) {
			if (draggingPage) {
				Vector3 deltaPosition = Vector3.zero;
				deltaPosition.y = Input.mousePosition.y - startDragPosition.y;
				if (deltaPosition.y > 0) {
					Debug.Log("Drag up");
				}

				pageScrollSpeed = deltaPosition * Time.deltaTime * dragSpeedModifier;
			}
			else {
				if (pageScrollSpeed.magnitude > 0) {
					if (pageScrollSpeed.y >= 0) {
						pageScrollSpeed -= Time.deltaTime * deceleration;
					}
					else {
						pageScrollSpeed += Time.deltaTime * deceleration;
					}
				}

				if (pageScrollSpeed.magnitude <= 0.99f) {
					pageScrollSpeed = Vector3.zero;
				}
			}

			if (pageIndex <= 0 && pageScrollSpeed.y > 0) {
				pageScrollSpeed = Vector3.zero;
			}
			
			if (pageIndex >= maxPages - 1 && pageScrollSpeed.y < 0) {
				pageScrollSpeed = Vector3.zero;
			}
		}

		transform.position += pageScrollSpeed * Time.deltaTime;

		if (transform.position.y >= menuBoundsUpper.y) {
			transform.position = Vector3.zero;

			pageIndex--;
			UpdateLevelNodes(pageIndex);

			if (!animatingPage)
				pageScrollSpeed = Vector3.zero;
		}
		else if (transform.position.y <= menuBoundsLower.y) {
			transform.position = Vector3.zero;

			pageIndex++;
			UpdateLevelNodes(pageIndex);

			if (!animatingPage)
				pageScrollSpeed = Vector3.zero;
		}
	}

	private void UpdateLevelNodes(int mainMenuPage) {

		currentLevelOverlay.SetActive (false);

		// sub menu page 1
		UpdatePageLevelNodes (subButtons1, 1);

		if (mainMenuPage % 2 == 0) {
			sub1Background.GetComponent<SpriteRenderer>().sprite = screenSizeManager.GetSpriteSize(background2);
		}
		else {
			sub1Background.GetComponent<SpriteRenderer>().sprite = screenSizeManager.GetSpriteSize(background1);
		}

		// main menu page
		UpdatePageLevelNodes (mainButtons, 0);

		if (mainMenuPage % 2 == 0) {
			mainBackground.GetComponent<SpriteRenderer>().sprite = screenSizeManager.GetSpriteSize(background1);
		}
		else {
			mainBackground.GetComponent<SpriteRenderer>().sprite = screenSizeManager.GetSpriteSize(background2);
		}

		// sub menu page 2
		UpdatePageLevelNodes (subButtons2, -1);

		if (mainMenuPage % 2 == 0) {
			sub2Background.GetComponent<SpriteRenderer>().sprite = screenSizeManager.GetSpriteSize(background2);
		}
		else {
			sub2Background.GetComponent<SpriteRenderer>().sprite = screenSizeManager.GetSpriteSize(background1);
		}

		UpdateNodeConnectorCurves ();
	}

	private void UpdateNodeConnectorCurves() {
		string curve8Name = "curveline_8_9";
		
		//GameObject curvesRoot = subPage1.transform.FindChild ("Curves").gameObject;
		GameObject level8CurveSub1 = subButtons1.transform.FindChild("Curves").FindChild (curve8Name).gameObject;
		
		//curvesRoot = mainMenu.transform.FindChild ("Curves").gameObject;
		GameObject level8CurveMain = mainButtons.transform.FindChild("Curves").FindChild (curve8Name).gameObject;

		//curvesRoot = subPage2.transform.FindChild ("Curves").gameObject;
		GameObject level8CurveSub2 = subButtons2.transform.FindChild("Curves").FindChild (curve8Name).gameObject;
		GameObject level8CurveSub2a = subButtons2.transform.FindChild("Curves").FindChild (curve8Name + "a").gameObject;
		if (pageIndex <= 0) {
			level8CurveSub2.SetActive(false);
		}
		else {
			level8CurveSub2.SetActive(true);
		}

		if (pageIndex <= 1) {
			level8CurveSub2a.SetActive(false);
		}
		else {
			level8CurveSub2a.SetActive(true);
		}
		
		if (pageIndex >= maxPages - 1) {
			level8CurveSub2.SetActive(false);
			level8CurveMain.SetActive(false);
			level8CurveSub1.SetActive(false);
		}
	}

	private void UpdatePageLevelNodes (GameObject levelNodesRoot, int pageOffset)
	{
		List<LevelNode> levelNodes = new List<LevelNode> (levelNodesRoot.GetComponentsInChildren<LevelNode> ());

		for (int index = 0; index < levelNodes.Count; index++) {
			LevelNode levelNode = levelNodes.Find (x => x.levelIndex == index);
			if (levelNode != null) {
				GameObject textMesh = levelNode.gameObject.transform.FindChild ("textmesh").gameObject;
				if (textMesh != null) {
					int level = (pageIndex + pageOffset) * levelNodesPerPage + (levelNode.levelIndex + 1);
					levelNode.level = level;

					string levelLabel = level.ToString();

					Vector3 parentScale = textMesh.transform.parent.localScale;
					textMesh.transform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1);

					float scaleX = 1 / screenSizeManager.scaleX;
					textMesh.transform.localPosition = levelNode.TextStartPosition + textMeshOffsets[levelLabel.Length - 1] * scaleX;
					textMesh.GetComponent<TextMesh>().fontSize = fontSizes[levelLabel.Length - 1];
					textMesh.GetComponent<TextMesh> ().text = levelLabel;

					if (levelNode.level == currentLevel) {
						UpdateActiveLevelOverlay (levelNode, currentLevel);
					}

					string lockedLevelName = "Level_Locked_Level" + (index + 1);
					GameObject lockedLevelOverlay = levelNode.transform.FindChild(lockedLevelName).gameObject;
					//lockedLevelOverlay.transform.localScale = new Vector3(1, 1, 1);
					if (level > currentLevel) {
						lockedLevelOverlay.GetComponent<LockManager>().Lock();
					}
					else {
						lockedLevelOverlay.GetComponent<LockManager>().Unlock();
					}
				}
			}
		}
	}
	
	private IEnumerator CheckForLevelUnlock(int nextLevel)
	{
		yield return StartCoroutine (UnlockPageNodes (subButtons1, nextLevel));
		yield return StartCoroutine (UnlockPageNodes (mainButtons, nextLevel));
		//yield return StartCoroutine (UnlockPageNodes (subPage2Nodes, nextLevel));

		PlayerPrefs.SetInt (PlayerData.CurrentLevelKey, nextLevel);
		currentLevel = nextLevel;

		performingUnlock = false;
	}

	private IEnumerator UnlockPageNodes (GameObject pageNodes, int nextLevel)
	{
		int levelDelta = nextLevel - currentLevel;

		List<LevelNode> levelNodes = new List<LevelNode> (pageNodes.GetComponentsInChildren<LevelNode> ());
		for (int levelIndex = 1; levelIndex <= levelDelta; levelIndex++) {
			int level = currentLevel + levelIndex;
			LevelNode levelNode = levelNodes.Find (x => x.level == level);
			if (levelNode != null) {
				string levelLockName = "Level_Locked_Level" + (levelNode.levelIndex + 1);
				GameObject levelUnlocked = levelNode.transform.FindChild (levelLockName).gameObject;
				yield return StartCoroutine (levelUnlocked.GetComponent<LockManager> ().BeginUnlock ());
			}
		}

		LevelNode activeLevelNode = levelNodes.Find (x => x.level == nextLevel);
		if (activeLevelNode != null) {
			UpdateActiveLevelOverlay (activeLevelNode, nextLevel);
		}
	}

	private void UpdateActiveLevelOverlay (LevelNode levelNode, int activeLevel)
	{
		currentLevelOverlay.transform.parent = levelNode.gameObject.transform;
		float zPosition = levelNode.transform.localPosition.z;
		currentLevelOverlay.transform.localPosition = new Vector3 (0, 0, -2);
		currentLevelOverlay.transform.localScale = new Vector3 (1.1f, 1.1f, 1);

		currentLevelOverlay.SetActive (true);
	}
}
