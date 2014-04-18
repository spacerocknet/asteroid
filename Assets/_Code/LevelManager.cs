using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public class Level
	{
		public int attackCount;
		public List<int> spawns = new List<int>();

		public Level(string _spawns)
		{
			attackCount = 0;

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

	private void Awake()
	{
		AllLevels.Add(new Level("5 2 3 3 2 1 1 3 1 3 2"));
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
