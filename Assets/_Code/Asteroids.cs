using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroids : MonoBehaviour {

	public Sprite[] defaultSizeAsteroids;

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

	private ScreenSizeManager screenSizeManager;

	private int totalAsteroids;

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

		public Asteroid(AsteroidColorTypes _colorType, Sprite colorSprite, float _life, GameObject _obj, 
		                float smallLifeHits, Vector3 smallScale)
		{
			isDead = false;
			colorType = _colorType;
			life = _life;
			obj = _obj;

			smallAsteroidLifeHits = smallLifeHits;
			smallAsteroidScale = smallScale;

			UpdateDebugText ();

			this.colorsprite = colorSprite;
			obj.GetComponent<SpriteRenderer>().sprite = this.colorsprite;
		}

		public IEnumerator DoDamage(float hitPoints)
		{
			life -= hitPoints;
			UpdateDebugText ();

			if (life < smallAsteroidLifeHits) {
				obj.transform.localScale *= smallAsteroidScale.x / obj.transform.localScale.x;
			}

			if(life<=0)
			{
				isDead = true;
			}

			yield return null;
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
		screenSizeManager = GameObject.FindObjectOfType<ScreenSizeManager> ();

		totalAsteroids = levelInfo.selectedNodeInfo.totalRocks;
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
		count = Mathf.Clamp (count, count, totalAsteroids);

		for (int index = 0; index < count; index++) {
			int bigRocks = levelInfo.selectedNodeInfo.bigRocks;
			int smallRocks = levelInfo.selectedNodeInfo.smallRocks;
			int allRocks = bigRocks + smallRocks;

			int asteroid = Random.Range(1, allRocks);

			int multiplier = levelInfo.selectedNodeInfo.multiplier;
			float lifeHits = 0;

			float smallLifeHits = 0.75f * multiplier;

			AsteroidColorTypes forcedColorType = AsteroidColorTypes.Unknown;
			if (levelInfo.selectedNodeInfo.level == 16 && totalAsteroids >= levelInfo.selectedNodeInfo.totalRocks) {
				forcedColorType = AsteroidColorTypes.Red;
				asteroid = 0;
			}

			AsteroidColorTypes asteroidColor = GetAsteroidColor (allRocks, forcedColorType);

			if (asteroid <= bigRocks) {
				levelInfo.selectedNodeInfo.bigRocks--;
				lifeHits = 1.3125f * multiplier;

				SpawnAsteroid(lifeHits, bigAsteroidScale, asteroidColor, smallLifeHits);
			}
			else if (asteroid > bigRocks && asteroid <= allRocks) {
				levelInfo.selectedNodeInfo.smallRocks--;
				lifeHits = smallLifeHits;

				SpawnAsteroid(lifeHits, smallAsteroidScale, asteroidColor, smallLifeHits);
			}

			totalAsteroids--;
		}

		yield return null;
	}

	private void SpawnAsteroid (float lifeHits, Vector3 scale, AsteroidColorTypes asteroidColor,
	                            float smallAsteroidsLifeHits)
	{
		Sprite colorSprite = defaultSizeAsteroids [(int)asteroidColor];

		float spawnX = Random.Range (-2.0f, 2.0f);
		float spawnY = Random.Range (3.5f, 4f);
		Vector3 spawnPosition = new Vector3 (spawnX, spawnY, -1);

		GameObject asteroid = (GameObject)Instantiate (asteroidRef, spawnPosition, Quaternion.identity);
		Asteroid ast = new Asteroid (asteroidColor, colorSprite, lifeHits, asteroid, smallAsteroidsLifeHits, 
		                             smallAsteroidScale);
		//ast.obj.transform.localScale *= scale.x;
		screenSizeManager.UpdateSpriteRenderer(ast.obj.GetComponent<SpriteRenderer> ());
		CircleCollider2D circleCollider = ast.obj.AddComponent<CircleCollider2D> ();
		circleCollider.radius = 0.7f;
		currentAsteroids.Add (ast);

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

	AsteroidColorTypes GetAsteroidColor (int allRocks, AsteroidColorTypes colorType)
	{
		AsteroidColorTypes asteroidColor = AsteroidColorTypes.Unknown;

		int totalRocks = levelInfo.selectedNodeInfo.totalRocks;
		float basePercent = 100.0f / totalRocks;
		int percentModifier = Random.Range (1, allRocks);

		float redPercent = levelInfo.selectedNodeInfo.redPercent;
		float bluePercent = redPercent + levelInfo.selectedNodeInfo.bluePercent;
		float greenPercent = bluePercent + levelInfo.selectedNodeInfo.greenPercent;

		if (colorType != AsteroidColorTypes.Unknown) {
			asteroidColor = colorType;

			if (asteroidColor == AsteroidColorTypes.Red) {
				levelInfo.selectedNodeInfo.redPercent -= basePercent;
			}
			else if (asteroidColor == AsteroidColorTypes.Green) {
				levelInfo.selectedNodeInfo.bluePercent -= basePercent;
			}
			else {
				levelInfo.selectedNodeInfo.greenPercent -= basePercent;
			}

			return asteroidColor;
		}

		float percent = Random.Range (0, greenPercent);

		if (percent < redPercent) {
			asteroidColor = AsteroidColorTypes.Red;
			levelInfo.selectedNodeInfo.redPercent -= basePercent;
		}
		else if (percent >= redPercent && percent < bluePercent) {
			asteroidColor = AsteroidColorTypes.Blue;
			levelInfo.selectedNodeInfo.bluePercent -= basePercent;
		}
		else if (percent >= bluePercent && percent <= greenPercent) {
			asteroidColor = AsteroidColorTypes.Green;
			levelInfo.selectedNodeInfo.greenPercent -= basePercent;
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

			if(asteroid.obj.transform.position.y < -2.5f)
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
