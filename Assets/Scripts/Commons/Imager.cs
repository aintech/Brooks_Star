using UnityEngine;
using System.Collections;

public static class Imager {

	private static bool initialized;

	//Star systems
	private static Sprite aluria;

    //System Stars
    private static Sprite aluriaStar;

	//Planet Surfaces
	private static Sprite corasSurf, paletteSurf;

    //Planets
    private static Sprite coras, palette; 

	//Portraits
	private static Texture alika, rokot;

	public static Sprite getPlanetSurface (PlanetType type) {
		switch (type) {
			case PlanetType.CORAS: return corasSurf;
            case PlanetType.PALETTE: return paletteSurf;
			default: Debug.Log("Unknown planet type: " + type); return null;
		}
	}

    public static Sprite getPlanet (PlanetType type) {
        switch (type) {
            case PlanetType.CORAS: return coras;
            case PlanetType.PALETTE: return palette;
            default: Debug.Log("Unknown planet type: " + type); return null;
        }
    }

	public static Sprite getStarSystem (StarSystemType type) {
		switch (type) {
			case StarSystemType.ALURIA: return aluria;
			default: Debug.Log("Unknown system type: " + type); return null;
		}
	}

    public static Sprite getStar (StarSystemType type) {
        switch (type) {
            case StarSystemType.ALURIA: return aluriaStar;
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
                case "StarSystem_Aluria": aluria = sprite; break;
                case "Star_Aluria": aluriaStar = sprite; break;
				case "Planet_Coras": coras = sprite; break;
                case "Planet_Palette": palette = sprite; break;
                case "PlanetSurface_Coras": corasSurf = sprite; break;
                case "PlanetSurface_Palette": paletteSurf = sprite; break;
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