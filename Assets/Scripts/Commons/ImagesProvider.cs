using UnityEngine;
using System.Collections;

public class ImagesProvider : MonoBehaviour {

	public Sprite[] weaponSprites, engineSprites, armorSprites, generatorSprites, radarSprites, shieldSprites, repairDroidSprites, harvesterSprites, handWeaponSprites, bodyArmorSprites, goodsSprites;

	public static Sprite[] weapons, engines, armors, generators, radars, shields, repairDroids, harvesters, handWeapons, bodyArmors, goods;

	public void init () {
		weapons = weaponSprites;
		engines = engineSprites;
		armors = armorSprites;
		generators = generatorSprites;
		radars = radarSprites;
		shields = shieldSprites;
		repairDroids = repairDroidSprites;
		harvesters = harvesterSprites;
		handWeapons = handWeaponSprites;
		bodyArmors = bodyArmorSprites;
		goods = goodsSprites;
	}

	public static Sprite getWeaponSprite (WeaponType type) {
		switch (type) {
			case WeaponType.BLASTER: return weapons[0];
			case WeaponType.PLASMER: return weapons[1];
			case WeaponType.CHARGER: return weapons[2];
			case WeaponType.EMITTER: return weapons[3];
			case WeaponType.WAVER: return weapons[4];
			case WeaponType.LAUNCHER: return weapons[5];
			case WeaponType.SUPPRESSOR: return weapons[6];
			default: Debug.Log("Unknown weapon type: " + type); return null;
		}
	}

	public static Sprite getEngineSprite (EngineType type) {
		switch (type) {
			case EngineType.FORCE: return engines[0];
			case EngineType.GRADUAL: return engines[1];
			case EngineType.PROTON: return engines[2];
			case EngineType.ALLUR: return engines[3];
			case EngineType.QUAZAR: return engines[4];
			default: Debug.Log("Unknown engine type: " + type); return null;
		}
	}

	public static Sprite getArmorSprite (ArmorType type) {
		switch (type) {
			case ArmorType.STEEL: return armors[0];
			case ArmorType.HARDENED_STEEL: return armors[1];
			case ArmorType.TITANIUM: return armors[2];
			case ArmorType.ASTRON: return armors[3];
			case ArmorType.ADAMANT: return armors[4];
			default: Debug.Log("Unknown armor type: " + type); return null;
		}
	}

	public static Sprite getGeneratorSprite (GeneratorType type) {
		switch (type) {
			case GeneratorType.ATOMIC: return generators[0];
			case GeneratorType.PLASMA: return generators[1];
			case GeneratorType.MULTYPHASE: return generators[2];
			case GeneratorType.TUNNEL: return generators[3];
			default: Debug.Log("Unknown generator type: " + type); return null;
		}
	}

	public static Sprite getRadarSprite (RadarType type) {
		switch (type) {
			case RadarType.SEQUESTER: return radars[0];
			case RadarType.PLANAR: return radars[1];
			case RadarType.MATRIX: return radars[2];
			case RadarType.PATAN_CORSAC: return radars[3];
			case RadarType.SNAKE: return radars[4];
			case RadarType.ASTRAL: return radars[5];
			default: Debug.Log("Unknown radar type: " + type); return null;
		}
	}

	public static Sprite getShieldSprite (ShieldType type) {
		switch (type) {
			case ShieldType.BLOCK: return shields[0];
			case ShieldType.QUADRATIC: return shields[1];
			case ShieldType.CELL: return shields[2];
			case ShieldType.PHASE: return shields[3];
			default: Debug.Log("Unknown shield type: " + type); return null;
		}
	}

	public static Sprite getRepairDroidSprite (RepairDroidType type) {
		switch (type) {
			case RepairDroidType.RAIL: return repairDroids[0];
			case RepairDroidType.CHANNEL: return repairDroids[1];
			case RepairDroidType.BIPHASIC: return repairDroids[2];
			case RepairDroidType.THREAD: return repairDroids[3];
			default: Debug.Log("Unknown repair droid type: " + type); return null;
		}
	}

	public static Sprite getHarvesterSprite (HarvesterType type) {
		switch (type) {
			case HarvesterType.MECHANICAL: return harvesters[0];
			case HarvesterType.PLASMATIC: return harvesters[1];
			case HarvesterType.GENERATIVE: return harvesters[2];
			default: Debug.Log("Unknown harvester type: " + type); return null;
		}
	}

	public static Sprite getHandWeaponSprite (HandWeaponType type) {
		switch (type) {
			case HandWeaponType.GUN: return handWeapons[0];
			case HandWeaponType.REVOLVER: return handWeapons[1];
			case HandWeaponType.MINIGUN: return handWeapons[2];
			case HandWeaponType.GAUSSE: return handWeapons[3];
			case HandWeaponType.RAILGUN: return handWeapons[4];
			default: Debug.Log("Unknown hand weapon type: " + type); return null;
		}
	}

	public static Sprite getBodyArmorSprite (BodyArmorType type) {
		switch (type) {
			case BodyArmorType.SPACESUIT: return bodyArmors[0];
			case BodyArmorType.HARDENED_SPACESUIT: return bodyArmors[1];
			case BodyArmorType.ARMORED_SPACESUIT: return bodyArmors[2];
			case BodyArmorType.COMBAT_ARMOR: return bodyArmors[3];
			default: Debug.Log("Unknown body armor type: " + type); return null;
		}
	}

	public static Sprite getGoodsSprite (GoodsType type) {
		switch (type) {
			case GoodsType.JEWELRY: return goods[0];
			case GoodsType.PRECIOUS_METALS: return goods[1];
			case GoodsType.BOOZE: return goods[2];
			case GoodsType.ELECTRONICS: return goods[3];
			case GoodsType.MEAL: return goods[4];
			default: Debug.Log("Unknown goods type: " + type); return null;
		}
	}
}