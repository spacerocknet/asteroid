using UnityEngine;
using System.Collections;

public class ProgressBarManager : MonoBehaviour {
	public GameObject asteroidsDestroyedBar;

	private float progressBarScaleMaxY;
	private float segmentScaleY;

	private LevelInfo levelInfo;

	private System.DateTime startTimeStamp;
	private System.DateTime endTimeStamp;

	// Use this for initialization
	void Start () {
		levelInfo = GameObject.FindObjectOfType<LevelInfo> ();

		progressBarScaleMaxY = 1;
		int segments = levelInfo.selectedNodeInfo.totalRocks;
		segmentScaleY = progressBarScaleMaxY / segments;
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
