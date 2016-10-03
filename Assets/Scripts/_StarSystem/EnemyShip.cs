using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyShip : Ship {

	public Transform healthBarPrefab;

	private Transform barTrans, shieldBar, healthBar;

	private static List<Vector2> shipLevels = new List<Vector2>();

	private Vector3 barOffset = new Vector3(1, 1);

	private Transform trans, playerShip;

	private Vector3 shieldValue = Vector3.one, healthValue = Vector3.one;
	
	private int sortingOrder = -1;
	
	private bool alive = true;

	private float distanceToPlayer;

	public void initRandomShip (int shipLevel, Transform playerShip) {
		initInner();

		if (sortingOrder == -1) {
			sortingOrder = Vars.freeSortingOrder;
		}

		if (shipLevels.Count == 0) {
			shipLevels.Add(new Vector2(0, 2));
			shipLevels.Add(new Vector2(3, 5));
			shipLevels.Add(new Vector2(6, 9));
			shipLevels.Add(new Vector2(10, 13));
			shipLevels.Add(new Vector2(14, 15));
			shipLevels.Add(new Vector2(16, 16));
		}

		this.playerShip = playerShip;

		int rand = (int) UnityEngine.Random.Range (shipLevels[shipLevel].x, shipLevels[shipLevel].y+1);

		setHullType (rand == 0? HullType.Little: rand == 1? HullType.Needle: rand == 2? HullType.Gnome:
		             rand == 3? HullType.Cricket: rand == 4? HullType.Argo: rand == 5? HullType.Falcon:
		             rand == 6? HullType.Adventurer: rand == 7? HullType.Corvette: rand == 8? HullType.Buffalo:
		             rand == 9? HullType.Legionnaire: rand == 10? HullType.StarWalker: rand == 11? HullType.Warship:
		             rand == 12? HullType.Asterix: rand == 13? HullType.Prime: rand == 14? HullType.Titan:
		             rand == 15? HullType.Dreadnaut: HullType.Armageddon);

		health = fullHealth = getHullType().getMaxHealth();
		shield = fullShield = 300;
		initArmor(shipLevel);
		initRadarRange(shipLevel);
		initEngine ();
		initWeapons ();
		initHealthBar ();

		trans = transform;
		getEngineRender().sortingOrder = sortingOrder;
		getHullRender().sortingOrder = sortingOrder + 1;
		shieldRenderOrder = sortingOrder + 3;
		Vars.freeSortingOrder += getHullType().getWeaponSlots() > 0? 4: 3;
		alive = true;
	}

	private void initArmor (int shipLevel) {
		armor = shipLevel * 3;
	}

	private void initRadarRange (int shipLevel) {
		switch (shipLevel) {
			case 0: radarRange = RadarType.Sequester.getRange(); break;
			case 1: radarRange = RadarType.Planar.getRange(); break;
			case 2: radarRange = RadarType.Matrix.getRange(); break;
			case 3: radarRange = RadarType.PatanCorsac.getRange(); break;
			case 4: radarRange = RadarType.Snake.getRange(); break;
			case 5: radarRange = RadarType.Astral.getRange(); break;
			default: Debug.Log("Неизвестный уровень корабля"); break;
		}
	}

	private void initEngine () {
		int rand = UnityEngine.Random.Range(0, Enum.GetNames(typeof(EngineType)).Length);
		EngineType eType = 	rand == 0? EngineType.Force:
						   	rand == 1? EngineType.Gradual:
						   	rand == 2? EngineType.Proton:
							rand == 3? EngineType.Allur: EngineType.Quazar;
		engine.setEngine(eType, eType.getMainPower(), eType.getRotatePower());
		setEngineSprite ();
	}

	private void initWeapons () {
		int availSlots = getHullType().getWeaponSlots();
		if (availSlots >= 1) initWeapon(0);
		if (availSlots >= 2) initWeapon(1);
		if (availSlots >= 3) initWeapon(2);
		if (availSlots >= 4) initWeapon(3);
		if (availSlots >= 5) initWeapon(4);
	}

	private void initWeapon (int weaponIndex) {
		Weapon weapon = null;

//		int rand = UnityEngine.Random.Range(0, Enum.GetNames(typeof(WeaponType)).Length);

		WeaponType type = WeaponType.Blaster;//rand == 0? WeaponType.Blaster:
						  //rand == 1? WeaponType.Plasmer:
						  //rand == 2? WeaponType.Charger:
						  //rand == 3? WeaponType.Emitter:
						  //rand == 4? WeaponType.Waver:
					  	  //rand == 5? WeaponType.Launcher:
						  //WeaponType.Suppressor;

		switch (type) {
			case WeaponType.Blaster: weapon = Instantiate<Transform>(blasterPrefab).GetComponent<Blaster>(); break;
			case WeaponType.Plasmer: weapon = Instantiate<Transform>(plasmerPrefab).GetComponent<Plasmer>(); break;
			case WeaponType.Charger: weapon = Instantiate<Transform>(chargerPrefab).GetComponent<Charger>(); break;
			case WeaponType.Emitter: weapon = Instantiate<Transform>(emitterPrefab).GetComponent<Emitter>(); break;
			case WeaponType.Waver: weapon = Instantiate<Transform>(waverPrefab).GetComponent<Waver>(); break;
			case WeaponType.Launcher: weapon = Instantiate<Transform>(launcherPrefab).GetComponent<Launcher>(); break;
			case WeaponType.Suppressor: weapon = Instantiate<Transform>(suppressorPrefab).GetComponent<Suppressor>(); break;
		}

		weapon.setWeaponType(type);
		weapon.setDamage(type.getDamage() - 3, type.getDamage() + 3);
		weapon.setReloadTime(type.getReloadTime());
		weapon.setPlayerTransform(playerShip);

		weapon.transform.SetParent(transform);
		
		Vector3 posit = getWeaponPosition(weaponIndex);
		weapon.transform.position = new Vector3(transform.position.x + posit.x, transform.position.y + posit.y, posit.z);

		weapon.getRender().sortingOrder = sortingOrder + 2;

		switch (weaponIndex) {
			case 0: weapon_1 = weapon; break;
			case 1: weapon_2 = weapon; break;
			case 2: weapon_3 = weapon; break;
			case 3: weapon_4 = weapon; break;
			case 4: weapon_5 = weapon; break;
		}
	}

	private void initHealthBar () {
		barTrans = Instantiate<Transform>(healthBarPrefab);
		shieldBar = barTrans.FindChild("Shield");
		healthBar = barTrans.FindChild("Health");

		barTrans.position = transform.position + barOffset;
	}

	void FixedUpdate () {
		decideNextMove();
		arrangeBarToShip();
	}

	private void decideNextMove () {
		distanceToPlayer = Vector2.Distance(trans.position, playerShip.position);

		if (distanceToPlayer <= radarRange) {
			float maxRange = 0;
			if (weapon_1 != null && weapon_1.getWeaponType().getRange() > maxRange) maxRange = weapon_1.getWeaponType().getRange();
			if (weapon_2 != null && weapon_2.getWeaponType().getRange() > maxRange) maxRange = weapon_2.getWeaponType().getRange();
			if (weapon_3 != null && weapon_3.getWeaponType().getRange() > maxRange) maxRange = weapon_3.getWeaponType().getRange();
			if (weapon_4 != null && weapon_4.getWeaponType().getRange() > maxRange) maxRange = weapon_4.getWeaponType().getRange();
			if (weapon_5 != null && weapon_5.getWeaponType().getRange() > maxRange) maxRange = weapon_5.getWeaponType().getRange();

			if (maxRange == 0) {
				//У корабля нет оружия
			} else if (distanceToPlayer > maxRange) {
				moveCloserToPlayer();
			}
		}
	}

	private void moveCloserToPlayer () {
		//TODO: Lerp-ом поворачиваемся к игроку и двигаемся в его сторону
	}

	override protected void updateHealthAndShieldInfo () {
		shieldValue.x = (float)getShield() / getFullShield();
		healthValue.x = (float)getHealth() / getFullHealth();

		shieldBar.transform.localScale = shieldValue;
		healthBar.transform.localScale = healthValue;
	}

	override protected void destroyShip () {
		alive = false;
		barTrans.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}

	private void moveShip () {
		//Vector3 transPos = transform.position;
		//transform.position = new Vector3(transPos.x, transPos.y + 0.1f, transPos.z);
	}

	private void arrangeBarToShip () {
		barTrans.position = trans.position + barOffset;
	}

	public bool isAlive () {
		return alive;
	}

	public override bool isPlayerShip () {
		return false;
	}
}