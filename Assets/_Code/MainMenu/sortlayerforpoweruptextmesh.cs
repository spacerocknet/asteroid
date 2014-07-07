using UnityEngine;
using System.Collections;

public class sortlayerforpoweruptextmesh : MonoBehaviour {

	public GameObject[] textmeshes;

	public GameObject poweruptoplist;

	void Awake () {
		for(int i=0;i<textmeshes.Length;i++)
		{
			textmeshes[i].renderer.sortingOrder=7;
		}

		foreach (Transform child in poweruptoplist.transform)
		{
			Debug.Log("CALLED");
			if(child.gameObject.GetComponent<TextMesh>()!=null)
			{
				Debug.Log(child.gameObject.name);
				child.gameObject.renderer.sortingOrder=7;
			}
		}
	}
}
