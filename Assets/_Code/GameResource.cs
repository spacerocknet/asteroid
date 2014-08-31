using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Threading;
using System;
using SimpleJSON;

public class GameResource : MonoBehaviour 
{
	private RestClient rc;
	private string categories;
	private string config;
	
	public GameResource ()
	{
		rc = new RestClient ();
		config = rc.GetGameConfig ();
	}

	public string GetGameConfig()
	{
		if (config == null) {
			config = rc.GetGameConfig ();
		}
		
		return config;
	}

	public string GetCategories()
	{
		if (categories == null) {
			categories = rc.GetCategories ();
		}

		return categories;
	}

	public Questions.Question GetQuizzes(CategorySelect.CategoryTypes category, int num)
	{
		string cat = category.ToString();
		string result = rc.GetQuizzes(cat, num, (string s) => {
			Debug.Log ("HEHEHE: " + s);
			//currentQuestion = GetQuestionByCategoryLocal(cat);
		});

		//Debug.Log ("Minh1: " + result);
		JSONNode json = SimpleJSON.JSON.Parse (result);
		Debug.Log ("Json info, answer[0]: " + json[0] ["answers"] [0]);
		Questions.Question q = new Questions.Question (json[0] ["qid"].AsInt, category, json[0] ["question"], 
		                                               Questions.GenerateAnswerList (json[0] ["answers"] [0], json[0] ["answers"] [1], json[0] ["answers"] [2], json[0] ["answers"] [3]),
		                           1);

		return q;
	}
	
}