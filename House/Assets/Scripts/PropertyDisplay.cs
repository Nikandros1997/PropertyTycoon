using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyDisplay : MonoBehaviour {

	public Sprite[] sprites;
	public static Sprite[] sprites2;

	public Image image;
	public static Image image2;


	public static Sprite empty;

	// Use this for initialization
	void Start () {
		sprites2 = sprites;
		image2 = image;
		empty = image.sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * Sets the sprite of the property.
	 */
	public static void SetSprite(string spriteName) {
		foreach(Sprite s in sprites2) {
			//Debug.Log (s.name); 
			if(s.name.Equals(spriteName)) {

				if(image2.gameObject.activeInHierarchy) {
					Debug.Log ("Change the image or turn off the image " + spriteName);
					Debug.Log ("This is where it goes wrong.");

					if (image2.sprite.name.Equals(spriteName)) {
						image2.gameObject.SetActive (false);
						//image2.sprite = new Sprite();
					} else {
						image2.sprite = s;
					}

				} else if(!image2.gameObject.activeInHierarchy) {
					Debug.Log ("Turn on the image " + spriteName);
					image2.sprite = s;
					image2.gameObject.SetActive (true);
				}
			}
		}
	}
}
