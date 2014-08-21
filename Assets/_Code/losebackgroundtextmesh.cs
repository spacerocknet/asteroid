using UnityEngine;
using System.Collections;

public class losebackgroundtextmesh : MonoBehaviour {


	public GameObject lose_renderer;

	void Awake()
	{
		foreach (Transform child in lose_renderer.transform)
		{
			Debug.Log("called tuhin");
			if(child.gameObject.GetComponent<TextMesh>()!=null)
			{
		
				Debug.Log(child.gameObject.name);
				child.gameObject.renderer.sortingOrder=7;
			}
		}
	}

}
