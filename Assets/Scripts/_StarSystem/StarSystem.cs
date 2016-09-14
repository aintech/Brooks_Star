﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class StarSystem : MonoBehaviour {

	public Transform planetPrefab, playerShipPrefab, enemyShipPrefab;

	public static bool gamePaused;

	private SpriteRenderer backgroundGalaxy, star;

	private Transform mainCamera;

	private PlayerShip playerShip;

	private Inventory inventory;

	private ShipData shipData;

	private CameraController cameraController;

    private List<Planet> planets;

	private ShieldsPool shieldsPool;

	//Выведеное опытным путем расстояние от центра до края сектора
	//при условии что разрешение картинки сектора 4096х4096, pixelsToUnit = 50, sectorMoveSpeed = 0.1
//	private float sectorHalfSide = 409.1f;

	//Тот же опытный путь - высота картинки сектора
//	private float sectorImageHeight = 81.9f;

	void Awake () {
		if (backgroundGalaxy == null) { init(); }
	}

	private void init () {
		mainCamera = Camera.main.transform;

		Imager.initialize();

        backgroundGalaxy = transform.Find("BG").GetComponent<SpriteRenderer>();
		backgroundGalaxy.gameObject.SetActive(true);
        star = transform.Find("Star").GetComponent<SpriteRenderer>();
        star.gameObject.SetActive(true);

		ShipInformationScreen shipInformation = GameObject.Find ("Ship Information").GetComponent<ShipInformationScreen> ();
		inventory = shipInformation.transform.FindChild ("Inventory").GetComponent<Inventory> ();
		shipData = shipInformation.transform.FindChild ("Ship Data").GetComponent<ShipData> ();

		shipData.init();
		inventory.init(Inventory.InventoryType.INVENTORY);

		if (Vars.shipCurrentHealth == -1) {
			shipData.initializeRandomShip (HullType.Corvette);
		} else {
			shipData.initializeFromVars ();
		}
		
		initPlayerShip ();
		shipInformation.init(this, shipData, inventory);

		Vars.userInterface = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().init(this, shipInformation, playerShip);

		inventory.setCapacity (shipData.getHullType ().getStorageCapacity ());
		inventory.loadItems (Vars.inventory);

        loadStarSystem();

		shieldsPool = GameObject.Find("ShieldsPool").GetComponent<ShieldsPool>();

		//for (int i = 0; i <= 10; i++) {
			spawnAnEnemy ();
		//}
		gamePaused = false;
	}

    private void loadStarSystem () {
        PlanetType[] types = Vars.starSystemType.getPlanetTypes();
        planets = new List<Planet>(types.Length);
        foreach (PlanetType type in types) {
            planets.Add(Instantiate<Transform>(planetPrefab).GetComponent<Planet>().init(type, playerShip.transform));
        }
        backgroundGalaxy.sprite = Imager.getStarSystem(Vars.starSystemType);
        star.sprite = Imager.getStar(Vars.starSystemType);
    }

	private void initPlayerShip () {
		playerShip = Instantiate<Transform> (playerShipPrefab).GetComponent<PlayerShip> ();
		playerShip.initPlayerShip(shipData);
		cameraController = mainCamera.GetComponent<CameraController>();
		cameraController.init(playerShip.transform);
	}

	private void spawnAnEnemy () {
		bool found = false;
		EnemyShip enemy = null;
		foreach (EnemyShip ship in Vars.enemyShipsPool) {
			if (!ship.isAlive()) {
				enemy = ship;
				found = true;
				break;
			}
		}
		if (!found) {
			enemy = Instantiate<Transform>(enemyShipPrefab).GetComponent<EnemyShip>();
			Vars.enemyShipsPool.Add(enemy);
		}
		enemy.initRandomShip(Random.Range(0, 6), playerShip.transform);
		enemy.transform.position = new Vector3(Random.Range(-1f, 1f) * 2, Random.Range(-1f, 1f) * 2);
	}

	public void landOnPlanet (PlanetType planetType) {
		gamePaused = true;
		shieldsPool.clearPool();
		Vars.userInterface.setEnabled(false);
		shipData.sendToVars();
		Vars.inventory = inventory.getItems ();
		Vars.planetType = planetType;
		SceneManager.LoadScene("PlanetSurface");
	}

	//Возвращает порядковый номер сектора от 11 до 55 (в котором находится камера)
	//Всего секторов 25, их порядок:
	//	51 52 53 54 55
	//	41 42 43 44 45
	//	31 32 33 34 35
	//	21 22 23 24 25
	//	11 12 13 14 15
//	private void checkSectorWithCamera () {
//		//порядковый номер сектора от левого края
//		int fromLeft = (int)(mainCamera.position.x / sectorHalfSide) + 3;
//		//порядковый номер сектора от нижнего края
//		int fromBottom = (int)(mainCamera.position.y / sectorHalfSide) + 3;
//		
//		currentSectorNumber = (fromLeft * 10) + fromBottom;
//	}
}