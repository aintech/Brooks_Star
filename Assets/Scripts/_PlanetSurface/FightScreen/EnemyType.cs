using UnityEngine;
using System.Collections;

public enum EnemyType {
	PIRATE, CAPTAIN, COMMANDER,  DRAGON, MINOTAUR, DRUID, HARPY, KNIGHT, FIGHTER, BEE, GUNNER, SWORDMASTER
}

public static class EnemyDescriptor {
	public static string getName (this EnemyType type) {
		switch (type) {
			case EnemyType.DRAGON: return "Дракон";
			case EnemyType.DRUID: return "Друид";
			case EnemyType.MINOTAUR: return "Минотавр";
			case EnemyType.PIRATE: return "Пират";
			case EnemyType.CAPTAIN: return "Капитан пиратов";
			case EnemyType.COMMANDER: return "Командир пиратов";
			case EnemyType.HARPY: return "Гарпия";
			case EnemyType.KNIGHT: return "Рыцарь";
			case EnemyType.FIGHTER: return "Боец";
			case EnemyType.BEE: return "Пчела";
			case EnemyType.GUNNER: return "Стрелок";
			case EnemyType.SWORDMASTER: return "Мастер меча";
			default: Debug.Log("Unknown enemy type: " + type); return "";
		}
	}

	public static int getHealth (this EnemyType type) {
		return 100;
	}

	public static int getDamage (this EnemyType type) {
		return 10;
	}

	public static int getDexterity (this EnemyType type) {
		return 10;
	}

	public static int getArmor (this EnemyType type) {
		return 0;
	}
}