using UnityEngine;
using System.Collections;

public enum ArmorType {
	STEEL,
	HARDENED_STEEL,
	TITANIUM,
	ASTRON,
	ADAMANT
}

public static class ArmorDescriptor {
	public static string name (this ArmorType type) {
		switch(type) {
		case ArmorType.STEEL: return "Стальная броня";
		case ArmorType.HARDENED_STEEL: return "Усиленная броня";
		case ArmorType.TITANIUM: return "Титановая броня";
		case ArmorType.ASTRON: return "Астрониевая броня";
		case ArmorType.ADAMANT: return "Адамантовая броня";
		default: return "Неизвестный материал защиты корпуса";
		}
	}

	public static int armorClass (this ArmorType type) {
		switch(type) {
		case ArmorType.STEEL: return 1;
		case ArmorType.HARDENED_STEEL: return 2;
		case ArmorType.TITANIUM: return 3;
		case ArmorType.ASTRON: return 4;
		case ArmorType.ADAMANT: return 5;
		default: return 0;
		}
	}

	public static float volume (this ArmorType type) {
		switch(type) {
		case ArmorType.STEEL: return 1;
		case ArmorType.HARDENED_STEEL: return 1;
		case ArmorType.TITANIUM: return 1;
		case ArmorType.ASTRON: return 1;
		case ArmorType.ADAMANT: return 1;
		default: return 0;
		}
	}

	public static int cost (this ArmorType type) {
		switch(type) {
		case ArmorType.STEEL: return 100;
		case ArmorType.HARDENED_STEEL: return 200;
		case ArmorType.TITANIUM: return 300;
		case ArmorType.ASTRON: return 400;
		case ArmorType.ADAMANT: return 500;
		default: return 0;
		}
	}

	public static string description (this ArmorType type) {
		switch(type) {
		case ArmorType.STEEL: return "Броня из обычной\nстали";
		case ArmorType.HARDENED_STEEL: return "Броня из усиленной\nстали";
		case ArmorType.TITANIUM: return "Броня из Титанового\nсплава";
		case ArmorType.ASTRON: return "Броня из сплава\nАстрония";
		case ArmorType.ADAMANT: return "Броня из Адамантия";
		default: return "Неизвестный материал защиты корпуса";
		}
	}
}