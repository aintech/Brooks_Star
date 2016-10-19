using UnityEngine;
using System.Collections;

public enum HullType {
	LITTLE,
	NEEDLE,
	GNOME,
	CRICKET,
	ARGO,
	FALCON,
	ADVENTURER,
	CORVETTE,
	BUFFALO,
	LEGIONNAIRE,
	STARWALKER,
	WARSHIP,
	ASTERIX,
	PRIME,
	TITAN,
	DREADNAUT,
	ARMAGEDDON
}

public static class HullDescriptor {

	public static string getName (this HullType type) {
		switch(type) {
			case HullType.LITTLE: return "Малыш";
			case HullType.NEEDLE: return "Игла";
			case HullType.GNOME: return "Гном";
			case HullType.CRICKET: return "Сверчёк";
			case HullType.ARGO: return "Арго";
			case HullType.FALCON: return "Фалкон";
			case HullType.ADVENTURER: return "Авантюрист";
			case HullType.CORVETTE: return "Корвет";
			case HullType.BUFFALO: return "Буйвол";
			case HullType.LEGIONNAIRE: return "Легионер";
			case HullType.STARWALKER: return "Звёздоход";
			case HullType.WARSHIP: return "Воршип";
			case HullType.ASTERIX: return "Астерикс";
			case HullType.PRIME: return "Прайм";
			case HullType.TITAN: return "Титан";
			case HullType.DREADNAUT: return "Дреднаут";
			case HullType.ARMAGEDDON: return "Армагеддон";
			default: return "Неизвестный тип корпуса";
		}
	}
	
	public static int getMaxHealth (this HullType type) {
		switch(type) {
			case HullType.LITTLE:		return 70;
			case HullType.NEEDLE: 		return 100;
			case HullType.GNOME: 		return 120;
			case HullType.CRICKET: 		return 150;
			case HullType.ARGO: 		return 300;
			case HullType.FALCON: 		return 400;
			case HullType.ADVENTURER: 	return 450;
			case HullType.CORVETTE: 	return 550;
			case HullType.BUFFALO: 		return 800;
			case HullType.LEGIONNAIRE: 	return 900;
			case HullType.STARWALKER: 	return 1050;
			case HullType.WARSHIP: 		return 1200;
			case HullType.ASTERIX: 		return 1400;
			case HullType.PRIME: 		return 2000;
			case HullType.TITAN: 		return 2500;
			case HullType.DREADNAUT: 	return 3200;
			case HullType.ARMAGEDDON: 	return 3800;
			default: return 0;
		}
	}

	public static int getGeneratorSlots (this HullType type) {
		switch (type) {
			case HullType.LITTLE:
			case HullType.NEEDLE:
			case HullType.GNOME:
			case HullType.CRICKET:
			case HullType.ARGO:
			case HullType.FALCON:
			case HullType.ADVENTURER:
			case HullType.CORVETTE:
			case HullType.BUFFALO:
			case HullType.LEGIONNAIRE: return 1;
			case HullType.STARWALKER:
			case HullType.WARSHIP:
			case HullType.ASTERIX:
			case HullType.PRIME:
			case HullType.TITAN: return 2;
			case HullType.DREADNAUT:
			case HullType.ARMAGEDDON: return 3;
			default: return 0;
		}
	}

	public static int getHarvesterSlots (this HullType type) {
		switch (type) {
			case HullType.LITTLE:
			case HullType.NEEDLE:
			case HullType.GNOME:
			case HullType.CRICKET:
			case HullType.ARGO:
			case HullType.FALCON:
			case HullType.ADVENTURER:
			case HullType.CORVETTE:
			case HullType.BUFFALO:
			case HullType.LEGIONNAIRE: return 1;
			case HullType.STARWALKER:
			case HullType.WARSHIP:
			case HullType.ASTERIX:
			case HullType.PRIME:
			case HullType.TITAN:
			case HullType.DREADNAUT:
			case HullType.ARMAGEDDON: return 2;
			default: return 0;
		}
	}

	public static int getRepairDroidSlots (this HullType type) {
		switch (type) {
			case HullType.LITTLE:
			case HullType.NEEDLE:
			case HullType.GNOME:
			case HullType.CRICKET:
			case HullType.ARGO:
			case HullType.FALCON: return 1;
			case HullType.ADVENTURER:
			case HullType.CORVETTE:
			case HullType.BUFFALO:
			case HullType.LEGIONNAIRE: return 2;
			case HullType.STARWALKER:
			case HullType.WARSHIP:
			case HullType.ASTERIX:
			case HullType.PRIME: return 3;
			case HullType.TITAN:
			case HullType.DREADNAUT:
			case HullType.ARMAGEDDON: return 4;
			default: return 0;
		}
	}

