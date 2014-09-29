using UnityEngine;
using System.Collections;

public class ScrollableSprite : MonoBehaviour {

	public Vector3 scrollSpeed;

	private Vector3 localTop;
	private Vector3 localBottom;
	private Bounds bounds;

	private Vector3 destroyLocation;
	private GameObject child;

	// Use this for initialization
	void Start () {
		bounds = GetComponent<SpriteRenderer>().bounds;
		localTop = new Vector3(0, 0, 0);
		localBottom = transform.position + 2 * new Vector3(0, bounds.extents.y, 0);

		destroyLocation = localTop + new Vector3 (bounds.extents.x, bounds.extents.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Time.deltaTime * scrollSpeed;

		if (transform.position.y > destroyLocation.y) {
			GameObject.Destroy(gameObject);
		}

		if (child == null && transform.position.y + bounds.extents.y * 2 * 2 > localBottom.y) {
			child = (GameObject) GameObject.Instantiate(gameObject);
			Vector3 position = transform.position;
			position.y -= bounds.extents.y * 2;
			child.transform.position = position;
		}

//		Vector3 spawnLocationBottomRight = 1.5f * localBottomRight;
//		if (transform.position.y > spawnLocationBottomRight.y) {
//			GameObject sprite = (GameObject) GameObject.Instantiate(gameObject);
//		}
	}
}
