using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CategorySelect : MonoBehaviour {

	private GameObject catRef;
	private int catCount = 4;
	private GameObject INIT;
	[HideInInspector]
	public List<Category> currentCategories = new List<Category>();
	private bool canSelect;
	[HideInInspector]
	public bool targetSelect;
	[HideInInspector]
	public Questions QE;
	private int [] catBitmap = new int[4]  { 14,  13, 11, 7}; //different num categories have different bitmap list. It is 4 now
	public bool animationIsPlaying;
	//private GameObject battleengineobject;
	private GameObject soundmanager;

	public Sprite[] defaultCategorySprites;

	private ScreenSizeManager screenSizeManager;
	private LevelInfo levelInfo;
	private Categories categories;

	public enum CategoryTypes
	{
		Sport = 0,
		Movie = 1,
		Music = 2,
		Geography = 3
	}

	public enum ColorTypes
	{
		Unknown = -1,
		Green = 0,
		Red = 1,
		Blue = 2
	}

	public class Category
	{
		public int id;
		public CategoryTypes categoryType;
		public ColorTypes colorType;
		public Color color;
		public Sprite colorsprite;
		public GameObject obj;

		public Category(int _id, CategoryTypes _categoryType, Sprite categorySprite, float scaleX, float scaleY, ColorTypes _colorType, GameObject _obj)
		{
			id = _id;
			categoryType = _categoryType;
			colorType = _colorType;
			obj = _obj;

			this.colorsprite = categorySprite;
			obj.GetComponent<SpriteRenderer>().sprite=this.colorsprite;

			DefineLabel(scaleX, scaleY);
			//DefineSize();
			DefinePosition();
		}

		private void DefineLabel(float scaleX, float scaleY)
		{
			TextMesh textM = (TextMesh)obj.transform.FindChild ("label").GetComponent<TextMesh> ();
			textM.font=BattleEngine.font1;
			textM.renderer.material=BattleEngine.material1[0];
			textM.text = categoryType.ToString();
			textM.fontSize = 15;
			textM.characterSize= 0.2f;
			textM.fontStyle=FontStyle.Normal;
			textM.color=Color.white;
			textM.transform.localScale = new Vector3 (1 / scaleX, 1 / scaleY, 1);
			textM.gameObject.transform.position=new Vector3(textM.transform.position.x,0,textM.transform.position.z);

			Vector3 localPostion = textM.transform.localPosition;
        	localPostion.y = 0;

			textM.gameObject.transform.localPosition = localPostion;

			if(textM.text.Length>6)
			{
				int len = textM.text.Length-6;

				for(int i=0;i<len;i++)
				{
					//textM.characterSize -= 0.05f;
				}
			}

		}

		private void DefineSize()
		{
			obj.transform.localScale = new Vector3((Screen.width/512.0f) * (1.0f/3.0f),0.24f,1);
		}

		private void DefinePosition()
		{
			float offsetX = obj.renderer.bounds.extents.x * 2;
			obj.transform.localPosition = new Vector3(98f + (id * (offsetX + 0.03f)), -4.5f, -1f);
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
		animationIsPlaying = false;
		catCount = System.Enum.GetNames(typeof(CategoryTypes)).Length;
		canSelect = false;
		targetSelect = false;
		catRef = (GameObject) GameObject.Find("REFERENCES/category_bg");
		INIT = (GameObject) GameObject.Find("RUNTIME_INIT");
		QE = (Questions) GameObject.Find("MAIN").AddComponent<Questions>();
		//battleengineobject=GameObject.Find("MAIN");
		soundmanager=GameObject.Find("Secondary_SoundManager");

		screenSizeManager = GameObject.FindObjectOfType<ScreenSizeManager> ();
		categories = GameObject.FindObjectOfType<Categories> ();

		categories.transform.localPosition = new Vector3(0,0,-1);

		levelInfo = GameObject.FindObjectOfType<LevelInfo> ();
	}

	public void PlaceCategories(int diff, bool start)
	{
		foreach (Category category in currentCategories) {
			GameObject.Destroy(category.obj);
		}

		currentCategories.Clear ();
	
		int bitmap = catBitmap [Random.Range(0, catCount)];
		int index = 0;
		for (int i=0; i<catCount; i++) {
			bool flag = (0x1 & bitmap) == 1;
			bitmap = bitmap >> 1;
	
			if (flag) {
				Sprite sprite = null;
				ColorTypes colorType = ColorTypes.Unknown;

				if (levelInfo.selectedNodeInfo.level == 16 && start) {
					sprite = defaultCategorySprites[2];
					colorType = ColorTypes.Blue;
				}
				else {
					sprite = defaultCategorySprites[index];
					colorType = (ColorTypes) index;
				}

				GameObject newCatObj = (GameObject) Instantiate(catRef);
				newCatObj.name = index.ToString();
				newCatObj.transform.parent = categories.transform;

				currentCategories.Add(new Category(index,(CategoryTypes)i, sprite, screenSizeManager.scaleX, 
				                                   screenSizeManager.scaleY, colorType, newCatObj));
				screenSizeManager.UpdateSpriteRenderer(newCatObj.GetComponent<SpriteRenderer>());

				canSelect = true;
				//StartCoroutine(PlaceCategoryToPlace(index,newCatObj));
				index++;
			}
		}
	}

	//The Code for the powerup change color category.. This code changes colortype and color of all the categories

	public void PlaceCategoriesByCategoryChangePowerup(string color)
	{
		if(color=="red")
		{
			for(int i=0;i<currentCategories.Count;i++)
			{

				Category tempcategory=currentCategories[i];
				tempcategory.colorType=ColorTypes.Red;
				tempcategory.colorsprite = defaultCategorySprites[1];
				tempcategory.obj.GetComponent<SpriteRenderer>().sprite = screenSizeManager.GetSpriteSize(defaultCategorySprites[1]);;

				currentCategories[i]=tempcategory;
			}
		}
		else if(color=="blue")
		{
			for(int i=0;i<currentCategories.Count;i++)
			{
				Category tempcategory=currentCategories[i];
				tempcategory.colorType=ColorTypes.Blue;
				tempcategory.colorsprite = defaultCategorySprites[2];
				tempcategory.obj.GetComponent<SpriteRenderer>().sprite = screenSizeManager.GetSpriteSize(defaultCategorySprites[2]);
				currentCategories[i]=tempcategory;
			}
		}
		else if(color=="green")
		{
			for(int i=0;i<currentCategories.Count;i++)
			{
				Category tempcategory=currentCategories[i];
				tempcategory.colorType=ColorTypes.Green;
				tempcategory.colorsprite = defaultCategorySprites[0];
				tempcategory.obj.GetComponent<SpriteRenderer>().sprite = screenSizeManager.GetSpriteSize(defaultCategorySprites[0]);
				currentCategories[i]=tempcategory;
			}
		}
	}


	//Below is code to disable the question togglebutton while the powerup deactivated
	public void disablequestioncollidersandtriggers()
	{
		for(int i=0;i<currentCategories.Count;i++)
		{
			Category tempcategory=currentCategories[i];
			tempcategory.obj.GetComponent<BoxCollider2D>().enabled=false;
			currentCategories[i]=tempcategory;
		}
	}

	//Below is code to enable the question button colliders and triggers while the powerup is activated
	public void enablequestioncollidersandtriggers()
	{
		for(int i=0;i<currentCategories.Count;i++)
		{
			Category tempcategory=currentCategories[i];
			tempcategory.obj.GetComponent<BoxCollider2D>().enabled=true;
			currentCategories[i]=tempcategory;
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

		canSelect = true;
		
		yield return 0;
	}

	public IEnumerator HideCategories()
	{
		for(int i=0;i<currentCategories.Count;i++)
		{
			Category cat = (Category) currentCategories[i];
			for(int j=0;j<5;j++)
			{
				if (cat != null) {
					cat.obj.transform.position -= new Vector3(0,0.12f,0);
				}

				yield return 0;
			}
		}

		yield return 0;
	}

	private void Update()
	{
		if(canSelect&&!animationIsPlaying)
		{
			if(Input.GetMouseButtonDown(0))
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if(hit.collider)
				{
					if(hit.collider.gameObject.tag.Equals("Category"))
					{
						soundmanager.GetComponent<SoundManager>().selectcategory_soundplay();

						if(!targetSelect)
						{
							QE.AttackNierest();
						}

						int index = (int) System.Convert.ToInt32(hit.collider.gameObject.name);

					    Category cat = (Category) GetCategoryByIndex(index);
					    
						canSelect = false;
						//StartCoroutine(HideCategories());
						QE.ShowAnswersByCategory(cat.categoryType,cat.colorType);

					}
				}
			}
		}
	}
}
