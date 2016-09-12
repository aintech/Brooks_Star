using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceTravel : MonoBehaviour {

	public Transform sectorPrefab, playerShipPrefab, enemyShipPrefab;

	public Texture inventoryBtnTexture;

	public GUIStyle shieldStyle1, shieldStyle2, healthStyle1, healthStyle2;

	private ShipInformationScreen shipInformation;

	private Rect inventoryBtnRect = new Rect (10, 10, 50, 50);

	private Transform mainCamera;

	private PlayerShip playerShip;

	private Inventory inventory;

	private ShipData shipData;

	private int currentSectorNumber;

	private Transform currentSectorTrans;

	private Sector currentSector;
	
	private Sector[] localSectors = new Sector[9];

	private CameraController cameraController;

	private CircleCollider2D closeBtn;

	private RaycastHit2D hit;

	private Vector3 mouseToWorldPosition;

	private Rect healthRect = new Rect(Screen.width - 70, Screen.height - 20, 0, 0), 
				 shieldRect = new Rect(Screen.width - 190, Screen.height - 20, 0, 0);

	//Выведеное опытным путем расстояние от центра до края сектора
	//при условии что разрешение картинки сектора 4096х4096, pixelsToUnit = 50, sectorMoveSpeed = 0.1
	private float sectorHalfSide = 409.1f;

	//Тот же опытный путь - высота картинки сектора
	private float sectorImageHeight = 81.9f;

	void Awake () {
		if (currentSectorTrans == null) {
			currentSectorTrans = Instantiate(sectorPrefab) as Transform;
			currentSectorTrans.name = "Center Sector";
			currentSector = currentSectorTrans.GetComponent<Sector>();
			init();
		}
	}

	private void init () {
		mainCamera = Camera.main.transform;

		shipInformation = GameObject.Find ("Ship Information").GetComponent<ShipInformationScreen> ();
		closeBtn = shipInformation.transform.FindChild ("CloseBtn").GetComponent<CircleCollider2D> ();
		inventory = shipInformation.transform.FindChild ("Inventory").GetComponent<Inventory> ();
		shipData = shipInformation.transform.FindChild ("Ship Data").GetComponent<ShipData> ();

		if (Vars.shipCurrentHealth == -1) {
			shipData.initializeRandomShip (HullType.Corvette);
		} else {
			shipData.initializeFromVars ();
		}
		
		initPlayerShip ();

		inventory.setCapacity (shipData.getHullType ().getStorageCapacity ());
		inventory.loadItems (Vars.inventory);

		initSectors();

		hideShipInformation();

		//for (int i = 0; i <= 10; i++) {
			spawnAnEnemy ();
		//}
	}

	private void initPlayerShip () {
		playerShip = Instantiate<Transform> (playerShipPrefab).GetComponent<PlayerShip> ();
		playerShip.initPlayerShip(shipData);
		cameraController = mainCamera.GetComponent<CameraController>();
		cameraController.init(playerShip.transform);
	}

	private void initSectors () {
		currentSector.initSector(22, 0, 0);
		checkSectorWithCamera();
		currentSector.setBackgroundImage(currentSectorNumber);
		localSectors[4] = currentSector;
		initSectorsAround();
	}

	private void initSectorsAround () {
		for (int i = 0; i < localSectors.Length; i++) {
			if (i == 4) continue;
			Transform nearSector = Instantiate(sectorPrefab) as Transform;
			Sector sector = nearSector.GetComponent<Sector>();
			localSectors[i] = sector;
			int sectorLocalNum = 0;
			int sectorNum = 0;
			float moveOffsetX = 0;
			float moveOffsetY = 0;
			string sectorName = "";
			switch (i) {
				case 0: 
					sectorLocalNum = 11; 
					sectorNum = currentSectorNumber - 11;
					moveOffsetX = sectorImageHeight * -1;
					moveOffsetY = sectorImageHeight * -1;
					sectorName = "Lower Left Sector";
				break;
				case 1: 
					sectorLocalNum = 12;
					sectorNum = currentSectorNumber - 10;
					moveOffsetY = sectorImageHeight * -1;
					sectorName = "Lower Center Sector";
				break;
				case 2: 
					sectorLocalNum = 13;
					sectorNum = currentSectorNumber - 9;
					moveOffsetX = sectorImageHeight;
					moveOffsetY = sectorImageHeight * -1;
					sectorName = "Lower Right Sector";
				break;
				case 3: 
					sectorLocalNum = 21;
					sectorNum = currentSectorNumber - 1;
					moveOffsetX = sectorImageHeight * -1;
					sectorName = "Center Left Sector";
				break;
				case 5: 
					sectorLocalNum = 23;
					sectorNum = currentSectorNumber + 1;
					moveOffsetX = sectorImageHeight;
					sectorName = "Center Right Sector";
				break;
				case 6: 
					sectorLocalNum = 31;
					sectorNum = currentSectorNumber + 9;
					moveOffsetX = sectorImageHeight * -1;
					moveOffsetY = sectorImageHeight;
					sectorName = "Upper Left Sector";
				break;
				case 7: 
					sectorLocalNum = 32;
					sectorNum = currentSectorNumber + 10;
					moveOffsetY = sectorImageHeight;
					sectorName = "Upper Center Sector";
				break;
				case 8: 
					sectorLocalNum = 33;
					sectorNum = currentSectorNumber + 11;
					moveOffsetX = sectorImageHeight;
					moveOffsetY = sectorImageHeight;
					sectorName = "Upper Right Sector";
				break;
			}
			sector.initSector(sectorLocalNum, moveOffsetX, moveOffsetY);
			sector.setBackgroundImage(sectorNum);
			nearSector.name = sectorName;
		}
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.I)) {
			if (shipInformation.gameObject.activeInHierarchy) {
				hideShipInformation();
			} else {
				showShipInformation();
			}
		}
		if (Input.GetMouseButtonDown (0)) {
			mouseToWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			hit = Physics2D.Raycast(mouseToWorldPosition, Vector2.zero, 1);
			if (hit.collider != null) {
				if (hit.collider == closeBtn) {
					hideShipInformation();
				}
			}	
		}
	}

	void FixedUpdate () {
		//checkSectorWithCamera();
	}
	
	void OnGUI () {
		if (GUI.Button (inventoryBtnRect, inventoryBtnTexture)) {
			if (shipInformation.gameObject.activeInHierarchy) {
				hideShipInformation();
			} else {
				showShipInformation();
			}
		}

		GUI.Label(shieldRect, playerShip.getShield().ToString(), shieldStyle1);
		GUI.Label(shieldRect, "/" + playerShip.getFullShield().ToString(), shieldStyle2);
		GUI.Label(healthRect, playerShip.getHealth().ToString(), healthStyle1);
		GUI.Label(healthRect, "/" + playerShip.getFullHealth().ToString(), healthStyle2);
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

	//Возвращает порядковый номер сектора от 11 до 55 (в котором находится камера)
	//Всего секторов 25, их порядок:
	//	51 52 53 54 55
	//	41 42 43 44 45
	//	31 32 33 34 35
	//	21 22 23 24 25
	//	11 12 13 14 15
	private void checkSectorWithCamera () {
		//порядковый номер сектора от левого края
		int fromLeft = (int)(mainCamera.position.x / sectorHalfSide) + 3;
		//порядковый номер сектора от нижнего края
		int fromBottom = (int)(mainCamera.position.y / sectorHalfSide) + 3;
		
		currentSectorNumber = (fromLeft * 10) + fromBottom;
	}

	private void showShipInformation () {
		shipInformation.showScreen (inventory, shipData);
		setGamePaused(true);
		Camera.main.orthographicSize = 5;
	}

	private void hideShipInformation () {
		shipInformation.hideScreen ();
		setGamePaused(false);
	}

	private void setGamePaused (bool gamePaused) {
		cameraController.setGamePaused(gamePaused);
		playerShip.setGamePaused(gamePaused);
	}
}
