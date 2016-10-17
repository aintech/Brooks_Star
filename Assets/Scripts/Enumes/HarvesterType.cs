using UnityEngine;
using System.Collections;

public enum HarvesterType {
	MECHANICAL,
	PLASMATIC,
	GENERATIVE
}

public static class HarvesterDescriptor {
	public static string name (this HarvesterType type) {
		switch(type) {
			case HarvesterType.MECHANICAL: return "Механический сборщик";
			case HarvesterType.PLASMATIC: return "Плазматический сборщик";
			case HarvesterType.GENERATIVE: return "Генеративный сборщик";
			default: return "Неизвестный тип сборщика";
		}
	}

	public static int harvestTime (this HarvesterType type) {
		switch(type) {
			case HarvesterType.MECHANICAL: return 300;
			case HarvesterType.PLASMATIC: return 200;
			case HarvesterType.GENERATIVE: return 100;
			default: return 0;
		}
	}

	public static float volume (this HarvesterType type) {
		switch(type) {
			case HarvesterType.MECHANICAL: return 1;
			case HarvesterType.PLASMATIC: return 1;
			case HarvesterType.GENERATIVE: return 1;
			default: return 0;
		}
	}

	public static int cost (this HarvesterType type) {
		switch(type) {
			case HarvesterType.MECHANICAL: return 100;
			case HarvesterType.PLASMATIC: return 200;
			case HarvesterType.GENERATIVE: return 300;
			default: return 0;
		}
	}

	public static string description (this HarvesterType type) {
		switch(type) {
			case HarvesterType.MECHANICAL: return "Дроид-Харвестер на\nмеханической основе";
			case HarvesterType.PLASMATIC: return "Дроид-Харвестер на\nплазматической основе";
			case HarvesterType.GENERATIVE: return "Дроид-Харвестре на\nгенеративной основе";
			default: return "Неизвестный тип сборщика";
		}
	}
}