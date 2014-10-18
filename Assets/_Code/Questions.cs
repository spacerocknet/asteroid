using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Threading;

using SimpleJSON;



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

	private Dictionary<CategorySelect.CategoryTypes, List<Question>> AllQuestions = new Dictionary<CategorySelect.CategoryTypes, List<Question>>();
	private GameObject answerObjRef;
	private GameObject questionBoxRef;
	private GameObject questionTimer;
	private Question currentQuestion;
	private GameObject fadeBG;
	private bool canAnswer;
	private List<GameObject> currentAnswers = new List<GameObject>();
	private BattleEngine BATTLE_ENGINE;
	private static float TIME_FOR_ANSWER = 10.0f;
	private bool cancelIndicator;
	private bool testMode = false; //should use isDebugBuild()?
	private int maxWordsPerLine = 35;
	private CategorySelect.ColorTypes currentColorType;

	private Vector3 questionStartScale;
	private Vector3 answerStartPosition;

	private Vector3 indicatorPosition;

	//GameObject PowerUp References
	private GameObject powerup1;
	private GameObject powerup2;
	private GameObject powerup3;
	private GameObject powerup4;

	private GameObject character;


	private GameObject soundmanager;

	private ScreenSizeManager screenSizeManager;

	private void Awake()
	{
		cancelIndicator = false;
		canAnswer = false;
		fadeBG = (GameObject) GameObject.Find("GFX/FadeBG");
		answerObjRef = (GameObject) GameObject.Find("REFERENCES/answer_bg");
        questionBoxRef = (GameObject) GameObject.Find("REFERENCES/question_box");
		questionTimer = GameObject.Find ("Question Timer");

		BATTLE_ENGINE = (BattleEngine) GameObject.Find("MAIN").GetComponent<BattleEngine>();


		//PowerupRefrences
		powerup1=GameObject.Find("button_power_up_02");
		powerup2=GameObject.Find("button_power_up_03");
		powerup3=GameObject.Find("button_power_up_04");
		powerup4=GameObject.Find("button_power_up_05");

		//Related to soundmanager
		soundmanager=GameObject.Find("Secondary_SoundManager");

		screenSizeManager = GameObject.FindObjectOfType<ScreenSizeManager> ();

		// temporary for testing
		//Movies
		AllQuestions.Add(CategorySelect.CategoryTypes.Movie, new List<Question> ());
		List<Question> movies = AllQuestions[CategorySelect.CategoryTypes.Movie];
		movies.Add(new Question(0,CategorySelect.CategoryTypes.Movie,"Who directed 'Avatar' in 2009",GenerateAnswerList("Werner Herzog","Sam Raimi","James Cameron","Kathryn Bigelow"),3));
	/*	movies.Add(new Question(1,CategorySelect.CategoryTypes.Movies,"Who is the director of the 2010 movie 'Kick Ass'",GenerateAnswerList("Spike Lee","Mathew Vaughn","Ridley Scott","Peter Jackson"),2));
		movies.Add(new Question(2,CategorySelect.CategoryTypes.Movies,"In 'Men in Black 3', if Will Smith is 'J', Tommy Lee Jones is 'K', who is 'O'",GenerateAnswerList("Nicolas Cage","Emma Thompson","Josh Brolin","Lady Gaga"),2));
		movies.Add(new Question(3,CategorySelect.CategoryTypes.Movies,"'True Lies' was directed by which of the following",GenerateAnswerList("Jan de Bont","John McTiernan","James Cameron","Tony Scott"),3));
		movies.Add(new Question(4,CategorySelect.CategoryTypes.Movies,"Which actor is the voice of Lightening McQueen in the 2011 movie 'Cars 2'",GenerateAnswerList("Charlie Sheen","Clive Owen","Liam Neeson","Owen Wilson"),4));
		movies.Add(new Question(5,CategorySelect.CategoryTypes.Movies,"Who plays the role of Iron Man in 'The Avengers'",GenerateAnswerList("Robert Downey Jr","Mark Ruffalo","Christian Bale","Brandon Routh"),1));
		movies.Add(new Question(6,CategorySelect.CategoryTypes.Movies,"In 'Iron Man 3', what is the name of the experimental regenerative drug created by Dr. Killian",GenerateAnswerList("Vitalis","J.A.R.V.I.S","Extremis","Payola"),3));
		movies.Add(new Question(7,CategorySelect.CategoryTypes.Movies,"Which of the following is not an Arnold Schwarzegegger film",GenerateAnswerList("Cliffhanger","Kindergarten Cop","Conan the Barbarian","The Last Stand"),1));
		movies.Add(new Question(8,CategorySelect.CategoryTypes.Movies,"How many colors are used as 'Mr' aliases in 'Reservoir Dogs'",GenerateAnswerList("6","7","8","3"),1));
		movies.Add(new Question(9,CategorySelect.CategoryTypes.Movies,"At the 2009 Golden Globe Awards, who won Best Director for 'Avatar'",GenerateAnswerList("Danny Boyle","James Cameron","Steven Spielberg","Martin Scorsese"),2));
		movies.Add(new Question(10,CategorySelect.CategoryTypes.Movies,"Which movie was written Aaron Sorkin",GenerateAnswerList("The King's Speech","Brokeback Mountain","Big Fish","The Social Network"),4));
		movies.Add(new Question(11,CategorySelect.CategoryTypes.Movies,"Who directed a 2011 adaptation of 'Jane Eyre' starring Mia Wasikowska",GenerateAnswerList("Cary Fukunaga","Miguel  Arteta","Shana Feste","Adam Deacon"),3));
		movies.Add(new Question(12,CategorySelect.CategoryTypes.Movies,"What is the sub-title of the 2002 Lord of the Rings movie",GenerateAnswerList("The Two Towers","The Return of the King","Rise of the Witch King","The Fellowship of the Ring"),1));
		movies.Add(new Question(13,CategorySelect.CategoryTypes.Movies,"Who played the role of Bruce Banner in the 2008 film 'The incredible Hulk'",GenerateAnswerList("Tim Roth","Mark Ruffalo","Eric Bana","Edward Norton"),4));
		movies.Add(new Question(14,CategorySelect.CategoryTypes.Movies,"Who played Peter Parker in the Spiderman re-boot 'The Amazing Spider-Man'",GenerateAnswerList("Andrew Garfield","Johnny Depp","James Marsden","Tobey Maguire"),1));
	*/
		//Sports
		AllQuestions.Add(CategorySelect.CategoryTypes.Sport, new List<Question> ());
		List<Question> sports = AllQuestions[CategorySelect.CategoryTypes.Sport];
		sports.Add(new Question(100,CategorySelect.CategoryTypes.Sport,"What team did Steve Nash play for before signing with the Lakers in 2012",GenerateAnswerList("Phoenix Suns","Miami Heat","New York Knicks","San Antonio Spurs"),1));
	/*	sports.Add(new Question(101,CategorySelect.CategoryTypes.Sports,"What legendary actor is often seen wearing sunglasses in his court side seat at Lakers home games",GenerateAnswerList("Al Pacino","Anthony Hopkins","Jack Nicholson","Robert De Niro"),3));
		sports.Add(new Question(102,CategorySelect.CategoryTypes.Sports,"What team did Mike D'Antoni coach before joining the Lakers",GenerateAnswerList("Houston Rockets","New York Knicks","Golden State Warriors","Brooklyn Nets"),2));
		sports.Add(new Question(103,CategorySelect.CategoryTypes.Sports,"What is the name of the arena where the Lakers play",GenerateAnswerList("The Mobil One Center","The Office Max Center","The Staples Center","The Times Union Center"),3));
		sports.Add(new Question(104,CategorySelect.CategoryTypes.Sports,"What one time Lakers has the record for most season leading league in defensive rebounds with six",GenerateAnswerList("Dwight Howard","Wilt Chamberlain","Kareem Jabbar","Shaquille O'Neal"),1));
		sports.Add(new Question(105,CategorySelect.CategoryTypes.Sports,"Who did Chick Hearn call Mr. Basketball",GenerateAnswerList("Kareem Jabbar","Jerry West","Magic Johnson","George Mikan"),4));
		sports.Add(new Question(106,CategorySelect.CategoryTypes.Sports,"Who was the Lakers' starting point guard in the 2004-05 season",GenerateAnswerList("Derek Fisher","Chucky Atkins","Smush Parker","Jordan Farmar"),2));
		sports.Add(new Question(107,CategorySelect.CategoryTypes.Sports,"Which club did Eden Hazard sign for in 2012",GenerateAnswerList("West Ham Utd","Manchester City","Chelsea","Newcastle United"),3));
		sports.Add(new Question(108,CategorySelect.CategoryTypes.Sports,"As of 2013, which of these is a soccer player in Liverpool FC",GenerateAnswerList("Matt Carpenter","Trevor Rosenthal","Daniel Sturridge","Andrei Kirilenko"),3));
		sports.Add(new Question(109,CategorySelect.CategoryTypes.Sports,"Which country is hosting the 2014 World Cup Finals",GenerateAnswerList("Italy","Mexico","Brazil","Argentian"),3));
		sports.Add(new Question(100,CategorySelect.CategoryTypes.Sports,"As of 2013, which of these is a soccer player in Liverpool FC",GenerateAnswerList("Lance Lynn","Peter Kozma","Jason Terry","Raheem Sterling"),4));
		sports.Add(new Question(110,CategorySelect.CategoryTypes.Sports,"The Estadio Santiago Bernabau is a soccer stadium in which city",GenerateAnswerList("Rome","Seville","Madrid","Milan"),3));
		sports.Add(new Question(111,CategorySelect.CategoryTypes.Sports,"Which 35 year old footballer won the PFA Player of the Year Award in 2009",GenerateAnswerList("Alan Shearer","Gary Neville","Alan Hansen","Ryan Giggs"),4));
		sports.Add(new Question(112,CategorySelect.CategoryTypes.Sports,"What was David Beckham's squad number in his first World Cup",GenerateAnswerList("7","11","10","23"),1));
		sports.Add(new Question(113,CategorySelect.CategoryTypes.Sports,"The Miami Dolphins compete in which sports",GenerateAnswerList("Basketball","Baseball","Ice Hockey","American Football"),4));
		sports.Add(new Question(114,CategorySelect.CategoryTypes.Sports,"Detroit Red Wings compete in which sport",GenerateAnswerList("American Football","Basketball","Baseball","Ice Hockey"),4));
		sports.Add(new Question(115,CategorySelect.CategoryTypes.Sports,"What colour is the 8-ball in a game of pool",GenerateAnswerList("Orange","Black","Yellow","Purple"),2));
		sports.Add(new Question(116,CategorySelect.CategoryTypes.Sports,"In american football, how many points are scored for a touchdown",GenerateAnswerList("5","4","3","6"),4));
		sports.Add(new Question(117,CategorySelect.CategoryTypes.Sports,"Which of these teams' coaches would wear the same uniform as the players",GenerateAnswerList("New York Giants","New York Knicks","New York Yankees","New York Rangers"),3));
		sports.Add(new Question(118,CategorySelect.CategoryTypes.Sports,"What trophy was found at the bottom of Mario Lemieux's swimming pool in 1991",GenerateAnswerList("Jules Rimet Trophy","Borg Warner","Stanley Cup","Heisman Trophy"),3));
		sports.Add(new Question(119,CategorySelect.CategoryTypes.Sports,"Goal Shooter and Goal Attack are the only players that can score in which sport",GenerateAnswerList("Lacrosse","Ice Hockey","Netball","Aussie Rules Football"),3));
		*/

		//Musics
		AllQuestions.Add(CategorySelect.CategoryTypes.Music, new List<Question> ());
		List<Question> musics = AllQuestions[CategorySelect.CategoryTypes.Music];
		musics.Add(new Question(200,CategorySelect.CategoryTypes.Music,"Who was nominated as one of the three 'men of the decade' on a special UK television production in 1069",GenerateAnswerList("George","Paul","John","Ringo"),3));
	/*	musics.Add(new Question(201,CategorySelect.CategoryTypes.Music,"Who was the final Beatle to be married",GenerateAnswerList("Paul","John","Ringo","George"),1));
		musics.Add(new Question(202,CategorySelect.CategoryTypes.Music,"Who did Dana Carvey regularly impersonate on Saturday Night Live",GenerateAnswerList("John","Ringo","George","Paul"),4));
		musics.Add(new Question(203,CategorySelect.CategoryTypes.Music,"John Lennon said The Beatles were 'more popular than Jesus' in which newspaper",GenerateAnswerList("London Evening Standard","London Chronical","Daily Mail","Liverpool Daily Post And Echo"),1));
		musics.Add(new Question(204,CategorySelect.CategoryTypes.Music,"Which band had a 2004 hit with 'She will be loved'",GenerateAnswerList("Maroon 5","Creed","Los Lonely Boys","The Killers"),1));
		musics.Add(new Question(205,CategorySelect.CategoryTypes.Music,"Black Eyed Peas retooled their song 'let's Get Retarded' for commercials. What did they rename it",GenerateAnswerList("Let's Get Rebounded","Let's Go Rebound It","Let's Get It Started","Let's Get Rewarded"),3));
		musics.Add(new Question(206,CategorySelect.CategoryTypes.Music,"Who released the 2008 album 'I Am Sasha Fierce'",GenerateAnswerList("Lady Gaga","Madonna","Gwen Stefani","Beyonce"),4));
		musics.Add(new Question(207,CategorySelect.CategoryTypes.Music,"'Disturbia' was a 2008 No.1 hit for which female popstar",GenerateAnswerList("Rihanna","Lady Gaga","Leona Lewis","Kelly Clarkson"),1));
		musics.Add(new Question(208,CategorySelect.CategoryTypes.Music,"Which 2002 film starred Eminem",GenerateAnswerList("Meet The Fockers","Signs","8 Mile","The Footbal Factory"),3));
		musics.Add(new Question(209,CategorySelect.CategoryTypes.Music,"With which band is Adam Levine the lead singer",GenerateAnswerList("Maroon 5","Los Lonely Boys","The Killers","Creed"),1));
		musics.Add(new Question(210,CategorySelect.CategoryTypes.Music,"Good Riddance (Time of Your Life) was a 90s release by which band",GenerateAnswerList("The Offspring","Green Day","Blink 182","Bloodhound Gang"),2));
		musics.Add(new Question(211,CategorySelect.CategoryTypes.Music,"Which kind of love did Tupac Shakur have a hit with 1996",GenerateAnswerList("Florida","New York","Vegas","California"),4));
		musics.Add(new Question(212,CategorySelect.CategoryTypes.Music,"Complete the title of the Grammy wining Seal song 'Kiss From A ....",GenerateAnswerList("Sun","Love","God","Rose"),4));
		musics.Add(new Question(213,CategorySelect.CategoryTypes.Music,"Who had a 1999 hit with 'Man! I Feel Like a Woman'",GenerateAnswerList("Jennifer Rush","Shania Twain","Madonna","Sheryl Crow"),2));
		musics.Add(new Question(214,CategorySelect.CategoryTypes.Music,"'Unbreak My Heart' was a No.1 hit for which star in 1997",GenerateAnswerList("Toni Braxton","Brandy","Beyonce","Mariah Carey"),1));
		musics.Add(new Question(215,CategorySelect.CategoryTypes.Music,"Which band sang the theme tune to the TV Sitcom Friends",GenerateAnswerList("The Remembrance","The Rembrants","The Remembers","The Remingtons"),2));
	*/

		//Geography
		AllQuestions.Add(CategorySelect.CategoryTypes.Geography, new List<Question> ());
		List<Question> geography = AllQuestions[CategorySelect.CategoryTypes.Geography];
		geography.Add(new Question(300,CategorySelect.CategoryTypes.Geography,"Where is Lithuania",GenerateAnswerList("In US","In EU","In Asia","In Australia"),2));
	/*	geography.Add(new Question(301,CategorySelect.CategoryTypes.Geography,"Milan is a city in which Italian Region",GenerateAnswerList("Tuscany","Piedmonth","Lombardy","Sicilia"),3));
		geography.Add(new Question(302,CategorySelect.CategoryTypes.Geography,"Which of these is a city in Switzerland",GenerateAnswerList("InnsBruck","Geneva","LilyHammer","Hesse"),2));
		geography.Add(new Question(303,CategorySelect.CategoryTypes.Geography,"The largest ancient castle in the world is located in this city",GenerateAnswerList("Prague","Memphis","Bejing","Cairo"),1));
		geography.Add(new Question(304,CategorySelect.CategoryTypes.Geography,"Which of these is a city in Paraguay",GenerateAnswerList("Oruro","San Lorenzo","Saltao","La Plato"),2));
		geography.Add(new Question(305,CategorySelect.CategoryTypes.Geography,"Genoa is a major city in which Eurupean country",GenerateAnswerList("Bulgari","France","Italy","Russia"),3));
		geography.Add(new Question(306,CategorySelect.CategoryTypes.Geography,"Mount Catherine has been the highest point in two different countries.  Where is it",GenerateAnswerList("Sinai Peninsula","Kurdistan","Kuwait","Golan Heights"),1));
		geography.Add(new Question(307,CategorySelect.CategoryTypes.Geography,"Which of these countries does Oman NOT share a border with",GenerateAnswerList("Yemen","Kuwait","United Arab Emirates","Saudi Arabia"),2));
		geography.Add(new Question(308,CategorySelect.CategoryTypes.Geography,"What Arab nation has the highest percentage of Christian",GenerateAnswerList("Egypt","Syria","Iraq","Lebanon"),4));
		geography.Add(new Question(309,CategorySelect.CategoryTypes.Geography,"What was the only country ever to have archdukes, although it doesn't anymore",GenerateAnswerList("Germany","Netherlands","Austria","Sweden"),3));
	*/

	}

	void Start() {
		questionStartScale = questionBoxRef.transform.localScale;
		questionBoxRef.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
	}

	//temp
	public static List<string> GenerateAnswerList(string s1, string s2, string s3, string s4)
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

	private IEnumerator TimerForAnswer(float t)
	{
		cancelIndicator = false;
		float cDur = t;
		//float cSize = 8f;
		float cTime = Time.timeSinceLevelLoad;

		bool timeFalse = false;

		for(;;)
		{
			float timeLapse = Time.timeSinceLevelLoad-cTime;
			UpdateIndicator(timeLapse,cDur);

			if(cancelIndicator)
			{
				UpdateIndicator(cDur,cDur);
				timeFalse = false;
				break;
			}

			if(timeLapse>=cDur)
			{
				timeFalse = true;
				break;
			}

			yield return 0;
		}

		if(timeFalse)
		{
			StartCoroutine(UserAnswer(false,-1));
		}

		yield return 0;
	}

	private void UpdateIndicator(float timeLapse, float duration)
	{
		//float barXSize = (timeLapse/duration) * 8.0f;
		//indicatorColor.transform.localScale = new Vector3(8.0f-barXSize,1,1);
		float timeRemainingSeconds = duration - timeLapse;
		if (timeRemainingSeconds > 0) {
			System.DateTime dateTime = new System.DateTime ();
			dateTime = dateTime.AddSeconds (timeRemainingSeconds);

			questionTimer.GetComponent<TextMesh> ().text = "Time: " + dateTime.ToString ("ss.ff");
		}
	}


	//Test mode: get from AllQuestions
	//Release mode: get it from backend server
	private IEnumerator GetQuestionByCategory(CategorySelect.CategoryTypes cat)
	{
		  if (testMode) {
			  currentQuestion = GetQuestionByCategoryLocal (cat);
				//yield return currentQuestion;
		  } else {
				/*
                    GameResource gameResource = new GameResource ();
                    string catName = cat.ToString ();
                    JSONNode json = gameResource.GetQuizzes (catName, 1);

                    Question q = new Question (json ["qid"].AsInt, cat, json ["question"],
                                               GenerateAnswerList (json ["answers"] [0], json ["answers"] [1], json ["answers"] [2], json ["answers"] [3]),
                                               1);

                    return q;
                  */
		
				 /*
                    RestClient rc = new RestClient ();
                    string catName = cat.ToString ();
                    string result = rc.GetQuizzes(catName, 1, (string s) => {
                                                             Debug.Log ("HEHEHE: " + s);
                                                             currentQuestion = GetQuestionByCategoryLocal(cat);
                                                        });
                    Debug.Log ("TEsting here: " + result);
                  */
				GameResource gameResource = new GameResource ();
				currentQuestion = gameResource.GetQuizzes (cat, 1);
				Debug.Log ("currentQuestion: " + currentQuestion.title);
			}

			float scaleY = screenSizeManager.scaleY;
			float offsetY = questionBoxRef.transform.localPosition.y + questionBoxRef.renderer.bounds.extents.y * 0.7f;
			TextMesh tm = (TextMesh) questionBoxRef.GetComponentInChildren<TextMesh>();
			tm.transform.position = new Vector3(tm.transform.position.x, offsetY, tm.transform.position.z);
			
			GameObject answersStart = GameObject.Find ("Answers Start");
			answerStartPosition = new Vector3 (-5f, answersStart.transform.localPosition.y, -8f);
			
		    //yield return currentQuestion;
		   	yield return StartCoroutine(ShowQuestionBox());
	}
	
	private Question GetQuestionByCategoryLocal(CategorySelect.CategoryTypes cat)
	{
		List<Question> questions = AllQuestions[cat];
		
		if (questions != null) {
			int index = Random.Range(0, questions.Count);
			return (Question) questions[index];
		}

		Debug.LogError("[GetQuestionByCategory] Can't Happen.");
		
		IEnumerator enumerator = AllQuestions.Values.GetEnumerator();
		enumerator.MoveNext();
		//object first = enumerator.Current;

		return ((List<Question>)enumerator) [0]; //just return the first one
	}

	public IEnumerator ShowFadeBG(bool isHide=false, bool popupCloser=false)
	{
		if(popupCloser) {
			fadeBG.transform.position = new Vector3(fadeBG.transform.position.x,fadeBG.transform.position.y,-3.5f);
		}

		SpriteRenderer sr = (SpriteRenderer) fadeBG.GetComponent<SpriteRenderer>();
		Color cl = sr.color;

		if(!isHide) {
			fadeBG.GetComponent<BoxCollider2D>().enabled = true;
			cl.a = 0;
		} 
		else {
			cl.a = 1;
		}

		sr.color = cl;

		for(int i=0;i<10;i++) {
			if(!isHide) {
				cl.a += 0.1f;
			} 
			else {
				cl.a -= 0.1f;
			}

			sr.color = cl;

			yield return 0;
		}

		if(!isHide) {
			cl.a = 1.0f;
		} 
		else {
			fadeBG.GetComponent<BoxCollider2D>().enabled = false;
			cl.a = .0f;
		}

		sr.color = cl;

		yield return 0;
	}

	public IEnumerator ShowQuestionBox(bool isHide=false)
	{
		hidecharacter();

		float scaleModifier = 0.1f;

		if(!isHide)
		{
			TextMesh tm = (TextMesh) questionBoxRef.GetComponentInChildren<TextMesh>();
			//tm.fontSize = 13;
			tm.text = ResolveTextSize(currentQuestion.title, maxWordsPerLine) + '?';
			
			//Debug.Log(tm.text);
			questionBoxRef.transform.position = new Vector3(0f,0f,-8f); // Changed The Value To ,x.Zero, Y.Zero to center The Question Box in The screen  

			while (questionBoxRef.transform.localScale.x < questionStartScale.x) {
				questionBoxRef.transform.localScale += new Vector3(scaleModifier, scaleModifier, 0);

				yield return null;
			}
		}
		else
		{
			while (questionBoxRef.transform.localScale.x > 0.1f) {
				questionBoxRef.transform.localScale -= new Vector3(scaleModifier, scaleModifier, 0);

				yield return null;
			}
		}

		if(isHide)
		{	
			questionBoxRef.transform.position = new Vector3(-100,0,0);
		}
	}

	private IEnumerator DiscardAnswers(int index)
	{
		showcharacter();
		for(int i=0;i<4;i++)
		{
			currentAnswers[i].GetComponent<BoxCollider2D>().enabled = false;
		}

		int sIndex = index;

		if(index.Equals(-1))
		{
			sIndex = 0;
		}

		SpriteRenderer sr = (SpriteRenderer) currentAnswers[sIndex].GetComponent<SpriteRenderer>();
		Color cl = sr.color;

		for(int i=0;i<32;i++)
		{
			for(int j=0;j<currentAnswers.Count;j++)
			{
				if(i<22&&j!=index)
				{
					currentAnswers[j].transform.localScale -= new Vector3(0.025f,0.025f,0);

					if(currentAnswers[j].transform.localScale.x<0)
					{
						currentAnswers[j].transform.localScale = new Vector3(0,0,0);
					}
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


	private string ResolveTextSize(string input, int lineLength) {	   
		string[] words = input.Split(' ');
		string result = "";
		string line = "";
    
		foreach(string s in words){
			// Append current word into line
			string temp = line + " " + s;

			if(temp.Length > lineLength) {

				result += line + "\n";
				line = s;
			} else {
				line = temp;
			}
		}
		
		// Append last line into result   
		result += line;
		
		// Remove first " " char
		return result.Substring(1,result.Length-1);
	}
	
	public void ShowAnswersByCategory(CategorySelect.CategoryTypes cat, CategorySelect.ColorTypes colorType)
	{

		currentColorType = (CategorySelect.ColorTypes) colorType;
		BATTLE_ENGINE.canTarget = false;
		//currentQuestion = (Question) GetQuestionByCategory(cat);
		StartCoroutine(GetQuestionByCategory(cat));

		StartCoroutine(ShowFadeBG());
		StartCoroutine(TimerForAnswer(TIME_FOR_ANSWER));

		//Disable the powerup buttonss
		powerup1.GetComponent<BoxCollider2D>().enabled=false;
		powerup2.GetComponent<BoxCollider2D>().enabled=false;
		powerup3.GetComponent<BoxCollider2D>().enabled=false;
		powerup4.GetComponent<BoxCollider2D>().enabled=false;

		//while (currentQuestion == null) {
		//	Thread.Sleep (5);
		//}
		//StartCoroutine(ShowQuestionBox());

		currentAnswers.Clear();

		float offset = answerObjRef.renderer.bounds.extents.y + 0.45f;
		Vector3 scale = new Vector3 (1 / screenSizeManager.scaleX, 1 / screenSizeManager.scaleY, 1);

		for(int i=0;i<4;i++)
		{	
			Vector3 position = answerStartPosition;
			position.y += -offset * i;

			GameObject obj = (GameObject) Instantiate(answerObjRef, position, answerObjRef.transform.rotation); // Changed the value of y to center the answers
			obj.name = i.ToString();
			screenSizeManager.UpdateSpriteRenderer(obj.GetComponent<SpriteRenderer>());
			currentAnswers.Add(obj);
			TextMesh tm = (TextMesh) obj.GetComponentInChildren<TextMesh>();
			tm.text = ResolveTextSize(currentQuestion.answers[i], maxWordsPerLine);
			//tm.transform.localScale = new Vector3(1, 1, 1);

			tm.fontSize *= (int) (1 / screenSizeManager.scaleX); 
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

		powerup1.GetComponent<BoxCollider2D>().enabled=true;
		powerup2.GetComponent<BoxCollider2D>().enabled=true;
		powerup3.GetComponent<BoxCollider2D>().enabled=true;
		powerup4.GetComponent<BoxCollider2D>().enabled=true;

		if(!isCorrect)
		{
			ButtonManager.reducepowerupcount(ButtonManager.powerupselected); 
			soundmanager.GetComponent<SoundManager>().answerwrong_soundplay();
		}

		StartCoroutine(BATTLE_ENGINE.NextRound(isCorrect,currentColorType));

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
						cancelIndicator = true;
					    int answerIndex = (int) System.Convert.ToInt32(hit.collider.gameObject.name);
					   	bool isCorrectAnswer = currentQuestion.CheckAnswer(answerIndex+1);
					   	StartCoroutine(UserAnswer(isCorrectAnswer,answerIndex));
					   //Debug.Log(answerIndex+":"+isCorrectAnswer);
					}
				}
			}
		}
	}

	 void hidecharacter()
	{
		character=GameObject.Find("Character(Clone)");	
		character.transform.localScale=new Vector3(0f,0f,0f);
	}

	void showcharacter()
	{
		character=GameObject.Find("Character(Clone)");
		character.transform.localScale=new Vector3(1f,1f,1f);
	}

	public static void wasanswercorrect()
	{

	}

}
