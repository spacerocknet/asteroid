using UnityEngine;
using System.Collections;

public class dataforleaderboard : MonoBehaviour {


	//This script deals only with displaying of the date on the leaderboard
	//Headers
	public GameObject header1;
	public GameObject header2;
	public GameObject header3;
	public GameObject header4;
	public GameObject header5;


	//Names of Friends/Global
	public GameObject[] names;
	
	//Values GameObjects for corresponding Friends/Global
	public GameObject[] values;


	public string[] values_Strings_friends;
	public string[] names_Strings_friends;

	public string[] values_Strings_global;
	public string[] names_Strings_global;


	void Start () {

		//For Friends
		values_Strings_friends=new string[]{"10000","8000","7000","6500","2100"};
		names_Strings_friends=new string[]{"Paul","Tuhin","Minh","Jessie","Dhaval"};

		//For Global
		names_Strings_global=new string[]{"John","Charlie","Rudy","Alicia","John"};
		values_Strings_global=new string[]{"200000","140000","65031","31000","9585"};


		foreach(Transform child in this.transform)
		{
			child.gameObject.renderer.sortingOrder=6;
		}

		header1.GetComponent<TextMesh>().text="1";
		header2.GetComponent<TextMesh>().text="2";
		header3.GetComponent<TextMesh>().text="3";
		header4.GetComponent<TextMesh>().text="4";
		header5.GetComponent<TextMesh>().text="5";

		for(int i=0;i<5;i++)
		{
			//For local friends data 
			names[i].GetComponent<TextMesh>().text=names_Strings_friends[i].ToString();
			values[i].GetComponent<TextMesh>().text=values_Strings_friends[i].ToString();
		}

	}
	
	public void changecat(string from)
	{
		//Load the data from global and friends into respectice arrays on the start of the game so when global or friend
		//(cont)button is pressed the swapping is quick

		if(from=="global")
		{
		//Change data to global, and change the values of names and values here
			for(int i=0;i<5;i++)
			{
				//For local friends data 
				names[i].GetComponent<TextMesh>().text=names_Strings_global[i].ToString();
				values[i].GetComponent<TextMesh>().text=values_Strings_global[i].ToString();
			}

		}
		else if(from=="friends")
		{
			for(int i=0;i<5;i++)
			{
			names[i].GetComponent<TextMesh>().text=names_Strings_friends[i].ToString();
			values[i].GetComponent<TextMesh>().text=values_Strings_friends[i].ToString();
			}
		}
	}

	public void reloaddataforleaderboards()
	{
	//Repopulate the arrays as level progresses or whenever required just call this method to repopulate them with the new data 
	}
}
