using UnityEngine;
using System;
using System.Collections;

public class Minimap : MonoBehaviour {

	public GUIStyle radarBtnStyle, systemBtnStyle, showHideBtnStyle;

	public Texture bg, planetOrbit, planet, player, footer, radarBorder;

	private const float mapSize = 200;

	private const float planetImgSize = 16, halfSize = 8, mapOffset = 20;

	private float toSystemDiff;

	private Vector2 center = new Vector2(Screen.width - (mapSize / 2 + mapOffset), mapSize / 2 + mapOffset),
					tempVec;

	private Rect[] orbits;

	private Rect[] planetPositions;

	private float[] distances;

	private Rect 	playerRect = new Rect(0, 0, planetImgSize, planetImgSize),
				 	footerRect = new Rect(Screen.width - 230, 30, 230, 200),
					showBtnRect = new Rect(Screen.width - 32, 2, 32, 32),
					hideBtnRect = new Rect(Screen.width - 32, 231, 32, 32),
					systemRadarBtnRect = new Rect(Screen.width - 32 - 132, 231, 131, 32),
	radarBorderRect = new Rect(Screen.width - mapSize - mapOffset, mapOffset, mapSize, mapSize),
				 	tempRect;

	private StarSystem starSystem;

	private Transform playerShip;

	private bool onScreen = true;

	private MapType mapType = MapType.SYSTEM;

	public void init (StarSystem starSystem, Transform playerShip) {
		this.starSystem = starSystem;
		this.playerShip = playerShip;
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

		orbits = new Rect[types.Length];
		planetPositions = new Rect[orbits.Length];

		float diff;
		for (int i = 0; i < orbits.Length; i++) {
			diff = mapSize * (types[i].getDistanceToStar() / planetDistanceMax);
			orbits[i] = new Rect(center.x - diff/2, center.y - diff/2, diff, diff);//new Rect(Screen.width - diff + diff/2 - mapSize/2 - 20, (mapSize - diff/2 - mapSize/2) + 20, diff, diff);
			planetPositions[i] = new Rect(playerRect);
		}
	}

	void OnGUI () {
		if (!UserInterface.showInterface) { return; }

		if (onScreen) {
			GUI.DrawTexture(footerRect, footer);
			if (mapType == MapType.SYSTEM) {
				for (int i = 0; i < orbits.Length; i++) {
					GUI.DrawTexture(orbits[i], planetOrbit);
					GUI.DrawTexture(calcPlanetPos(i), planet);
				}
				playerRect.x = center.x - halfSize + (playerShip.position.x * toSystemDiff);
				playerRect.y = center.y - halfSize - (playerShip.position.y * toSystemDiff);
				GUI.DrawTexture(playerRect, player);

				if(GUI.Button(systemRadarBtnRect, "", systemBtnStyle)) {
					changeMapType(MapType.RADAR);
				}
			} else {
				GUI.DrawTexture(radarBorderRect, radarBorder);
				if (GUI.Button(systemRadarBtnRect, "", radarBtnStyle)) {
					changeMapType(MapType.SYSTEM);
				}
			}
			if (GUI.Button(hideBtnRect, "", showHideBtnStyle)) {
				showMap(false);
			}
		} else {
			if (GUI.Button(showBtnRect, "", showHideBtnStyle)) {
				showMap(true);
			}
		}
	}

	private void changeMapType (MapType mapType) {
		this.mapType = mapType;
	}

	private void showMap (bool show) {
		onScreen = show;
	}

	private Rect calcPlanetPos (int index) {
		tempRect = planetPositions[index];
		Vector3 temp = starSystem.getPlanets()[index].getPosition();
		tempRect.x = center.x - halfSize + (temp.x * toSystemDiff);
		tempRect.y = center.y - halfSize - (temp.y * toSystemDiff);
		return tempRect;
	}

	private enum MapType {
		RADAR, SYSTEM
	}
}