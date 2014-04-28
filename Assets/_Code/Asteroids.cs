using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroids : MonoBehaviour {

	private GameObject asteroidRef;
	[HideInInspector]
	public List<Asteroid> currentAsteroids = new List<Asteroid>();
	private GameObject INIT;
	private static int ASTEROID_LIMIT = 8;
	private List<float> lifeList = new List<float>();

	public class Asteroid
	{
		public AsteroidColorTypes colorType;
		public int life;
		public Color color;
		public GameObject obj;
		public bool isDead;

		public Asteroid(AsteroidColorTypes _colorType, int _life, GameObject _obj)
		{
			isDead = false;
			colorType = _colorType;
			life = _life;
			obj = _obj;

			DefineColor();
		}

		private void DefineColor()
		{
			switch(colorType)
			{
				case AsteroidColorTypes.Red:
					this.color = Color.red;
					break;
				case AsteroidColorTypes.Green:
					this.color = Color.green;
					break;
				case AsteroidColorTypes.Blue:
					this.color = Color.cyan;
					break;
				default: break;
			}

			obj.GetComponent<SpriteRenderer>().color = this.color;
		}

		public IEnumerator DoDamage(int hitPoints)
		{
			if(life<hitPoints)
			{
				hitPoints = life;
			}

			life -= hitPoints;

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
	}

	public enum AsteroidColorTypes
	{
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

	public IEnumerator SpawnAsteroids(int count, float diff)
	{
		if(currentAsteroids.Count+count>ASTEROID_LIMIT)
		{
			while(currentAsteroids.Count+count>ASTEROID_LIMIT)
			{
				--count;
			}
		}

		for(int i=0;i<count;i++)
		{
			GameObject asteroid = (GameObject) Instantiate(asteroidRef,new Vector3(Random.Range(-1.8f,2.0f),Random.Range(3.4f,3.6f),-1.0f),Quaternion.identity);

			int lifeHits = Random.Range(1,4);

			currentAsteroids.Add(new Asteroid((AsteroidColorTypes)Random.Range(0,3),lifeHits,asteroid));

			asteroid.transform.localScale = new Vector3(0,0,1);
			asteroid.transform.Rotate(new Vector3(0,0,Random.Range(0,361)));

			for(int j=0;j<lifeHits;j++)
			{
				for(int k=0;k<5;k++)
				{
					asteroid.transform.localScale += new Vector3(0.1f,0.1f,0);
					yield return 0;
				}
			}
			
			asteroid.transform.localScale = new Vector3(0.5f*lifeHits,0.5f*lifeHits,1);
			asteroid.transform.parent = INIT.transform;

			yield return 0;
		}
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

			if(asteroid.obj.transform.position.y<-2.0f)
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
}
