using UnityEngine;
using System.Collections;

public class ItemFactory {

	private static float qualityMult = 0;

	private ItemFactory(){}

	public static void createItemData (InventoryItem item, InventoryItem.Type type) {

		float maxRand = type == InventoryItem.Type.Weapon? 7:
					  	type == InventoryItem.Type.Engine? 5:
					  	type == InventoryItem.Type.Armor? 5:
					  	type == InventoryItem.Type.Generator? 4:
					  	type == InventoryItem.Type.Radar? 6:
					  	type == InventoryItem.Type.Shield? 4:
					  	type == InventoryItem.Type.RepairDroid? 4:
					  	type == InventoryItem.Type.Harvester? 3: -1;

		int rand = Mathf.RoundToInt (Random.value * maxRand);
		
		setItemQualityAndLevel (item, type);

		InventoryItem.ItemData data = null;

		switch (type) {
			case InventoryItem.Type.Weapon: data = createWeaponData (item, rand); break;
			case InventoryItem.Type.Engine: data = createEngineData (item, rand); break;
			case InventoryItem.Type.Armor: data = createArmorData (item, rand); break;
			case InventoryItem.Type.Generator: data = createGeneratorData (item, rand); break;
			case InventoryItem.Type.Radar: data = createRadarData (item, rand); break;
			case InventoryItem.Type.Shield: data = createShieldData (item, rand); break;
			case InventoryItem.Type.RepairDroid: data = createRepairDroidData (item, rand); break;
			case InventoryItem.Type.Harvester: data = createHarvesterData (item, rand); break;
		}

		setItemValues (item, data, type);
	}

	private static void setItemQualityAndLevel (InventoryItem item, InventoryItem.Type type) {
		if (type == InventoryItem.Type.Armor) {
			item.setItemLevel(1);
			item.setItemQuality(InventoryItem.Quality.Normal);
		} else {
			int randQuality = Mathf.RoundToInt (Random.value * 100);
			
			if (randQuality > 90) item.setItemQuality(InventoryItem.Quality.Unique);
			else if (randQuality > 60) item.setItemQuality(InventoryItem.Quality.Superior);
			else item.setItemQuality(InventoryItem.Quality.Normal);

			item.setItemLevel(1 + (Random.value * 0.3f));
		}

		qualityMult = item.getItemQuality() == InventoryItem.Quality.Unique? 3: item.getItemQuality() == InventoryItem.Quality.Superior? 2: 1;
	}

	private static void setItemValues (InventoryItem item, InventoryItem.ItemData data, InventoryItem.Type type) {
		item.setCost (calculateCost (data, type, item.getItemLevel(), item.getItemQuality()));
		item.setEnergyNeeded(calculateEnergy(data, type, item.getItemLevel()));
		item.setItemData(data);
	}

	public static void createWeaponData (InventoryItem item, WeaponType type) {
		setItemQualityAndLevel (item, InventoryItem.Type.Weapon);
		InventoryItem.WeaponData data = createWeaponData (item, type == WeaponType.Blaster? 0:
		                                                  		type == WeaponType.Plasmer? 1:
		                                                  		type == WeaponType.Charger? 2:
		                                                  		type == WeaponType.Emitter? 3:
		                                                  		type == WeaponType.Waver? 4:
		                                                  		type == WeaponType.Launcher? 5: 6);

		setItemValues(item, data, InventoryItem.Type.Weapon);
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

		float reloadMulty = item.getItemQuality() == InventoryItem.Quality.Unique? 0.6f: item.getItemQuality() == InventoryItem.Quality.Superior? 0.8f: 1;

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

		float harvestMulty = item.getItemQuality() == InventoryItem.Quality.Unique? 0.6f: item.getItemQuality() == InventoryItem.Quality.Superior? 0.8f: 1;
		int harvestTime = Mathf.RoundToInt(type.getHarvestTime() / item.getItemLevel() * harvestMulty);

		item.setVolume (type.getVolume());
		return new InventoryItem.HarvesterData (type, harvestTime);
	}

	private static int calculateCost (InventoryItem.ItemData data, InventoryItem.Type type, float itemLevel, InventoryItem.Quality itemQuality) {
		int cost = 0;
		switch (type) {
			case InventoryItem.Type.Weapon: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.WeaponData)data).getType().getCost()); break;
			case InventoryItem.Type.Engine: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.EngineData)data).getType().getCost()); break;
			case InventoryItem.Type.Armor: return ((InventoryItem.ArmorData)data).getType().getCost();
			case InventoryItem.Type.Generator: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.GeneratorData)data).getType().getCost()); break;
			case InventoryItem.Type.Radar: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.RadarData)data).getType().getCost()); break;
			case InventoryItem.Type.Shield: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.ShieldData)data).getType().getCost()); break;
			case InventoryItem.Type.RepairDroid: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.RepairDroidData)data).getType().getCost()); break;
			case InventoryItem.Type.Harvester: cost = Mathf.RoundToInt(itemLevel * ((InventoryItem.HarvesterData)data).getType().getCost()); break;
			default: cost = 0; break;
		}
		return Mathf.RoundToInt(cost * (itemQuality == InventoryItem.Quality.Unique? 2.5f: itemQuality == InventoryItem.Quality.Superior? 1.5f: 1));
	}

	private static int calculateEnergy (InventoryItem.ItemData data, InventoryItem.Type type, float itemLevel) {
		switch (type) {
			case InventoryItem.Type.Weapon: return Mathf.RoundToInt(itemLevel * ((InventoryItem.WeaponData)data).getType().getEnergyNeeded());//randomizeValue(((InventoryItem.WeaponData)data).getType().getEnergyNeeded());
			case InventoryItem.Type.Engine: return Mathf.RoundToInt(itemLevel * ((InventoryItem.EngineData)data).getType().getEnergyNeeded());
			case InventoryItem.Type.Radar: return Mathf.RoundToInt(itemLevel * ((InventoryItem.RadarData)data).getType().getEnergyNeeded());
			case InventoryItem.Type.Shield: return Mathf.RoundToInt(itemLevel * ((InventoryItem.ShieldData)data).getType().getEnergyNeeded());
			case InventoryItem.Type.RepairDroid: return Mathf.RoundToInt(itemLevel * ((InventoryItem.RepairDroidData)data).getType().getEnergyNeeded());
			default: return 0;
		}
	}

	private static int randomizeValue (int value) {
		return Utils.getRandomValue(value, 30);
	}
}