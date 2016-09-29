using UnityEngine;
using System.Collections;

public enum HarvesterType {
	Mechanical,
	Plasmatic,
	Generative
}

public static class HarvesterDescriptor {
	public static string getName (this HarvesterType type) {
		switch(type) {
		case HarvesterType.Mechanical: return "Механический сборщик";
		case HarvesterType.Plasmatic: return "Плазматический сборщик";
		case HarvesterType.Generative: return "Генеративный сборщик";
		default: return "Неизвестный тип сборщика";
		}
	}

	public static int getHarvestTime (this HarvesterType type) {
		switch(type) {
		case HarvesterType.Mechanical: return 300;
		case HarvesterType.Plasmatic: return 200;
		case HarvesterType.Generative: return 100;
		default: return 0;
		}
	}

	public static float getVolume (this HarvesterType type) {
		switch(type) {
		case HarvesterType.Mechanical: return 1;
		case HarvesterType.Plasmatic: return 1;
		case HarvesterType.Generative: return 1;
		default: return 0;
		}
	}

	public static int getCost (this HarvesterType type) {
		switch(type) {
		case HarvesterType.Mechanical: return 100;
		case HarvesterType.Plasmatic: return 200;
		case HarvesterType.Generative: return 300;
		default: return 0;
		}
	}

	public static string getDescription (this HarvesterType type) {
		switch(type) {
		case HarvesterType.Mechanical: return "Дроид-Харвестер на\nмеханической основе";
		case HarvesterType.Plasmatic: return "Дроид-Харвестер на\nплазматической основе";
		case HarvesterType.Generative: return "Дроид-Харвестре на\nгенеративной основе";
		default: return "Неизвестный тип сборщика";
		}
	}
}