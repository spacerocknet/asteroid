using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeRewardInfoCollection : MonoBehaviour {
	public TextAsset rewardsTextAsset;

	private List<NodeRewardInfo> nodeRewardInfos;

	// Use this for initialization
	void Start () {
		ParseNodeRewardsCsvFile ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public NodeRewardInfo GetLevelNodeRewardInfo(int level) {
		NodeRewardInfo result = null;

		if (nodeRewardInfos != null) {
			result = nodeRewardInfos.Find(x => x.level == level);
		}

		return result;
	}

	private void ParseNodeRewardsCsvFile() {
		nodeRewardInfos = new List<NodeRewardInfo> ();

		string[] nodeRewardLines = CSVReader.SplitCsvLines (rewardsTextAsset.text);
		if (nodeRewardInfos != null) {
			for (int index = 1; index < nodeRewardLines.Length; index++) {
				string nodeRewardLine = nodeRewardLines[index];
				if (!string.IsNullOrEmpty(nodeRewardLine)) {
					string[] rewardInfo = CSVReader.SplitCsvLine(nodeRewardLine);
					if (rewardInfo != null) {
						NodeRewardInfo nodeRewardInfo = new NodeRewardInfo();
						nodeRewardInfo.level = int.Parse(rewardInfo[0]);
						nodeRewardInfo.goldPayout = int.Parse(rewardInfo[1]);
						nodeRewardInfo.xpPayout = int.Parse(rewardInfo[2]);

						nodeRewardInfos.Add(nodeRewardInfo);
					}
				}
			}
		}
	}

	public class NodeRewardInfo {
		public int level;
		public int goldPayout;
		public int xpPayout;
	}
}
