using UnityEngine;
using System.Collections;
using System.IO;

public class LoadStreamingAssets : MonoBehaviour {

	// Use this for initialization
	void Start () {
		WriteSprites ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void WriteSprites() {

		if (!Directory.Exists(Application.persistentDataPath + "/SpaceRock")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/SpaceRock");
		}

		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/Sprites/");
		FileInfo[] info = dir.GetFiles("*.png", SearchOption.AllDirectories);
		foreach (FileInfo fi in info) {
			byte[] bytes = File.ReadAllBytes(fi.FullName);

			int index = fi.DirectoryName.IndexOf("Sprites");
			string filePrefix = fi.DirectoryName.Substring(index).Replace("\\", "_");
		} 
	}
}
