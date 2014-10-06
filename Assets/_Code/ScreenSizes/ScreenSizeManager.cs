using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ScreenSizeManager : MonoBehaviour {

	public Vector2 defaultScreenSize;
	public Vector2[] supportedScreenSizes;
	public string defaultSpriteSizeLabel;

	void Awake() {
		//UpdateAllSprites (Screen.width, Screen.height);
		UpdateAllSprites (640, 960);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Sprite LoadSpriteScreenSize(Sprite sprite) {
		string spriteSizeName = GetSpriteSizeName(sprite, Screen.width, Screen.height);

		Sprite spriteSize = Resources.Load<Sprite>(spriteSizeName);
		if (spriteSize != null) {
			return spriteSize;
		}

		return sprite;
	}

	public void UpdateAllSprites(int screenWidth, int screenHeight)
	{
		List<Vector2> allScreenSizes = new List<Vector2> (supportedScreenSizes);

		Vector2 screenSize = allScreenSizes.Find (size => size.x == screenWidth && size.y == screenHeight);
		if (screenSize != null) {
			SpriteRenderer[] allSpriteRenderers = GameObject.FindObjectsOfType<SpriteRenderer> ();
			if (allSpriteRenderers != null) {
				foreach (SpriteRenderer spriteRenderer in allSpriteRenderers) {
					string spriteSizeName = GetSpriteSizeName(spriteRenderer.sprite, screenWidth, screenHeight);
					if (!string.IsNullOrEmpty(spriteSizeName)) {
						Sprite spriteSize = Resources.Load<Sprite>(spriteSizeName);
						if (spriteSize != null) {
							spriteRenderer.sprite = spriteSize;
						}
					}
				}
			}
		}
	}

	private string GetSpriteSizeName (Sprite defaultSprite, int screenWidth, int screenHeight) 
	{
		if (defaultSprite != null) {
			string spriteBaseName = defaultSprite.name.Replace (defaultSpriteSizeLabel, "");
			string spriteSizeName = spriteBaseName + screenWidth + "-x-" + screenHeight + ".png";

			return spriteSizeName;
		}

		return null;
	}
}
