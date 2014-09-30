using UnityEngine;
using System.Collections;

public class LockManager : MonoBehaviour {
	public bool isLocked;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator BeginUnlock() {
		float localScale = transform.localScale.x;
		
		while (localScale > 0.1f) {
			localScale -= Time.deltaTime * 1.25f;
			transform.localScale = new Vector3(localScale, localScale, 1);
			
			yield return null;
		}
		
		isLocked = false;
		gameObject.SetActive (false);
		
		yield return null;
	}

	public void Unlock() {
		isLocked = false;
		gameObject.SetActive (false);
	}

	public void Lock() {
		isLocked = true;
		transform.localScale = new Vector3(1, 1, 1);
		gameObject.SetActive (true);
	}
}
