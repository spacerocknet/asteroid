using UnityEngine;
using System.Collections;

public class TutorialBase : MonoBehaviour {
	public GameObject pointerPrefab;
	public GameObject instructionTextPrefab;

	public int tutorialLevel;
	public bool isRunning;

	public virtual void Begin(int currentLevel) {
		isRunning = true;
	}

	public virtual void End() {
		isRunning = false;
	}

	protected Vector3 GetPointerOffset() {
		Vector3 offset = Vector3.zero;

		offset.y = -pointerPrefab.renderer.bounds.extents.y * 2;

		return offset;
	}

	protected string ResolveTextSize(string input, int lineLength) {	   
		string[] words = input.Split(' ');
		string result = "";
		string line = "";
		
		foreach(string s in words){
			// Append current word into line
			string temp = line + " " + s;
			
			if(temp.Length > lineLength) {
				
				result += line + "\n";
				line = s;
			} else {
				line = temp;
			}
		}
		
		// Append last line into result   
		result += line;
		
		// Remove first " " char
		return result.Substring(1,result.Length-1);
	}
}