	public static int getShieldSlots (this HullType type) {
		switch (type) {
			case HullType.LITTLE:
			case HullType.NEEDLE:
			case HullType.GNOME:
			case HullType.CRICKET:
			case HullType.ARGO:
			case HullType.FALCON:
			case HullType.ADVENTURER:
			case HullType.CORVETTE:
			case HullType.BUFFALO:
			case HullType.LEGIONNAIRE: return 1;
			case HullType.STARWALKER:
			case HullType.WARSHIP:
			case HullType.ASTERIX:
			case HullType.PRIME:
			case HullType.TITAN:
			case HullType.DREADNAUT: return 2;
			case HullType.ARMAGEDDON: return 3;
			default: return 0;
		}
	}

	public static int getArmorSlots (this HullType type) {
		switch (type) {
			case HullType.LITTLE:
			case HullType.NEEDLE:
			case HullType.GNOME: return 1;
			case HullType.CRICKET:
			case HullType.ARGO:
			case HullType.FALCON: return 2;
			case HullType.ADVENTURER:
			case HullType.CORVETTE:
			case HullType.BUFFALO:
			case HullType.LEGIONNAIRE: return 3;
			case HullType.STARWALKER:
			case HullType.WARSHIP:
			case HullType.ASTERIX:
			case HullType.PRIME: return 4;
			case HullType.TITAN:
			case HullType.DREADNAUT:
			case HullType.ARMAGEDDON: return 5;
			default: return 0;
		}
	}

	public static int getWeaponSlots (this HullType type) {
		switch(type) {
			case HullType.LITTLE:
			case HullType.NEEDLE:
			case HullType.GNOME: 		return 0;
			case HullType.CRICKET:
			case HullType.ARGO:
			case HullType.FALCON: 		return 1;
			case HullType.ADVENTURER:
			case HullType.CORVETTE:
			case HullType.BUFFALO:
			case HullType.LEGIONNAIRE:	return 2;
			case HullType.STARWALKER:
			case HullType.WARSHIP:
			case HullType.ASTERIX:
			case HullType.PRIME: 		return 3;
			case HullType.TITAN:
			case HullType.DREADNAUT:	return 4;
			case HullType.ARMAGEDDON: 	return 5;
			default: return 0;
		}
	}
	
	public static int cost (this HullType type) {
		switch(type) {
			case HullType.LITTLE:		return 100;
			case HullType.NEEDLE: 		return 200;
			case HullType.GNOME: 		return 240;
			case HullType.CRICKET: 		return 300;
			case HullType.ARGO: 		return 600;
			case HullType.FALCON: 		return 800;
			case HullType.ADVENTURER: 	return 900;
			case HullType.CORVETTE: 	return 1100;
			case HullType.BUFFALO: 		return 1600;
			case HullType.LEGIONNAIRE: 	return 1800;
			case HullType.STARWALKER: 	return 2100;
			case HullType.WARSHIP: 		return 2400;
			case HullType.ASTERIX: 		return 2800;
			case HullType.PRIME: 		return 4000;
			case HullType.TITAN: 		return 5000;
			case HullType.DREADNAUT: 	return 6400;
			case HullType.ARMAGEDDON: 	return 7600;
			default: return 0;
		}
	}

	public static int getStorageCapacity (this HullType type) {
		switch(type) {
			case HullType.LITTLE:		return 10;
			case HullType.NEEDLE: 		return 14;
			case HullType.GNOME: 		return 18;
			case HullType.CRICKET: 		return 22;
			case HullType.ARGO: 		return 28;
			case HullType.FALCON: 		return 34;
			case HullType.ADVENTURER: 	return 40;
			case HullType.CORVETTE: 	return 48;
			case HullType.BUFFALO: 		return 56;
			case HullType.LEGIONNAIRE: 	return 64;
			case HullType.STARWALKER: 	return 74;
			case HullType.WARSHIP: 		return 86;
			case HullType.ASTERIX: 		return 100;
			case HullType.PRIME: 		return 120;
			case HullType.TITAN: 		return 140;
			case HullType.DREADNAUT: 	return 170;
			case HullType.ARMAGEDDON: 	return 200;
			default: return 0;
		}
	}

	public static int getHullClass (this HullType type) {
		switch(type) {
			case HullType.LITTLE:
			case HullType.NEEDLE:
			case HullType.GNOME: 		return 0;
			case HullType.CRICKET:
			case HullType.ARGO:
			case HullType.FALCON: 		return 1;
			case HullType.ADVENTURER:
			case HullType.CORVETTE:
			case HullType.BUFFALO:
			case HullType.LEGIONNAIRE:	return 2;
			case HullType.STARWALKER:
			case HullType.WARSHIP:
			case HullType.ASTERIX:
			case HullType.PRIME: 		return 3;
			case HullType.TITAN:
			case HullType.DREADNAUT:	return 4;
			case HullType.ARMAGEDDON: 	return 5;
			default: return 0;
		}
	}
}