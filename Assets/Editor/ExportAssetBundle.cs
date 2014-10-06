using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ExportAssetBundle {

	[@MenuItem("Assets/Build AssetBundles From Directory of Files - No dependency tracking")]
	static void ExportAssetBundles () 
	{
		string bundlePath = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");

		// Get the selected directory
		string path = "Assets/Resources/Sprites";
		Debug.Log(path);
		if (path.Length != 0)
		{
			Object mainObject = null;
			List<Object> assets = new List<Object>();

			path = path.Replace("Assets/", "");
			Debug.Log(path);
			string [] fileEntries = Directory.GetFiles(Application.dataPath+"/"+path, "*.png", SearchOption.AllDirectories);
			foreach(string fileName in fileEntries)
			{
				Debug.Log(fileName);
				int index = fileName.IndexOf("\\");
				string spriteAssetPath = fileName.Substring(index).Replace("\\", "/");

				string assetPath = "Assets/" + path + spriteAssetPath;

				Object t = AssetDatabase.LoadMainAssetAtPath(assetPath);
				if (t != null)
				{
					assets.Add(t);
					mainObject = t;
				}
			}

			Debug.Log(bundlePath);
			
			BuildPipeline.BuildAssetBundle(mainObject, assets.ToArray(), bundlePath, BuildAssetBundleOptions.CompleteAssets);
		}
	}
}
