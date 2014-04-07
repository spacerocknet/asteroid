using UnityEngine;
using System.Collections;

public class BattleEngine : MonoBehaviour {

	private CategorySelect categorySelect;
	private Asteroids asteroids;
	[HideInInspector]
	public bool canTarget;
	private bool isMouseDown;
	private GameObject AtkTarget;
	private AttackSystem AtkSystem;
	private Characters character;

	private void Awake()
	{
		AtkTarget = (GameObject) GameObject.Find("ATTACK_TARGET");
		isMouseDown = false;
		canTarget = false;
		categorySelect = (CategorySelect) this.gameObject.AddComponent<CategorySelect>();
		asteroids = (Asteroids) this.gameObject.AddComponent<Asteroids>();
		AtkSystem = (AttackSystem) this.gameObject.AddComponent<AttackSystem>();
		character = (Characters) this.gameObject.AddComponent<Characters>();
	}

	private IEnumerator Start()
	{	
		yield return StartCoroutine(asteroids.SpawnAsteroids(8,1.0f));

		categorySelect.PlaceCategories(0);

		canTarget = true;

		yield return 0;
	}

	public IEnumerator NextRound(bool isHitTarget)
	{
		Vector3 target = AtkTarget.transform.position;
		categorySelect.targetSelect = false;
		AtkTarget.transform.position = new Vector3(-100,0,0);
		categorySelect.PlaceCategories(0);
		canTarget = true;
		isMouseDown = false;
		
		if(isHitTarget)
		{
			yield return StartCoroutine(AtkSystem.AttackTarget(target,asteroids.currentAsteroids));
		}
		else
		{
			yield return StartCoroutine(AtkSystem.MissTarget());
		}

		yield return StartCoroutine(asteroids.MoveAsteroids());

		yield return new WaitForSeconds(0.1f);

		StartCoroutine(asteroids.SpawnAsteroids(Random.Range(2,6),1.0f));

		yield return 0;
	}

	private void Update()
	{
		if(canTarget)
		{
			if(Input.GetMouseButtonDown(0))
			{
				isMouseDown = true;
			}

			if(Input.GetMouseButtonUp(0))
			{
				isMouseDown = false;
			}

			if(isMouseDown)
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if(hit.collider)
				{
					if(hit.collider.gameObject.name.Equals("BackgroundLayer"))
					{
						categorySelect.targetSelect = true;
						AtkTarget.transform.position = hit.point;
						AtkTarget.transform.position += new Vector3(0,0,-1.9f);

						Vector3 relative = transform.InverseTransformPoint(AtkTarget.transform.position);
						float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
						character.currentChar.transform.localEulerAngles = new Vector3(0,0,angle);
					}
				}
			}
		}
	}
}