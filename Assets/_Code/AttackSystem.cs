using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSystem : MonoBehaviour {

	private Weapons weapons;
	private int currentWeapon;
	private GameObject label_ref;

	private void Awake()
	{
		weapons = (Weapons) this.gameObject.AddComponent<Weapons>();
		label_ref = (GameObject) GameObject.Find("REFERENCES/label_ref");
		currentWeapon = 0;
	}

	public IEnumerator AttackTarget(Vector3 target, List<Asteroids.Asteroid> currentAsteroids, LevelManager levelMRef)
	{
		Debug.Log("Attack!");

		List<int> destroyIndex = new List<int>();

		GameObject expT = (GameObject) Instantiate(weapons.AllWeapons[currentWeapon].obj,target,Quaternion.identity);
		Destroy(expT,2);

		for(int i=0;i<currentAsteroids.Count;i++)
		{
			Vector3 p1 = target;
			Vector3 p2 = new Vector3(currentAsteroids[i].obj.transform.position.x,currentAsteroids[i].obj.transform.position.y,p1.z);
			float dist = Vector3.Distance(p1,p2);

			if(dist<0.7f)
			{
				StartCoroutine(levelMRef.UpdateLevelProgressBar());
				//Instantiate(GameObject.Find("CUBETEST"),p1,Quaternion.identity);	
				
				GameObject exp = (GameObject) Instantiate(weapons.AllWeapons[currentWeapon].obj,p2,Quaternion.identity);
				Destroy(exp,2);
				destroyIndex.Add(i);
			}
		}
		
		for(int i=destroyIndex.Count-1;i>=0;i--)
		{
			Destroy(currentAsteroids[destroyIndex[i]].obj);
			currentAsteroids.RemoveAt(destroyIndex[i]);
		}

		yield return 0;
	}

	public IEnumerator MissTarget()
	{
		GameObject missInit = (GameObject) Instantiate(label_ref,new Vector3(0f,0f,-1.45f),label_ref.transform.rotation);
		TextMesh tm = (TextMesh) missInit.GetComponent<TextMesh>();
		tm.text = "Miss!";
		missInit.transform.localScale = new Vector3(0,0,1);

		yield return new WaitForSeconds(0.2f);

		for(int i=0;i<12;i++)
		{
			missInit.transform.localScale += new Vector3(0.1f,0.1f,0);

			yield return 0;
		}

		Color cl = tm.color;

		for(int i=0;i<20;i++)
		{
			missInit.transform.position += new Vector3(0,0.04f,0);

			if(i>10)
			{
				cl.a -= 0.1f;
				tm.color = cl;
			}

			yield return 0;
		}

		Destroy(missInit);
		Debug.Log("Miss!");

		yield return 0;
	}

	public void SelectWeapon(int id)
	{
		currentWeapon = id;
	}
}
