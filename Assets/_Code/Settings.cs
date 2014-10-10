using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {
	[HideInInspector]
	public float orthoSize;
	private Camera cam;

	private void Start()
	{
		Screen.SetResolution(600,1024,false);
		Application.targetFrameRate = 60;
		cam = (Camera) this.gameObject.GetComponent<Camera>();
		SetOrthoSize();
	}

	private void LateUpdate()
	{
		SetOrthoSize();
	}

	private void SetOrthoSize()
	{
		orthoSize = Screen.height/2.0f/100.0f;
		cam.orthographicSize = orthoSize;
	}
}