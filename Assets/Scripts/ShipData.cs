﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipData : MonoBehaviour {

	public Sprite[] hullSprites;

	private bool initialized = false;

	private SpriteRenderer render;

	public HullType hullType { get; private set; }

	private HullSlot radarSlot,
					 engineSlot,
					 generatorSlot_1, generatorSlot_2, generatorSlot_3,
					 harvesterSlot_1, harvesterSlot_2,
					 repairDroidSlot_1, repairDroidSlot_2, repairDroidSlot_3, repairDroidSlot_4,
					 shieldSlot_1, shieldSlot_2, shieldSlot_3,
					 weaponSlot_1, weaponSlot_2, weaponSlot_3, weaponSlot_4, weaponSlot_5,
					 armorSlot_1, armorSlot_2, armorSlot_3, armorSlot_4, armorSlot_5;

	private Transform trans;

	public HullSlot[] slots { get; private set; }

	private int currentHealth, currentShield;

	private TextMesh healthValue, armorValue, shieldValue, energyValue;

	private Color32 okColor, badColor = new Color32(255, 0, 0, 255);

	private bool onPlanetSurface;

	public int repairCost { get; private set; }

	public ShipData init (bool onPlanetSurface) {
		this.onPlanetSurface = onPlanetSurface;
		trans = transform;

		HullSlot tempslot;
		for (int i = 0; i < trans.childCount; i++) {
			tempslot = trans.GetChild(i).GetComponent<HullSlot>();
			if (tempslot != null) {
				tempslot.init();
				switch (tempslot.slotType) {
					case HullSlot.Type.RADAR: radarSlot = tempslot; break;
					case HullSlot.Type.ENGINE: engineSlot = tempslot; break;
					case HullSlot.Type.GENERATOR:
						if (tempslot.index == 0) { generatorSlot_1 = tempslot; }
						else if (tempslot.index == 1) { generatorSlot_2 = tempslot; }
						else if (tempslot.index == 2) { generatorSlot_3 = tempslot; }
						break;
					case HullSlot.Type.HARVESTER:
						if (tempslot.index == 0) { harvesterSlot_1 = tempslot; }
						else if (tempslot.index == 1) { harvesterSlot_2 = tempslot; }
						break;
					case HullSlot.Type.REPAIR_DROID:
						if (tempslot.index == 0) { repairDroidSlot_1 = tempslot; }
						else if (tempslot.index == 1) { repairDroidSlot_2 = tempslot; }
						else if (tempslot.index == 2) { repairDroidSlot_3 = tempslot; }
						else if (tempslot.index == 3) { repairDroidSlot_4 = tempslot; }
						break;
					case HullSlot.Type.SHIELD:
						if (tempslot.index == 0) { shieldSlot_1 = tempslot; }
						else if (tempslot.index == 1) { shieldSlot_2 = tempslot; }
						else if (tempslot.index == 2) { shieldSlot_3 = tempslot; }
						break;
					case HullSlot.Type.WEAPON:
						if (tempslot.index == 0) { weaponSlot_1 = tempslot; }
						else if (tempslot.index == 1) { weaponSlot_2 = tempslot; }
						else if (tempslot.index == 2) { weaponSlot_3 = tempslot; }
						else if (tempslot.index == 3) { weaponSlot_4 = tempslot; }
						else if (tempslot.index == 4) { weaponSlot_5 = tempslot; }
						break;
					case HullSlot.Type.ARMOR:
						if (tempslot.index == 0) { armorSlot_1 = tempslot; }
						else if (tempslot.index == 1) { armorSlot_2 = tempslot; }
						else if (tempslot.index == 2) { armorSlot_3 = tempslot; }
						else if (tempslot.index == 3) { armorSlot_4 = tempslot; }
						else if (tempslot.index == 4) { armorSlot_5 = tempslot; }
						break;
					default: Debug.Log("Unknown slot type: " + tempslot.slotType); break;
				}
			}
		}

		slots = new HullSlot[]{radarSlot, engineSlot, generatorSlot_1, generatorSlot_2, generatorSlot_3, harvesterSlot_1, harvesterSlot_2,
							   repairDroidSlot_1, repairDroidSlot_2, repairDroidSlot_3, repairDroidSlot_4, shieldSlot_1, shieldSlot_2, shieldSlot_3,
							   weaponSlot_1, weaponSlot_2, weaponSlot_3, weaponSlot_4, weaponSlot_5,
							   armorSlot_1, armorSlot_2, armorSlot_3, armorSlot_4, armorSlot_5};

		Transform hullInfo = transform.Find ("Hull Information");
		hullInfo.gameObject.SetActive(true);

		healthValue = hullInfo.Find ("Health Value").GetComponent<TextMesh> ();
		armorValue = hullInfo.Find ("Armor Value").GetComponent<TextMesh> ();
		shieldValue = hullInfo.Find("Shield Value").GetComponent<TextMesh>();
		energyValue = hullInfo.Find ("Energy Value").GetComponent<TextMesh> ();

		okColor = energyValue.color;

		healthValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		armorValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		shieldValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		energyValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";

		healthValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		armorValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		shieldValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		energyValue.GetComponent<MeshRenderer> ().sortingOrder = 3;

        transform.Find("Hull Image").gameObject.SetActive(true);

		gameObject.SetActive(false);

		return this;
	}

	public void setHullType (HullType hullType, int health) {
		this.hullType = hullType;
		setSlotPositions ();
		setSlotAvailables ();
		setHullSprite ();
		setCurrentHealth (health);
	}

	private void setHullSprite () {
		if (render == null) getRender();
		switch (hullType) {
			case HullType.LITTLE: render.sprite = hullSprites[0]; break;
			case HullType.NEEDLE: render.sprite = hullSprites[1]; break;
			case HullType.GNOME: render.sprite = hullSprites[2]; break;
			case HullType.CRICKET: render.sprite = hullSprites[3]; break;
			case HullType.ARGO: render.sprite = hullSprites[4]; break;
			case HullType.FALCON: render.sprite = hullSprites[5]; break;
			case HullType.ADVENTURER: render.sprite = hullSprites[6]; break;
			case HullType.CORVETTE: render.sprite = hullSprites[7]; break;
			case HullType.BUFFALO: render.sprite = hullSprites[8]; break;
			case HullType.LEGIONNAIRE: render.sprite = hullSprites[9]; break;
			case HullType.STARWALKER: render.sprite = hullSprites[10]; break;
			case HullType.WARSHIP: render.sprite = hullSprites[11]; break;
			case HullType.ASTERIX: render.sprite = hullSprites[12]; break;
			case HullType.PRIME: render.sprite = hullSprites[13]; break;
			case HullType.TITAN: render.sprite = hullSprites[14]; break;
			case HullType.DREADNAUT: render.sprite = hullSprites[15]; break;
			case HullType.ARMAGEDDON: render.sprite = hullSprites[16]; break;
			default: Debug.Log("Неизвестный тип корпуса"); break;
		}
	}
	
	void getRender () {
		render = transform.Find("Hull Image").GetComponent<SpriteRenderer>();
	}

	private void setSlotPositions () {
        foreach (HullSlot slot in slots) {
            slot.transform.position = trans.position - HullSlot.getSlotPosition(hullType, slot.slotType, slot.index);
        }
	}

	private void setSlotAvailables () {
        foreach (HullSlot slot in slots) {
            slot.setSlotAvailable(HullSlot.checkSlotAvailability(hullType, slot.slotType, slot.index));
        }
	}

	public void arrangeItemsToSlots () {
		foreach (HullSlot slot in slots) {
			if (slot.item != null) {
				slot.item.transform.position = slot.transform.position;
				slot.item.transform.SetParent (slot.transform);
				slot.item.changeSortOrder(3);//.GetComponent<SpriteRenderer>().sortingOrder = 3;
			}
		}
	}

	public void initializeRandomShip (HullType initType) {
		if (initialized) { return; }

		setHullType (initType, initType.getMaxHealth());

		int generatorSlots = hullType.getGeneratorSlots();
		int harvesterSlots = hullType.getHarvesterSlots();
		int repairDroids = hullType.getRepairDroidSlots();
		int shieldSlots = hullType.getShieldSlots();
		int weaponSlots = hullType.getWeaponSlots();
		int armorSlots = hullType.getArmorSlots();

		Item radar = null, engine = null, generator_1 = null, generator_2 = null, generator_3 = null,
		  	 harvester_1 = null, harvester_2 = null,
		  	 repairDroid_1 = null, repairDroid_2 = null, repairDroid_3 = null, repairDroid_4 = null,
		  	 shield_1 = null, shield_2 = null, shield_3 = null,
		  	 weapon_1 = null, weapon_2 = null, weapon_3 = null, weapon_4 = null, weapon_5 = null,
		  	 armor_1 = null, armor_2 = null, armor_3 = null, armor_4 = null, armor_5 = null;

		radar = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		engine = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (generatorSlots >= 1) generator_1 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (generatorSlots >= 2) generator_2 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (generatorSlots >= 3) generator_3 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (harvesterSlots >= 1) harvester_1 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (harvesterSlots >= 2) harvester_2 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (repairDroids >= 1) repairDroid_1 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (repairDroids >= 2) repairDroid_2 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (repairDroids >= 3) repairDroid_3 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (repairDroids >= 4) repairDroid_4 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (shieldSlots >= 1) shield_1 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (shieldSlots >= 2) shield_2 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (shieldSlots >= 3) shield_3 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (weaponSlots >= 1) weapon_1 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (weaponSlots >= 2) weapon_2 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (weaponSlots >= 3) weapon_3 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (weaponSlots >= 4) weapon_4 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (weaponSlots >= 5) weapon_5 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (armorSlots >= 1) armor_1 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (armorSlots >= 2) armor_2 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (armorSlots >= 3) armor_3 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (armorSlots >= 4) armor_4 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();
		if (armorSlots >= 5) armor_5 = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>();

		getSlot(HullSlot.Type.RADAR, 0).setItem(radar);
		getSlot(HullSlot.Type.ENGINE, 0).setItem(engine);
		if (generatorSlots >= 1) getSlot(HullSlot.Type.GENERATOR, 0).setItem(generator_1);
		if (generatorSlots >= 2) getSlot(HullSlot.Type.GENERATOR, 1).setItem(generator_2);
		if (generatorSlots >= 3) getSlot(HullSlot.Type.GENERATOR, 2).setItem(generator_3);
		if (harvesterSlots >= 1) getSlot(HullSlot.Type.HARVESTER, 0).setItem(harvester_1);
		if (harvesterSlots >= 2) getSlot(HullSlot.Type.HARVESTER, 1).setItem(harvester_2);
		if (repairDroids >= 1) getSlot(HullSlot.Type.REPAIR_DROID, 0).setItem(repairDroid_1);
		if (repairDroids >= 2) getSlot(HullSlot.Type.REPAIR_DROID, 1).setItem(repairDroid_2);
		if (repairDroids >= 3) getSlot(HullSlot.Type.REPAIR_DROID, 2).setItem(repairDroid_3);
		if (repairDroids >= 4) getSlot(HullSlot.Type.REPAIR_DROID, 3).setItem(repairDroid_4);
		if (shieldSlots >= 1) getSlot(HullSlot.Type.SHIELD, 0).setItem(shield_1);
		if (shieldSlots >= 2) getSlot(HullSlot.Type.SHIELD, 1).setItem(shield_2);
		if (shieldSlots >= 3) getSlot(HullSlot.Type.SHIELD, 2).setItem(shield_3);
		if (weaponSlots >= 1) getSlot(HullSlot.Type.WEAPON, 0).setItem(weapon_1);
		if (weaponSlots >= 2) getSlot(HullSlot.Type.WEAPON, 1).setItem(weapon_2);
		if (weaponSlots >= 3) getSlot(HullSlot.Type.WEAPON, 2).setItem(weapon_3);
		if (weaponSlots >= 4) getSlot(HullSlot.Type.WEAPON, 3).setItem(weapon_4);
		if (weaponSlots >= 5) getSlot(HullSlot.Type.WEAPON, 4).setItem(weapon_5);
		if (armorSlots >= 1) getSlot(HullSlot.Type.ARMOR, 0).setItem(armor_1);
		if (armorSlots >= 2) getSlot(HullSlot.Type.ARMOR, 1).setItem(armor_2);
		if (armorSlots >= 3) getSlot(HullSlot.Type.ARMOR, 2).setItem(armor_3);
		if (armorSlots >= 4) getSlot(HullSlot.Type.ARMOR, 3).setItem(armor_4);
		if (armorSlots >= 5) getSlot(HullSlot.Type.ARMOR, 4).setItem(armor_5);

		radar.init(ItemFactory.createRadarData());
		engine.init(ItemFactory.createEngineData());
		if (generatorSlots >= 1) generator_1.init(ItemFactory.createGeneratorData());
		if (generatorSlots >= 2) generator_2.init(ItemFactory.createGeneratorData());
		if (generatorSlots >= 3) generator_3.init(ItemFactory.createGeneratorData());
		if (harvesterSlots >= 1) harvester_1.init(ItemFactory.createHarvesterData());
		if (harvesterSlots >= 2) harvester_2.init(ItemFactory.createHarvesterData());
		if (repairDroids >= 1) repairDroid_1.init(ItemFactory.createRepairDroidData());
		if (repairDroids >= 2) repairDroid_2.init(ItemFactory.createRepairDroidData());
		if (repairDroids >= 3) repairDroid_3.init(ItemFactory.createRepairDroidData());
		if (repairDroids >= 4) repairDroid_4.init(ItemFactory.createRepairDroidData());
		if (shieldSlots >= 1) shield_1.init(ItemFactory.createShieldData());
		if (shieldSlots >= 2) shield_2.init(ItemFactory.createShieldData());
		if (shieldSlots >= 3) shield_3.init(ItemFactory.createShieldData());
		if (weaponSlots >= 1) weapon_1.init(ItemFactory.createWeaponData());
		if (weaponSlots >= 2) weapon_2.init(ItemFactory.createWeaponData());
		if (weaponSlots >= 3) weapon_3.init(ItemFactory.createWeaponData());
		if (weaponSlots >= 4) weapon_4.init(ItemFactory.createWeaponData());
		if (weaponSlots >= 5) weapon_5.init(ItemFactory.createWeaponData());
		if (armorSlots >= 1) armor_1.init(ItemFactory.createArmorData());
		if (armorSlots >= 2) armor_2.init(ItemFactory.createArmorData());
		if (armorSlots >= 3) armor_3.init(ItemFactory.createArmorData());
		if (armorSlots >= 4) armor_4.init(ItemFactory.createArmorData());
		if (armorSlots >= 5) armor_5.init(ItemFactory.createArmorData());

		arrangeItemsToSlots();

		setCurrentShield (getShield());
		setCurrentHealth (hullType.getMaxHealth ());
		calculateRepairCost();

		initialized = true;
	}

	public void sendToVars () {
		Vars.shipCurrentHealth = currentHealth;
		Vars.shipHullType =hullType;
		Vars.shipHullSlotsMap.Clear ();
		foreach (HullSlot slot in slots) {
			if (slot.item != null) {
				Vars.shipHullSlotsMap.Add(new KeyValuePair<HullSlot.Type, int>(slot.slotType, slot.index), slot.item.itemData);
			}
		}
	}

	public void initializeFromVars () {
		if (initialized) { return; }

		setHullType (Vars.shipHullType, Vars.shipCurrentHealth);
		foreach (KeyValuePair<KeyValuePair<HullSlot.Type, int>, ItemData> pair in Vars.shipHullSlotsMap) {
			Item item = Instantiate<Transform>(ItemFactory.itemPrefab).GetComponent<Item>().init(pair.Value);
			item.changeSortOrder(3);//.GetComponent<SpriteRenderer>().sortingOrder = 3;
			getSlot(pair.Key.Key, pair.Key.Value).setItem(item);
		}
		Vars.shipHullSlotsMap.Clear();
		arrangeItemsToSlots ();

		setCurrentShield(getShield());
		setCurrentHealth(Vars.shipCurrentHealth);
		calculateRepairCost();

		initialized = true;
	}

	private void calculateRepairCost () {
		repairCost = Mathf.RoundToInt(hullType.cost() * .1f * (1 - ((float)currentHealth / (float)hullType.getMaxHealth())));
	}

	public HullSlot[] getSlots (HullSlot.Type type) {
		switch (type) {
			case Slot.Type.RADAR: return new HullSlot[] {radarSlot};
			case Slot.Type.ENGINE: return new HullSlot[] {engineSlot};
			case Slot.Type.GENERATOR: return new HullSlot[] {generatorSlot_1, generatorSlot_2, generatorSlot_3};
			case Slot.Type.HARVESTER: return new HullSlot[] {harvesterSlot_1, harvesterSlot_2};
			case Slot.Type.REPAIR_DROID: return new HullSlot[] {repairDroidSlot_1, repairDroidSlot_2, repairDroidSlot_3, repairDroidSlot_4};
			case Slot.Type.SHIELD: return new HullSlot[] {shieldSlot_1, shieldSlot_2, shieldSlot_3};
			case Slot.Type.WEAPON: return new HullSlot[] {weaponSlot_1, weaponSlot_2, weaponSlot_3, weaponSlot_4, weaponSlot_5};
			case Slot.Type.ARMOR: return new HullSlot[] {armorSlot_1, armorSlot_2, armorSlot_3, armorSlot_4, armorSlot_5};
			default: Debug.Log("Unknown slot type: " + type); return new HullSlot[0];
		}
	}

    public HullSlot getSlot (HullSlot.Type type, int slotIndex) {
		return getSlots(type)[slotIndex];
	}

	public void repairShip (bool forceRepair) {
		if (!forceRepair) {
			if (Vars.cash < repairCost) { Messenger.showMessage("Недостаточно кредитов на ремонт!"); return; }
			Vars.cash -= repairCost;
		}
		setCurrentHealth(hullType.getMaxHealth());
		repairCost = 0;
		updateHealthValue();
	}

	public void updateHullInfo () {
		updateShieldValue();
		updateHealthValue ();
		updateArmorValue ();
		updateShieldValue();
		updateEnergyValue ();
	}

	public void updateHealthValue () {
		healthValue.text = currentHealth.ToString() + (currentHealth < hullType.getMaxHealth()? "/" + hullType.getMaxHealth(): "");
	}

	public void updateArmorValue () {
		armorValue.text = getArmor().ToString ();
	}

	public void updateShieldValue () {
		int shield = getShield();
		if (onPlanetSurface) { setCurrentShield(shield); }
		shieldValue.text = currentShield.ToString() + (currentShield < shield? "/" + shield: ""); //(onPlanetSurface? "": currentShield.ToString() + "/") +  getShield().ToString();
	}

	public void updateEnergyValue () {
		int energy = energyNeeded();
		energyValue.text = energy.ToString ();
		energyValue.color = energy < 0? badColor: okColor;
	}
	
	public void setCurrentHealth (int currentHealth) {
		this.currentHealth = currentHealth;
	}
	
	public int getCurrentHealth () {
		return currentHealth;
	}

	public void setCurrentShield (int currentShield) {
		this.currentShield = currentShield;
	}

	public void setShieldToMax () {
		currentShield = getShield();
	}

	public int getShield () {
		int shield = 0;
		foreach (HullSlot slot in slots) {
			if (slot.item != null && slot.item.type == ItemType.SHIELD) {
				shield += ((ShieldData) slot.item.itemData).shieldLevel;
			}	
		}
		return shield;
	}

	public int getArmor () {
		int armor = 0;
		foreach (HullSlot slot in slots) {
			if (slot.item != null && slot.item.type == ItemType.ARMOR) {
				armor += ((ArmorData)slot.item.itemData).armorClass;
			}
		}
		return armor;
	}

	public int getRadarRange () {
		if (radarSlot.item == null) return 0;
		return ((RadarData)radarSlot.item.itemData).range;
	}

	public int energyNeeded () {
		int energy = 0;
		foreach (HullSlot slot in slots) {
			if (slot.item != null) {
				if (slot.item.type == ItemType.GENERATOR) {
					energy += ((GeneratorData)slot.item.itemData).maxEnergy;
				}
				energy -= slot.item.energyNeeded;
			}
		}
		return energy;
	}
}