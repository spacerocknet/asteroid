using UnityEngine;
using System.Collections;

public class ScreenTextSizeManager : MonoBehaviour {

	public Vector3 offset;

	private ScreenSizeManager screenSizeManager;

	// Use this for initialization
	void Start () {
		screenSizeManager = GameObject.FindObjectOfType<ScreenSizeManager> ();
		
		float scaleX = 1 / screenSizeManager.scaleX;
		
		TextMesh textMesh = gameObject.GetComponent<TextMesh> ();
		textMesh.transform.localPosition += offset * scaleX;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
