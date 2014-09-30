using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour {
	public GameObject currentLevelOverlay;

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

	private GameObject subPage1Nodes;
	private GameObject mainPageNodes;
	private GameObject subPage2Nodes;

	private int levelsUnlocked;

	private Vector3 menuBoundsUpper;
	private Vector3 menuBoundsLower;
	private Bounds menuBounds;
	private Vector3 touchPosition;
	private int pageIndex;
	private int levelNodesPerPage = 8;
	private int maxLevels = 200;
	private int maxPages;

	private bool draggingPage;
	private bool animatingPage;

	void Start () {
		subPage2Nodes = subPage2.transform.FindChild ("Buttons_Location").gameObject;
		mainPageNodes = mainMenu.transform.FindChild ("Buttons_Location").gameObject;
		subPage1Nodes = subPage1.transform.FindChild ("Buttons_Location").gameObject;

		int currentLevel = PlayerPrefs.GetInt (PlayerData.CurrentLevelDataLabel);
		if (currentLevel == 0) {
			currentLevel = 1;
			PlayerPrefs.SetInt(PlayerData.CurrentLevelDataLabel, currentLevel);
		}

		maxPages = (int) ((float) maxLevels / (float) levelNodesPerPage);
		pageIndex = currentLevel / levelNodesPerPage;;
		UpdateLevelNodes(pageIndex);

		Bounds spriteBounds = mainMenu.GetComponent<SpriteRenderer>().sprite.bounds;
		Vector3 menuBounds = new Vector3 (spriteBounds.extents.x * 2, spriteBounds.extents.y * 2, 0);
		menuBoundsUpper = menuBounds;
		menuBoundsLower = transform.position - menuBounds;

		//// get the gameobject from the game level to get new unlocked level;
		int level = 6;
		StartCoroutine (CheckForLevelUnlock (level));
	}
	
	void Update () {

		int currentLevel = PlayerPrefs.GetInt (PlayerData.CurrentLevelDataLabel);
		int currentPage = currentLevel / levelNodesPerPage;
		if (!animatingPage && pageIndex < currentPage) {
			animatingPage = true;
			pageScrollSpeed = pageAnimationSpeed;
		}

		if (animatingPage && pageIndex == currentPage) {
			animatingPage = false;
		}

		if (Input.GetMouseButtonDown(0) && !draggingPage && !animatingPage) {
			touchPosition = Input.mousePosition;
			draggingPage = true;
		}

		if (Input.GetMouseButtonUp(0)) {
			draggingPage = false;
		}

		if (!animatingPage) {
			if (draggingPage) {
				Vector3 deltaPosition = Vector3.zero;
				deltaPosition.y = Input.mousePosition.y - touchPosition.y;
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

			pageIndex -= 1;
			UpdateLevelNodes(pageIndex);

			if (!animatingPage)
				pageScrollSpeed = Vector3.zero;
		}
		else if (transform.position.y <= menuBoundsLower.y) {
			transform.position = Vector3.zero;

			pageIndex += 1;
			UpdateLevelNodes(pageIndex);

			if (!animatingPage)
				pageScrollSpeed = Vector3.zero;
		}
	}

	private void UpdateLevelNodes(int mainMenuPage) {

		// sub menu page 2
		UpdatePageLevelNodes (subPage2Nodes, -1);

		// main menu page
		UpdatePageLevelNodes (mainPageNodes, 0);

		// sub menu page 2
		UpdatePageLevelNodes (subPage1Nodes, 1);

		UpdateNodeConnectorCurves ();
	}

	private void UpdateNodeConnectorCurves() {
		string curve8Name = "curveline_8_9";
		
		GameObject curvesRoot = subPage1.transform.FindChild ("Curves").gameObject;
		GameObject level8CurveSub1 = curvesRoot.transform.FindChild (curve8Name).gameObject;
		
		curvesRoot = mainMenu.transform.FindChild ("Curves").gameObject;
		GameObject level8CurveMain = curvesRoot.transform.FindChild (curve8Name).gameObject;

		curvesRoot = subPage2.transform.FindChild ("Curves").gameObject;
		GameObject level8CurveSub2 = curvesRoot.transform.FindChild (curve8Name).gameObject;
		if (pageIndex <= 0) {
			level8CurveSub2.SetActive(false);
				
		}
		else {
			level8CurveSub2.SetActive(true);
		}
		
		if (pageIndex >= maxPages - 1) {
			level8CurveSub2.SetActive(false);
			level8CurveMain.SetActive(false);
			level8CurveSub1.SetActive(false);
		}
	}

	private void UpdatePageLevelNodes (GameObject levelNodesRoot, int pageOffset)
	{
		int activeLevel = PlayerPrefs.GetInt (PlayerData.CurrentLevelDataLabel);
		if (activeLevel == 0) {
			PlayerPrefs.SetInt(PlayerData.CurrentLevelDataLabel, 1);
		}

		activeLevel = 2;

		List<LevelNode> levelNodes = new List<LevelNode> (levelNodesRoot.GetComponentsInChildren<LevelNode> ());

		for (int index = 0; index < levelNodes.Count; index++) {
			LevelNode levelNode = levelNodes.Find (x => x.levelIndex == index);
			if (levelNode != null) {
				GameObject textMesh = levelNode.gameObject.transform.FindChild ("textmesh").gameObject;
				if (textMesh != null) {
					int level = (pageIndex + pageOffset) * levelNodesPerPage + (levelNode.levelIndex + 1);
					levelNode.level = level;

					string levelLabel = level.ToString();

					textMesh.transform.position = levelNode.TextStartPosition + textMeshOffsets[levelLabel.Length - 1];
					textMesh.GetComponent<TextMesh>().fontSize = fontSizes[levelLabel.Length - 1];

					textMesh.GetComponent<TextMesh> ().text = levelLabel;

					if (levelNode.level == activeLevel) {
						UpdateActiveLevelOverlay (levelNode, activeLevel);
					}

					GameObject lockedLevelOverlay = levelNode.transform.FindChild("Level_Locked").gameObject;
					if (level > activeLevel) {
						lockedLevelOverlay.GetComponent<LockManager>().Lock();
					}
					else {
						lockedLevelOverlay.GetComponent<LockManager>().Unlock();
					}
				}
			}
		}
	}
	
	private IEnumerator CheckForLevelUnlock(int activeLevel)
	{
		int lastLockedLevel = PlayerPrefs.GetInt (PlayerData.CurrentLevelDataLabel);
		int levelDelta = activeLevel - lastLockedLevel;

		List<LevelNode> levelNodes = new List<LevelNode> (mainPageNodes.GetComponentsInChildren<LevelNode> ());

		for (int levelIndex = 1; levelIndex <= levelDelta; levelIndex++) {
			int level = lastLockedLevel + levelIndex;
			LevelNode levelNode = levelNodes.Find(x => x.level == level);
			if (levelNode != null) {
				GameObject levelUnlocked = levelNode.transform.FindChild("Level_Locked").gameObject;
				yield return StartCoroutine(levelUnlocked.GetComponent<LockManager>().BeginUnlock());
			}
		}

		LevelNode activeLevelNode = levelNodes.Find(x => x.level == activeLevel);
		UpdateActiveLevelOverlay (activeLevelNode, activeLevel);
	}

	private void UpdateActiveLevelOverlay (LevelNode levelNode, int activeLevel)
	{
		Vector3 nodePosition = levelNode.transform.position;
		currentLevelOverlay.transform.position = new Vector3 (nodePosition.x, nodePosition.y, -5);
		currentLevelOverlay.transform.parent = levelNode.gameObject.transform;
	}
}
