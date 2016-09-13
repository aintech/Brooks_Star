using UnityEngine;
using System;
using System.Collections;

public static class ItemFactory {

	private static float qualityMult = 0;

	public static void createItemData (InventoryItem item, InventoryItem.Type type) {



		float maxRand = type == InventoryItem.Type.WEAPON? Enum.GetNames(typeof(WeaponType)).Length:
						type == InventoryItem.Type.ENGINE?  Enum.GetNames(typeof(EngineType)).Length:
						type == InventoryItem.Type.ARMOR?  Enum.GetNames(typeof(ArmorType)).Length:
						type == InventoryItem.Type.GENERATOR?  Enum.GetNames(typeof(GeneratorType)).Length:
						type == InventoryItem.Type.RADAR?  Enum.GetNames(typeof(RadarType)).Length:
						type == InventoryItem.Type.SHIELD?  Enum.GetNames(typeof(ShieldType)).Length:
						type == InventoryItem.Type.REPAIR_DROID?  Enum.GetNames(typeof(RepairDroidType)).Length:
						type == InventoryItem.Type.HARVESTER?  Enum.GetNames(typeof(HarvesterType)).Length:
						-1;

		int rand = Mathf.RoundToInt (UnityEngine.Random.value * maxRand);
		
		setItemQualityAndLevel (item, type);

		InventoryItem.ItemData data = null;

		switch (type) {
			case InventoryItem.Type.WEAPON: data = createWeaponData (item, rand); break;
			case InventoryItem.Type.ENGINE: data = createEngineData (item, rand); break;
			case InventoryItem.Type.ARMOR: data = createArmorData (item, rand); break;
			case InventoryItem.Type.GENERATOR: data = createGeneratorData (item, rand); break;
			case InventoryItem.Type.RADAR: data = createRadarData (item, rand); break;
			case InventoryItem.Type.SHIELD: data = createShieldData (item, rand); break;
			case InventoryItem.Type.REPAIR_DROID: data = createRepairDroidData (item, rand); break;
			case InventoryItem.Type.HARVESTER: data = createHarvesterData (item, rand); break;
		}

		setItemValues (item, data, type);
	}

	private static void setItemQualityAndLevel (InventoryItem item, InventoryItem.Type type) {
		if (type == InventoryItem.Type.ARMOR) {
			item.setItemLevel(1);
			item.setItemQuality(InventoryItem.Quality.NORMAL);
		} else {
			int randQuality = Mathf.RoundToInt (UnityEngine.Random.value * 100);
			
			if (randQuality > 90) item.setItemQuality(InventoryItem.Quality.UNIQUE);
			else if (randQuality > 60) item.setItemQuality(InventoryItem.Quality.SUPERIOR);
			else item.setItemQuality(InventoryItem.Quality.NORMAL);

			item.setItemLevel(1 + (UnityEngine.Random.value * 0.3f));
		}

		qualityMult = item.getItemQuality() == InventoryItem.Quality.UNIQUE? 3: item.getItemQuality() == InventoryItem.Quality.SUPERIOR? 2: 1;
	}

	private static void setItemValues (InventoryItem item, InventoryItem.ItemData data, InventoryItem.Type type) {
		item.setCost (calculateCost (data, type, item.getItemLevel(), item.getItemQuality()));
		item.setEnergyNeeded(calculateEnergy(data, type, item.getItemLevel()));
		item.setItemData(data);
	}

	public static void createWeaponData (InventoryItem item, WeaponType type) {
		setItemQualityAndLevel (item, InventoryItem.Type.WEAPON);
		InventoryItem.WeaponData data = createWeaponData (item, type == WeaponType.Blaster? 0:
		                                                  		type == WeaponType.Plasmer? 1:
		                                                  		type == WeaponType.Charger? 2:
		                                                  		type == WeaponType.Emitter? 3:
		                                                  		type == WeaponType.Waver? 4:
		                                                  		type == WeaponType.Launcher? 5: 6);

		setItemValues(item, data, InventoryItem.Type.WEAPON);
	}

