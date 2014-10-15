using UnityEngine;
using System.Collections;

public class LevelCompleteInfo : MonoBehaviour {

	public int levelUnlocked;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
