using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Threading;
using System;

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

	public string GetQuizzes(string category, int num)
	{
		return rc.GetQuizzes(category, num);
	}
    
}