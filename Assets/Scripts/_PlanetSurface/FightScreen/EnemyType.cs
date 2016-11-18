using UnityEngine;
using System.Collections;

public enum EnemyType {
	DRUID, BEE, GUNNER
}

public static class EnemyDescriptor {
	public static string name (this EnemyType type) {
		switch (type) {
			case EnemyType.DRUID: return "Друид";
			case EnemyType.BEE: return "Пчела";
			case EnemyType.GUNNER: return "Стрелок";
			default: Debug.Log("Unknown enemy type: " + type); return "";
		}
	}

	public static int health (this EnemyType type) {
		return 100;
	}

	public static int damage (this EnemyType type) {
		return 10;
	}

	public static int dexterity (this EnemyType type) {
		return 10;
	}

	public static int armor (this EnemyType type) {
		switch (type) {
			case EnemyType.BEE: return 5;
			case EnemyType.DRUID: return 0;
			case EnemyType.GUNNER: return 10;
			default: Debug.Log("Unknown enemy type: " + type); return 0;
		}
	}

	public static int speed (this EnemyType type) {
		return 3;
	}

	public static int cost (this EnemyType type) {
		switch (type) {
			case EnemyType.DRUID: return 100;
			case EnemyType.BEE: return 150;
			case EnemyType.GUNNER: return 200;
			default: Debug.Log("Unknown enemy type: " + type); return 0;
		}
	}

	public static PlanetType planet (this EnemyType type) {
		switch (type) {
			case EnemyType.BEE: return PlanetType.PALETTE;
			case EnemyType.DRUID: return PlanetType.PALETTE;
			case EnemyType.GUNNER: return PlanetType.PALETTE;
			default: Debug.Log("Unknown planet type: " + type); return PlanetType.PALETTE;
		}
	}
}