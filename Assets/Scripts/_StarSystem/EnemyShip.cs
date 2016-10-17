﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyShip : Ship {

	public Transform healthBarPrefab;

	private Transform barTrans, shieldBar, healthBar;

	private static List<Vector2> shipClasses = new List<Vector2>();

	private Transform playerShip;

	private Vector3 shieldValue = Vector3.one, healthValue = Vector3.one;
	
	private int sortingOrder = -1;

	public void initRandomShip (int shipClass, Transform playerShip) {
		initInner();

		if (sortingOrder == -1) {
			sortingOrder = Vars.freeSortingOrder;
		}

		if (shipClasses.Count == 0) {
			shipClasses.Add(new Vector2(0, 2));
			shipClasses.Add(new Vector2(3, 5));
			shipClasses.Add(new Vector2(6, 9));
			shipClasses.Add(new Vector2(10, 13));
			shipClasses.Add(new Vector2(14, 15));
			shipClasses.Add(new Vector2(16, 16));
		}

		this.playerShip = playerShip;

		int rand = (int) UnityEngine.Random.Range (shipClasses[shipClass].x, shipClasses[shipClass].y + 1);

		setHullType (rand == 0? HullType.Little: rand == 1? HullType.Needle: rand == 2? HullType.Gnome:
		             rand == 3? HullType.Cricket: rand == 4? HullType.Argo: rand == 5? HullType.Falcon:
		             rand == 6? HullType.Adventurer: rand == 7? HullType.Corvette: rand == 8? HullType.Buffalo:
		             rand == 9? HullType.Legionnaire: rand == 10? HullType.StarWalker: rand == 11? HullType.Warship:
		             rand == 12? HullType.Asterix: rand == 13? HullType.Prime: rand == 14? HullType.TITAN:
		             rand == 15? HullType.Dreadnaut: HullType.Armageddon);

		health = fullHealth = getHullType().getMaxHealth();
		shield = fullShield = 300;
		initArmor(shipClass);
		initRadarRange(shipClass);
		initEngine ();
		initWeapons ();
		initHealthBar ();

		getEngineRender().sortingOrder = sortingOrder;
		getHullRender().sortingOrder = sortingOrder + 1;
		shieldRenderOrder = sortingOrder + 3;
		Vars.freeSortingOrder += getHullType().getWeaponSlots() > 0? 4: 3;
		controller.init(this);
		((EnemyShipController)controller).setStuff(playerShip, barTrans, radarRange, new Weapon[]{weapon_1, weapon_2, weapon_3, weapon_4, weapon_5});
		updateHealthAndShieldInfo();
	}

	private void initArmor (int shipLevel) {
		armor = shipLevel * 3;
	}

	private void initRadarRange (int shipLevel) {
		switch (shipLevel) {
			case 0: radarRange = RadarType.SEQUESTER.range(); break;
			case 1: radarRange = RadarType.PLANAR.range(); break;
			case 2: radarRange = RadarType.MATRIX.range(); break;
			case 3: radarRange = RadarType.PATAN_CORSAC.range(); break;
			case 4: radarRange = RadarType.SNAKE.range(); break;
			case 5: radarRange = RadarType.ASTRAL.range(); break;
			default: Debug.Log("Неизвестный уровень корабля"); break;
		}
	}

	private void initEngine () {
		int rand = UnityEngine.Random.Range(0, Enum.GetNames(typeof(EngineType)).Length);
		EngineType eType = 	rand == 0? EngineType.FORCE:
						   	rand == 1? EngineType.GRADUAL:
						   	rand == 2? EngineType.PROTON:
							rand == 3? EngineType.ALLUR: EngineType.QUAZAR;
		engine.setEngine(eType, eType.mainPower(), eType.rotatePower());
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

		WeaponType type = WeaponType.BLASTER;//rand == 0? WeaponType.BLASTER:
						  //rand == 1? WeaponType.PLASMER:
						  //rand == 2? WeaponType.CHARGER:
						  //rand == 3? WeaponType.EMITTER:
						  //rand == 4? WeaponType.WAVER:
					  	  //rand == 5? WeaponType.LAUNCHER:
						  //WeaponType.SUPPRESSOR;

		switch (type) {
			case WeaponType.BLASTER: weapon = Instantiate<Transform>(blasterPrefab).GetComponent<Blaster>(); break;
			case WeaponType.PLASMER: weapon = Instantiate<Transform>(plasmerPrefab).GetComponent<Plasmer>(); break;
			case WeaponType.CHARGER: weapon = Instantiate<Transform>(chargerPrefab).GetComponent<Charger>(); break;
			case WeaponType.EMITTER: weapon = Instantiate<Transform>(emitterPrefab).GetComponent<Emitter>(); break;
			case WeaponType.WAVER: weapon = Instantiate<Transform>(waverPrefab).GetComponent<Waver>(); break;
			case WeaponType.LAUNCHER: weapon = Instantiate<Transform>(launcherPrefab).GetComponent<Launcher>(); break;
			case WeaponType.SUPPRESSOR: weapon = Instantiate<Transform>(suppressorPrefab).GetComponent<Suppressor>(); break;
		}

		weapon.init(this);
		weapon.setWeaponType(type);
		weapon.setDamage(type.damage() - 3, type.damage() + 3);
		weapon.setReloadTime(type.reloadTime());
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
	}

	override protected void updateHealthAndShieldInfo () {
		shieldValue.x = (float)getShield() / getFullShield();
		healthValue.x = (float)getHealth() / getFullHealth();

		shieldBar.transform.localScale = shieldValue;
		healthBar.transform.localScale = healthValue;
	}

	override protected void disableShip () {
		base.disableShip();
		barTrans.gameObject.SetActive(false);
		Vars.userInterface.minimap.removeEnemy(transform);
	}

	public override bool isPlayerShip () {
		return false;
	}
}