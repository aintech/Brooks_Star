using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipData : MonoBehaviour {

	public Transform inventoryItemPrefab;

	public Sprite[] hullSprites;

	private bool initialized = false;

	private SpriteRenderer render;

	private HullType hullType;

	private HullSlot radarSlot;

	private HullSlot engineSlot;

	private HullSlot generatorSlot_1, generatorSlot_2, generatorSlot_3;

	private HullSlot harvesterSlot_1, harvesterSlot_2;

	private HullSlot repairDroidSlot_1, repairDroidSlot_2, repairDroidSlot_3, repairDroidSlot_4;

	private HullSlot shieldSlot_1, shieldSlot_2, shieldSlot_3;

	private HullSlot weaponSlot_1, weaponSlot_2, weaponSlot_3, weaponSlot_4, weaponSlot_5;
	
	private HullSlot armorSlot_1, armorSlot_2, armorSlot_3, armorSlot_4, armorSlot_5;

	private Transform trans;

	private HullSlot[] slots;

	private int currentHealth, currentShield;

	private TextMesh healthValue, armorValue, shieldValue, energyValue;

	private Color32 okColor, badColor = new Color32(255, 0, 0, 255);

	private bool onPlanetSurface;

	public ShipData init (bool onPlanetSurface) {
		this.onPlanetSurface = onPlanetSurface;
		trans = transform;
		radarSlot = trans.Find("HullSlot Radar").GetComponent<HullSlot>();
		engineSlot = trans.Find("HullSlot Engine").GetComponent<HullSlot>();
		generatorSlot_1 = trans.Find("HullSlot Generator 1").GetComponent<HullSlot>();
		generatorSlot_2 = trans.Find("HullSlot Generator 2").GetComponent<HullSlot>();
		generatorSlot_3 = trans.Find("HullSlot Generator 3").GetComponent<HullSlot>();
		harvesterSlot_1 = trans.Find("HullSlot Harvester 1").GetComponent<HullSlot>();
		harvesterSlot_2 = trans.Find("HullSlot Harvester 2").GetComponent<HullSlot>();
		repairDroidSlot_1 = trans.Find("HullSlot RepairDroid 1").GetComponent<HullSlot>();
		repairDroidSlot_2 = trans.Find("HullSlot RepairDroid 2").GetComponent<HullSlot>();
		repairDroidSlot_3 = trans.Find("HullSlot RepairDroid 3").GetComponent<HullSlot>();
		repairDroidSlot_4 = trans.Find("HullSlot RepairDroid 4").GetComponent<HullSlot>();
		shieldSlot_1 = trans.Find("HullSlot Shield 1").GetComponent<HullSlot>();
		shieldSlot_2 = trans.Find("HullSlot Shield 2").GetComponent<HullSlot>();
		shieldSlot_3 = trans.Find("HullSlot Shield 3").GetComponent<HullSlot>();
		weaponSlot_1 = trans.Find("HullSlot Weapon 1").GetComponent<HullSlot>();
		weaponSlot_2 = trans.Find("HullSlot Weapon 2").GetComponent<HullSlot>();
		weaponSlot_3 = trans.Find("HullSlot Weapon 3").GetComponent<HullSlot>();
		weaponSlot_4 = trans.Find("HullSlot Weapon 4").GetComponent<HullSlot>();
		weaponSlot_5 = trans.Find("HullSlot Weapon 5").GetComponent<HullSlot>();
		armorSlot_1 = trans.Find("HullSlot Armor 1").GetComponent<HullSlot>();
		armorSlot_2 = trans.Find("HullSlot Armor 2").GetComponent<HullSlot>();
		armorSlot_3 = trans.Find("HullSlot Armor 3").GetComponent<HullSlot>();
		armorSlot_4 = trans.Find("HullSlot Armor 4").GetComponent<HullSlot>();
		armorSlot_5 = trans.Find("HullSlot Armor 5").GetComponent<HullSlot>();

		slots = new HullSlot[]{radarSlot, engineSlot, generatorSlot_1, generatorSlot_2, generatorSlot_3, harvesterSlot_1, harvesterSlot_2,
							   repairDroidSlot_1, repairDroidSlot_2, repairDroidSlot_3, repairDroidSlot_4, shieldSlot_1, shieldSlot_2, shieldSlot_3,
							   weaponSlot_1, weaponSlot_2, weaponSlot_3, weaponSlot_4, weaponSlot_5,
							   armorSlot_1, armorSlot_2, armorSlot_3, armorSlot_4, armorSlot_5};

		
		Transform hullInfo = transform.Find ("Hull Information");
		hullInfo.gameObject.SetActive(true);

		TextMesh healthLabel = hullInfo.Find ("Health Label").GetComponent<TextMesh> ();
		TextMesh armorLabel = hullInfo.Find ("Armor Label").GetComponent<TextMesh> ();
		TextMesh shieldLabel = hullInfo.Find ("Shield Label").GetComponent<TextMesh> ();
		TextMesh energyLabel = hullInfo.Find ("Energy Label").GetComponent<TextMesh> ();
		healthValue = hullInfo.Find ("Health Value").GetComponent<TextMesh> ();
		armorValue = hullInfo.Find ("Armor Value").GetComponent<TextMesh> ();
		shieldValue = hullInfo.Find("Shield Value").GetComponent<TextMesh>();
		energyValue = hullInfo.Find ("Energy Value").GetComponent<TextMesh> ();

		okColor = energyValue.color;

		healthLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		armorLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		shieldLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		energyLabel.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		healthValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		armorValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		shieldValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";
		energyValue.GetComponent<MeshRenderer> ().sortingLayerName = "Inventory";

		healthLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
		armorLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
		shieldLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
		energyLabel.GetComponent<MeshRenderer> ().sortingOrder = 3;
		healthValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		armorValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		shieldValue.GetComponent<MeshRenderer> ().sortingOrder = 3;
		energyValue.GetComponent<MeshRenderer> ().sortingOrder = 3;

        transform.Find("Hull Image").gameObject.SetActive(true);
        transform.Find("Hull BG").gameObject.SetActive(true);

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

	public HullType getHullType () {
		return hullType;
	}

	private void setHullSprite () {
		if (render == null) getRender();
		switch (hullType) {
			case HullType.Little: render.sprite = hullSprites[0]; break;
			case HullType.Needle: render.sprite = hullSprites[1]; break;
			case HullType.Gnome: render.sprite = hullSprites[2]; break;
			case HullType.Cricket: render.sprite = hullSprites[3]; break;
			case HullType.Argo: render.sprite = hullSprites[4]; break;
			case HullType.Falcon: render.sprite = hullSprites[5]; break;
			case HullType.Adventurer: render.sprite = hullSprites[6]; break;
			case HullType.Corvette: render.sprite = hullSprites[7]; break;
			case HullType.Buffalo: render.sprite = hullSprites[8]; break;
			case HullType.Legionnaire: render.sprite = hullSprites[9]; break;
			case HullType.StarWalker: render.sprite = hullSprites[10]; break;
			case HullType.Warship: render.sprite = hullSprites[11]; break;
			case HullType.Asterix: render.sprite = hullSprites[12]; break;
			case HullType.Prime: render.sprite = hullSprites[13]; break;
			case HullType.Titan: render.sprite = hullSprites[14]; break;
			case HullType.Dreadnaut: render.sprite = hullSprites[15]; break;
			case HullType.Armageddon: render.sprite = hullSprites[16]; break;
			default: Debug.Log("Неизвестный тип корпуса"); break;
		}
	}
	
	void getRender () {
		render = transform.Find("Hull Image").GetComponent<SpriteRenderer>();
	}

	private void setSlotPositions () {
		radarSlot.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Radar");
		engineSlot.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Engine");
		generatorSlot_1.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Generator 1");
		generatorSlot_2.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Generator 2");
		generatorSlot_3.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Generator 3");
		harvesterSlot_1.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Harvester 1");
		harvesterSlot_2.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Harvester 2");
		repairDroidSlot_1.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot RepairDroid 1");
		repairDroidSlot_2.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot RepairDroid 2");
		repairDroidSlot_3.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot RepairDroid 3");
		repairDroidSlot_4.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot RepairDroid 4");
		shieldSlot_1.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Shield 1");
		shieldSlot_2.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Shield 2");
		shieldSlot_3.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Shield 3");
		weaponSlot_1.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Weapon 1");
		weaponSlot_2.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Weapon 2");
		weaponSlot_3.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Weapon 3");
		weaponSlot_4.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Weapon 4");
		weaponSlot_5.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Weapon 5");
		armorSlot_1.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Armor 1");
		armorSlot_2.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Armor 2");
		armorSlot_3.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Armor 3");
		armorSlot_4.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Armor 4");
		armorSlot_5.transform.position = trans.position - HullSlot.getSlotPosition (hullType, "HullSlot Armor 5");
	}

	private void setSlotAvailables () {
		radarSlot.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Radar"));
		engineSlot.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Engine"));
		generatorSlot_1.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Generator 1"));
		generatorSlot_2.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Generator 2"));
		generatorSlot_3.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Generator 3"));
		harvesterSlot_1.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Harvester 1"));
		harvesterSlot_2.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Harvester 2"));
		repairDroidSlot_1.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot RepairDroid 1"));
		repairDroidSlot_2.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot RepairDroid 2"));
		repairDroidSlot_3.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot RepairDroid 3"));
		repairDroidSlot_4.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot RepairDroid 4"));
		shieldSlot_1.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Shield 1"));
		shieldSlot_2.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Shield 2"));
		shieldSlot_3.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Shield 3"));
		weaponSlot_1.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Weapon 1"));
		weaponSlot_2.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Weapon 2"));
		weaponSlot_3.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Weapon 3"));
		weaponSlot_4.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Weapon 4"));
		weaponSlot_5.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Weapon 5"));
		armorSlot_1.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Armor 1"));
		armorSlot_2.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Armor 2"));
		armorSlot_3.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Armor 3"));
		armorSlot_4.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Armor 4"));
		armorSlot_5.setSlotAvailable(HullSlot.checkSlotAvailability (hullType, "HullSlot Armor 5"));
	}

	public HullSlot[] getSlots () {
		return slots;
	}

	public void arrangeItemsToSlots () {
		foreach (HullSlot slot in getSlots()) {
			if (slot.getItem() != null) {
				slot.getItem().transform.position = slot.transform.position;
				slot.getItem().transform.SetParent (slot.transform);
				slot.getItem().GetComponent<SpriteRenderer>().sortingOrder = 3;
			}
		}
	}

	public void initializeRandomShip (HullType initType) {
		if (initialized) { return; }

		setHullType (initType, initType.getMaxHealth());

		int generatorSlots = getHullType().getGeneratorSlots();
		int harvesterSlots = getHullType().getHarvesterSlots();
		int repairDroids = getHullType().getRepairDroidSlots();
		int shieldSlots = getHullType().getShieldSlots();
		int weaponSlots = getHullType().getWeaponSlots();
		int armorSlots = getHullType().getArmorSlots();

		Item radar = null, engine = null, generator_1 = null, generator_2 = null, generator_3 = null,
		  	 harvester_1 = null, harvester_2 = null,
		  	 repairDroid_1 = null, repairDroid_2 = null, repairDroid_3 = null, repairDroid_4 = null,
		  	 shield_1 = null, shield_2 = null, shield_3 = null,
		  	 weapon_1 = null, weapon_2 = null, weapon_3 = null, weapon_4 = null, weapon_5 = null,
		  	 armor_1 = null, armor_2 = null, armor_3 = null, armor_4 = null, armor_5 = null;

		radar = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		engine = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (generatorSlots >= 1) generator_1 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (generatorSlots >= 2) generator_2 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (generatorSlots >= 3) generator_3 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (harvesterSlots >= 1) harvester_1 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (harvesterSlots >= 2) harvester_2 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (repairDroids >= 1) repairDroid_1 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (repairDroids >= 2) repairDroid_2 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (repairDroids >= 3) repairDroid_3 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (repairDroids >= 4) repairDroid_4 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (shieldSlots >= 1) shield_1 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (shieldSlots >= 2) shield_2 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (shieldSlots >= 3) shield_3 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (weaponSlots >= 1) weapon_1 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (weaponSlots >= 2) weapon_2 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (weaponSlots >= 3) weapon_3 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (weaponSlots >= 4) weapon_4 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (weaponSlots >= 5) weapon_5 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (armorSlots >= 1) armor_1 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (armorSlots >= 2) armor_2 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (armorSlots >= 3) armor_3 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (armorSlots >= 4) armor_4 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();
		if (armorSlots >= 5) armor_5 = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>();

		getSlotByName("HullSlot Radar").setItem(radar);
		getSlotByName("HullSlot Engine").setItem(engine);
		if (generatorSlots >= 1) getSlotByName("HullSlot Generator 1").setItem(generator_1);
		if (generatorSlots >= 2) getSlotByName("HullSlot Generator 2").setItem(generator_2);
		if (generatorSlots >= 3) getSlotByName("HullSlot Generator 3").setItem(generator_3);
		if (harvesterSlots >= 1) getSlotByName("HullSlot Harvester 1").setItem(harvester_1);
		if (harvesterSlots >= 2) getSlotByName("HullSlot Harvester 2").setItem(harvester_2);
		if (repairDroids >= 1) getSlotByName("HullSlot RepairDroid 1").setItem(repairDroid_1);
		if (repairDroids >= 2) getSlotByName("HullSlot RepairDroid 2").setItem(repairDroid_2);
		if (repairDroids >= 3) getSlotByName("HullSlot RepairDroid 3").setItem(repairDroid_3);
		if (repairDroids >= 4) getSlotByName("HullSlot RepairDroid 4").setItem(repairDroid_4);
		if (shieldSlots >= 1) getSlotByName("HullSlot Shield 1").setItem(shield_1);
		if (shieldSlots >= 2) getSlotByName("HullSlot Shield 2").setItem(shield_2);
		if (shieldSlots >= 3) getSlotByName("HullSlot Shield 3").setItem(shield_3);
		if (weaponSlots >= 1) getSlotByName("HullSlot Weapon 1").setItem(weapon_1);
		if (weaponSlots >= 2) getSlotByName("HullSlot Weapon 2").setItem(weapon_2);
		if (weaponSlots >= 3) getSlotByName("HullSlot Weapon 3").setItem(weapon_3);
		if (weaponSlots >= 4) getSlotByName("HullSlot Weapon 4").setItem(weapon_4);
		if (weaponSlots >= 5) getSlotByName("HullSlot Weapon 5").setItem(weapon_5);
		if (armorSlots >= 1) getSlotByName("HullSlot Armor 1").setItem(armor_1);
		if (armorSlots >= 2) getSlotByName("HullSlot Armor 2").setItem(armor_2);
		if (armorSlots >= 3) getSlotByName("HullSlot Armor 3").setItem(armor_3);
		if (armorSlots >= 4) getSlotByName("HullSlot Armor 4").setItem(armor_4);
		if (armorSlots >= 5) getSlotByName("HullSlot Armor 5").setItem(armor_5);

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
		setCurrentHealth (getHullType ().getMaxHealth ());

		initialized = true;
	}

	public void sendToVars () {
		Vars.shipCurrentHealth = currentHealth;
		Vars.shipHullType = getHullType ();
		Vars.shipHullSlotsMap.Clear ();
		foreach (HullSlot slot in getSlots()) {
			if (slot.getItem() != null) {
				Vars.shipHullSlotsMap.Add(getSlotName(slot), slot.getItem().getItemData());
			}
		}
	}

	public void initializeFromVars () {
		if (initialized) { return; }

		setHullType (Vars.shipHullType, Vars.shipCurrentHealth);
		foreach (KeyValuePair<string, ItemData> pair in Vars.shipHullSlotsMap) {
			Item item = Instantiate<Transform>(inventoryItemPrefab).GetComponent<Item>().init(pair.Value);
			item.transform.parent = trans;
			item.GetComponent<SpriteRenderer>().sortingOrder = 3;
			getSlotByName(pair.Key).setItem(item);
		}
		Vars.shipHullSlotsMap.Clear();
		arrangeItemsToSlots ();

		setCurrentShield(getShield());
		setCurrentHealth(getHullType().getMaxHealth());

		initialized = true;
	}

	public HullSlot getSlotByName (string slotName) {
		switch (slotName) {
			case "HullSlot Radar": return radarSlot;
			case "HullSlot Engine": return engineSlot;
			case "HullSlot Generator 1": return generatorSlot_1;
			case "HullSlot Generator 2": return generatorSlot_2;
			case "HullSlot Generator 3": return generatorSlot_3;
			case "HullSlot Harvester 1": return harvesterSlot_1;
			case "HullSlot Harvester 2": return harvesterSlot_2;
			case "HullSlot RepairDroid 1": return repairDroidSlot_1;
			case "HullSlot RepairDroid 2": return repairDroidSlot_2;
			case "HullSlot RepairDroid 3": return repairDroidSlot_3;
			case "HullSlot RepairDroid 4": return repairDroidSlot_4;
			case "HullSlot Shield 1": return shieldSlot_1;
			case "HullSlot Shield 2": return shieldSlot_2;
			case "HullSlot Shield 3": return shieldSlot_3;
			case "HullSlot Weapon 1": return weaponSlot_1;
			case "HullSlot Weapon 2": return weaponSlot_2;
			case "HullSlot Weapon 3": return weaponSlot_3;
			case "HullSlot Weapon 4": return weaponSlot_4;
			case "HullSlot Weapon 5": return weaponSlot_5;
			case "HullSlot Armor 1": return armorSlot_1;
			case "HullSlot Armor 2": return armorSlot_2;
			case "HullSlot Armor 3": return armorSlot_3;
			case "HullSlot Armor 4": return armorSlot_4;
			case "HullSlot Armor 5": return armorSlot_5;
			default: Debug.Log("Неизвестное имя слота"); return null;
		}
	}

	public HullSlot getSlotByType (HullSlot.HullSlotType type, int slotIndex) {
		if (type == HullSlot.HullSlotType.Radar) { return radarSlot; }
		if (type == HullSlot.HullSlotType.Engine) { return engineSlot; }
		if (type == HullSlot.HullSlotType.Generator) {
			switch (slotIndex) {
				case 1: return generatorSlot_1;
				case 2: return generatorSlot_2;
				case 3: return generatorSlot_3;
				default: Debug.Log("Unknown generator slot: " + slotIndex); return null;
			}
		}
		if (type == HullSlot.HullSlotType.Harvester) {
			switch (slotIndex) {
				case 1: return harvesterSlot_1;
				case 2: return harvesterSlot_2;
				default: Debug.Log("Unknown harvester slot: " + slotIndex); return null;
			}
		}
		if (type == HullSlot.HullSlotType.RepairDroid) {
			switch (slotIndex) {
				case 1: return repairDroidSlot_1;
				case 2: return repairDroidSlot_2;
				case 3: return repairDroidSlot_3;
				case 4: return repairDroidSlot_4;
				default: Debug.Log("Unknown repair droid slot: " + slotIndex); return null;
			}
		}
		if (type == HullSlot.HullSlotType.Shield) {
			switch (slotIndex) {
				case 1: return shieldSlot_1;
				case 2: return shieldSlot_2;
				case 3: return shieldSlot_3;
				default: Debug.Log("Unknown shield slot: " + slotIndex); return null;
			}
		}
		if (type == HullSlot.HullSlotType.Weapon) {
			switch (slotIndex) {
				case 1: return weaponSlot_1;
				case 2: return weaponSlot_2;
				case 3: return weaponSlot_3;
				case 4: return weaponSlot_4;
				case 5: return weaponSlot_5;
				default: Debug.Log("Unknown weapon slot: " + slotIndex); return null;
			}
		}
		if (type == HullSlot.HullSlotType.Weapon) {
			switch (slotIndex) {
				case 1: return weaponSlot_1;
				case 2: return weaponSlot_2;
				case 3: return weaponSlot_3;
				case 4: return weaponSlot_4;
				case 5: return weaponSlot_5;
				default: Debug.Log("Unknown weapon slot: " + slotIndex); return null;
			}
		}
		if (type == HullSlot.HullSlotType.Armor) {
			switch (slotIndex) {
				case 1: return armorSlot_1;
				case 2: return armorSlot_2;
				case 3: return armorSlot_3;
				case 4: return armorSlot_4;
				case 5: return armorSlot_5;
				default: Debug.Log("Unknown armor slot: " + slotIndex); return null;
			}
		}

		Debug.Log("Slot type not found");
		return null;
	}

	private string getSlotName (HullSlot slot) {
		if (slot == radarSlot) return "HullSlot Radar";
		if (slot == engineSlot) return "HullSlot Engine";
		if (slot == generatorSlot_1) return "HullSlot Generator 1";
		if (slot == generatorSlot_2) return "HullSlot Generator 2";
		if (slot == generatorSlot_3) return "HullSlot Generator 3";
		if (slot == harvesterSlot_1) return "HullSlot Harvester 1";
		if (slot == harvesterSlot_2) return "HullSlot Harvester 2";
		if (slot == repairDroidSlot_1) return "HullSlot RepairDroid 1";
		if (slot == repairDroidSlot_2) return "HullSlot RepairDroid 2";
		if (slot == repairDroidSlot_3) return "HullSlot RepairDroid 3";
		if (slot == repairDroidSlot_4) return "HullSlot RepairDroid 4";
		if (slot == shieldSlot_1) return "HullSlot Shield 1";
		if (slot == shieldSlot_2) return "HullSlot Shield 2";
		if (slot == shieldSlot_3) return "HullSlot Shield 3";
		if (slot == weaponSlot_1) return "HullSlot Weapon 1";
		if (slot == weaponSlot_2) return "HullSlot Weapon 2";
		if (slot == weaponSlot_3) return "HullSlot Weapon 3";
		if (slot == weaponSlot_4) return "HullSlot Weapon 4";
		if (slot == weaponSlot_5) return "HullSlot Weapon 5";
		if (slot == armorSlot_1) return "HullSlot Armor 1";
		if (slot == armorSlot_2) return "HullSlot Armor 2";
		if (slot == armorSlot_3) return "HullSlot Armor 3";
		if (slot == armorSlot_4) return "HullSlot Armor 4";
		if (slot == armorSlot_5) return "HullSlot Armor 5";
		return "";
	}

	public void updateHullInfo () {
		updateShieldValue();
		updateHealthValue ();
		updateArmorValue ();
		updateShieldValue();
		updateEnergyValue ();
	}

	public void updateHealthValue () {
		healthValue.text = currentHealth.ToString() + "/" + getHullType ().getMaxHealth ().ToString();
	}

	public void updateArmorValue () {
		armorValue.text = getArmor().ToString ();
	}

	public void updateShieldValue () {
		shieldValue.text = (onPlanetSurface? "": currentShield.ToString() + "/") +  getShield().ToString();
	}

	public void updateEnergyValue () {
		int energy = getEnergyNeeded();
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
			if (slot.getItem() != null && slot.getItem().getItemType() == ItemData.Type.SHIELD) {
				shield += ((ShieldData) slot.getItem().getItemData()).shieldLevel;
			}	
		}
		return shield;
	}

	public int getArmor () {
		int armor = 0;
		foreach (HullSlot slot in slots) {
			if (slot.getItem() != null && slot.getItem ().getItemType() == ItemData.Type.ARMOR) {
				armor += ((ArmorData)slot.getItem ().getItemData()).armorClass;
			}
		}
		return armor;
	}

	public int getRadarRange () {
		if (radarSlot.getItem() == null) return 0;
		return ((RadarData)radarSlot.getItem().getItemData()).range;
	}

	public int getEnergyNeeded () {
		int energy = 0;
		foreach (HullSlot slot in slots) {
			if (slot.getItem() != null) {
				if (slot.getItem().getItemType() == ItemData.Type.GENERATOR) {
					energy += ((GeneratorData)slot.getItem().getItemData()).maxEnergy;
				}
				energy -= slot.getItem().getEnergyNeeded();
			}
		}
		return energy;
	}
}