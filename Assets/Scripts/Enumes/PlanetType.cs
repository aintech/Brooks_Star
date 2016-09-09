using UnityEngine;
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
}