using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class StarSystem : MonoBehaviour {

	public Transform itemPrefab;

	public Transform planetPrefab, playerShipPrefab;

	public static bool gamePaused {  get; private set;}
    
	private static StatusScreen statusScreen;

	private SpriteRenderer star;

	private Transform mainCamera;

	private PlayerShip playerShip;

	private CameraController cameraController;

    private List<Planet> planets;

	private ShieldsPool shieldsPool;

	private StarField starField;

	private EnemySpawner spawner;

	//Выведеное опытным путем расстояние от центра до края сектора
	//при условии что разрешение картинки сектора 4096х4096, pixelsToUnit = 50, sectorMoveSpeed = 0.1
//	private float sectorHalfSide = 409.1f;

	//Тот же опытный путь - высота картинки сектора
//	private float sectorImageHeight = 81.9f;

	void Awake () {
		if (starField == null) { init(); }
	}

	private void init () {
		ItemFactory.itemPrefab = itemPrefab;
		mainCamera = Camera.main.transform;

		Imager.initialize();
		Player.init();

		GameObject.Find("Images Provider").GetComponent<ImagesProvider>().init();

		starField = GameObject.Find("StarField").GetComponent<StarField>().init();
        star = transform.Find("Star").GetComponent<SpriteRenderer>();
        star.gameObject.SetActive(true);
        
		ItemDescriptor descriptor = GameObject.Find("Item Descriptor").GetComponent<ItemDescriptor>().init();

		statusScreen = GameObject.Find("Status Screen").GetComponent<StatusScreen>().init(this, descriptor);

		if (Vars.shipCurrentHealth == -1) {
			statusScreen.getShipData().initializeRandomShip (HullType.Corvette);
		} else {
			statusScreen.initFromVars();
		}
		
		initPlayerShip ();

		statusScreen.cameraController = cameraController;

		Vars.userInterface = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().init(statusScreen, this, playerShip);

		shieldsPool = GameObject.Find("ShieldsPool").GetComponent<ShieldsPool>();

		GameObject.Find("Explosions Manager").GetComponent<ExplosionsManager>().init();

		spawner = GetComponent<EnemySpawner>().init(Vars.userInterface.minimap, playerShip.transform);

        loadStarSystem();

		gamePaused = false;
	}

    private void loadStarSystem () {
        PlanetType[] types = Vars.starSystemType.getPlanetTypes();
        planets = new List<Planet>(types.Length);
        foreach (PlanetType type in types) {
            planets.Add(Instantiate<Transform>(planetPrefab).GetComponent<Planet>().init(type, playerShip.transform));
        }
		starField.initStarField();
        star.sprite = Imager.getStar(Vars.starSystemType);
		foreach (Planet planet in planets) {
			if (planet.getPlanetType() == Vars.planetType) {
				Vector3 shipPos = new Vector3(planet.transform.position.x, planet.transform.position.y, StarField.zOffset);
				playerShip.transform.position =  shipPos;
				cameraController.setDirectlyToShip();
				break;
			}
		}

		statusScreen.getShipData().setShieldToMax();

		spawner.spawnAnEnemy(2);
    }

	private void initPlayerShip () {
		playerShip = Instantiate<Transform> (playerShipPrefab).GetComponent<PlayerShip> ();
		playerShip.initPlayerShip(statusScreen.getShipData());
		cameraController = mainCamera.GetComponent<CameraController>();
		cameraController.init(playerShip.transform, starField);
	}

	public void landOnPlanet (PlanetType planetType) {
		gamePaused = true;
		shieldsPool.clearPool();
		Vars.enemyShipsPool.Clear();
		Vars.userInterface.setEnabled(false);
		statusScreen.sendToVars();
		Vars.planetType = planetType;
		ExplosionsManager.clear();
		SceneManager.LoadScene("PlanetSurface");
	}

	public List<Planet> getPlanets () {
		return planets;
	}

	public static void setGamePause (bool paused) {
        if (!paused && (statusScreen != null && statusScreen.gameObject.activeInHierarchy)) { return; }
		gamePaused = paused;
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