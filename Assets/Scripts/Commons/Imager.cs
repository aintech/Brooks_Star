using UnityEngine;
using System.Collections;

public static class Imager {

	private static bool initialized;

	//Star systems
	private static Sprite aluria;

	//Planets
	private static Sprite coras;

	//Portraits
	private static Texture alika, rokot;

	public static Sprite getPlanetBG (PlanetType type) {
		switch (type) {
			case PlanetType.CORAS: return coras;
			default: Debug.Log("Unknown planet type: " + type); return null;
		}
	}

	public static Sprite getStarSystemBG (StarSystemType type) {
		switch (type) {
			case StarSystemType.ALURIA: return aluria;
			default: Debug.Log("Unknown system type: " + type); return null;
		}
	}

	public static Texture getPortrait (CharacterType type) {
		switch (type) {
			case CharacterType.ALIKA: return alika;
			case CharacterType.ROKOT: return rokot;
			default: Debug.Log("Unknown character type: " + type); return null;
		}
	}

	public static void initialize () {
		if (initialized) { return; }

		Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites");
		Texture[] textures = Resources.LoadAll<Texture>("Textures");

		foreach (Sprite sprite in sprites) {
			switch (sprite.name) {
				case "Planet_Coras": coras = sprite; break;
				case "StarSystem_Aluria": aluria = sprite; break;
				default: Debug.Log("Unmapped sprite: " + sprite.name); break;
			}
		}

		foreach (Texture texture in textures) {
			switch (texture.name) {
				case "Portrait_Alika": alika = texture; break;
				case "Portrait_Rokot": rokot = texture; break;
				default: Debug.Log("Unmapped texture: " + texture.name); break;
			}
		}

		initialized = true;
	}
}