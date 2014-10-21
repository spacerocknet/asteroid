using UnityEngine;
using System.Collections;

public class CategorySboxColliderScale : MonoBehaviour {

	// Use this for initialization
	void Start () {
		(collider2D as BoxCollider2D).size += new Vector2(0.4f, 1.0f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
