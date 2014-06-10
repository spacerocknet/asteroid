using UnityEngine;
using System.Collections;

public class mainmenu : MonoBehaviour {

		
	private Vector2 touchposition;
	private RaycastHit2D hit;

	public enum state{settings,store,facebook,mainmenu};
	public state gamestate;
	public GameObject coinstore;
	public GameObject settings;
	public GameObject fadebg;

	public Sprite button_off;
	public Sprite button_on;

	private int totalgold;

	void Start()
	{
		gamestate=state.mainmenu;
		totalgold=PlayerPrefs.GetInt("totalgold");
	}

	void Update()
			{
			if(Input.touchCount>0)
				{
				foreach(Touch touch in Input.touches)
					{
					if(touch.phase==TouchPhase.Began)
						{	
						touchposition=touch.position;
						touchended();
						}
					}
				}
			
			//For testing with the mouse
			if(Input.GetMouseButtonUp(0))
			    {
				touchposition=Input.mousePosition;
				touchended();
				}

			if(Input.GetKey(KeyCode.Escape))
				{
				Application.Quit();
				}

			}


	void touchended()
			{
			hit=Physics2D.Raycast(camera.ScreenToWorldPoint(new Vector3(touchposition.x,touchposition.y,0)),Vector2.zero);
				if(hit.collider!=null)
					{
					if(gamestate==state.mainmenu)
					{
						if(hit.collider.gameObject.name=="button_settings")
									{
							fadebg.renderer.enabled=true;
							StartCoroutine("showsettings");
									}
						else if(hit.collider.gameObject.name=="button_store")
									{
							fadebg.renderer.enabled=true;
							StartCoroutine("showcoinstore");
									}
						else if(hit.collider.gameObject.name=="button_facebook")
									{
							Debug.Log("works");
									}
						else if(hit.collider.gameObject.name=="Location_01_Level1")
									{
										Debug.Log("Location 1 works");
										Application.LoadLevel(1);
									}
						else if(hit.collider.gameObject.name=="Location_01_Level2")
									{
										Debug.Log("Location 2 works");
									}
						else if(hit.collider.gameObject.name=="Location_01_Level3")
									{
										Debug.Log("Location 3 works");
									}
						else if(hit.collider.gameObject.name=="Location_01_Level4")
									{
									Debug.Log("Location 4 works");
									}
					}
					else if(gamestate==state.store)
					{
						if(hit.collider.gameObject.name=="button_buy_pileofgold")
							{
							Debug.Log("works");
							totalgold +=100;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_buy_boxofgold")
							{
							Debug.Log("works");
							totalgold +=800;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_buy_chestofgold")
							{
							Debug.Log("works");
							totalgold +=1000;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_buy_bagofgold")
							{
							Debug.Log("works");
							totalgold +=250;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_buy_sackofgold")
							{
							Debug.Log("works");
							totalgold +=500;
							PlayerPrefs.SetInt("totalgold",totalgold);
							}
						else if(hit.collider.gameObject.name=="button_hidestore")
							{
							StartCoroutine("hidecoinstore");
							fadebg.renderer.enabled=false;
							}
					}
			
					else if(gamestate==state.settings)
					{
						if(hit.collider.gameObject.name=="button_hidesettings")
						{
							StartCoroutine("hidesettings");
							fadebg.renderer.enabled=false;
						}
						else if(hit.collider.gameObject.name=="buttononstatefacebook")
						{
							if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_off")
							{
							hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;
							}
							else if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_on")
					        {
							hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
							}
						}
						else if(hit.collider.gameObject.name=="buttononstategoogle")
						{
							Debug.Log("Working");
							if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_off")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;
							}
							else if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_on")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
							}
						}
						else if(hit.collider.gameObject.name=="button_help_feedback")
						{
							Debug.Log("Works");
						}
						else if(hit.collider.gameObject.name=="buttonstatemusic")
						{
							if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_off")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_on;
							}
							else if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name=="button_on")
							{
								hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite=button_off;
							}
						}
						else if(hit.collider.gameObject.name=="button_about")
						{
							Debug.Log("Works");
						}
						else if(hit.collider.gameObject.name=="button_service")
						{
							Debug.Log("Works");
						}
					}
				}
			}


			IEnumerator showcoinstore()
			{
				for(int i=0;i<20;i++)
				{
				Vector3 oldposition=coinstore.transform.position;
				float newposition=Mathf.Lerp(oldposition.x,-0.538f,0.25f);
				coinstore.transform.position=new Vector3(newposition,coinstore.transform.position.y,0f);
				yield return null;
				}
				yield return new WaitForEndOfFrame();
				gamestate=state.store;
			}

			IEnumerator hidecoinstore()
			{
				for(int i=0;i<20;i++)
				{
				Vector3 oldposition=coinstore.transform.position;
				float newposition=Mathf.Lerp(oldposition.x,-7.55f,0.25f);
				coinstore.transform.position=new Vector3(newposition,coinstore.transform.position.y,0f);
				yield return null;
				}
			yield return new WaitForEndOfFrame();
			gamestate=state.mainmenu;
			}
		
			IEnumerator showsettings()
			{
				for(int i=0;i<20;i++)
				{
				Vector3 oldposition=settings.transform.position;
				float newposition=Mathf.Lerp(oldposition.x,-0.04f,0.25f);
				settings.transform.position=new Vector3(newposition,settings.transform.position.y,0f);
				yield return null;
				}
			yield return new WaitForEndOfFrame();
			gamestate=state.settings;
			}
		
			IEnumerator hidesettings()
			{
				for(int i=0;i<20;i++)
				{
					Vector3 oldposition=settings.transform.position;
					float newposition=Mathf.Lerp(oldposition.x,7.231156f,0.25f);
					settings.transform.position=new Vector3(newposition,settings.transform.position.y,0f);
					yield return null;
				}
				yield return new WaitForEndOfFrame();
				gamestate=state.mainmenu;
			}

		}


