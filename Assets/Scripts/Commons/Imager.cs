using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Imager {

	private static bool initialized;

	private static Array starSystemTypes = Enum.GetValues(typeof(StarSystemType)),
						 planetTypes = Enum.GetValues(typeof(PlanetType)),
						 characterTypes = Enum.GetValues(typeof(CharacterType)),
						 enemyTypes = Enum.GetValues(typeof(EnemyType));

	private static Dictionary<StarSystemType, Sprite> starSystems = new Dictionary<StarSystemType, Sprite>(),
													  stars = new Dictionary<StarSystemType, Sprite>();

	private static Dictionary<PlanetType, Sprite> planets = new Dictionary<PlanetType, Sprite>(),
												  planetSurfaces = new Dictionary<PlanetType, Sprite>();

	private static Dictionary<CharacterType, Texture> portraits = new Dictionary<CharacterType, Texture>();

	private static Dictionary<EnemyType, Sprite[]> enemies = new Dictionary<EnemyType, Sprite[]>();

	private static char delimiter = '=';

	private static string[] typeName = new string[2];

	public static void initialize () {
		if (initialized) { return; }

		foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites")) { addSpriteToList(sprite); }

		foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/Enemy")) { addSpriteToList(sprite); }

		foreach (Texture texture in Resources.LoadAll<Texture>("Textures")) { addTextureToList(texture); }

		initialized = true;
	}

	public static Sprite getStarSystem (StarSystemType type) { return starSystems[type]; }
	public static Sprite getStar (StarSystemType type) { return stars[type]; }
	public static Sprite getPlanet (PlanetType type) { return planets[type]; }
	public static Sprite getPlanetSurface (PlanetType type) { return planetSurfaces[type]; }
	public static Texture getPortrait (CharacterType type) { return portraits[type]; }

	public static Sprite getEnemy (EnemyType type, float healthLevel) { return enemies[type][!Vars.NSFW? 0: healthLevel <= .3f? 2: healthLevel <= .7f? 1: 0]; }

	private static void addSpriteToList (Sprite sprite) {
		typeName = sprite.name.ToUpper().Split(delimiter);
		switch (typeName[0]) {
			case "ENEMY":
				int index;
				foreach (EnemyType type in enemyTypes) {
					index = typeName.Length == 2? 0: typeName[2].Equals("NUDE")? 2: 1;
					if (type.ToString().Equals(typeName[1])) {
						if (!enemies.ContainsKey(type)) { enemies.Add(type, new Sprite[3]); }
						enemies[type][index] = sprite;
						return;
					}
				}
				Debug.Log("Unmapped enemy: " + typeName[1] + " " + typeName[2]);
				break;
			case "STARSYSTEM": 
				foreach (StarSystemType type in starSystemTypes) {
					if (type.ToString().Equals(typeName[1])) { 
						starSystems.Add(type, sprite);
						return;
					}
				}
				Debug.Log("Unmapped star system: " + typeName[1]);
				break;
			case "STAR": 
				foreach (StarSystemType type in starSystemTypes) {
					if (type.ToString().Equals(typeName[1])) { 
						stars.Add(type, sprite);
						return;
					}
				}
				Debug.Log("Unmapped star: " + typeName[1]);
				break;
			case "PLANET": 
				foreach (PlanetType type in planetTypes) {
					if (type.ToString().Equals(typeName[1])) { 
						planets.Add(type, sprite);
						return;
					}
				}
				Debug.Log("Unmapped planet: " + typeName[1]);
				break;
			case "PLANETSURFACE": 
				foreach (PlanetType type in planetTypes) {
					if (type.ToString().Equals(typeName[1])) { 
						planetSurfaces.Add(type, sprite);
						return;
					}
				}
				Debug.Log("Unmapped planet surface: " + typeName[1]);
				break;
			default: Debug.Log("Unmapped sprite: " + typeName[0] + " - " + typeName[1]); break;
		}
	}

	private static void addTextureToList (Texture texture) {
		typeName = texture.name.ToUpper().Split(delimiter);
		switch (typeName[0]) {
			case "PORTRAIT" :
				foreach (CharacterType type in characterTypes) {
					if (type.ToString().Equals(typeName[1])) { 
						portraits.Add(type, texture);
						return;
					}
				}
				Debug.Log("Unmapped portrait: " + typeName[1]);
				break;
			default: Debug.Log("Unmapped texture: " + typeName[0] + " - " + typeName[1]); break;
		}
	}
}