using UnityEngine;
using System.Collections;

public enum ArmorType {
	Steel,
	HardenedSteel,
	Titan,
	Astron,
	Adamant
}

public static class ArmorDescriptor {
	public static string getName (this ArmorType type) {
		switch(type) {
		case ArmorType.Steel: return "Стальная броня";
		case ArmorType.HardenedSteel: return "Усиленная броня";
		case ArmorType.Titan: return "Титановая броня";
		case ArmorType.Astron: return "Астрониевая броня";
		case ArmorType.Adamant: return "Адамантовая броня";
		default: return "Неизвестный материал защиты корпуса";
		}
	}

	public static int getArmorClass (this ArmorType type) {
		switch(type) {
		case ArmorType.Steel: return 1;
		case ArmorType.HardenedSteel: return 2;
		case ArmorType.Titan: return 3;
		case ArmorType.Astron: return 4;
		case ArmorType.Adamant: return 5;
		default: return 0;
		}
	}

	public static float getVolume (this ArmorType type) {
		switch(type) {
		case ArmorType.Steel: return 1;
		case ArmorType.HardenedSteel: return 1;
		case ArmorType.Titan: return 1;
		case ArmorType.Astron: return 1;
		case ArmorType.Adamant: return 1;
		default: return 0;
		}
	}

	public static int getCost (this ArmorType type) {
		switch(type) {
		case ArmorType.Steel: return 100;
		case ArmorType.HardenedSteel: return 200;
		case ArmorType.Titan: return 300;
		case ArmorType.Astron: return 400;
		case ArmorType.Adamant: return 500;
		default: return 0;
		}
	}

	public static string getDescription (this ArmorType type) {
		switch(type) {
		case ArmorType.Steel: return "Броня из обычной\nстали";
		case ArmorType.HardenedSteel: return "Броня из усиленной\nстали";
		case ArmorType.Titan: return "Броня из Титанового\nсплава";
		case ArmorType.Astron: return "Броня из сплава\nАстрония";
		case ArmorType.Adamant: return "Броня из Адамантия";
		default: return "Неизвестный материал защиты корпуса";
		}
	}
}