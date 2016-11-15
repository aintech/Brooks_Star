using UnityEngine;
using System.Collections;

public enum SupplyType {
	MEDKIT_SMALL, MEDKIT_MEDIUM, MEDKIT_LARGE, MEDKIT_ULTRA,
	GRENADE_FLASH, GRENADE_PARALIZE,
	INJECTION_SPEED, INJECTION_ARMOR, INJECTION_REGENERATION
}

public static class SupplyDescription {
	public static string name (this SupplyType type) {
		switch (type) {
			case SupplyType.MEDKIT_SMALL: return "Маленькая аптечка";
			case SupplyType.MEDKIT_MEDIUM: return "Средняя аптечка";
			case SupplyType.MEDKIT_LARGE: return "Большая аптечка";
			case SupplyType.MEDKIT_ULTRA: return "Огромная аптечка";
			case SupplyType.GRENADE_FLASH: return "Светошумовая граната";
			case SupplyType.GRENADE_PARALIZE: return "Парализующая граната";
			case SupplyType.INJECTION_SPEED: return "Инъекция скорости";
			case SupplyType.INJECTION_ARMOR: return "Инъекция защиты";
			case SupplyType.INJECTION_REGENERATION: return "Инъекция регенерации";
			default: Debug.Log("Unknown supply type: " + type); return "";
		}
	}

	public static string description (this SupplyType type) {
		return "";
	}

	public static float volume (this SupplyType type) {
		return 0;
	}

	public static int cost (this SupplyType type) {
		return 10;
	}

	public static int value (this SupplyType type) {
		switch (type) {
			case SupplyType.MEDKIT_SMALL: return 50;
			case SupplyType.MEDKIT_MEDIUM: return 100;
			case SupplyType.MEDKIT_LARGE: return 200;
			case SupplyType.MEDKIT_ULTRA: return 500;
			case SupplyType.INJECTION_SPEED: return 1;
			case SupplyType.INJECTION_ARMOR: return 10;
			case SupplyType.INJECTION_REGENERATION: return 30;
			default: return 0;
		}
	}

	public static int sortWeight (this SupplyType type) {
		switch (type) {
			case SupplyType.MEDKIT_ULTRA: return 9000000;
			case SupplyType.MEDKIT_LARGE: return 8000000;
			case SupplyType.MEDKIT_MEDIUM: return 7000000;
			case SupplyType.MEDKIT_SMALL: return 6000000;
			case SupplyType.INJECTION_SPEED: return 5000000;
			case SupplyType.INJECTION_REGENERATION: return 4000000;
			case SupplyType.INJECTION_ARMOR: return 3000000;
			case SupplyType.GRENADE_PARALIZE: return 2000000;
			case SupplyType.GRENADE_FLASH: return 1000000;
			default: Debug.Log("Unknown supply type: " + type); return 0;
		}
	}

	public static StatusEffectType toStatusEffectType (this SupplyType type) {
		switch (type) {
			case SupplyType.GRENADE_FLASH: return StatusEffectType.BLINDED;
			case SupplyType.GRENADE_PARALIZE: return StatusEffectType.PARALIZED;
			case SupplyType.INJECTION_ARMOR: return StatusEffectType.ARMORED;
			case SupplyType.INJECTION_REGENERATION: return StatusEffectType.REGENERATION;
			case SupplyType.INJECTION_SPEED: return StatusEffectType.SPEED;
			case SupplyType.MEDKIT_SMALL: case SupplyType.MEDKIT_MEDIUM: case SupplyType.MEDKIT_LARGE: case SupplyType.MEDKIT_ULTRA: return StatusEffectType.HEAL;
			default: Debug.Log ("Unmapped supply type: " + type); return StatusEffectType.NONE;
		}
	}

	public static FightEffectType toFightEffectType (this SupplyType type) {
		switch (type) {
			case SupplyType.GRENADE_FLASH: return FightEffectType.BLIND;
			case SupplyType.GRENADE_PARALIZE: return FightEffectType.PARALIZED;
			case SupplyType.INJECTION_ARMOR: return FightEffectType.ARMORED;
			case SupplyType.INJECTION_REGENERATION: return FightEffectType.REGENERATION;
			case SupplyType.INJECTION_SPEED: return FightEffectType.SPEED;
			case SupplyType.MEDKIT_SMALL: case SupplyType.MEDKIT_MEDIUM: case SupplyType.MEDKIT_LARGE: case SupplyType.MEDKIT_ULTRA: return FightEffectType.HEAL;
			default: Debug.Log("Unknown supply type: " + type); return FightEffectType.DAMAGE;
		}
	}

	public static bool isMedkit (this SupplyType type) {
		return type == SupplyType.MEDKIT_SMALL || type == SupplyType.MEDKIT_MEDIUM || type == SupplyType.MEDKIT_LARGE || type == SupplyType.MEDKIT_ULTRA;
	}

	public static bool isGrenade (this SupplyType type) {
		return type == SupplyType.GRENADE_FLASH || type == SupplyType.GRENADE_PARALIZE;
	}

	public static bool isInjection (this SupplyType type) {
		return type == SupplyType.INJECTION_ARMOR || type == SupplyType.INJECTION_REGENERATION || type == SupplyType.INJECTION_SPEED;
	}
}