using UnityEngine;
using System.Collections;

public class ProgressBarManager : MonoBehaviour {
	public GameObject asteroidsDestroyedBar;

	private float progressBarScaleMaxY;
	private float segmentScaleY;

	private LevelInfo levelInfo;

	public float totalLevelAsteroidHitPoints;
	private float totalLevelHitPointsDestroyed;

	private System.DateTime startTimeStamp;
	private System.DateTime endTimeStamp;

	private ScreenSizeManager screenSizeManager;

	void Awake() {
		levelInfo = GameObject.FindObjectOfType<LevelInfo> ();
		screenSizeManager = GameObject.FindObjectOfType<ScreenSizeManager> ();

		totalLevelAsteroidHitPoints = GetTotalLevelAsteroidHitPoints ();
	}

	// Use this for initialization
	void Start () {
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

	public void UpdateProgressBar(float hitPointsDone) {
		Vector3 destroyedBarScale = new Vector3(1, progressBarScaleMaxY, 1);
		destroyedBarScale.y = hitPointsDone / totalLevelAsteroidHitPoints;
		
		asteroidsDestroyedBar.transform.localScale = destroyedBarScale;
	}

	public int GetCompletionTimeSeconds() {
		return (int) (endTimeStamp - startTimeStamp).TotalSeconds;
	}

	private float GetTotalLevelAsteroidHitPoints() {
		float totalHitPoints = 0;
		
		totalHitPoints += levelInfo.selectedNodeInfo.smallRocks * 0.75f * levelInfo.selectedNodeInfo.multiplier;
		totalHitPoints += levelInfo.selectedNodeInfo.bigRocks * 1.3125f * levelInfo.selectedNodeInfo.multiplier;
		
		return totalHitPoints;
	}
}
