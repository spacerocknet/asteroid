using UnityEngine;
using System.Collections;

public class OnOffButton : MonoBehaviour {

	private bool buttonOn;

	void Awake() {
		buttonOn = false;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleButtonState() {
		buttonOn = !buttonOn;
	}

	public bool ButtonIsOn() {
		return buttonOn;
	}

	public void SetButtonState(bool state) {
		buttonOn = state;
	}
}
