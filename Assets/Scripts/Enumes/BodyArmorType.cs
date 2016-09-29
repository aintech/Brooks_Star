using UnityEngine;
using System.Collections;

public enum BodyArmorType {
	SUIT, METAL, HEAVY
}

public static class BodyArmorDescriptor {
	public static string getName(this BodyArmorType type) {
		switch (type) {
			case BodyArmorType.SUIT: return "Космический скафандр";
			case BodyArmorType.METAL: return "Металическая броня";
			case BodyArmorType.HEAVY: return "Тяжёлый доспех";
			default: Debug.Log("Unknown body armor type: " + type); return "";
		}
	}

	public static int getArmorClass (this BodyArmorType type) {
		switch (type) {
			case BodyArmorType.SUIT: return 10;
			case BodyArmorType.METAL: return 20;
			case BodyArmorType.HEAVY: return 30;
			default: Debug.Log("Unknown body armor type: " + type); return 0;
		}
	}

	public static string getDescription (this BodyArmorType type) {
		switch (type) {
			case BodyArmorType.SUIT: return "Космический скафандр";
			case BodyArmorType.METAL: return "Металическая броня";
			case BodyArmorType.HEAVY: return "Тяжёлый доспех";
			default: Debug.Log("Unknown body armor type: " + type); return "";
		}
	}

	public static float getVolume (this BodyArmorType type) {
		return 0;
	}

	public static int getCost (this BodyArmorType type) {
		switch(type) {
			case BodyArmorType.SUIT: return 100;
			case BodyArmorType.METAL: return 200;
			case BodyArmorType.HEAVY: return 300;
			default: Debug.Log("Unknown body armor type: " + type); return 0;
		}
	}
}