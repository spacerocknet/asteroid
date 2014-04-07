using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CategorySelect : MonoBehaviour {

	private GameObject catRef;
	private const int catCount = 3;
	private GameObject INIT;
	[HideInInspector]
	public List<Category> currentCategories = new List<Category>();
	private bool canSelect;
	[HideInInspector]
	public bool targetSelect;
	private Questions QE;

	public enum CategoryTypes
	{
		History = 0,
		Math = 1,
		Food = 2,
		Pets = 3,
		Sports = 4,
		Movies = 5,
		Music = 6,
		People = 7,
		General = 8,
		Geography = 9,
		Art = 10,
		Science = 11,
		Religion = 12,
		Computers = 13,
	}

	public enum ColorTypes
	{
		Red = 0,
		Yellow = 1,
		Green = 2,
		Blue = 3
	}

	public class Category
	{
		public int id;
		public CategoryTypes categoryType;
		public ColorTypes colorType;
		public Color color;
		public GameObject obj;

		public Category(int _id, CategoryTypes _categoryType, ColorTypes _colorType, GameObject _obj)
		{
			id = _id;
			categoryType = _categoryType;
			colorType = _colorType;
			obj = _obj;

			DefineColor();
			DefineLabel();
			DefineSize();
			DefinePosition();
		}

		private void DefineColor()
		{
			switch(colorType)
			{
				case ColorTypes.Red:
					this.color = Color.red;
					break;
				case ColorTypes.Yellow:
					this.color = Color.yellow;
					break;
				case ColorTypes.Green:
					this.color = Color.green;
					break;
				case ColorTypes.Blue:
					this.color = Color.cyan;
					break;
				default: break;
			}

			obj.GetComponent<SpriteRenderer>().color = this.color;
		}

		private void DefineLabel()
		{
			TextMesh textM = (TextMesh) obj.GetComponentInChildren<TextMesh>();
			textM.text = categoryType.ToString();

			if(textM.text.Length>6)
			{
				int len = textM.text.Length-6;

				for(int i=0;i<len;i++)
				{
					textM.characterSize -= 0.05f;
				}
			}
		}

		private void DefineSize()
		{
			obj.transform.localScale = new Vector3((Screen.width/512.0f) * (1.0f/3.0f),0.24f,1);
		}

		private void DefinePosition()
		{
			obj.transform.localPosition = new Vector3(98.40041f+(id*1.6f),-4.691885f,-1f);
		}
	}

	private Category GetCategoryByIndex(int index)
	{
		for(int i=0;i<currentCategories.Count;i++)
		{
			Category cat = (Category) currentCategories[i];

			if(cat.id.Equals(index))
			{
				return cat;
			}
		}

		Debug.LogError("[GetCategoryByIndex] Can't Happen.");

		return null;
	}

	private void Awake()
	{
		canSelect = false;
		targetSelect = false;
		catRef = (GameObject) GameObject.Find("REFERENCES/category_bg");
		INIT = (GameObject) GameObject.Find("RUNTIME_INIT");
		QE = (Questions) GameObject.Find("MAIN").AddComponent<Questions>();
	}

	public void PlaceCategories(int diff)
	{
		if(currentCategories.Count>0)
		{
			currentCategories.Clear();
		}

		for(int i=0;i<catCount;i++)
		{
			GameObject newCatObj = (GameObject) Instantiate(catRef,new Vector3(-10,0,-1),Quaternion.identity);
			newCatObj.name = i.ToString();
			newCatObj.transform.parent = INIT.transform;

			currentCategories.Add(new Category(i,(CategoryTypes)Random.Range(0,14),(ColorTypes)i,newCatObj));

			StartCoroutine(PlaceCategoryToPlace(i,newCatObj));
		}
	}

	private IEnumerator PlaceCategoryToPlace(int index, GameObject go)
	{
		yield return new WaitForSeconds((float)index * 0.1f);

		for(int i=0;i<20;i++)
		{
			go.transform.localPosition += new Vector3(0,0.05f,0);
			yield return 0;
		}

		if(index==2)
		{
			canSelect = true;
		}

		yield return 0;
	}

	private IEnumerator HideCategories()
	{
		for(int i=0;i<currentCategories.Count;i++)
		{
			Category cat = (Category) currentCategories[i];

			for(int j=0;j<5;j++)
			{
				cat.obj.transform.position -= new Vector3(0,0.12f,0);
				yield return 0;
			}
		}

		yield return 0;
	}

	private void Update()
	{
		if(canSelect&&targetSelect)
		{
			if(Input.GetMouseButtonDown(0))
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if(hit.collider)
				{
					if(hit.collider.gameObject.tag.Equals("Category"))
					{
					    int index = (int) System.Convert.ToInt32(hit.collider.gameObject.name);
					    Category cat = (Category) GetCategoryByIndex(index);
					    
					    Debug.Log("Selected: ("+cat.id+") "+cat.categoryType+", "+cat.colorType+", "+cat.obj);
						canSelect = false;
						StartCoroutine(HideCategories());
						QE.ShowAnswersByCategory(cat.categoryType);
					}
				}
			}
		}
	}
}
