using UnityEngine;
using System.Collections;

public class LevelNode : MonoBehaviour {
	public int levelIndex;
	public int level;

	public Vector3 TextStartPosition {
		get { return textStartPosition; }
	}

	private Vector3 textStartPosition;

	// Use this for initialization
	void Start () {
		textStartPosition = transform.FindChild ("textmesh").position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
