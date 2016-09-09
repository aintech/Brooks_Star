using UnityEngine;
using System.Collections;

public class Imager : MonoBehaviour {

	public Sprite corasBg;

	private static Sprite coras;

	public Texture portraitAlika, portraitRokot;

	public static Texture alikaPortrait, rokotPortrait;

	public void init () {
		alikaPortrait = portraitAlika;
		rokotPortrait = portraitRokot;
		coras = corasBg;
		gameObject.SetActive(false);
	}

	public static Sprite getPlanetBG (PlanetType type) {
		switch (type) {
			case PlanetType.CORAS: return coras;
			default: Debug.Log("Unknown planet type"); return null;
		}
	}
}