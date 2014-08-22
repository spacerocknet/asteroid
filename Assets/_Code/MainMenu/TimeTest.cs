using UnityEngine;
using System;
using System.Collections;

public class TimeTest : MonoBehaviour {


	public GameObject timer;
	public GameObject timerremaining;
	public GameObject totallives_gameobject;
	DateTime oldtime;
	DateTime currenttime;

	int paused;
	int totallives;
	string previoustime="15:00";

	void Awake()
	{
		paused=0;
		totallives=2;
	}


	void Update()
	{
		totallives_gameobject.GetComponent<TextMesh>().text=totallives.ToString();
	}

	void OnApplicationPause(bool pause)
	{
	if(pause)
		{
			PlayerPrefs.SetString("oldtime",DateTime.Now.ToBinary().ToString());
			int previouselapsedminutes=int.Parse(previoustime.Substring(0,2));
			int previouselapsedseconds=int.Parse(previoustime.Substring(3,2));
			int totalelapsedseconds=(((1800-(previouselapsedminutes*60))-((00-previouselapsedseconds)*-1)));
			PlayerPrefs.SetInt("totalelapsedseconds",totalelapsedseconds);
			Debug.Log("Total elapsedseconds "+totalelapsedseconds.ToString());
			paused=1;
		}
		else
		{
			if(paused==1)
			{
				Debug.Log("Called");
				currenttime=DateTime.Now;
				long temp=Convert.ToInt64(PlayerPrefs.GetString("oldtime"));
				oldtime=DateTime.FromBinary(temp);
				Debug.Log("Oldtime is :"+oldtime.ToString());
				Debug.Log("Currenttime is :"+currenttime.ToString());
				
				TimeSpan difference = currenttime.Subtract(oldtime);
				int totaldifferenceseconds=(int)difference.TotalSeconds;
				Debug.Log("totaldifferenceseconds is " + totaldifferenceseconds.ToString());

				//Display on the text mesh the total difference
				int previouslyelapsedseconds=PlayerPrefs.GetInt("totalelapsedseconds");
				int finalelapsedseconds=totaldifferenceseconds+previouslyelapsedseconds;


				timer.GetComponent<TextMesh>().text=finalelapsedseconds.ToString();


				if(finalelapsedseconds>1800 && finalelapsedseconds< 3600)
				{
					totallives++;
					int timeremaininginseconds=Mathf.Abs(finalelapsedseconds-3600);
					TimeSpan t=TimeSpan.FromSeconds(timeremaininginseconds);
				}
				else
				{
					int timeremainingseconds=1800-finalelapsedseconds;
					TimeSpan newtimespan=TimeSpan.FromSeconds(timeremainingseconds);
					timerremaining.GetComponent<TextMesh>().text=newtimespan.ToString().Substring(3,5);
				}
				
			}

		}
	}
}