using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : Ship {

	private ShipData shipData;

	private ShipController controller;

	public void initPlayerShip (ShipData shipData) {
		this.shipData = shipData;
		this.name = "Player ship";
		initInner();
		controller = transform.GetComponent<ShipController>();
		setHullType (shipData.getHullType());
		health = shipData.getCurrentHealth ();
		fullHealth = shipData.getHullType().getMaxHealth();
		shield = fullShield = shipData.getShield ();
		armor = shipData.getArmor ();
		radarRange = shipData.getRadarRange();
		initEngine ();
		initWeapons ();
		controller.initController(this);
	}
	
	private void initEngine () {
		Item engineItem = shipData.getSlot (HullSlot.Type.ENGINE, 0).item;
		EngineData data = (EngineData)engineItem.itemData;
		engine.setEngine (data.type, data.power, data.type.getRotatePower());
		setEngineSprite();
	}

	private void initWeapons () {
		HullSlot slot1 = shipData.getSlot(HullSlot.Type.WEAPON, 0);
		HullSlot slot2 = shipData.getSlot(HullSlot.Type.WEAPON, 1);
        HullSlot slot3 = shipData.getSlot(HullSlot.Type.WEAPON, 2);
        HullSlot slot4 = shipData.getSlot(HullSlot.Type.WEAPON, 3);
        HullSlot slot5 = shipData.getSlot(HullSlot.Type.WEAPON, 4);

        WeaponData slot1WeaponData = slot1.item == null? null: (WeaponData)slot1.item.itemData;
		WeaponData slot2WeaponData = slot2.item == null? null: (WeaponData)slot2.item.itemData;
		WeaponData slot3WeaponData = slot3.item == null? null: (WeaponData)slot3.item.itemData;
		WeaponData slot4WeaponData = slot4.item == null? null: (WeaponData)slot4.item.itemData;
		WeaponData slot5WeaponData = slot5.item == null? null: (WeaponData)slot5.item.itemData;

		if (slot1WeaponData != null) initWeapon (slot1WeaponData, 0);
		if (slot2WeaponData != null) initWeapon (slot2WeaponData, 1);
		if (slot3WeaponData != null) initWeapon (slot3WeaponData, 2);
		if (slot4WeaponData != null) initWeapon (slot4WeaponData, 3);
		if (slot5WeaponData != null) initWeapon (slot5WeaponData, 4);
	}
	
	private void initWeapon(WeaponData data, int weaponIndex) {
		Weapon weapon = null;
		
		switch (data.type) {
			case WeaponType.Blaster: weapon = Instantiate<Transform>(blasterPrefab).GetComponent<Blaster>(); break;
			case WeaponType.Plasmer: weapon = Instantiate<Transform>(plasmerPrefab).GetComponent<Plasmer>(); break;
			case WeaponType.Charger: weapon = Instantiate<Transform>(chargerPrefab).GetComponent<Charger>(); break;
			case WeaponType.Emitter: weapon = Instantiate<Transform>(emitterPrefab).GetComponent<Emitter>(); break;
			case WeaponType.Waver: weapon = Instantiate<Transform>(waverPrefab).GetComponent<Waver>(); break;
			case WeaponType.Launcher: weapon = Instantiate<Transform>(launcherPrefab).GetComponent<Launcher>(); break;
			case WeaponType.Suppressor: weapon = Instantiate<Transform>(suppressorPrefab).GetComponent<Suppressor>(); break;
		}
		
		weapon.setWeaponType(data.type);
		weapon.setDamage(data.minDamage, data.maxDamage);
		weapon.setReloadTime(data.reloadTime);
		weapon.setAsPlayerWeapon();
		
		weapon.transform.SetParent(transform);
		
		Vector3 posit = getWeaponPosition(weaponIndex);
		weapon.transform.position = new Vector3(transform.position.x + posit.x, transform.position.y + posit.y);
		
		switch (weaponIndex) {
			case 0: weapon_1 = weapon; break;
			case 1: weapon_2 = weapon; break;
			case 2: weapon_3 = weapon; break;
			case 3: weapon_4 = weapon; break;
			case 4: weapon_5 = weapon; break;
		}
	}

	protected override void updateHealthAndShieldInfo () {
		shipData.setCurrentShield(getShield());
		shipData.setCurrentHealth(getHealth());
		Vars.userInterface.updateShip();
	}

	public Engine getEngine () {
		return engine;
	}

	public ShipController getController () {
		return controller;
	}

	public override bool isPlayerShip () {
		return true;
	}
}