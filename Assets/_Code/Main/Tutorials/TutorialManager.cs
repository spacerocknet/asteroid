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
				activeTutorial = allTutorials.Find(x => x.tutorialLevel == currentLevel);
				if (activeTutorial != null) {
					activeTutorial.enabled = true;

					activeTutorial.Begin(currentLevel);
				}

				if (currentLevel < powerUpsLevelStart) {
					if (powerUpButons != null) {
						powerUpButons.SetActive(false);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
