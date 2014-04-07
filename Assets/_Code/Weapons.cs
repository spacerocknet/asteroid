using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapons : MonoBehaviour {

	public class Weapon
	{
		public string name;
		public float damage;
		public GameObject obj;

		public Weapon(string _name, float _damage, GameObject _obj)
		{
			name = _name;
			damage = _damage;
			obj = _obj;
		}
	}

	public List<Weapon> AllWeapons = new List<Weapon>();

	private void Awake()
	{
		AllWeapons.Add(new Weapon("Bombarder",1.0f,(GameObject)Resources.Load("Weapons/explosion_1")));
	}
}
