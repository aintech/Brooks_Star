using UnityEngine;
using System.Collections;

public enum PerkType {
	MARKSMAN, GUNNER
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
			case PerkType.MARKSMAN: return "Увеличивает урон\nручным оружием\nна ";
			case PerkType.GUNNER: return "Увеличивает урон\nот корабельных\nорудий на ";
			default: Debug.Log("Unknown perk type: " + type); return "";
		}
	}

	public static int getValuePerLevel (this PerkType type) {
		return 5;
	}
}