using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelNodeInfoCollection : MonoBehaviour {

	public TextAsset levelNodeCSVTextAsset;

	private List<NodeInfo> levelNodeInfos;

	// Use this for initialization
	void Start () {
		ParseLevelNodeCsvFile ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public NodeInfo GetLevelNodeInfo(int level) {
		NodeInfo result = null;

		if (levelNodeInfos != null) {
			result = levelNodeInfos.Find(x => x.level == level);
		}

		return result;
	}

	private void ParseLevelNodeCsvFile ()
	{
		levelNodeInfos = new List<NodeInfo> ();

		string[] levelInfoCsvLines = CSVReader.SplitCsvLines (levelNodeCSVTextAsset.text);
		if (levelInfoCsvLines != null) {
			// skip the header
			for (int index = 1; index < levelInfoCsvLines.Length - 1; index++) {
				string levelInfoSCsvLine = levelInfoCsvLines [index];
				string[] levelInfo = CSVReader.SplitCsvLine (levelInfoSCsvLine);
				NodeInfo levelNodeInfo = new NodeInfo ();
				levelNodeInfo.tier = levelInfo [0];
				levelNodeInfo.theme = levelInfo [1];
				levelNodeInfo.level = int.Parse (levelInfo [2]);
				levelNodeInfo.difficultyRating = double.Parse (levelInfo [3]);
				levelNodeInfo.bigRocks = int.Parse (levelInfo [4]);
				levelNodeInfo.smallRocks = int.Parse (levelInfo [5]);
				levelNodeInfo.totalRocks = int.Parse (levelInfo [6]);
				levelNodeInfo.multiplier = int.Parse (levelInfo [7]);
				levelNodeInfo.totalHealth = double.Parse (levelInfo [8]);
				levelNodeInfo.targetValue = float.Parse (levelInfo [9]);
				levelNodeInfo.bluePercent = float.Parse (levelInfo [10].Remove (levelInfo [10].Length - 1));
				levelNodeInfo.redPercent = float.Parse (levelInfo [11].Remove (levelInfo [11].Length - 1));
				levelNodeInfo.greenPercent = float.Parse (levelInfo [12].Remove (levelInfo [12].Length - 1));
				levelNodeInfo.totalPercent = float.Parse (levelInfo [13].Remove (levelInfo [13].Length - 1));

				levelNodeInfos.Add (levelNodeInfo);
			}
		}
	}

	public class NodeInfo {
		
		public string tier;
		public string theme;
		public int level;
		public double difficultyRating;
		public int bigRocks;
		public int smallRocks;
		public int totalRocks;
		public int multiplier;
		public double totalHealth;
		public float targetValue;
		public float bluePercent;
		public float redPercent;
		public float greenPercent;
		public float totalPercent;

		public float hitPointsDone;
	}
}
