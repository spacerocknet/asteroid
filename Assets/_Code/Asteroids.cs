using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroids : MonoBehaviour {

	public Vector3 bigAsteroidScale = Vector3.zero;
	public Vector3 smallAsteroidScale = Vector3.zero;

	private GameObject asteroidRef;
	[HideInInspector]
	public List<Asteroid> currentAsteroids = new List<Asteroid>();
	public int asteroidsDestroyed;
	private GameObject INIT;
	private static int ASTEROID_LIMIT = 8;
	private List<float> lifeList = new List<float>();
	private LevelInfo levelInfo;

	public class Asteroid
	{
		public AsteroidColorTypes colorType;
		public float life;
		public Color color;
		public Sprite colorsprite;
		public GameObject obj;
		public bool isDead;

		public float smallAsteroidLifeHits;
		private Vector3 smallAsteroidScale;

		private GameObject lifeHitsLabel;
		
		public Asteroid(AsteroidColorTypes _colorType, float _life, GameObject _obj, float smallLifeHits, Vector3 smallScale)
		{
			isDead = false;
			colorType = _colorType;
			life = _life;
			obj = _obj;

			smallAsteroidLifeHits = smallLifeHits;
			smallAsteroidScale = smallScale;

			UpdateDebugText ();

			DefineColor();
		}

		private void DefineColor()
		{
			switch(colorType)
			{
				case AsteroidColorTypes.Red:
				this.colorsprite=Resources.Load("Rocks/rock_red",typeof(Sprite)) as Sprite; 
					break;
				case AsteroidColorTypes.Green:
				this.colorsprite=Resources.Load("Rocks/rock_green",typeof(Sprite)) as Sprite; 
					break;
				case AsteroidColorTypes.Blue:
				this.colorsprite=Resources.Load("Rocks/rock_blue",typeof(Sprite)) as Sprite; 
					break;
				default: break;
			}

			obj.GetComponent<SpriteRenderer>().sprite=this.colorsprite;
		}

		public IEnumerator DoDamage(float hitPoints)
		{
			if(life<hitPoints)
			{
				hitPoints = life;
			}

			life -= hitPoints;
			UpdateDebugText ();

			if (life < smallAsteroidLifeHits) {
				obj.transform.localScale = smallAsteroidScale;
			}

			if(life<=0)
			{
				isDead = true;
			}

			for(int i=0;i<hitPoints;i++)
			{
				for(int j=0;j<5;j++)
				{
					obj.transform.localScale -= new Vector3(0.1f,0.1f,0);
					yield return 0;
				}
			}
		}

		void UpdateDebugText ()
		{
			if (obj.transform.FindChild ("textmesh") != null) {
				lifeHitsLabel = obj.transform.FindChild ("textmesh").gameObject;
				lifeHitsLabel.GetComponent<TextMesh> ().text = life.ToString ();
			}
		}
	}

	public enum AsteroidColorTypes
	{
		Unknown = -1,
		Green = 0,
		Red = 1,
		Blue = 2
	}

	private void Awake()
	{
		asteroidRef = (GameObject) GameObject.Find("REFERENCES/Asteroid");
		INIT = (GameObject) GameObject.Find("RUNTIME_INIT");

		lifeList.Add(0.6f);
		lifeList.Add(1.0f);
		lifeList.Add(1.5f);

		levelInfo = GameObject.FindObjectOfType<LevelInfo> ();
	}

	public IEnumerator MoveAsteroids()
	{
		for(int j=0;j<20;j++)
		{
			for(int i=0;i<currentAsteroids.Count;i++)
			{
				currentAsteroids[i].obj.transform.position -= new Vector3(0,0.03f,0);
			}

			yield return 0;
		}

		yield return 0;
	}
	
	public IEnumerator ReverseTimePowerUp()
	{
		for(int j=0;j<20;j++)
		{
			for(int i=0;i<currentAsteroids.Count;i++)
			{
				currentAsteroids[i].obj.transform.position -=new Vector3(0,-0.03f,0);
			}
			yield return 0;
		}
		yield return new WaitForSeconds(0.35f);

		for(int j=0;j<20;j++)
		{
			for(int i=0;i<currentAsteroids.Count;i++)
			{
				currentAsteroids[i].obj.transform.position -=new Vector3(0,-0.03f,0);
			}
			yield return 0;
		}
	}

	public IEnumerator SpawnAsteroids(int count, float diff)
	{

		if(currentAsteroids.Count+count>ASTEROID_LIMIT)
		{
			while(currentAsteroids.Count+count>ASTEROID_LIMIT)
			{
				--count;
			}
		}

		
		//New Changes ***
		StartCoroutine("NotifyProgressBarForAsteriodSpawn",count);
		//

		for(int i=0;i<count;i++)
		{
			int lifeHits = Random.Range(1,4);
			SpawnAsteroid (lifeHits, Vector3.zero, (AsteroidColorTypes)Random.Range (0, 3), lifeHits);
		}

		yield return null;
	}

	public IEnumerator SpawnAsteroids(int count) {
		count = Mathf.Clamp (count, count, levelInfo.selectedLevelNodeInfo.totalRocks);

		for (int index = 0; index < count; index++) {
			int bigRocks = levelInfo.selectedLevelNodeInfo.bigRocks;
			int smallRocks = levelInfo.selectedLevelNodeInfo.smallRocks;
			int allRocks = bigRocks + smallRocks;

			int asteroid = Random.Range(0, allRocks);

			int multiplier = levelInfo.selectedLevelNodeInfo.multiplier;
			float lifeHits = 0;

			float smallLifeHits = 0.75f * multiplier;

			AsteroidColorTypes asteroidColor = GetAsteroidColor (allRocks);

			if (asteroid < bigRocks) {
				levelInfo.selectedLevelNodeInfo.bigRocks--;
				lifeHits = 1.3125f * multiplier;

				SpawnAsteroid(lifeHits, bigAsteroidScale, asteroidColor, smallLifeHits);
			}
			else if (asteroid >= bigRocks && asteroid <= allRocks) {
				levelInfo.selectedLevelNodeInfo.smallRocks--;
				lifeHits = smallLifeHits;

				SpawnAsteroid(lifeHits, smallAsteroidScale, asteroidColor, smallLifeHits);
			}
		}

		yield return null;
	}

	private void SpawnAsteroid (float lifeHits, Vector3 scale, AsteroidColorTypes asteroidColor,
	                            float smallAsteroidsLifeHits)
	{
		GameObject asteroid = (GameObject)Instantiate (asteroidRef, new Vector3 (Random.Range (-1.8f, 2.0f), Random.Range (2.6f, 2.8f), -1.0f), Quaternion.identity);
		currentAsteroids.Add (new Asteroid (asteroidColor, lifeHits, asteroid, smallAsteroidsLifeHits, smallAsteroidScale));

		asteroid.transform.parent = INIT.transform;

		if (scale == Vector3.zero) {
			asteroid.transform.localScale = new Vector3 (0, 0, 1);
			asteroid.transform.Rotate (new Vector3 (0, 0, Random.Range (0, 361)));

			for (int j = 0; j < lifeHits; j++) {
				for (int k = 0; k < 5; k++) {
					asteroid.transform.localScale += new Vector3 (0.1f, 0.1f, 0);
				}
			}

			asteroid.transform.localScale = new Vector3 (0.5f * lifeHits, 0.5f * lifeHits, 1);
		}
		else {
			asteroid.transform.localScale = scale;
		} 
	}

	AsteroidColorTypes GetAsteroidColor (int allRocks)
	{
		AsteroidColorTypes asteroidColor = AsteroidColorTypes.Unknown;

		int totalRocks = levelInfo.selectedLevelNodeInfo.totalRocks;
		float basePercent = 100.0f / totalRocks;
		int percentModifier = Random.Range (1, allRocks);

		float redPercent = levelInfo.selectedLevelNodeInfo.redPercent;
		float bluePercent = redPercent + levelInfo.selectedLevelNodeInfo.bluePercent;
		float greenPercent = bluePercent + levelInfo.selectedLevelNodeInfo.greenPercent;

		float percent = Random.Range (0, greenPercent);

		if (percent < redPercent) {
			asteroidColor = AsteroidColorTypes.Red;
			levelInfo.selectedLevelNodeInfo.redPercent -= basePercent;
		}
		else if (percent >= redPercent && percent < bluePercent) {
			asteroidColor = AsteroidColorTypes.Blue;
			levelInfo.selectedLevelNodeInfo.bluePercent -= basePercent;
		}
		else if (percent >= bluePercent && percent <= greenPercent) {
			asteroidColor = AsteroidColorTypes.Green;
			levelInfo.selectedLevelNodeInfo.greenPercent -= basePercent;
		}
		return asteroidColor;
	}

	public bool CheckIfAnyExists()
	{
		if(currentAsteroids.Count==0)
		{
			return false;
		}

		return true;
	}

	public bool CheckIfAnyCrossesTheLine()
	{
		for(int i=0;i<currentAsteroids.Count;i++)
		{
			Asteroid asteroid = (Asteroid) currentAsteroids[i];

			if(asteroid.obj.transform.position.y<-1.85f)
			{
				return true;
			}
		}

		return false;
	}

	public Vector3 GetPositionToNierestEnemy()
	{
		float lastDist = 1000000.0f;
		Vector3 pos = Vector3.zero;

		for(int i=0;i<currentAsteroids.Count;i++)
		{
			float distance = Vector3.Distance(currentAsteroids[i].obj.transform.position,new Vector3(0,-2.83305f,-1.4f));

			if(distance<lastDist)
			{
				lastDist = distance;
				pos = currentAsteroids[i].obj.transform.position;
			}
		}

		return pos;
	}


	//New Changes ***
	IEnumerator NotifyProgressBarForAsteriodSpawn(int numberOfAsteriodsSpawned)
	{
		yield return new WaitForSeconds (0.15f);
//		StartCoroutine(GameObject.Find("MAIN").GetComponent<LevelManager>().UpdateLevelProgressBarForAsteriodsSpawned(numberOfAsteriodsSpawned));
	}


	//
}
