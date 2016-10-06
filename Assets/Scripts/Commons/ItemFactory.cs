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
			case ItemType.WEAPON: cost = Mathf.RoundToInt(data.level * ((WeaponData)data).type.getCost()); break;
			case ItemType.ENGINE: cost = Mathf.RoundToInt(data.level * ((EngineData)data).type.getCost()); break;
			case ItemType.ARMOR: return ((ArmorData)data).type.getCost();
			case ItemType.GENERATOR: cost = Mathf.RoundToInt(data.level * ((GeneratorData)data).type.getCost()); break;
			case ItemType.RADAR: cost = Mathf.RoundToInt(data.level * ((RadarData)data).type.getCost()); break;
			case ItemType.SHIELD: cost = Mathf.RoundToInt(data.level * ((ShieldData)data).type.getCost()); break;
			case ItemType.REPAIR_DROID: cost = Mathf.RoundToInt(data.level * ((RepairDroidData)data).type.getCost()); break;
			case ItemType.HARVESTER: cost = Mathf.RoundToInt(data.level * ((HarvesterData)data).type.getCost()); break;
			case ItemType.HAND_WEAPON: cost = Mathf.RoundToInt(data.level * ((HandWeaponData)data).type.getCost()); break;
			case ItemType.BODY_ARMOR:
				BodyArmorData bad = (BodyArmorData)data;
				return Mathf.RoundToInt(bad.type.getCost() + (bad.armorClass * 10));
			default: Debug.Log("Unknown type: " + data.itemType); break;
		}
		return Mathf.RoundToInt (cost * qualityMultiplier (data.quality));
	}

	private static int calculateEnergy (ItemData data) {
		switch (data.itemType) {
			case ItemType.WEAPON: return Mathf.RoundToInt(data.level * ((WeaponData)data).type.getEnergyNeeded());
			case ItemType.ENGINE: return Mathf.RoundToInt(data.level * ((EngineData)data).type.getEnergyNeeded());
			case ItemType.RADAR: return Mathf.RoundToInt(data.level * ((RadarData)data).type.getEnergyNeeded());
			case ItemType.SHIELD: return Mathf.RoundToInt(data.level * ((ShieldData)data).type.getEnergyNeeded());
			case ItemType.REPAIR_DROID: return Mathf.RoundToInt(data.level * ((RepairDroidData)data).type.getEnergyNeeded());
			default: return 0;
		}
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

		int damage = Mathf.RoundToInt(type.getDamage() * level * qualityMultiplier(quality));
		int minDamage = damage - Mathf.RoundToInt((float)damage * .25f);
		int maxDamage = damage + Mathf.RoundToInt((float)damage * .25f);

		HandWeaponData data = new HandWeaponData(quality, level, type, minDamage, maxDamage);
		data.initCommons(calculateCost(data), 0);

		return data;
	}

	public static BodyArmorData createBodyArmorData () {
		BodyArmorType type = BodyArmorType.SUIT;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(BodyArmorType)).Length)) {
			case 0: type = BodyArmorType.SUIT; break;
			case 1: type = BodyArmorType.METAL; break;
			case 2: type = BodyArmorType.HEAVY; break;
			default: Debug.Log("Unmapped value for body armor"); break;
		}
		return createBodyArmorData (type);
	}

	public static BodyArmorData createBodyArmorData (BodyArmorType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int armorClass = Mathf.RoundToInt(type.getArmorClass() * level * qualityMultiplier(quality));

		BodyArmorData data = new BodyArmorData(quality, level, type, armorClass);
		data.initCommons(calculateCost(data), 0);

		return data;
	}

	public static WeaponData createWeaponData () {
		WeaponType type = WeaponType.Blaster;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(WeaponType)).Length)) {
			case 0: type = WeaponType.Blaster; break;
			case 1: type = WeaponType.Plasmer; break;
			case 2: type = WeaponType.Charger; break;
			case 3: type = WeaponType.Emitter; break;
			case 4: type = WeaponType.Waver; break;
			case 5: type = WeaponType.Launcher; break;
			case 6: type = WeaponType.Suppressor; break;
			default: Debug.Log("Unmapped value for weapon"); break;
		}
		return createWeaponData(type);
	}

	public static WeaponData createWeaponData (WeaponType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int damage = Mathf.RoundToInt(type.getDamage() * level * qualityMultiplier(quality));
		float reloadTime = (type.getReloadTime() / level) * (quality == ItemQuality.UNIQUE? 0.6f: quality == ItemQuality.SUPERIOR? 0.8f: 1);

		WeaponData data = new WeaponData(quality, level, type, damage - type.getDamageRange(), damage + type.getDamageRange(), reloadTime);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static EngineData createEngineData () {
		EngineType type = EngineType.Force;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(EngineType)).Length)) {
			case 0: type = EngineType.Force; break;
			case 1: type = EngineType.Gradual; break;
			case 2: type = EngineType.Proton; break;
			case 3: type = EngineType.Allur; break;
			case 4: type = EngineType.Quazar; break;
			default: Debug.Log("Unmapped value for engine"); break;
		}
		return createEngineData(type);
	}

	public static EngineData createEngineData (EngineType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		float power = type.getMainPower() * level * qualityMultiplier(quality);

		EngineData data = new EngineData(quality, level, type, power);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static ArmorData createArmorData () {
		ArmorType type = ArmorType.Steel;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(ArmorType)).Length)) {
			case 0: type = ArmorType.Steel; break;
			case 1: type = ArmorType.HardenedSteel; break;
			case 2: type = ArmorType.Titan; break;
			case 3: type = ArmorType.Astron; break;
			case 4: type = ArmorType.Adamant; break;
			default: Debug.Log("Unmapped value for armor"); break;
		}
		return createArmorData(type);
	}

	public static ArmorData createArmorData (ArmorType type) {
		ArmorData data = new ArmorData(ItemQuality.COMMON, 1, type, type.getArmorClass());
		data.initCommons(type.getCost(), 0);
		return data;
	}

	public static GeneratorData createGeneratorData () {
		GeneratorType type = GeneratorType.Atomic;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(GeneratorType)).Length)) {
			case 0: type = GeneratorType.Atomic; break;
			case 1: type = GeneratorType.Plasma; break;	
			case 2: type = GeneratorType.Multyphase; break;	
			case 3: type = GeneratorType.Tunnel; break;	
			default: Debug.Log("Unmapped value for generator"); break;
		}
		return createGeneratorData(type);
	}

	public static GeneratorData createGeneratorData (GeneratorType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int maxEnergy = Mathf.RoundToInt(type.getMaxEnergy() * level * qualityMultiplier(quality));

		GeneratorData data = new GeneratorData(quality, level, type, maxEnergy);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}
	
	public static RadarData createRadarData () {
		RadarType type = RadarType.Sequester;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(RadarType)).Length)) {
			case 0: type = RadarType.Sequester; break;
			case 1: type = RadarType.Planar; break;
			case 2: type = RadarType.Matrix; break;
			case 3: type = RadarType.PatanCorsac; break;
			case 4: type = RadarType.Snake; break;
			case 5: type = RadarType.Astral; break;	
			default: Debug.Log("Unmapped value for radar"); break;
		}
		return createRadarData(type);
	}

	public static RadarData createRadarData (RadarType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int range = Mathf.RoundToInt(type.getRange() * level * qualityMultiplier(quality));

		RadarData data = new RadarData(quality, level, type, range);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static ShieldData createShieldData () {
		ShieldType type = ShieldType.Block;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(ShieldType)).Length)) {
			case 0: type = ShieldType.Block; break;
			case 1: type = ShieldType.Quadratic; break;
			case 2: type = ShieldType.Cell; break;
			case 3: type = ShieldType.Phase; break;
			default: Debug.Log("Unmapped value for shield"); break;
		}
		return createShieldData(type);
	}

	public static ShieldData createShieldData (ShieldType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int shieldLevel = Mathf.RoundToInt(type.getShieldProtection() * level * qualityMultiplier(quality));
		int rechargeSpeed = Mathf.RoundToInt(type.getRechargeSpeed() * level * qualityMultiplier(quality));

		ShieldData data = new ShieldData(quality, level, type, shieldLevel, rechargeSpeed);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static RepairDroidData createRepairDroidData () {
		RepairDroidType type = RepairDroidType.Rail;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(RepairDroidType)).Length)) {
			case 0: type = RepairDroidType.Rail; break;
			case 1: type = RepairDroidType.Channel; break;
			case 2: type = RepairDroidType.Biphasic; break;
			case 3: type = RepairDroidType.Thread; break;
			default: Debug.Log("Unmapped value for repair droid"); break;
		}
		return createRepairDroidData(type);
	}

	public static RepairDroidData createRepairDroidData (RepairDroidType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		int repairSpeed = Mathf.RoundToInt(type.getRepairSpeed() * level * qualityMultiplier(quality));

		RepairDroidData data = new RepairDroidData(quality, level, type, repairSpeed);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}

	public static HarvesterData createHarvesterData () {
		HarvesterType type = HarvesterType.Mechanical;
		switch (UnityEngine.Random.Range(0, Enum.GetNames(typeof(HarvesterType)).Length)) {
			case 0: type = HarvesterType.Mechanical; break;
			case 1: type = HarvesterType.Plasmatic; break;
			case 2: type = HarvesterType.Generative; break;
			default: Debug.Log("Unmapped value for harvester"); break;
		}
		return createHarvesterData(type);
	}

	public static HarvesterData createHarvesterData (HarvesterType type) {
		ItemQuality quality = randQuality();
		float level = randLevel();

		float harvestMulty = quality == ItemQuality.UNIQUE? 0.6f: quality == ItemQuality.SUPERIOR? 0.8f: 1;
		int harvestTime = Mathf.RoundToInt((type.getHarvestTime() / level) * harvestMulty);

		HarvesterData data = new HarvesterData(quality, level, type, harvestTime);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}
}