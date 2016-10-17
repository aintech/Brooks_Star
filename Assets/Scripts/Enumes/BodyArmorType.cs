using UnityEngine;
using System.Collections;

public enum BodyArmorType {
	SPACESUIT, HARDENED_SPACESUIT, ARMORED_SPACESUIT, COMBAT_ARMOR
}

public static class BodyArmorDescriptor {
	public static string name (this BodyArmorType type) {
		switch (type) {
			case BodyArmorType.SPACESUIT: return "Скафандр";
			case BodyArmorType.HARDENED_SPACESUIT: return "Укреплённый скафандр";
			case BodyArmorType.ARMORED_SPACESUIT: return "Бронированный скафандр";
			case BodyArmorType.COMBAT_ARMOR: return "Боевая броня";
			default: Debug.Log("Unknown body armor type: " + type); return "";
		}
	}

	public static int armorClass (this BodyArmorType type) {
		switch (type) {
			case BodyArmorType.SPACESUIT: return 10;
			case BodyArmorType.HARDENED_SPACESUIT: return 20;
			case BodyArmorType.ARMORED_SPACESUIT: return 30;
			case BodyArmorType.COMBAT_ARMOR: return 40;
			default: Debug.Log("Unknown body armor type: " + type); return 0;
		}
	}

	public static string description (this BodyArmorType type) {
		return type.name();
	}

	public static float volume (this BodyArmorType type) {
		return 0;
	}

	public static int cost (this BodyArmorType type) {
		switch(type) {
			case BodyArmorType.SPACESUIT: return 100;
			case BodyArmorType.HARDENED_SPACESUIT: return 200;
			case BodyArmorType.ARMORED_SPACESUIT: return 300;
			case BodyArmorType.COMBAT_ARMOR: return 400;
			default: Debug.Log("Unknown body armor type: " + type); return 0;
		}
	}
}