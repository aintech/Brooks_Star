using UnityEngine;
using System;
using System.Collections;

public class Minimap : MonoBehaviour {

	public Texture bg, planetOrbit, planet;

	private const float mapSize = 200;

	private const float planetImgSize = 16, halfSize = 8;

	private float toSystemDiff;

	private Vector2 center = new Vector2(Screen.width - ((mapSize + 20) / 2), (mapSize + 20) / 2);

	private Rect[] orbits;

	private Rect[] planetPositions;

	private float[] distances;

	private StarSystem starSystem;

	public void init (StarSystem starSystem) {
		this.starSystem = starSystem;
		float planetDistanceMax = 0;
		PlanetType[] types = Vars.starSystemType.getPlanetTypes();
		distances = new float[types.Length];

		foreach (PlanetType type in types) {
			planetDistanceMax = Mathf.Max(planetDistanceMax, type.getDistanceToStar());
		}

		for (int c = 0; c < types.Length; c++) {
			distances[c] = (types[c].getDistanceToStar() / planetDistanceMax) * (mapSize / 2);
		}

		toSystemDiff = mapSize / 2 / planetDistanceMax;

		Debug.Log(toSystemDiff);
		orbits = new Rect[types.Length];
		planetPositions = new Rect[orbits.Length];

		float diff;
		for (int i = 0; i < orbits.Length; i++) {
			diff = mapSize * (types[i].getDistanceToStar() / planetDistanceMax);
			orbits[i] = new Rect(Screen.width - diff + diff/2 - mapSize/2 - 20, (mapSize - diff/2 - mapSize/2) + 20, diff, diff);
			planetPositions[i] = new Rect(0, 0, planetImgSize, planetImgSize);
		}
	}

	void OnGUI () {
		if (!UserInterface.showInterface) { return; }

		for (int i = 0; i < orbits.Length; i++) {
			GUI.DrawTexture(orbits[i], planetOrbit);
			GUI.DrawTexture(calcPlanetPos(i), planet);
		}
	}

	private Rect calcPlanetPos (int index) {
		Rect tempR = planetPositions[index];
//		tempR.x = center.x - 16 + (distances[index] * Mathf.Cos(starSystem.getPlanets()[index].rotationAngle));
//		tempR.y = center.y + (distances[index] * Mathf.Sin(starSystem.getPlanets()[index].rotationAngle));
		Vector3 temp = starSystem.getPlanets()[index].getPosition();
		tempR.x = center.x + (temp.x * toSystemDiff) - planetImgSize;
		tempR.y = center.y - (temp.y * toSystemDiff);
		return tempR;
	}
}