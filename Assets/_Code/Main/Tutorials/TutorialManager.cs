using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour {

	public List<TutorialBase> allTutorials;
	public int powerUpsLevelStart;
	public GameObject powerUpButons;

	private TutorialBase activeTutorial;
	private LevelInfo levelInfo;

	// Use this for initialization
	void Start () {
		if (allTutorials != null) {
			levelInfo = GameObject.FindObjectOfType<LevelInfo>();
			if (levelInfo != null) {
				int currentLevel = levelInfo.selectedNodeInfo.level;
				StartLevelTutorial (currentLevel);

			}
		}
	}

	public void StartTutorial(int level) {
		StartLevelTutorial (level);
	}

	public void EndTutorial(int level) {
		activeTutorial = allTutorials.Find (x => x.tutorialLevel == level);
		if (activeTutorial != null) {
			activeTutorial.enabled = true;
			activeTutorial.End ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void StartLevelTutorial (int level) {
		activeTutorial = allTutorials.Find (x => x.tutorialLevel == level);
		if (activeTutorial != null) {
			activeTutorial.enabled = true;
			activeTutorial.Begin (level);
		}
		if (level < powerUpsLevelStart) {
			if (powerUpButons != null) {
				powerUpButons.SetActive (false);
			}
		}
	}
}
