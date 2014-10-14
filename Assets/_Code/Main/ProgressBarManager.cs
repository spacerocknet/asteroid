using UnityEngine;
using System.Collections;

public class ProgressBarManager : MonoBehaviour {
	public GameObject asteroidsDestroyedBar;

	private float progressBarScaleMaxY;
	private float segmentScaleY;

	private LevelInfo levelInfo;

	private System.DateTime startTimeStamp;
	private System.DateTime endTimeStamp;

	private ScreenSizeManager screenSizeManager;

	// Use this for initialization
	void Start () {
		levelInfo = GameObject.FindObjectOfType<LevelInfo> ();
		screenSizeManager = GameObject.FindObjectOfType<ScreenSizeManager> ();

		progressBarScaleMaxY = 1;
		int segments = levelInfo.selectedNodeInfo.totalRocks;
		segmentScaleY = progressBarScaleMaxY / segments;

		Vector3 position = transform.position;
		position.y -= renderer.bounds.extents.y;
		asteroidsDestroyedBar.transform.position = position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartTimer() {
		startTimeStamp = System.DateTime.Now;
	}

	public void StopTimer() {
		endTimeStamp = System.DateTime.Now;
	}

	public void UpdateProgressBar(int asteroidsDestroyed) {
		Vector3 destroyedBarScale = new Vector3(1, progressBarScaleMaxY, 1);
		destroyedBarScale.y = segmentScaleY * asteroidsDestroyed;

		asteroidsDestroyedBar.transform.localScale = destroyedBarScale;
	}

	public int GetCompletionTimeSeconds() {
		return (int) (endTimeStamp - startTimeStamp).TotalSeconds;
	}
}