	private static InventoryItem.WeaponData createWeaponData (InventoryItem item, int random) {
		WeaponType type = WeaponType.Blaster;
		switch (random) {
			case 0: type = WeaponType.Blaster; break;
			case 1: type = WeaponType.Plasmer; break;
			case 2: type = WeaponType.Charger; break;
			case 3: type = WeaponType.Emitter; break;
			case 4: type = WeaponType.Waver; break;
			case 5: type = WeaponType.Launcher; break;
			case 6: type = WeaponType.Suppressor; break;
		}

		float reloadMulty = item.getItemQuality() == InventoryItem.Quality.UNIQUE? 0.6f: item.getItemQuality() == InventoryItem.Quality.SUPERIOR? 0.8f: 1;

		int damage = Mathf.RoundToInt(type.getDamage() * item.getItemLevel() * qualityMult);
		float reloadTime = (type.getReloadTime() / item.getItemLevel()) * reloadMulty;
		item.setVolume (type.getVolume ());
		return new InventoryItem.WeaponData (type, damage - type.getDamageRange(), damage + type.getDamageRange(), reloadTime);
	}

	private static InventoryItem.EngineData createEngineData (InventoryItem item, int random) {
		EngineType type = EngineType.Force;
		switch (random) {
			case 0: type = EngineType.Force; break;
			case 1: type = EngineType.Gradual; break;
			case 2: type = EngineType.Proton; break;
			case 3: type = EngineType.Allur; break;
			case 4: type = EngineType.Quazar; break;
		}

		float power = type.getMainPower() * item.getItemLevel() * qualityMult;

		item.setVolume (type.getVolume());
		return new InventoryItem.EngineData (type, power);
	}
	
	private static InventoryItem.ArmorData createArmorData (InventoryItem item, int random) {
		ArmorType type = ArmorType.Steel;
		switch (random) {
			case 0: type = ArmorType.Steel; break;
			case 1: type = ArmorType.HardenedSteel; break;
			case 2: type = ArmorType.Titan; break;
			case 3: type = ArmorType.Astron; break;
			case 4: type = ArmorType.Adamant; break;	
		}
		item.setVolume (type.getVolume());
		return new InventoryItem.ArmorData (type, type.getArmorClass());
	}

	private static InventoryItem.GeneratorData createGeneratorData (InventoryItem item, int random) {
		GeneratorType type = GeneratorType.Atomic;
		switch (random) {
			case 0: type = GeneratorType.Atomic; break;
			case 1: type = GeneratorType.Plasma; break;	
			case 2: type = GeneratorType.Multyphase; break;	
			case 3: type = GeneratorType.Tunnel; break;	
		}

		int maxEnergy = Mathf.RoundToInt(type.getMaxEnergy() * item.getItemLevel() * qualityMult);

		item.setVolume (type.getVolume());
		return new InventoryItem.GeneratorData (type, maxEnergy);
	}
	
	private static InventoryItem.RadarData createRadarData (InventoryItem item, int random) {
		RadarType type = RadarType.Sequester;
		switch (random) {
			case 0: type = RadarType.Sequester; break;
			case 1: type = RadarType.Planar; break;
			case 2: type = RadarType.Matrix; break;
			case 3: type = RadarType.PatanCorsac; break;
			case 4: type = RadarType.Snake; break;
			case 5: type = RadarType.Astral; break;	
		}

		int range = Mathf.RoundToInt(type.getRange() * item.getItemLevel() * qualityMult);

		item.setVolume (type.getVolume());
		return new InventoryItem.RadarData (type, range);
	}
	
	private static InventoryItem.ShieldData createShieldData (InventoryItem item, int random) {
		ShieldType type = ShieldType.Block;
		switch (random) {
			case 0: type = ShieldType.Block; break;
			case 1: type = ShieldType.Quadratic; break;
			case 2: type = ShieldType.Cell; break;
			case 3: type = ShieldType.Phase; break;
		}

		int shieldLevel = Mathf.RoundToInt(type.getShieldProtection() * item.getItemLevel() * qualityMult);
		int rechargeSpeed = Mathf.RoundToInt(type.getRechargeSpeed() * item.getItemLevel() * qualityMult);

		item.setVolume (type.getVolume());
		return new InventoryItem.ShieldData (type, shieldLevel, rechargeSpeed);
	}

