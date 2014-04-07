using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroids : MonoBehaviour {

	private GameObject asteroidRef;
	[HideInInspector]
	public List<Asteroid> currentAsteroids = new List<Asteroid>();
	private GameObject INIT;

	public class Asteroid
	{
		public AsteroidColorTypes colorType;
		public float life;
		public Color color;
		public GameObject obj;

		public Asteroid(AsteroidColorTypes _colorType, float _life, GameObject _obj)
		{
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
				case AsteroidColorTypes.Yellow:
					this.color = Color.yellow;
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
	}

	public enum AsteroidColorTypes
	{
		Red = 0,
		Yellow = 1,
		Green = 2,
		Blue = 3
	}

	private void Awake()
	{
		asteroidRef = (GameObject) GameObject.Find("REFERENCES/Asteroid");
		INIT = (GameObject) GameObject.Find("RUNTIME_INIT");
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
		for(int i=0;i<count;i++)
		{
			GameObject asteroid = (GameObject) Instantiate(asteroidRef,new Vector3(Random.Range(-1.8f,2.0f),Random.Range(3.4f,3.6f),-1.0f),Quaternion.identity);

			currentAsteroids.Add(new Asteroid((AsteroidColorTypes)Random.Range(0,4),1.0f*diff,asteroid));

			asteroid.transform.localScale = new Vector3(0,0,0);
			asteroid.transform.Rotate(new Vector3(0,0,Random.Range(0,361)));

			for(int j=0;j<10;j++)
			{
				asteroid.transform.localScale += new Vector3(0.1f,0.1f,0.1f);
				yield return 0;
			}

			
			asteroid.transform.parent = INIT.transform;

			yield return 0;
		}
	}
}
