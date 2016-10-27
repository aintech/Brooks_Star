using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Minimap : MonoBehaviour {

	public GUIStyle radarBtnStyle, systemBtnStyle, showBtnStyle, hideBtnStyle, galaxyBtnStyle;

	public Texture bg, planetOrbit, planet, planetColonized, planetPopulated, player, enemy, footer, radarBorder;

	private const float mapSize = 200;

	private const float imgSize = 16, halfSize = 8, mapOffset = 20, planetSize = 8, planetHalf = 4;

	private float toSystemDiff, radarDiff;

	private Vector2 center = new Vector2(Screen.width - (mapSize / 2 + mapOffset), mapSize / 2 + mapOffset);

	private Vector2 tempVec;

	private Rect[] orbits;

	private Rect[] planetPositions;

	private float[] distances;

	private Rect 	playerRect = new Rect(0, 0, imgSize, imgSize),
					planetRect = new Rect(0, 0, planetSize, planetSize),
				 	footerRect = new Rect(Screen.width - 230, 30, 230, 200),
					showBtnRect = new Rect(Screen.width - 32, 2, 32, 32),
					hideBtnRect = new Rect(Screen.width - 32, 231, 32, 32),
					galaxyMapBtnRectUp = new Rect(Screen.width - 32 - 233, 2, 232, 32),
					galaxyMapBtnRectDown = new Rect(Screen.width - 233, 2, 232, 32),//new Rect(Screen.width - 32 - 78, 231, 204, 32),
					systemRadarBtnRect = new Rect(Screen.width - 32 - 102, 231, 101, 32),
					radarBorderRect = new Rect(Screen.width - mapSize - mapOffset, mapOffset, mapSize, mapSize);

	private List<Rect> enemyPositions = new List<Rect>();

	private List<Transform> enemies = new List<Transform>();

	private StarSystem starSystem;

	private Transform playerShip;

	private bool onScreen = true;

	private MapType mapType = MapType.SYSTEM;

	private float radarRange;

	private float tempDist;

	private Rect currPlanetPosition;

	private Planet currPlanet;

	private Texture currPlanetText;

	private GalaxyMap galaxyMap;

	public void init (StarSystem starSystem, GalaxyMap galaxyMap, Transform playerShip, float radarRange) {
		this.starSystem = starSystem;
		this.galaxyMap = galaxyMap;
		this.playerShip = playerShip;
		this.radarRange = radarRange;
//		loadSystem();
	}

	public void loadSystem () {
		float planetDistanceMax = 0;
		PlanetType[] types = Vars.starSystemType.planetTypes();
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
			planetPositions[i] = new Rect(planetRect);
		}

		radarDiff = mapSize / 2 / radarRange;

		enemies.Clear();
		enemyPositions.Clear();
	}

	void OnGUI () {
		if (!UserInterface.showInterface) { return; }

		if (onScreen) {
			GUI.DrawTexture(footerRect, footer);
			if (mapType == MapType.SYSTEM) {
				for (int i = 0; i < orbits.Length; i++) {
					GUI.DrawTexture(orbits[i], planetOrbit);
					calcPlanetPos(i);
					GUI.DrawTexture(currPlanetPosition, currPlanetText);
				}
				playerRect.x = center.x - halfSize + (playerShip.position.x * toSystemDiff);
				playerRect.y = center.y - halfSize - (playerShip.position.y * toSystemDiff);
				GUI.DrawTexture(playerRect, player);

				if(GUI.Button(systemRadarBtnRect, "", systemBtnStyle)) {
					changeMapType(MapType.RADAR);
				}
			} else {
				GUI.DrawTexture(radarBorderRect, radarBorder);
				calcEnemyPositions();
				foreach (Rect pos in enemyPositions) {
					GUI.DrawTexture(pos, enemy);
				}
				if (GUI.Button(systemRadarBtnRect, "", radarBtnStyle)) {
					changeMapType(MapType.SYSTEM);
				}
			}
			if (GUI.Button(hideBtnRect, "", hideBtnStyle)) {
				showMap(false);
			}
			if (GUI.Button(galaxyMapBtnRectDown, "", galaxyBtnStyle)) {
				showGalaxyMap();
			}
		} else {
			if (GUI.Button(showBtnRect, "", showBtnStyle)) {
				showMap(true);
			}
			if (GUI.Button(galaxyMapBtnRectUp, "", galaxyBtnStyle)) {
				showGalaxyMap();
			}
		}
	}

	private void showGalaxyMap () {
		galaxyMap.show();
	}

	private void changeMapType (MapType mapType) {
		this.mapType = mapType;
	}

	private void showMap (bool show) {
		onScreen = show;
	}

	private void calcEnemyPositions () {
		Rect rect = new Rect();
		Transform trans;
		Vector2 pos;
		for (int i = 0; i < enemies.Count; i++) {
			trans = enemies[i];
			rect = enemyPositions[i];
			tempDist = Vector2.Distance(trans.position, playerShip.position);
			if (tempDist > radarRange) {
				rect.x = -10000;
				rect.y = -10000;
			} else {
				pos = playerShip.transform.position - trans.position;
				rect.x = center.x - halfSize - (pos.x * radarDiff);
				rect.y = center.y - halfSize + (pos.y * radarDiff);
			}
			enemyPositions[i] = rect;
		}
	}

	private void calcPlanetPos (int index) {
		currPlanetPosition = planetPositions[index];
		currPlanet = starSystem.getPlanets()[index];
		currPlanetText = currPlanet.isColonized? planetColonized: currPlanet.isPopulated? planetPopulated: planet;
		tempVec = starSystem.getPlanets()[index].getPosition();
		currPlanetPosition.x = center.x - planetHalf + (tempVec.x * toSystemDiff);
		currPlanetPosition.y = center.y - planetHalf - (tempVec.y * toSystemDiff);
	}

	public void addEnemy (Transform enemy) {
		enemies.Add(enemy);
		enemyPositions.Add(new Rect(0, 0, imgSize, imgSize));
	}

	public void removeEnemy (Transform enemy) {
		enemies.Remove(enemy);
		enemyPositions.RemoveAt(0);
	}

	private enum MapType {
		RADAR, SYSTEM
	}
}