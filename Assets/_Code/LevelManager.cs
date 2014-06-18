using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public class Level
	{
		public int attackCount;
		public List<int> spawns = new List<int>();
		public int progressNeed;
		public float progressBarPartSize;

		//New Changes *** Used below in the constructor
		public float progressBarPartSizePerAsteriodSpawn;
		public float totalsizeprogressbar;
		public int totalnumberofasteriods;
		//


		public Level(string _spawns, int _progressNeed)
		{
			totalsizeprogressbar=1.0f;
			attackCount = 0;
			progressNeed =_progressNeed;
			totalnumberofasteriods=0;
			_totalnumberofasteriods=0;

			//progressBarPartSize = (8/(float)progressNeed);


			for(int i=0;i<_spawns.Length;i++)
			{
				string str = _spawns[i].ToString();

				str = str.Replace(" ","");

				if(!str.Equals(""))
				{
					attackCount++;
					spawns.Add(System.Convert.ToInt32(str.ToString()));
				}
			}

			//New Changes ***
			//Below code is used to calculate part size requried to increase in progress bar when each asteriod instantiates

			foreach(int integer in spawns)
			{
				totalnumberofasteriods += integer;
			}
		

			progressBarPartSizePerAsteriodSpawn=(totalsizeprogressbar/2)/totalnumberofasteriods;
			_partsizeperasteriodspawn=progressBarPartSizePerAsteriodSpawn;
			_totalnumberofasteriods=totalnumberofasteriods;
			//
		}
	}

	public List<Level> AllLevels = new List<Level>();
	public int currentLevel;
	public int currentINC;
	private GameObject levelProgressBar;
	//private int currentProgress;

	//New Changes ***
	public static float _partsizeperasteriodspawn;
	
	//

	private void Awake()
	{
		//currentProgress = 0;
		AllLevels.Add(new Level("3 3",30));
		levelProgressBar = (GameObject) GameObject.Find("level_progress/blank_color");
		levelProgressBar.transform.localScale=new Vector3(1.01f,0f,0f);
	}

	public bool CheckIfProgressIfFull()
	{
		// New Changes ***
		// Enter Method Modified

		if(levelProgressBar.transform.localScale.y>0.98f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static int _totalnumberofasteriods;


	public IEnumerator UpdateLevelProgressBarForAsteriodsSpawned(int numberOfAsteriodsSpawned)
	{
		float pluspart=(_partsizeperasteriodspawn*numberOfAsteriodsSpawned)/7;
		for(int i=0;i<7;i++)
		{
			if(levelProgressBar.transform.localScale.y<0.99f)
			{
				yield return new WaitForSeconds(0.01f);
				levelProgressBar.transform.localScale +=new Vector3(0f,pluspart,0f);
			}
		}	
		yield return null;
	}
	

	public IEnumerator UpdateLevelProgressBarForAsteroidsDestroyed()
	{
		float pluspart=_partsizeperasteriodspawn/7;
		for(int i=0;i<7;i++)
		{
			if(levelProgressBar.transform.localScale.y<0.99f)
			{
				yield return new WaitForSeconds(0.01f);
				levelProgressBar.transform.localScale +=new Vector3(0f,pluspart,0f);
			}
		}
		yield return null;
	}
	


	public int GetSpawnCountAutoINC()
	{
		int spawnCount = AllLevels[currentLevel].spawns[currentINC];

		if(currentINC < AllLevels[currentLevel].spawns.Count-1)
		{
			currentINC++;
		}
		else
		{
			return Random.Range(0,2);
		}

		return spawnCount;
	}

}
