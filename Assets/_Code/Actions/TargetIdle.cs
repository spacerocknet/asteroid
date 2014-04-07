using UnityEngine;
using System.Collections;

public class TargetIdle : MonoBehaviour {

	private float sizeIdle;
	private float rotSpeed;

	private IEnumerator Start()
	{
		sizeIdle = 0.005f;
		rotSpeed = 0.5f;

		for(;;)
		{
			for(int i=0;i<10;i++)
			{
				this.transform.localScale -= new Vector3(sizeIdle,sizeIdle,0);
				this.transform.Rotate(new Vector3(0,0,rotSpeed));
				yield return 0;
			}

			for(int i=0;i<10;i++)
			{
				this.transform.localScale += new Vector3(sizeIdle,sizeIdle,0);
				this.transform.Rotate(new Vector3(0,0,rotSpeed));
				yield return 0;
			}
		}
	}
}
