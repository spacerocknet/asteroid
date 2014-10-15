using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ScreenSizeManager : MonoBehaviour {
	public Vector2 defaultScreenSize;
	public Vector2[] supportedScreenSizes;
	public string defaultSpriteSizeLabel;
	public float scaleX;
	public float scaleY;

	private float ScreenWidth;
	private float ScreenHeight;

	private Vector2 closestScreenSize;

	void Awake() {
		ScreenWidth = 800;
		ScreenHeight = 1280;

		Vector2 screenSize = new Vector2 (ScreenWidth, ScreenHeight);

		closestScreenSize = GetClosestScreenSize (screenSize);
		if (closestScreenSize == Vector2.zero) {
			closestScreenSize = defaultScreenSize;
		}

		scaleX = defaultScreenSize.x / closestScreenSize.x;
		scaleY = defaultScreenSize.y / closestScreenSize.y;

		UpdateAllSprites ();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateAllSprites()
	{
		SpriteRenderer[] allSpriteRenderers = GameObject.FindObjectsOfType<SpriteRenderer> ();
		if (allSpriteRenderers != null) {
			foreach (SpriteRenderer spriteRenderer in allSpriteRenderers) {
				UpdateSpriteRenderer (spriteRenderer);
			}
		}
	}

	public void UpdateSpriteRenderer (SpriteRenderer spriteRenderer)
	{
		string spriteSizeName = GetSpriteSizeName (spriteRenderer.sprite, (int) closestScreenSize.x, (int) closestScreenSize.y);
		if (!string.IsNullOrEmpty (spriteSizeName)) {
			Sprite spriteSize = Resources.Load<Sprite> (spriteSizeName);
			if (spriteSize != null) {
				spriteRenderer.sprite = spriteSize;

				Vector3 scale = GetSpriteScaleScreenSize (spriteRenderer, (int) (closestScreenSize.x), (int) (closestScreenSize.y));
				Transform parent = spriteRenderer.gameObject.transform.parent;
				if (!parent || parent.transform.GetComponent<SpriteRenderer>() == null) {

					spriteRenderer.transform.localScale = Vector3.Scale(spriteRenderer.transform.localScale, scale);
					//spriteRenderer.transform.position = Vector3.Scale(spriteRenderer.transform.position, scale);

					Vector3 stretchToFitScale = new Vector3(1, 1, 1);
					stretchToFitScale.x = ScreenWidth > closestScreenSize.x ? ScreenWidth / closestScreenSize.x : 1;
					stretchToFitScale.y = ScreenHeight > closestScreenSize.y ? ScreenHeight / closestScreenSize.y : 1;

					spriteRenderer.transform.localScale = Vector3.Scale(spriteRenderer.transform.localScale, stretchToFitScale);
					spriteRenderer.transform.position = Vector3.Scale(spriteRenderer.transform.position, stretchToFitScale);
				}
				BoxCollider2D boxCollider = spriteRenderer.collider2D as BoxCollider2D;
				if (boxCollider != null) {
					bool isTrigger = boxCollider.isTrigger;
					GameObject.Destroy(spriteRenderer.collider2D);
					BoxCollider2D newBoxCollider = spriteRenderer.gameObject.AddComponent<BoxCollider2D>();
					newBoxCollider.isTrigger = isTrigger;
				}
			}
		}
	}

	public Sprite GetSpriteSize(Sprite sprite) {
		string SpriteSizeName = GetSpriteSizeName (sprite,  (int) closestScreenSize.x, (int) closestScreenSize.y);
		Sprite spriteSize = Resources.Load<Sprite>(SpriteSizeName);
		if (spriteSize != null) {
			return spriteSize;
		}

		return sprite;
	}

	private Vector2 GetClosestScreenSize (Vector2 screenSize)
	{
		Vector2 result = Vector2.zero;

		float shortestDistance = 10000000000;
		foreach (Vector2 supportedScreenSize in supportedScreenSizes) {
			float distance = Vector2.Distance (supportedScreenSize, screenSize);
			if (distance < shortestDistance) {
				result = supportedScreenSize;
				shortestDistance = distance;
			}
		}

		return result;
	}

	private string GetSpriteSizeName (Sprite defaultSprite, int screenWidth, int screenHeight) 
	{
		if (defaultSprite != null) {
			string path = "Sprites/" + screenWidth + " x " + screenHeight;
			string spriteBaseName = defaultSprite.name.Replace (defaultSpriteSizeLabel, "");
			string spriteSizeName = path + "/" + spriteBaseName + screenWidth + "-x-" + screenHeight;

			return spriteSizeName;
		}

		return null;
	}

	private Vector3 GetSpriteScaleScreenSize(SpriteRenderer spriteRenderer, int screenWidth, int screenHeight) {
		float pixelUnitScale = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.bounds.size.x / 100;
		return new Vector3(scaleX * pixelUnitScale, scaleY * pixelUnitScale, 1f);
	}
}
