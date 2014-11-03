using UnityEngine;
using System.Collections;

public class LevelTextZSorting : MonoBehaviour {
	public string sortingLayerName;
	public int sortingOrder;


	// Use this for initialization
	void Start () {
		renderer.sortingLayerName = sortingLayerName;
		renderer.sortingOrder = sortingOrder;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