	private static InventoryItem.RepairDroidData createRepairDroidData (InventoryItem item, int random) {
		RepairDroidType type = RepairDroidType.Rail;
		switch (random) {
			case 0: type = RepairDroidType.Rail; break;
			case 1: type = RepairDroidType.Channel; break;
			case 2: type = RepairDroidType.Biphasic; break;
			case 3: type = RepairDroidType.Thread; break;	
		}

		int repairSpeed = Mathf.RoundToInt(type.getRepairSpeed() * item.getItemLevel() * qualityMult);

		item.setVolume (type.getVolume());
		return new InventoryItem.RepairDroidData (type, repairSpeed);
	}
	
	private static InventoryItem.HarvesterData createHarvesterData (InventoryItem item, int random) {
		HarvesterType type = HarvesterType.Mechanical;
		switch (random) {
			case 0: type = HarvesterType.Mechanical; break;
			case 1: type = HarvesterType.Plasmatic; break;
			case 2: type = HarvesterType.Generative; break;	
		}

		float harvestMulty = item.getItemQuality() == InventoryItem.Quality.UNIQUE? 0.6f: item.getItemQuality() == InventoryItem.Quality.SUPERIOR? 0.8f: 1;
		int harvestTime = Mathf.RoundToInt(type.getHarvestTime() / item.getItemLevel() * harvestMulty);

		item.setVolume (type.getVolume());
		return new InventoryItem.HarvesterData (type, harvestTime);
	}

	private static int calculateCost (InventoryItem.ItemData data, InventoryItem.Type type, float itemLevel, InventoryItem.Quality itemQuality) {
		int cost = 0;
		switch (type) {
			case InventoryItem.Type.WEAPON: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.WeaponData)data).getType().getCost()); break;
			case InventoryItem.Type.ENGINE: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.EngineData)data).getType().getCost()); break;
			case InventoryItem.Type.ARMOR: return ((InventoryItem.ArmorData)data).getType().getCost();
			case InventoryItem.Type.GENERATOR: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.GeneratorData)data).getType().getCost()); break;
			case InventoryItem.Type.RADAR: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.RadarData)data).getType().getCost()); break;
			case InventoryItem.Type.SHIELD: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.ShieldData)data).getType().getCost()); break;
			case InventoryItem.Type.REPAIR_DROID: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.RepairDroidData)data).getType().getCost()); break;
			case InventoryItem.Type.HARVESTER: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.HarvesterData)data).getType().getCost()); break;
			default: cost = 0; break;
		}
		return Mathf.RoundToInt(cost * (itemQuality == InventoryItem.Quality.UNIQUE? 2.5f: itemQuality == InventoryItem.Quality.SUPERIOR? 1.5f: 1));
	}

	private static int calculateEnergy (InventoryItem.ItemData data, InventoryItem.Type type, float itemLevel) {
		switch (type) {
			case InventoryItem.Type.WEAPON: return Mathf.RoundToInt(itemLevel * ((InventoryItem.WeaponData)data).getType().getEnergyNeeded());//randomizeValue(((InventoryItem.WeaponData)data).getType().getEnergyNeeded());
			case InventoryItem.Type.ENGINE: return Mathf.RoundToInt(itemLevel * ((InventoryItem.EngineData)data).getType().getEnergyNeeded());
			case InventoryItem.Type.RADAR: return Mathf.RoundToInt(itemLevel * ((InventoryItem.RadarData)data).getType().getEnergyNeeded());
			case InventoryItem.Type.SHIELD: return Mathf.RoundToInt(itemLevel * ((InventoryItem.ShieldData)data).getType().getEnergyNeeded());
			case InventoryItem.Type.REPAIR_DROID: return Mathf.RoundToInt(itemLevel * ((InventoryItem.RepairDroidData)data).getType().getEnergyNeeded());
			default: return 0;
		}
	}

	private static int randomizeValue (int value) {
		return Utils.getRandomValue(value, 30);
	}
}