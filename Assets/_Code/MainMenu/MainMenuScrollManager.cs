using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScrollManager : MonoBehaviour {

	public Vector3 dragSpeed;
	public float dragSpeedModifier;
	
	public Vector3 deceleration;

	public GameObject submenu1;
	public GameObject mainMenu;
	public GameObject submenu2;

	public GameObject level1Curve;
	public GameObject level8Curve;

	public int[] fontSizes;
	public Vector3[] textMeshOffsets;

	private Vector3 menuBoundsUpper;
	private Vector3 menuBoundsLower;
	private Bounds menuBounds;
	private Vector3 touchPosition;
	private int mainMenuPageIndex;
	private int levelNodesPerPage = 8;
	private int maxLevels = 200;
	private int maxPages;

	private bool dragging;

	void Start () {
		maxPages = (int) ((float) maxLevels / (float) levelNodesPerPage);
		mainMenuPageIndex = 0;
		UpdateLevelNodes(mainMenuPageIndex);

		Bounds spriteBounds = mainMenu.GetComponent<SpriteRenderer>().sprite.bounds;
		Vector3 menuBounds = new Vector3 (spriteBounds.extents.x * 2, spriteBounds.extents.y * 2, 0);
		menuBoundsUpper = menuBounds;
		menuBoundsLower = transform.position - menuBounds;
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0) && !dragging) {
			touchPosition = Input.mousePosition;
			dragging = true;
		}

		if (Input.GetMouseButtonUp(0)) {
			dragging = false;
		}

		if (dragging) {
			Vector3 deltaPosition = Vector3.zero;
			deltaPosition.y = Input.mousePosition.y - touchPosition.y;
			dragSpeed = deltaPosition * Time.deltaTime * dragSpeedModifier;
		}
		else {
			if (dragSpeed.magnitude > 0) {
				if (dragSpeed.y >= 0) {
					dragSpeed -= Time.deltaTime * deceleration;
				}
				else {
					dragSpeed += Time.deltaTime * deceleration;
				}
			}
		}

		if (mainMenuPageIndex <= 0 && dragSpeed.y > 0) {
			dragSpeed = Vector3.zero;
		}

		if (mainMenuPageIndex >= maxPages - 1 && dragSpeed.y < 0) {
			dragSpeed = Vector3.zero;
		}

		transform.position += dragSpeed * Time.deltaTime;

		if (transform.position.y >= menuBoundsUpper.y) {
			transform.position = Vector3.zero;

			mainMenuPageIndex -= 1;
			UpdateLevelNodes(mainMenuPageIndex);

			dragSpeed = Vector3.zero;
		}
		else if (transform.position.y <= menuBoundsLower.y) {
			transform.position = Vector3.zero;

			mainMenuPageIndex += 1;
			UpdateLevelNodes(mainMenuPageIndex);

			dragSpeed = Vector3.zero;
		}
	}

	private void UpdateLevelNodes(int mainMenuPage) {

		// sub menu page 2
		GameObject subMenuPageNodes2 = submenu2.transform.FindChild ("Buttons_Location").gameObject;
		UpdatePageLevelNodes (subMenuPageNodes2, -1);

		// main menu page
		GameObject mainPageNodes = mainMenu.transform.FindChild ("Buttons_Location").gameObject;
		UpdatePageLevelNodes (mainPageNodes, 0);

		// sub menu page 2
		GameObject subMenuPageNodes1 = submenu1.transform.FindChild ("Buttons_Location").gameObject;
		UpdatePageLevelNodes (subMenuPageNodes1, 1);

		UpdateNodeConnectorCurves ();
	}

	private void UpdateNodeConnectorCurves() {
		string curve1Name = "curveline_1_2";
		string curve8Name = "curveline_8_9";
		
		GameObject curvesRoot = submenu1.transform.FindChild ("Curves").gameObject;
		GameObject level1CurveSub1 = curvesRoot.transform.FindChild (curve1Name).gameObject;
		GameObject level8CurveSub1 = curvesRoot.transform.FindChild (curve8Name).gameObject;
		
		curvesRoot = mainMenu.transform.FindChild ("Curves").gameObject;
		GameObject level1CurveMain = curvesRoot.transform.FindChild (curve1Name).gameObject;
		GameObject level8CurveMain = curvesRoot.transform.FindChild (curve8Name).gameObject;
		
		curvesRoot = submenu2.transform.FindChild ("Curves").gameObject;
		GameObject level1CurveSub2 = curvesRoot.transform.FindChild (curve1Name).gameObject;
		GameObject level8CurveSub2 = curvesRoot.transform.FindChild (curve8Name).gameObject;
		if (mainMenuPageIndex <= 0) {
			level8CurveSub2.SetActive(false);
				
		}
		else {
			level8CurveSub2.SetActive(true);
		}
		
		if (mainMenuPageIndex >= maxPages - 1) {
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
					int level = (mainMenuPageIndex + pageOffset) * levelNodesPerPage + (levelNode.levelIndex + 1);
					levelNode.level = level;

					string levelLabel = level.ToString();

					textMesh.transform.position = levelNode.TextStartPosition + textMeshOffsets[levelLabel.Length - 1];
					textMesh.GetComponent<TextMesh>().fontSize = fontSizes[levelLabel.Length - 1];

					textMesh.GetComponent<TextMesh> ().text = levelLabel;
				}
			}
		}
	}
}
