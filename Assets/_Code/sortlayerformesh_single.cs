using UnityEngine;
using System.Collections;

public class sortlayerformesh_single : MonoBehaviour {

	void Awake()
	{
		this.renderer.sortingOrder=7;
	}
}
