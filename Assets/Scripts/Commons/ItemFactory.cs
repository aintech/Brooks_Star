using UnityEngine;
using System;
using System.Collections;

public static class ItemFactory {

	public static Transform itemPrefab;

	public static ItemData createItemData (ItemType type) {
		switch (type) {
			case ItemType.WEAPON: return createWeaponData ();
			case ItemType.ENGINE: return createEngineData ();
			case ItemType.ARMOR: return createArmorData ();
			case ItemType.GENERATOR: return createGeneratorData ();
			case ItemType.RADAR: return createRadarData ();
			case ItemType.SHIELD: return createShieldData ();
			case ItemType.REPAIR_DROID: return createRepairDroidData ();
			case ItemType.HARVESTER: return createHarvesterData ();
			case ItemType.HAND_WEAPON: return createHandWeaponData ();
			case ItemType.BODY_ARMOR: return createBodyArmorData ();
			case ItemType.GOODS: return createGoodsData (UnityEngine.Random.Range(1, 21));
			default: Debug.Log("Unknown type: " + type); return null;
		}
	}

	private static ItemQuality randQuality () {
		float rand = UnityEngine.Random.value;
		return rand <= .5f ? ItemQuality.COMMON : rand <= .7f ? ItemQuality.GOOD : rand <= .85f ? ItemQuality.SUPERIOR : rand <= .95 ? ItemQuality.RARE : ItemQuality.UNIQUE;
	}

	private static float randLevel () {
		return 1 + (UnityEngine.Random.value * .3f);
	}

	private static float qualityMultiplier (ItemQuality quality) {
		return quality == ItemQuality.UNIQUE? 2f: quality == ItemQuality.RARE? 1.7f: quality == ItemQuality.SUPERIOR? 1.4f: quality == ItemQuality.GOOD? 1.2f: 1;
	}

	private static int calculateCost (ItemData data) {
		int cost = 0;
		switch (data.itemType) {
			case ItemType.WEAPON: cost = Mathf.RoundToInt(data.level * ((WeaponData)data).type.cost()); break;
			case ItemType.ENGINE: cost = Mathf.RoundToInt(data.level * ((EngineData)data).type.cost()); break;
			case ItemType.ARMOR: return ((ArmorData)data).type.cost();
			case ItemType.GENERATOR: cost = Mathf.RoundToInt(data.level * ((GeneratorData)data).type.cost()); break;
			case ItemType.RADAR: cost = Mathf.RoundToInt(data.level * ((RadarData)data).type.cost()); break;
			case ItemType.SHIELD: cost = Mathf.RoundToInt(data.level * ((ShieldData)data).type.cost()); break;
			case ItemType.REPAIR_DROID: cost = Mathf.RoundToInt(data.level * ((RepairDroidData)data).type.cost()); break;
			case ItemType.HARVESTER: cost = Mathf.RoundToInt(data.level * ((HarvesterData)data).type.cost()); break;
			case ItemType.HAND_WEAPON: cost = Mathf.RoundToInt(data.level * ((HandWeaponData)data).type.cost()); break;
			case ItemType.BODY_ARMOR:
				BodyArmorData bad = (BodyArmorData)data;
				return Mathf.RoundToInt(bad.type.cost() + (bad.armorClass * 10));
			default: Debug.Log("Unknown type: " + data.itemType); break;
		}
		return Mathf.RoundToInt (cost * qualityMultiplier (data.quality));
	}

	private static int calculateEnergy (ItemData data) {
		switch (data.itemType) {
			case ItemType.WEAPON: return Mathf.RoundToInt(data.level * ((WeaponData)data).type.energyNeeded());
			case ItemType.ENGINE: return Mathf.RoundToInt(data.level * ((EngineData)data).type.energyNeeded());
			case ItemType.RADAR: return Mathf.RoundToInt(data.level * ((RadarData)data).type.energyNeeded());
			case ItemType.SHIELD: return Mathf.RoundToInt(data.level * ((ShieldData)data).type.energyNeeded());
			case ItemType.REPAIR_DROID: return Mathf.RoundToInt(data.level * ((RepairDroidData)data).type.energyNeeded());
			default: return 0;
		}
	}

