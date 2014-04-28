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

		public Level(string _spawns, int _progressNeed)
		{
			attackCount = 0;
			progressNeed = _progressNeed;

			progressBarPartSize = (8.0f/(float)progressNeed);

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
		}
	}

	public List<Level> AllLevels = new List<Level>();
	public int currentLevel;
	public int currentINC;
	private GameObject levelProgressBar;
	private int currentProgress;

	private void Awake()
	{
		currentProgress = 0;
		AllLevels.Add(new Level("5 2 3 3 2 1 1 3 1 3 2",30));
		levelProgressBar = (GameObject) GameObject.Find("level_progress/blank_color");
	}

	public bool CheckIfProgressIfFull()
	{
		if(currentProgress>=AllLevels[currentLevel].progressNeed)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public IEnumerator UpdateLevelProgressBar()
	{
		currentProgress++;

		float plusPart = AllLevels[currentLevel].progressBarPartSize / 10.0f;

		for(int i=0;i<10;i++)
		{	
			if(levelProgressBar.transform.localScale.y<8.0f)
			{
				levelProgressBar.transform.localScale += new Vector3(0,plusPart,0);
			}

			yield return 0;
		}

		if(levelProgressBar.transform.localScale.y>8.0f)
		{
			levelProgressBar.transform.localScale = new Vector3(0.9761209f,8.0f,1f);
		}

		yield return 0;
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
