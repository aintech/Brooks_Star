﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : Ship {

	private ShipData shipData;

	public GalaxyJumpController jumpController { get; private set; }

	public void initPlayerShip (ShipData shipData, StarSystem starSystem) {
		this.shipData = shipData;
		this.name = "Player ship";
		initInner();
		jumpController = GetComponent<GalaxyJumpController>();
		setHullType (shipData.hullType);
		health = shipData.getCurrentHealth ();
		fullHealth = shipData.hullType.getMaxHealth();
		shield = fullShield = shipData.getShield ();
		armor = shipData.getArmor ();
		radarRange = shipData.getRadarRange();
		initEngine ();
		initWeapons ();
		initShield ();
		initRepair ();
		jumpController.initJumper(starSystem);
		controller.init();
	}

	private void initEngine () {
		Item engineItem = shipData.getSlot (HullSlot.Type.ENGINE, 0).item;
		EngineData data = (EngineData)engineItem.itemData;
		engine.setEngine (data.type, data.power, data.type.rotatePower());
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

		weapons = new Weapon[5];
		weapons[0] = weapon_1;
		weapons[1] = weapon_2;
		weapons[2] = weapon_3;
		weapons[3] = weapon_4;
		weapons[4] = weapon_5;
	}
	
	private void initWeapon(WeaponData data, int weaponIndex) {
		Weapon weapon = null;
		
		switch (data.type) {
			case WeaponType.BLASTER: weapon = Instantiate<Transform>(blasterPrefab).GetComponent<Blaster>(); break;
			case WeaponType.PLASMER: weapon = Instantiate<Transform>(plasmerPrefab).GetComponent<Plasmer>(); break;
			case WeaponType.CHARGER: weapon = Instantiate<Transform>(chargerPrefab).GetComponent<Charger>(); break;
			case WeaponType.EMITTER: weapon = Instantiate<Transform>(emitterPrefab).GetComponent<Emitter>(); break;
			case WeaponType.WAVER: weapon = Instantiate<Transform>(waverPrefab).GetComponent<Waver>(); break;
			case WeaponType.LAUNCHER: weapon = Instantiate<Transform>(launcherPrefab).GetComponent<Launcher>(); break;
			case WeaponType.SUPPRESSOR: weapon = Instantiate<Transform>(suppressorPrefab).GetComponent<Suppressor>(); break;
		}

		weapon.init(this);
		weapon.setWeaponType(data.type);
		weapon.setDamage(data.minDamage, data.maxDamage);
		weapon.setReloadTime(data.reloadTime);
		weapon.setAsPlayerWeapon();
		
		weapon.transform.SetParent(transform);
		
		Vector3 posit = getWeaponPosition(weaponIndex);
		weapon.transform.position = new Vector3(transform.position.x + posit.x, transform.position.y + posit.y, posit.z);
		
		switch (weaponIndex) {
			case 0: weapon_1 = weapon; break;
			case 1: weapon_2 = weapon; break;
			case 2: weapon_3 = weapon; break;
			case 3: weapon_4 = weapon; break;
			case 4: weapon_5 = weapon; break;
		}
	}

	private void initShield () {
		shieldRechargeValue = 0;
		foreach (HullSlot slot in shipData.getSlots(Slot.Type.SHIELD)) {
			if (slot.item != null) {
				shieldRechargeValue += ((ShieldData)slot.item.itemData).rechargeSpeed;
			}
		}
	}

	private void initRepair () {
		repairValue = 0;
		foreach (HullSlot slot in shipData.getSlots(Slot.Type.REPAIR_DROID)) {
			if (slot.item != null) {
				repairValue += ((RepairDroidData)slot.item.itemData).repairSpeed;
			}
		}
		repairValue *= .01f;
	}

	protected override void updateHealthAndShieldInfo () {
		shipData.setCurrentShield(shield);
		shipData.setCurrentHealth(health);
		Vars.userInterface.updateShip();
	}

	public override bool isPlayerShip () {
		return true;
	}
}