	public static GoodsData createGoodsData (int quantity) {
		GoodsType type = GoodsType.JEWELRY;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(GoodsType)).Length)) {
			case 0: type = GoodsType.JEWELRY; break;
			case 1: type = GoodsType.PRECIOUS_METALS; break;
			case 2: type = GoodsType.BOOZE; break;
			case 3: type = GoodsType.ELECTRONICS; break;
			case 4: type = GoodsType.MEAL; break;
			default: Debug.Log("Unmapped value for goods"); break;
		}
		return createGoodsData (type, quantity);
	}

	public static GoodsData createGoodsData (GoodsType type, int quantity) {
		GoodsData data = new GoodsData(type, quantity);
		data.initCommons(type.cost(), 0);

		return data;
	}

	public static HandWeaponData createHandWeaponData () {
		HandWeaponType type = HandWeaponType.GUN;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(HandWeaponType)).Length)) {
			case 0: type = HandWeaponType.GUN; break;
			case 1: type = HandWeaponType.REVOLVER; break;
			case 2: type = HandWeaponType.MINIGUN; break;
			case 3: type = HandWeaponType.GAUSSE; break;
			case 4: type = HandWeaponType.RAILGUN; break;
			default: Debug.Log("Unmapped value for hand weapon"); break;
		}
		return createHandWeaponData(type);
	}

	public static HandWeaponData createHandWeaponData (HandWeaponType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int damage = Mathf.RoundToInt(type.damage() * level * qualityMultiplier(quality));
		int minDamage = damage - Mathf.RoundToInt((float)damage * .25f);
		int maxDamage = damage + Mathf.RoundToInt((float)damage * .25f);

		HandWeaponData data = new HandWeaponData(quality, level, type, minDamage, maxDamage);
		data.initCommons(calculateCost(data), 0);

		return data;
	}

	public static BodyArmorData createBodyArmorData () {
		BodyArmorType type = BodyArmorType.SPACESUIT;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(BodyArmorType)).Length)) {
			case 0: type = BodyArmorType.SPACESUIT; break;
			case 1: type = BodyArmorType.HARDENED_SPACESUIT; break;
			case 2: type = BodyArmorType.ARMORED_SPACESUIT; break;
			case 3: type = BodyArmorType.COMBAT_ARMOR; break;
			default: Debug.Log("Unmapped value for body armor"); break;
		}
		return createBodyArmorData (type);
	}

	public static BodyArmorData createBodyArmorData (BodyArmorType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int armorClass = Mathf.RoundToInt(type.armorClass() * level * qualityMultiplier(quality));

		BodyArmorData data = new BodyArmorData(quality, level, type, armorClass);
		data.initCommons(calculateCost(data), 0);

		return data;
	}

	public static WeaponData createWeaponData () {
		WeaponType type = WeaponType.BLASTER;
//		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(WeaponType)).Length)) {
//			case 0: type = WeaponType.BLASTER; break;
//			case 1: type = WeaponType.PLASMER; break;
//			case 2: type = WeaponType.CHARGER; break;
//			case 3: type = WeaponType.EMITTER; break;
//			case 4: type = WeaponType.WAVER; break;
//			case 5: type = WeaponType.LAUNCHER; break;
//			case 6: type = WeaponType.SUPPRESSOR; break;
//			default: Debug.Log("Unmapped value for weapon"); break;
//		}
		return createWeaponData(type);
	}

	public static WeaponData createWeaponData (WeaponType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int damage = Mathf.RoundToInt(type.damage() * level * qualityMultiplier(quality));
		float reloadTime = (type.reloadTime() / level) * (quality == ItemQuality.UNIQUE? 0.6f: quality == ItemQuality.SUPERIOR? 0.8f: 1);

		WeaponData data = new WeaponData(quality, level, type, damage - type.damageRange(), damage + type.damageRange(), reloadTime);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static EngineData createEngineData () {
		EngineType type = EngineType.FORCE;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(EngineType)).Length)) {
			case 0: type = EngineType.FORCE; break;
			case 1: type = EngineType.GRADUAL; break;
			case 2: type = EngineType.PROTON; break;
			case 3: type = EngineType.ALLUR; break;
			case 4: type = EngineType.QUAZAR; break;
			default: Debug.Log("Unmapped value for engine"); break;
		}
		return createEngineData(type);
	}

	public static EngineData createEngineData (EngineType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		float power = type.mainPower() * level * qualityMultiplier(quality);

		EngineData data = new EngineData(quality, level, type, power);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static ArmorData createArmorData () {
		ArmorType type = ArmorType.STEEL;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(ArmorType)).Length)) {
			case 0: type = ArmorType.STEEL; break;
			case 1: type = ArmorType.HARDENED_STEEL; break;
			case 2: type = ArmorType.TITANIUM; break;
			case 3: type = ArmorType.ASTRON; break;
			case 4: type = ArmorType.ADAMANT; break;
			default: Debug.Log("Unmapped value for armor"); break;
		}
		return createArmorData(type);
	}

	public static ArmorData createArmorData (ArmorType type) {
		ArmorData data = new ArmorData(ItemQuality.COMMON, 1, type, type.armorClass());
		data.initCommons(type.cost(), 0);
		return data;
	}

	public static GeneratorData createGeneratorData () {
		GeneratorType type = GeneratorType.ATOMIC;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(GeneratorType)).Length)) {
			case 0: type = GeneratorType.ATOMIC; break;
			case 1: type = GeneratorType.PLASMA; break;	
			case 2: type = GeneratorType.MULTYPHASE; break;	
			case 3: type = GeneratorType.TUNNEL; break;	
			default: Debug.Log("Unmapped value for generator"); break;
		}
		return createGeneratorData(type);
	}

	public static GeneratorData createGeneratorData (GeneratorType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int maxEnergy = Mathf.RoundToInt(type.maxEnergy() * level * qualityMultiplier(quality));

		GeneratorData data = new GeneratorData(quality, level, type, maxEnergy);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}
	
	public static RadarData createRadarData () {
		RadarType type = RadarType.SEQUESTER;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(RadarType)).Length)) {
			case 0: type = RadarType.SEQUESTER; break;
			case 1: type = RadarType.PLANAR; break;
			case 2: type = RadarType.MATRIX; break;
			case 3: type = RadarType.PATAN_CORSAC; break;
			case 4: type = RadarType.SNAKE; break;
			case 5: type = RadarType.ASTRAL; break;	
			default: Debug.Log("Unmapped value for radar"); break;
		}
		return createRadarData(type);
	}

	public static RadarData createRadarData (RadarType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int range = Mathf.RoundToInt(type.range() * level * qualityMultiplier(quality));

		RadarData data = new RadarData(quality, level, type, range);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static ShieldData createShieldData () {
		ShieldType type = ShieldType.BLOCK;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(ShieldType)).Length)) {
			case 0: type = ShieldType.BLOCK; break;
			case 1: type = ShieldType.QUADRATIC; break;
			case 2: type = ShieldType.CELL; break;
			case 3: type = ShieldType.PHASE; break;
			default: Debug.Log("Unmapped value for shield"); break;
		}
		return createShieldData(type);
	}

	public static ShieldData createShieldData (ShieldType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int shieldLevel = Mathf.RoundToInt(type.shieldProtection() * level * qualityMultiplier(quality));
		int rechargeSpeed = Mathf.RoundToInt(type.rechargeSpeed() * level * qualityMultiplier(quality));

		ShieldData data = new ShieldData(quality, level, type, shieldLevel, rechargeSpeed);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static RepairDroidData createRepairDroidData () {
		RepairDroidType type = RepairDroidType.RAIL;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(RepairDroidType)).Length)) {
			case 0: type = RepairDroidType.RAIL; break;
			case 1: type = RepairDroidType.CHANNEL; break;
			case 2: type = RepairDroidType.BIPHASIC; break;
			case 3: type = RepairDroidType.THREAD; break;
			default: Debug.Log("Unmapped value for repair droid"); break;
		}
		return createRepairDroidData(type);
	}

	public static RepairDroidData createRepairDroidData (RepairDroidType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int repairSpeed = Mathf.RoundToInt(type.repairSpeed() * level * qualityMultiplier(quality));

		RepairDroidData data = new RepairDroidData(quality, level, type, repairSpeed);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static HarvesterData createHarvesterData () {
		HarvesterType type = HarvesterType.MECHANICAL;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(HarvesterType)).Length)) {
			case 0: type = HarvesterType.MECHANICAL; break;
			case 1: type = HarvesterType.PLASMATIC; break;
			case 2: type = HarvesterType.GENERATIVE; break;
			default: Debug.Log("Unmapped value for harvester"); break;
		}
		return createHarvesterData(type);
	}

	public static HarvesterData createHarvesterData (HarvesterType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		float harvestMulty = quality == ItemQuality.UNIQUE? 0.6f: quality == ItemQuality.SUPERIOR? 0.8f: 1;
		int harvestTime = Mathf.RoundToInt((type.harvestTime() / level) * harvestMulty);

		HarvesterData data = new HarvesterData(quality, level, type, harvestTime);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}
}