using UnityEngine;
using System;
using System.Collections;

public static class ItemFactory {

	private static float qualityMult = 0;

	public static void createItemData (Item item, Item.Type type) {



		float maxRand = type == Item.Type.WEAPON? Enum.GetNames(typeof(WeaponType)).Length:
						type == Item.Type.ENGINE?  Enum.GetNames(typeof(EngineType)).Length:
						type == Item.Type.ARMOR?  Enum.GetNames(typeof(ArmorType)).Length:
						type == Item.Type.GENERATOR?  Enum.GetNames(typeof(GeneratorType)).Length:
						type == Item.Type.RADAR?  Enum.GetNames(typeof(RadarType)).Length:
						type == Item.Type.SHIELD?  Enum.GetNames(typeof(ShieldType)).Length:
						type == Item.Type.REPAIR_DROID?  Enum.GetNames(typeof(RepairDroidType)).Length:
						type == Item.Type.HARVESTER?  Enum.GetNames(typeof(HarvesterType)).Length:
						-1;

		int rand = Mathf.RoundToInt (UnityEngine.Random.value * maxRand);
		
		setItemQualityAndLevel (item, type);

		Item.ItemData data = null;

		switch (type) {
			case Item.Type.WEAPON: data = createWeaponData (item, rand); break;
			case Item.Type.ENGINE: data = createEngineData (item, rand); break;
			case Item.Type.ARMOR: data = createArmorData (item, rand); break;
			case Item.Type.GENERATOR: data = createGeneratorData (item, rand); break;
			case Item.Type.RADAR: data = createRadarData (item, rand); break;
			case Item.Type.SHIELD: data = createShieldData (item, rand); break;
			case Item.Type.REPAIR_DROID: data = createRepairDroidData (item, rand); break;
			case Item.Type.HARVESTER: data = createHarvesterData (item, rand); break;
		}

		setItemValues (item, data, type);
	}

	private static void setItemQualityAndLevel (Item item, Item.Type type) {
		if (type == Item.Type.ARMOR) {
			item.setItemLevel(1);
			item.setItemQuality(Item.Quality.NORMAL);
		} else {
			int randQuality = Mathf.RoundToInt (UnityEngine.Random.value * 100);
			
			if (randQuality > 90) item.setItemQuality(Item.Quality.UNIQUE);
			else if (randQuality > 60) item.setItemQuality(Item.Quality.SUPERIOR);
			else item.setItemQuality(Item.Quality.NORMAL);

			item.setItemLevel(1 + (UnityEngine.Random.value * 0.3f));
		}

		qualityMult = item.getItemQuality() == Item.Quality.UNIQUE? 3: item.getItemQuality() == Item.Quality.SUPERIOR? 2: 1;
	}

	private static void setItemValues (Item item, Item.ItemData data, Item.Type type) {
		item.setCost (calculateCost (data, type, item.getItemLevel(), item.getItemQuality()));
		item.setEnergyNeeded(calculateEnergy(data, type, item.getItemLevel()));
		item.setItemData(data);
	}

	public static void createWeaponData (Item item, WeaponType type) {
		setItemQualityAndLevel (item, Item.Type.WEAPON);
		Item.WeaponData data = createWeaponData (item, type == WeaponType.Blaster? 0:
		                                                  		type == WeaponType.Plasmer? 1:
		                                                  		type == WeaponType.Charger? 2:
		                                                  		type == WeaponType.Emitter? 3:
		                                                  		type == WeaponType.Waver? 4:
		                                                  		type == WeaponType.Launcher? 5: 6);

		setItemValues(item, data, Item.Type.WEAPON);
	}

	private static Item.WeaponData createWeaponData (Item item, int random) {
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

		float reloadMulty = item.getItemQuality() == Item.Quality.UNIQUE? 0.6f: item.getItemQuality() == Item.Quality.SUPERIOR? 0.8f: 1;

		int damage = Mathf.RoundToInt(type.getDamage() * item.getItemLevel() * qualityMult);
		float reloadTime = (type.getReloadTime() / item.getItemLevel()) * reloadMulty;
		item.setVolume (type.getVolume ());
		return new Item.WeaponData (type, damage - type.getDamageRange(), damage + type.getDamageRange(), reloadTime);
	}

	private static Item.EngineData createEngineData (Item item, int random) {
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
		return new Item.EngineData (type, power);
	}
	
	private static Item.ArmorData createArmorData (Item item, int random) {
		ArmorType type = ArmorType.Steel;
		switch (random) {
			case 0: type = ArmorType.Steel; break;
			case 1: type = ArmorType.HardenedSteel; break;
			case 2: type = ArmorType.Titan; break;
			case 3: type = ArmorType.Astron; break;
			case 4: type = ArmorType.Adamant; break;	
		}
		item.setVolume (type.getVolume());
		return new Item.ArmorData (type, type.getArmorClass());
	}

