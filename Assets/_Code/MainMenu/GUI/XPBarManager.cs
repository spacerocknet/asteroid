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

//		PlayerPrefs.SetString (PlayerData.TotalXPKey, "0");
//		PlayerPrefs.SetString (PlayerData.CurrentXPLevel, "1");

		xpLevelInfoCollection = GameObject.FindObjectOfType<XPLevelInfoCollection> ();
		ulong totalXP = ulong.Parse(PlayerPrefs.GetString(PlayerData.TotalXPKey, "0"));

		UpdateBarPosition ();

		XPLevelInfo currentLevelInfo = xpLevelInfoCollection.GetCurrentLevelInfo (totalXP);
		XPLevelInfo nextXPLevelInfo = xpLevelInfoCollection.GetNextLevelInfo (totalXP);

		float xpToNextLevel = (float) nextXPLevelInfo.cumulativeXP - nextXPLevelInfo.xpNextLevel;
		if (currentLevelInfo.level == 1) {
			xpToNextLevel = 0;
		}

		float scaleX = maxScaleX * (float) (totalXP - xpToNextLevel) / (float) (nextXPLevelInfo.cumulativeXP - xpToNextLevel);
		Vector3 scale = xpProgressBar.transform.localScale;
		scale.x = scaleX;
		xpProgressBar.transform.localScale = scale;

		xpLevelText.GetComponent<TextMesh> ().text = nextXPLevelInfo.level.ToString ();
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
