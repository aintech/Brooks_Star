using UnityEngine;
using System;
using System.Collections;

public static class ItemFactory {

	public static ItemData createItemData (ItemData.Type type) {
		switch (type) {
			case ItemData.Type.WEAPON: return createWeaponData ();
			case ItemData.Type.ENGINE: return createEngineData ();
			case ItemData.Type.ARMOR: return createArmorData ();
			case ItemData.Type.GENERATOR: return createGeneratorData ();
			case ItemData.Type.RADAR: return createRadarData ();
			case ItemData.Type.SHIELD: return createShieldData ();
			case ItemData.Type.REPAIR_DROID: return createRepairDroidData ();
			case ItemData.Type.HARVESTER: return createHarvesterData ();
			default: Debug.Log("Unknown type: " + type); return null;
		}
	}

	private static ItemData.Quality randQuality () {
		float rand = UnityEngine.Random.value;
		return rand >= .9f? ItemData.Quality.UNIQUE: rand >= .6f? ItemData.Quality.SUPERIOR: ItemData.Quality.NORMAL;
	}

	private static float randLevel () {
		return 1 + (UnityEngine.Random.value * .3f);
	}

	private static float qualityMultiplier (ItemData.Quality quality) {
		return quality == ItemData.Quality.UNIQUE? 3: quality == ItemData.Quality.SUPERIOR? 2: 1;
	}

	private static int calculateCost (ItemData data) {
		int cost = 0;
		switch (data.itemType) {
			case ItemData.Type.WEAPON: cost = Mathf.RoundToInt(data.level * ((WeaponData)data).type.getCost()); break;
			case ItemData.Type.ENGINE: cost = Mathf.RoundToInt(data.level * ((EngineData)data).type.getCost()); break;
			case ItemData.Type.ARMOR: return ((ArmorData)data).type.getCost();
			case ItemData.Type.GENERATOR: cost = Mathf.RoundToInt(data.level * ((GeneratorData)data).type.getCost()); break;
			case ItemData.Type.RADAR: cost = Mathf.RoundToInt(data.level * ((RadarData)data).type.getCost()); break;
			case ItemData.Type.SHIELD: cost = Mathf.RoundToInt(data.level * ((ShieldData)data).type.getCost()); break;
			case ItemData.Type.REPAIR_DROID: cost = Mathf.RoundToInt(data.level * ((RepairDroidData)data).type.getCost()); break;
			case ItemData.Type.HARVESTER: cost = Mathf.RoundToInt(data.level * ((HarvesterData)data).type.getCost()); break;
			default: Debug.Log("Unknown type: " + data.itemType); break;
		}
		return Mathf.RoundToInt(cost * (data.quality == ItemData.Quality.UNIQUE? 2.5f: data.quality == ItemData.Quality.SUPERIOR? 1.5f: 1));
	}

	private static int calculateEnergy (ItemData data) {
		switch (data.itemType) {
			case ItemData.Type.WEAPON: return Mathf.RoundToInt(data.level * ((WeaponData)data).type.getEnergyNeeded());
			case ItemData.Type.ENGINE: return Mathf.RoundToInt(data.level * ((EngineData)data).type.getEnergyNeeded());
			case ItemData.Type.RADAR: return Mathf.RoundToInt(data.level * ((RadarData)data).type.getEnergyNeeded());
			case ItemData.Type.SHIELD: return Mathf.RoundToInt(data.level * ((ShieldData)data).type.getEnergyNeeded());
			case ItemData.Type.REPAIR_DROID: return Mathf.RoundToInt(data.level * ((RepairDroidData)data).type.getEnergyNeeded());
			default: return 0;
		}
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
		ItemData.Quality quality = randQuality();
		float level = randLevel();

		int damage = Mathf.RoundToInt(type.getDamage() * level * qualityMultiplier(quality));
		float reloadTime = (type.getReloadTime() / level) * (quality == ItemData.Quality.UNIQUE? 0.6f: quality == ItemData.Quality.SUPERIOR? 0.8f: 1);

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
		ItemData.Quality quality = randQuality();
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
		ArmorData data = new ArmorData(ItemData.Quality.NORMAL, 1, type, type.getArmorClass());
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
		ItemData.Quality quality = randQuality();
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
		ItemData.Quality quality = randQuality();
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
		ItemData.Quality quality = randQuality();
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
		ItemData.Quality quality = randQuality();
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
		ItemData.Quality quality = randQuality();
		float level = randLevel();

		float harvestMulty = quality == ItemData.Quality.UNIQUE? 0.6f: quality == ItemData.Quality.SUPERIOR? 0.8f: 1;
		int harvestTime = Mathf.RoundToInt((type.getHarvestTime() / level) * harvestMulty);

		HarvesterData data = new HarvesterData(quality, level, type, harvestTime);
		data.initCommons(calculateCost(data), calculateEnergy(data));

		return data;
	}
}