using UnityEngine;
using System.Collections;

public class XPBarManager : MonoBehaviour {

	public GameObject xpLevelText;
	public GameObject xpProgressBar;

	private float maxScaleX;

	private XPLevelInfoCollection xpLevelInfoCollection;

	// Use this for initialization
	void Start () {
		maxScaleX = xpProgressBar.transform.localScale.x;

		xpLevelInfoCollection = GameObject.FindObjectOfType<XPLevelInfoCollection> ();
		ulong totalXP = ulong.Parse(PlayerPrefs.GetString(PlayerData.TotalXPKey, "0"));

		UpdateBarPosition ();

		XPLevelInfo currentLevelInfo = xpLevelInfoCollection.GetCurrentLevelInfo (totalXP);
		XPLevelInfo nextXPLevelInfo = xpLevelInfoCollection.GetNextLevelInfo (totalXP);

		ulong currentXPLevel = currentLevelInfo != null ? currentLevelInfo.xpNextLevel : 0;
		float cumulativeXPDelta = nextXPLevelInfo.xpNextLevel - currentXPLevel;

		if (cumulativeXPDelta == 0) {
			cumulativeXPDelta = nextXPLevelInfo.xpNextLevel;
		}

		float scaleX = 1 - maxScaleX * ((float)nextXPLevelInfo.xpNextLevel - (float)totalXP) / cumulativeXPDelta;
		Vector3 scale = xpProgressBar.transform.localScale;
		scale.x = scaleX;
		xpProgressBar.transform.localScale = scale;

		xpLevelText.GetComponent<TextMesh> ().text = currentLevelInfo != null ? currentLevelInfo.level.ToString() : "0";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void UpdateBarPosition ()
	{
		Vector3 parentPosition = xpProgressBar.transform.parent.position;
		Vector3 position = new Vector3(parentPosition.x, parentPosition.y, xpProgressBar.transform.position.z);
		float width = xpProgressBar.renderer.bounds.extents.x;
		position.x -= width;
		xpProgressBar.transform.position = position;
	}
}
