using UnityEngine;
using System.Collections;

public enum EnemyType {
	DRUID, BEE, GUNNER
}

public static class EnemyDescriptor {
	public static string getName (this EnemyType type) {
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
		return 0;
	}
}