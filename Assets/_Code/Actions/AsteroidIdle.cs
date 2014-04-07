using UnityEngine;
using System.Collections;

public class AsteroidIdle : MonoBehaviour {

	private float sizeIdle;

	private IEnumerator Start()
	{
		sizeIdle = 0.005f;

		for(;;)
		{
			for(int i=0;i<10;i++)
			{
				this.transform.localScale -= new Vector3(sizeIdle,sizeIdle,0);
				yield return 0;
			}

			for(int i=0;i<10;i++)
			{
				this.transform.localScale += new Vector3(sizeIdle,sizeIdle,0);
				yield return 0;
			}
		}
	}
}
