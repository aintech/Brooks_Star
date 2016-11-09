using UnityEngine;
using System.Collections;

public enum SupplyType {
	MEDKIT_SMALL, MEDKIT_MEDIUM, MEDKIT_LARGE, MEDKIT_ULTRA,
	GRENADE_SMOKE, GRENADE_PARALIZE,
	INJECTION_SPEED, INJECTION_ARMOR, INJECTION_HIT_CHANCE
}

public static class SupplyDescriotion {
	public static string name (this SupplyType type) {
		switch (type) {
			case SupplyType.MEDKIT_SMALL: return "Маленькая аптечка";
			case SupplyType.MEDKIT_MEDIUM: return "Средняя аптечка";
			case SupplyType.MEDKIT_LARGE: return "Большая аптечка";
			case SupplyType.MEDKIT_ULTRA: return "Огромная аптечка";
			case SupplyType.GRENADE_SMOKE: return "Дымовая граната";
			case SupplyType.GRENADE_PARALIZE: return "Парализующая граната";
			case SupplyType.INJECTION_SPEED: return "Инъекция скорости";
			case SupplyType.INJECTION_ARMOR: return "Инъекция защиты";
			case SupplyType.INJECTION_HIT_CHANCE: return "Инъекция меткости";
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

	public static float value (this SupplyType type) {
		return 1;
	}

	public static int sortWeight (this SupplyType type) {
		switch (type) {
			case SupplyType.MEDKIT_ULTRA: return 9000000;
			case SupplyType.MEDKIT_LARGE: return 8000000;
			case SupplyType.MEDKIT_MEDIUM: return 7000000;
			case SupplyType.MEDKIT_SMALL: return 6000000;
			case SupplyType.INJECTION_SPEED: return 5000000;
			case SupplyType.INJECTION_HIT_CHANCE: return 4000000;
			case SupplyType.INJECTION_ARMOR: return 3000000;
			case SupplyType.GRENADE_PARALIZE: return 2000000;
			case SupplyType.GRENADE_SMOKE: return 1000000;
			default: Debug.Log("Unknown supply type: " + type); return 0;
		}
	}
}