using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Characters : MonoBehaviour {

	public class Character
	{
		public string name;
		public int life;
		public GameObject obj;

		public Character(string _name, int _life, GameObject _obj)
		{
			name = _name;
			life = _life;
			obj = _obj;
		}
	}

	public List<Character> AllCharacters = new List<Character>();
	public GameObject currentChar;

	private void Awake()
	{
		AllCharacters.Add(new Character("Batman",3,(GameObject)Resources.Load("Characters/char_2")));
	}

	private void Start()
	{
		// for now
		currentChar = (GameObject) Instantiate(AllCharacters[0].obj,new Vector3(0,-2.83305f,-1.4f),AllCharacters[0].obj.transform.rotation);

		//New Changes *** Rotating the character so it is not inverted, and also repositioning it
		currentChar.transform.eulerAngles=new Vector3(0f,0f,180f);
		currentChar.transform.position=new Vector3(currentChar.transform.position.x,currentChar.transform.position.y+0.25f,currentChar.transform.position.z);
		//
	}
}
