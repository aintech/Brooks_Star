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
		InventoryItem engineItem = shipData.getSlotByName ("Engine Slot").getItem ();
		InventoryItem.EngineData data = (InventoryItem.EngineData)engineItem.getItemData ();
		engine.setEngine (data.getType(), data.getPower(), data.getType().getRotatePower());
		setEngineSprite();
	}

	private void initWeapons () {
		HullSlot slot1 = shipData.getSlotByName("Weapon Slot 1");
		HullSlot slot2 = shipData.getSlotByName("Weapon Slot 2");
		HullSlot slot3 = shipData.getSlotByName("Weapon Slot 3");
		HullSlot slot4 = shipData.getSlotByName("Weapon Slot 4");
		HullSlot slot5 = shipData.getSlotByName("Weapon Slot 5");

		InventoryItem.WeaponData slot1WeaponData = slot1.getItem() == null? null: (InventoryItem.WeaponData)slot1.getItem().getItemData();
		InventoryItem.WeaponData slot2WeaponData = slot2.getItem() == null? null: (InventoryItem.WeaponData)slot2.getItem().getItemData();
		InventoryItem.WeaponData slot3WeaponData = slot3.getItem() == null? null: (InventoryItem.WeaponData)slot3.getItem().getItemData();
		InventoryItem.WeaponData slot4WeaponData = slot4.getItem() == null? null: (InventoryItem.WeaponData)slot4.getItem().getItemData();
		InventoryItem.WeaponData slot5WeaponData = slot5.getItem() == null? null: (InventoryItem.WeaponData)slot5.getItem().getItemData();

		if (slot1WeaponData != null) initWeapon (slot1WeaponData, 1);
		if (slot2WeaponData != null) initWeapon (slot2WeaponData, 2);
		if (slot3WeaponData != null) initWeapon (slot3WeaponData, 3);
		if (slot4WeaponData != null) initWeapon (slot4WeaponData, 4);
		if (slot5WeaponData != null) initWeapon (slot5WeaponData, 5);
	}
	
	private void initWeapon(InventoryItem.WeaponData data, int weaponIndex) {
		Weapon weapon = null;
		
		switch (data.getType()) {
			case WeaponType.Blaster: weapon = Instantiate<Transform>(blasterPrefab).GetComponent<Blaster>(); break;
			case WeaponType.Plasmer: weapon = Instantiate<Transform>(plasmerPrefab).GetComponent<Plasmer>(); break;
			case WeaponType.Charger: weapon = Instantiate<Transform>(chargerPrefab).GetComponent<Charger>(); break;
			case WeaponType.Emitter: weapon = Instantiate<Transform>(emitterPrefab).GetComponent<Emitter>(); break;
			case WeaponType.Waver: weapon = Instantiate<Transform>(waverPrefab).GetComponent<Waver>(); break;
			case WeaponType.Launcher: weapon = Instantiate<Transform>(launcherPrefab).GetComponent<Launcher>(); break;
			case WeaponType.Suppressor: weapon = Instantiate<Transform>(suppressorPrefab).GetComponent<Suppressor>(); break;
		}
		
		weapon.setWeaponType(data.getType());
		weapon.setDamage(data.getMinDamage(), data.getMaxDamage());
		weapon.setReloadTime(data.getReloadTime());
		weapon.setAsPlayerWeapon();
		
		weapon.transform.SetParent(transform);
		
		Vector3 posit = getWeaponPosition(weaponIndex);
		weapon.transform.position = new Vector3(transform.position.x + posit.x, transform.position.y + posit.y);
		
		switch (weaponIndex) {
			case 1: weapon_1 = weapon; break;
			case 2: weapon_2 = weapon; break;
			case 3: weapon_3 = weapon; break;
			case 4: weapon_4 = weapon; break;
			case 5: weapon_5 = weapon; break;
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