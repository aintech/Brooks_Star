using UnityEngine;
using System.Collections;

public enum HandWeaponType {
	GUN, REVOLVER, MINIGUN, GAUSSE, RAILGUN
}

public static class HandWeaponDescriptor {
	public static string name (this HandWeaponType type) {
		switch (type) {
			case HandWeaponType.GAUSSE: return "Ружьё Гаусса";
			case HandWeaponType.GUN: return "Пистолет";
			case HandWeaponType.MINIGUN: return "Миниган";
			case HandWeaponType.RAILGUN: return "Рельсовое ружьё";
			case HandWeaponType.REVOLVER: return "Револьвер";
			default: Debug.Log("Unknown hand weapon type: " + type); return "";
		}
	}

	public static int damage (this HandWeaponType type) {
		switch (type) {
			case HandWeaponType.GAUSSE: return 60;
			case HandWeaponType.GUN: return 20;
			case HandWeaponType.MINIGUN: return 50;
			case HandWeaponType.RAILGUN: return 40;
			case HandWeaponType.REVOLVER: return 30;
			default: Debug.Log("Unknown hand weapon type: " + type); return 0;
		}
	}

	public static string description (this HandWeaponType type) {
		switch (type) {
			case HandWeaponType.GAUSSE: return "Ружьё Гаусса";
			case HandWeaponType.GUN: return "Пистолет";
			case HandWeaponType.MINIGUN: return "Миниган";
			case HandWeaponType.RAILGUN: return "Рельсовое ружьё";
			case HandWeaponType.REVOLVER: return "Револьвер";
			default: Debug.Log("Unknown hand weapon type: " + type); return "";
		}
	}

	public static float volume (this HandWeaponType type) {
		return 0;
	}

	public static int cost (this HandWeaponType type) {
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