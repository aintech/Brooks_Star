using UnityEngine;
using System.Collections;

public enum HandWeaponType {
	BASE_LASER
}

public static class HandWeaponDescriptor {
	public static string getName (this HandWeaponType type) {
		return "Лазерный пистолет";
	}

	public static int getDamage (this HandWeaponType type) {
		return 20;
	}

	public static string getDescription (this HandWeaponType type) {
		return "Лазерный пистолет";
	}

	public static float getVolume (this HandWeaponType type) {
		return .1f;
	}
}