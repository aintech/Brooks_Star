﻿using UnityEngine;
using System.Collections;

public enum PlanetType {
	CORAS,
	PALETTE,
	VADERPAN,
	PARPARIS
}

public static class PlanetDescriptor {

	public static string getName (this PlanetType type) {
		switch (type) {
		case PlanetType.CORAS: return "Корас";
		case PlanetType.PALETTE: return "Палетта";
		case PlanetType.VADERPAN: return "Вадерпан";
		case PlanetType.PARPARIS: return "Парпарис";
		default: return "Неизвестное наименование планеты";
		}
	}

	public static string getDescription (this PlanetType type) {
		switch (type) {
		case PlanetType.CORAS: return "Планета-завод, основанная...";
		case PlanetType.PALETTE: return "Небольшая планета-поселение...";
		case PlanetType.VADERPAN: return "Планета известная горячими источниками...";
		case PlanetType.PARPARIS: return "Центральная планета Союза...";
		default: return "Неизвестное наименование планеты";
		}
	}

    public static float getDistanceToStar (this PlanetType type) {
        switch (type) {
            case PlanetType.CORAS: return 8;
            case PlanetType.PALETTE: return 20;
            default: Debug.Log("Unknown planet type: " + type); return 0;
        }
    }

	public static EnemyType[] getEnemyTypes (this PlanetType type) {
		return new EnemyType[]{EnemyType.PIRATE, EnemyType.DRAGON, EnemyType.DRUID, EnemyType.MINOTAUR, EnemyType.CAPTAIN, EnemyType.BEE, EnemyType.COMMANDER, EnemyType.FIGHTER, EnemyType.GUNNER, EnemyType.HARPY, EnemyType.KNIGHT, EnemyType.SWORDMASTER};
//		switch (type) {
//			case PlanetType.CORAS: return new EnemyType[]{EnemyType.PIRATE, EnemyType.DRAGON, EnemyType.DRUID, EnemyType.MINOTAUR, EnemyType.CAPTAIN, 
//					EnemyType.BEE, EnemyType.COMMANDER, EnemyType.FIGHTER, EnemyType.GUNNER, EnemyType.HARPY, EnemyType.KNIGHT, EnemyType.SWORDMASTER};
//			default: Debug.Log("Unknown planet type: " + type); return new EnemyType[0];
//		}
	}
}