using UnityEngine;
using System.Collections;

public enum BodyArmorType {
	SPACESUIT
}

public static class BodyArmorDescriptor {
	public static string getName(this BodyArmorType type) {
		return "Скафандр";
	}

	public static int getArmorClass (this BodyArmorType type) {
		return 5;
	}

	public static string getDescription (this BodyArmorType type) {
		return "Космический скафандр";
	}

	public static float getVolume (this BodyArmorType type) {
		return .1f;
	}
}