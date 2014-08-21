using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip answerwrong;
	public AudioClip weapon_attack;
	public AudioClip battle_win;
	public AudioClip battle_lose;
	public AudioClip rewardsmenu;
	public AudioClip select_category;
	public AudioClip powerup_category;


	public GameObject mainthemesoundmanager;

	void Awake()
	{
		mainthemesoundmanager=GameObject.Find("Main Camera");
	}

	public void selectcategory_soundplay()
	{
		this.audio.clip=select_category;
		this.audio.Play();
	}

	public void answerwrong_soundplay()
	{
		this.audio.clip=answerwrong;
		this.audio.Play();
	}

	public void weapon_attack_soundplay()
	{
		this.audio.clip=weapon_attack;
		this.audio.Play();
	}

	public void powerupcategory_select_soundplay()
	{
		this.audio.clip=powerup_category;
		this.audio.Play();
	}

	public IEnumerator winbattle_soundplay()
	{
		yield return new WaitForSeconds(0.65f);
		this.audio.clip=battle_win;
		this.audio.Play();
	}

	public IEnumerator lostbattle_soundplay()
	{
		yield return new WaitForSeconds(0.65f);
		this.audio.clip=battle_lose;
		this.audio.Play();
	}

	public void mutemaintheme_sound()
	{
		mainthemesoundmanager.audio.mute=true;
	}
	
}
