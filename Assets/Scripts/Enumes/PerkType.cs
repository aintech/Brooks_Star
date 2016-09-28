using UnityEngine;
using System.Collections;

public enum PerkType {
	MARKSMAN, GUNNER
//	GEOLOGIST - уменьшает время разработки астероидов и увеличивает получаемые ресурсы
//	EXPLORER - уменьшает время поиска противников на планетах
}

public static class PerkDescription {
	public static string getName(this PerkType type) {
		switch (type) {
			case PerkType.MARKSMAN: return "Стрелок";
			case PerkType.GUNNER: return "Наводчик";
			default: Debug.Log("Unknown perk type: " + type); return "";
		}
	}

	public static string getDescription (this PerkType type) {
		switch(type) {
			case PerkType.MARKSMAN: return "Урон ручным оружием";
			case PerkType.GUNNER: return "Урон от корабельных орудий";
			default: Debug.Log("Unknown perk type: " + type); return "";
		}
	}

	public static int getValuePerLevel (this PerkType type) {
		return 5;
	}
}