using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlanetType {
	CORAS,
	PALETTE,
	VADERPAN,
	PARPARIS,
	TERANA,
	VOLARIA,
	POSTERA
}

public static class PlanetDescriptor {

	private static Dictionary<PlanetType, EnemyType[]> enemyTypesOnPlanet = new Dictionary<PlanetType, EnemyType[]>();

	public static string getName (this PlanetType type) {
		switch (type) {
			case PlanetType.CORAS: return "Корас";
			case PlanetType.PALETTE: return "Палетта";
			case PlanetType.VADERPAN: return "Вадерпан";
			case PlanetType.PARPARIS: return "Парпарис";
			case PlanetType.TERANA: return "Терана";
			case PlanetType.VOLARIA: return "Волария";
			case PlanetType.POSTERA: return "Постера";
			default: return "Неизвестное наименование планеты";
		}
	}

	public static StarSystemType getStarSystemType (this PlanetType type) {
		return StarSystemType.ALURIA;
	}

	public static bool isColonized (this PlanetType type) {
		switch (type) {
			case PlanetType.PARPARIS:
			case PlanetType.TERANA:
				return true;
		}
		return false;
	}

	public static bool isPopulated (this PlanetType type) {
		switch (type) {
			case PlanetType.PALETTE:
			case PlanetType.VOLARIA:
			case PlanetType.POSTERA:
				return true;
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
			case PlanetType.CORAS: return 25;
			case PlanetType.PALETTE: return 45;
			case PlanetType.VADERPAN: return 70;
			case PlanetType.PARPARIS: return 120;
			case PlanetType.TERANA: return 160;
			case PlanetType.VOLARIA: return 210;
			case PlanetType.POSTERA: return 300;
            default: Debug.Log("Unknown planet type: " + type); return 0;
        }
    }

	public static EnemyType[] getEnemyTypes (this PlanetType type) {
		if (enemyTypesOnPlanet.Count == 0) { initEnemiesOnPlanets(); }
		return enemyTypesOnPlanet[type];
	}

	private static void initEnemiesOnPlanets () {
		enemyTypesOnPlanet.Add(PlanetType.PALETTE, new EnemyType[]{EnemyType.DRUID, EnemyType.BEE, EnemyType.GUNNER});
		enemyTypesOnPlanet.Add(PlanetType.VOLARIA, new EnemyType[]{EnemyType.DRUID, EnemyType.BEE, EnemyType.GUNNER});
		enemyTypesOnPlanet.Add(PlanetType.POSTERA, new EnemyType[]{EnemyType.DRUID, EnemyType.BEE, EnemyType.GUNNER});
	}
}