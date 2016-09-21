using UnityEngine;
using System.Collections;

public enum HandWeaponType {
	GUN, REVOLVER, MINIGUN, GAUSSE, RAILGUN
}

public static class HandWeaponDescriptor {
	public static string getName (this HandWeaponType type) {
		switch (type) {
			case HandWeaponType.GAUSSE: return "Ружьё\nГаусса";
			case HandWeaponType.GUN: return "Пистолет";
			case HandWeaponType.MINIGUN: return "Миниган";
			case HandWeaponType.RAILGUN: return "Рельсовое\nружьё";
			case HandWeaponType.REVOLVER: return "Револьвер";
			default: Debug.Log("Unknown hand weapon type: " + type); return "";
		}
	}

	public static int getDamage (this HandWeaponType type) {
		switch (type) {
			case HandWeaponType.GAUSSE: return 60;
			case HandWeaponType.GUN: return 20;
			case HandWeaponType.MINIGUN: return 50;
			case HandWeaponType.RAILGUN: return 40;
			case HandWeaponType.REVOLVER: return 30;
			default: Debug.Log("Unknown hand weapon type: " + type); return 0;
		}
	}

	public static string getDescription (this HandWeaponType type) {
		switch (type) {
			case HandWeaponType.GAUSSE: return "Ружьё Гаусса";
			case HandWeaponType.GUN: return "Пистолет";
			case HandWeaponType.MINIGUN: return "Миниган";
			case HandWeaponType.RAILGUN: return "Рельсовое ружьё";
			case HandWeaponType.REVOLVER: return "Револьвер";
			default: Debug.Log("Unknown hand weapon type: " + type); return "";
		}
	}

	public static float getVolume (this HandWeaponType type) {
		return .1f;
	}

	public static int getCost (this HandWeaponType type) {
		switch(type) {
			case HandWeaponType.GUN: return 100;
			case HandWeaponType.REVOLVER: return 150;
			case HandWeaponType.MINIGUN: return 200;
			case HandWeaponType.GAUSSE: return 300;
			case HandWeaponType.RAILGUN: return 500;
			default: Debug.Log("Unknown hand weapon type: " + type); return 0;
		}
	}
}