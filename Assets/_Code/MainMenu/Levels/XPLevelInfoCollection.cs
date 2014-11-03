using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XPLevelInfoCollection : MonoBehaviour {

	public TextAsset xpLevelCSVTextAsset;

	private List<XPLevelInfo> xpLevelInfos;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);

		xpLevelInfos = new List<XPLevelInfo>();

		string[] xpLevelInfoCsvLines = CSVReader.SplitCsvLines (xpLevelCSVTextAsset.text);
		if (xpLevelInfoCsvLines != null) {
			for (int index = 1; index < xpLevelInfoCsvLines.Length - 1; index++) {
				string[] xpInfo = CSVReader.SplitCsvLine(xpLevelInfoCsvLines[index]);

				XPLevelInfo xpLevelInfo = new XPLevelInfo();
				xpLevelInfo.level = int.Parse(xpInfo[0]);
				xpLevelInfo.xpNextLevel = ulong.Parse(xpInfo[1]);
				xpLevelInfo.cumulativeXP = ulong.Parse(xpInfo[2]);

				xpLevelInfos.Add(xpLevelInfo);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public XPLevelInfo GetCurrentLevelInfo(ulong totalXP) {
		XPLevelInfo result = null;

		if (xpLevelInfos != null) {
			result = xpLevelInfos[0];
			for (int index = 0; index < xpLevelInfos.Count; index++) {
				if (index < xpLevelInfos.Count - 1) {
					XPLevelInfo xpLevelInfo = xpLevelInfos[index];
					XPLevelInfo nextxpLevelInfo = xpLevelInfos[index + 1];
					if (totalXP == xpLevelInfo.cumulativeXP) {
						result = nextxpLevelInfo;
						break;
					}

					if (totalXP >= xpLevelInfo.cumulativeXP && totalXP < nextxpLevelInfo.cumulativeXP) {
						result = xpLevelInfo;						
						break;
					}
				}
				else {
					XPLevelInfo xpLevelInfo = xpLevelInfos[index];
					if (totalXP >= xpLevelInfo.cumulativeXP) {
						result = xpLevelInfo;						
						break;
					}
				}
			}
		}

		return result;
	}

	public XPLevelInfo GetNextLevelInfo(ulong totalXP) {
		XPLevelInfo result = null;

		if (xpLevelInfos != null) {
			result = xpLevelInfos[0];
			for (int index = 0; index < xpLevelInfos.Count; index++) {
				if (index < xpLevelInfos.Count - 1) {
					XPLevelInfo xpLevelInfo = xpLevelInfos[index];
					XPLevelInfo nextxpLevelInfo = xpLevelInfos[index + 1];

					if (totalXP >= xpLevelInfo.cumulativeXP && totalXP < nextxpLevelInfo.cumulativeXP) {
						result = nextxpLevelInfo;

						break;
					}
				}
				else {
					XPLevelInfo xpLevelInfo = xpLevelInfos[index];
					if (totalXP >= xpLevelInfo.cumulativeXP) {
						result = xpLevelInfo;

						break;
					}
				}
			}
		}

		return result;
	}
}
