using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Questions : MonoBehaviour {

	public class Question
	{
		public int id;
		public CategorySelect.CategoryTypes category;
		public string title;
		public List<string> answers = new List<string>();
		public int correctAnswerIndex;

		public Question(int _id, CategorySelect.CategoryTypes _category, string _title, List<string> _answers, int _correctAnswerIndex)
		{
			id = _id;
			category = _category;
			title = _title;
			answers = _answers;
			correctAnswerIndex = _correctAnswerIndex;
		}

		public bool CheckAnswer(int index)
		{
			if(index.Equals(correctAnswerIndex))
			{
				return true;
			}

			return false;
		}
	}

	private List<Question> AllQuestions = new List<Question>();
	private GameObject answerObjRef;
	private GameObject questionBoxRef;
	private Question currentQuestion;
	private GameObject fadeBG;
	private bool canAnswer;
	private List<GameObject> currentAnswers = new List<GameObject>();
	private BattleEngine BATTLE_ENGINE;

	private void Awake()
	{
		canAnswer = false;
		fadeBG = (GameObject) GameObject.Find("GFX/FadeBG");
		answerObjRef = (GameObject) GameObject.Find("REFERENCES/answer_bg");
		questionBoxRef = (GameObject) GameObject.Find("REFERENCES/question_box");
		BATTLE_ENGINE = (BattleEngine) GameObject.Find("MAIN").GetComponent<BattleEngine>();

		// temporary for testing

		AllQuestions.Add(new Question(0,CategorySelect.CategoryTypes.History,"When WW2 started?",GenerateAnswerList("1221","1450","1939","2008"),3));
		AllQuestions.Add(new Question(1,CategorySelect.CategoryTypes.Math,"2 + 2 = ?",GenerateAnswerList("4","5","200","-1"),1));
		AllQuestions.Add(new Question(2,CategorySelect.CategoryTypes.Food,"What is orange color?",GenerateAnswerList("Not Orange","Orange","Blue","I'm color blind"),2));
		AllQuestions.Add(new Question(3,CategorySelect.CategoryTypes.Pets,"How you spell CAT?",GenerateAnswerList("TAC","ACT","KAC","CAT"),4));
		AllQuestions.Add(new Question(4,CategorySelect.CategoryTypes.Sports,"Do sports use balls?",GenerateAnswerList("Yes","No","Which?","Banana"),1));
		AllQuestions.Add(new Question(5,CategorySelect.CategoryTypes.Movies,"Movies cool!",GenerateAnswerList("Hmm","YES!","Nope","Banana"),2));
		AllQuestions.Add(new Question(6,CategorySelect.CategoryTypes.Music,"Bum psh tkskskk trrr",GenerateAnswerList("Opra","Dubstep","Justing Blyber","Watermelon"),2));
		AllQuestions.Add(new Question(7,CategorySelect.CategoryTypes.People,"Abraham Lincoln was?",GenerateAnswerList("iPhone creator","a Priest","US President","Jesus"),3));
		AllQuestions.Add(new Question(8,CategorySelect.CategoryTypes.General,"Are people happy?",GenerateAnswerList("What?","Who?","Where?","Orange"),1));
		AllQuestions.Add(new Question(9,CategorySelect.CategoryTypes.Geography,"Where is Lithuania?",GenerateAnswerList("In US","In EU","In Asia","In Australia"),2));
		AllQuestions.Add(new Question(10,CategorySelect.CategoryTypes.Art,"I have nothing.",GenerateAnswerList("Wrong answer","Wrong answer","Wrong answers","Correct answer"),4));
		AllQuestions.Add(new Question(11,CategorySelect.CategoryTypes.Science,"What is H2O?",GenerateAnswerList("a Banana","a Batman","Water","a CAT"),3));
		AllQuestions.Add(new Question(12,CategorySelect.CategoryTypes.Religion,"Where is GOD?",GenerateAnswerList("In the sky","In hell","In bathroom","I'm an atheist"),4));
		AllQuestions.Add(new Question(13,CategorySelect.CategoryTypes.Computers,"Which is the best?",GenerateAnswerList("OSX","Windows","Linux","Batman"),4));
		AllQuestions.Add(new Question(14,CategorySelect.CategoryTypes.Cars,"Which is a car?",GenerateAnswerList("Boeing 737","Titanic","Toyota","Samsung"),3));
	}

	//temp
	private List<string> GenerateAnswerList(string s1, string s2, string s3, string s4)
	{
		List<string> ls = new List<string>();
		ls.Add(s1);
		ls.Add(s2);
		ls.Add(s3);
		ls.Add(s4);

		return ls;
	}

	public void AttackNierest()
	{
		BATTLE_ENGINE.SetAttackToNierestEnemy();
	}

	private Question GetQuestionByCategory(CategorySelect.CategoryTypes cat)
	{
		for(int i=0;i<AllQuestions.Count;i++)
		{
			Question question = (Question) AllQuestions[i];

			if(question.category.Equals(cat))
			{
				return question;
			}
		}

		Debug.LogError("[GetQuestionByCategory] Can't Happen.");

		return AllQuestions[0]; //returning first question if not found
	}

	public IEnumerator ShowFadeBG(bool isHide=false, bool popupCloser=false)
	{
		if(popupCloser)
		{
			fadeBG.transform.position = new Vector3(fadeBG.transform.position.x,fadeBG.transform.position.y,-3.5f);
		}

		SpriteRenderer sr = (SpriteRenderer) fadeBG.GetComponent<SpriteRenderer>();
		Color cl = sr.color;

		if(!isHide)
		{
			fadeBG.GetComponent<BoxCollider2D>().enabled = true;
			cl.a = 0;
		}
		else
		{
			cl.a = 1;
		}

		sr.color = cl;

		for(int i=0;i<10;i++)
		{
			if(!isHide)
			{
				cl.a += 0.1f;
			}
			else
			{
				cl.a -= 0.1f;
			}

			sr.color = cl;

			yield return 0;
		}

		if(!isHide)
		{
			cl.a = 1.0f;
		}
		else
		{
			fadeBG.GetComponent<BoxCollider2D>().enabled = false;
			cl.a = .0f;
		}

		sr.color = cl;

		yield return 0;
	}

	public IEnumerator ShowQuestionBox(bool isHide=false)
	{
		if(!isHide)
		{
			TextMesh tm = (TextMesh) questionBoxRef.GetComponentInChildren<TextMesh>();
			tm.text = currentQuestion.title;
		
			questionBoxRef.transform.position = new Vector3(0.01865721f,2.044059f,-2f);
			questionBoxRef.transform.localScale = new Vector3(0,0,0.1f);
		}

		for(int i=0;i<6;i++)
		{
			if(!isHide)
			{
				questionBoxRef.transform.localScale += new Vector3(0.1f,0.1f,0);
			}
			else
			{
				questionBoxRef.transform.localScale -= new Vector3(0.1f,0.1f,0);
			}

			yield return 0;
		}

		if(isHide)
		{	
			questionBoxRef.transform.position = new Vector3(-100,0,0);
		}
	}

	private IEnumerator DiscardAnswers(int index)
	{
		for(int i=0;i<4;i++)
		{
			currentAnswers[i].GetComponent<BoxCollider2D>().enabled = false;
		}

		SpriteRenderer sr = (SpriteRenderer) currentAnswers[index].GetComponent<SpriteRenderer>();
		Color cl = sr.color;

		for(int i=0;i<32;i++)
		{
			for(int j=0;j<currentAnswers.Count;j++)
			{
				if(i<20&&j!=index)
				{
					currentAnswers[j].transform.localScale -= new Vector3(0.025f,0.025f,0);
				}
				
				if(j==index&&i>2)
				{
					currentAnswers[j].transform.position += new Vector3(0,0.05f,0);

					if(i>5)
					{
						cl.a -= 0.03f;
						sr.color = cl;
					}
				}
			}

			yield return 0;
		}

		for(int i=0;i<currentAnswers.Count;i++)
		{
			Destroy(currentAnswers[i]);
		}

		yield return 0;
	}

	public void ShowAnswersByCategory(CategorySelect.CategoryTypes cat)
	{
		BATTLE_ENGINE.canTarget = false;
		
		currentQuestion = (Question) GetQuestionByCategory(cat);

		StartCoroutine(ShowFadeBG());
		StartCoroutine(ShowQuestionBox());

		currentAnswers.Clear();

		for(int i=0;i<4;i++)
		{	
			GameObject obj = (GameObject) Instantiate(answerObjRef,new Vector3(-5f,-3.2f+(0.7f*i),-2.1f),answerObjRef.transform.rotation);
			obj.name = i.ToString();
			currentAnswers.Add(obj);
			TextMesh tm = (TextMesh) obj.GetComponentInChildren<TextMesh>();
			tm.text = currentQuestion.answers[i];
			StartCoroutine(SpawnAnswer(i,currentQuestion.answers[i],obj));
		}
	}

	private IEnumerator SpawnAnswer(int index, string answer, GameObject obj)
	{
		yield return new WaitForSeconds((float)index * 0.1f);

		for(int i=0;i<20;i++)
		{
			obj.transform.position += new Vector3(0.25f,0,0);
			yield return 0;
		}

		yield return new WaitForSeconds(0.5f);

		canAnswer = true;

		yield return 0;
	}

	private IEnumerator UserAnswer(bool isCorrect, int index)
	{
		canAnswer = false;

		StartCoroutine(ShowQuestionBox(true));
		StartCoroutine(ShowFadeBG(true));
		yield return StartCoroutine(DiscardAnswers(index));

		StartCoroutine(BATTLE_ENGINE.NextRound(isCorrect));

		yield return 0;
	}

	private void Update()
	{
		if(canAnswer)
		{
			if(Input.GetMouseButtonDown(0))
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if(hit.collider)
				{
					if(hit.collider.gameObject.tag.Equals("AnswerButton"))
					{
					    int answerIndex = (int) System.Convert.ToInt32(hit.collider.gameObject.name);
					   	bool isCorrectAnswer = currentQuestion.CheckAnswer(answerIndex+1);
					   	StartCoroutine(UserAnswer(isCorrectAnswer,answerIndex));
					   	Debug.Log(answerIndex+":"+isCorrectAnswer);
					}
				}
			}
		}
	}
}