	private static Item.GeneratorData createGeneratorData (Item item, int random) {
		GeneratorType type = GeneratorType.Atomic;
		switch (random) {
			case 0: type = GeneratorType.Atomic; break;
			case 1: type = GeneratorType.Plasma; break;	
			case 2: type = GeneratorType.Multyphase; break;	
			case 3: type = GeneratorType.Tunnel; break;	
		}

		int maxEnergy = Mathf.RoundToInt(type.getMaxEnergy() * item.getItemLevel() * qualityMult);

		item.setVolume (type.getVolume());
		return new Item.GeneratorData (type, maxEnergy);
	}
	
	private static Item.RadarData createRadarData (Item item, int random) {
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
		return new Item.RadarData (type, range);
	}
	
	private static Item.ShieldData createShieldData (Item item, int random) {
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
		return new Item.ShieldData (type, shieldLevel, rechargeSpeed);
	}

	private static Item.RepairDroidData createRepairDroidData (Item item, int random) {
		RepairDroidType type = RepairDroidType.Rail;
		switch (random) {
			case 0: type = RepairDroidType.Rail; break;
			case 1: type = RepairDroidType.Channel; break;
			case 2: type = RepairDroidType.Biphasic; break;
			case 3: type = RepairDroidType.Thread; break;	
		}

		int repairSpeed = Mathf.RoundToInt(type.getRepairSpeed() * item.getItemLevel() * qualityMult);

		item.setVolume (type.getVolume());
		return new Item.RepairDroidData (type, repairSpeed);
	}
	
	private static Item.HarvesterData createHarvesterData (Item item, int random) {
		HarvesterType type = HarvesterType.Mechanical;
		switch (random) {
			case 0: type = HarvesterType.Mechanical; break;
			case 1: type = HarvesterType.Plasmatic; break;
			case 2: type = HarvesterType.Generative; break;	
		}

		float harvestMulty = item.getItemQuality() == Item.Quality.UNIQUE? 0.6f: item.getItemQuality() == Item.Quality.SUPERIOR? 0.8f: 1;
		int harvestTime = Mathf.RoundToInt(type.getHarvestTime() / item.getItemLevel() * harvestMulty);

		item.setVolume (type.getVolume());
		return new Item.HarvesterData (type, harvestTime);
	}

	private static int calculateCost (Item.ItemData data, Item.Type type, float itemLevel, Item.Quality itemQuality) {
		int cost = 0;
		switch (type) {
			case Item.Type.WEAPON: cost = Mathf.RoundToInt(itemLevel * ((Item.WeaponData)data).getType().getCost()); break;
			case Item.Type.ENGINE: cost = Mathf.RoundToInt(itemLevel * ((Item.EngineData)data).getType().getCost()); break;
			case Item.Type.ARMOR: return ((Item.ArmorData)data).getType().getCost();
			case Item.Type.GENERATOR: cost = Mathf.RoundToInt(itemLevel * ((Item.GeneratorData)data).getType().getCost()); break;
			case Item.Type.RADAR: cost = Mathf.RoundToInt(itemLevel * ((Item.RadarData)data).getType().getCost()); break;
			case Item.Type.SHIELD: cost = Mathf.RoundToInt(itemLevel * ((Item.ShieldData)data).getType().getCost()); break;
			case Item.Type.REPAIR_DROID: cost = Mathf.RoundToInt(itemLevel * ((Item.RepairDroidData)data).getType().getCost()); break;
			case Item.Type.HARVESTER: cost = Mathf.RoundToInt(itemLevel * ((Item.HarvesterData)data).getType().getCost()); break;
			default: cost = 0; break;
		}
		return Mathf.RoundToInt(cost * (itemQuality == Item.Quality.UNIQUE? 2.5f: itemQuality == Item.Quality.SUPERIOR? 1.5f: 1));
	}

	private static int calculateEnergy (Item.ItemData data, Item.Type type, float itemLevel) {
		switch (type) {
			case Item.Type.WEAPON: return Mathf.RoundToInt(itemLevel * ((Item.WeaponData)data).getType().getEnergyNeeded());//randomizeValue(((Item.WeaponData)data).getType().getEnergyNeeded());
			case Item.Type.ENGINE: return Mathf.RoundToInt(itemLevel * ((Item.EngineData)data).getType().getEnergyNeeded());
			case Item.Type.RADAR: return Mathf.RoundToInt(itemLevel * ((Item.RadarData)data).getType().getEnergyNeeded());
			case Item.Type.SHIELD: return Mathf.RoundToInt(itemLevel * ((Item.ShieldData)data).getType().getEnergyNeeded());
			case Item.Type.REPAIR_DROID: return Mathf.RoundToInt(itemLevel * ((Item.RepairDroidData)data).getType().getEnergyNeeded());
			default: return 0;
		}
	}

	private static int randomizeValue (int value) {
		return Utils.getRandomValue(value, 30);
	}
}