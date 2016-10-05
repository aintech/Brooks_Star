using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlanetType {
	CORAS,
	PALETTE,
	VADERPAN,
	PARPARIS
}

public static class PlanetDescriptor {

	private static Dictionary<PlanetType, EnemyType[]> enemyTypesOnPlanet = new Dictionary<PlanetType, EnemyType[]>();

	public static string getName (this PlanetType type) {
		switch (type) {
			case PlanetType.CORAS: return "Корас";
			case PlanetType.PALETTE: return "Палетта";
			case PlanetType.VADERPAN: return "Вадерпан";
			case PlanetType.PARPARIS: return "Парпарис";
			default: return "Неизвестное наименование планеты";
		}
	}

	public static bool isColonized (this PlanetType type) {
		switch (type) {
			case PlanetType.CORAS: return true;
		}
		return false;
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
            case PlanetType.CORAS: return 15;
            case PlanetType.PALETTE: return 45;
            default: Debug.Log("Unknown planet type: " + type); return 0;
        }
    }

	public static EnemyType[] getEnemyTypes (this PlanetType type) {
		if (enemyTypesOnPlanet.Count == 0) { initEnemiesOnPlanets(); }
		return enemyTypesOnPlanet[type];
	}

	private static void initEnemiesOnPlanets () {
		enemyTypesOnPlanet.Add(PlanetType.CORAS, new EnemyType[]{EnemyType.DRUID, EnemyType.BEE, EnemyType.GUNNER});
	}
}