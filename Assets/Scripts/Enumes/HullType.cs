using UnityEngine;
using System.Collections;

public enum HullType {
	Little,
	Needle,
	Gnome,
	Cricket,
	Argo,
	Falcon,
	Adventurer,
	Corvette,
	Buffalo,
	Legionnaire,
	StarWalker,
	Warship,
	Asterix,
	Prime,
	Titan,
	Dreadnaut,
	Armageddon
}

public static class HullDescriptor {

	public static string getName (this HullType type) {
		switch(type) {
			case HullType.Little: return "Малыш";
			case HullType.Needle: return "Игла";
			case HullType.Gnome: return "Гном";
			case HullType.Cricket: return "Сверчёк";
			case HullType.Argo: return "Арго";
			case HullType.Falcon: return "Фалкон";
			case HullType.Adventurer: return "Авантюрист";
			case HullType.Corvette: return "Корвет";
			case HullType.Buffalo: return "Буйвол";
			case HullType.Legionnaire: return "Легионер";
			case HullType.StarWalker: return "Звёздоход";
			case HullType.Warship: return "Воршип";
			case HullType.Asterix: return "Астерикс";
			case HullType.Prime: return "Прайм";
			case HullType.Titan: return "Титан";
			case HullType.Dreadnaut: return "Дреднаут";
			case HullType.Armageddon: return "Армагеддон";
			default: return "Неизвестный тип корпуса";
		}
	}
	
	public static int getMaxHealth (this HullType type) {
		switch(type) {
			case HullType.Little:		return 50;
			case HullType.Needle: 		return 100;
			case HullType.Gnome: 		return 120;
			case HullType.Cricket: 		return 150;
			case HullType.Argo: 		return 300;
			case HullType.Falcon: 		return 400;
			case HullType.Adventurer: 	return 450;
			case HullType.Corvette: 	return 550;
			case HullType.Buffalo: 		return 800;
			case HullType.Legionnaire: 	return 900;
			case HullType.StarWalker: 	return 1050;
			case HullType.Warship: 		return 1200;
			case HullType.Asterix: 		return 1400;
			case HullType.Prime: 		return 2000;
			case HullType.Titan: 		return 2500;
			case HullType.Dreadnaut: 	return 3200;
			case HullType.Armageddon: 	return 3800;
			default: return 0;
		}
	}

	public static int getGeneratorSlots (this HullType type) {
		switch (type) {
			case HullType.Little:
			case HullType.Needle:
			case HullType.Gnome:
			case HullType.Cricket:
			case HullType.Argo:
			case HullType.Falcon:
			case HullType.Adventurer:
			case HullType.Corvette:
			case HullType.Buffalo:
			case HullType.Legionnaire: return 1;
			case HullType.StarWalker:
			case HullType.Warship:
			case HullType.Asterix:
			case HullType.Prime:
			case HullType.Titan: return 2;
			case HullType.Dreadnaut:
			case HullType.Armageddon: return 3;
			default: return 0;
		}
	}

	public static int getHarvesterSlots (this HullType type) {
		switch (type) {
			case HullType.Little:
			case HullType.Needle:
			case HullType.Gnome:
			case HullType.Cricket:
			case HullType.Argo:
			case HullType.Falcon:
			case HullType.Adventurer:
			case HullType.Corvette:
			case HullType.Buffalo:
			case HullType.Legionnaire: return 1;
			case HullType.StarWalker:
			case HullType.Warship:
			case HullType.Asterix:
			case HullType.Prime:
			case HullType.Titan:
			case HullType.Dreadnaut:
			case HullType.Armageddon: return 2;
			default: return 0;
		}
	}

	public static int getRepairDroidSlots (this HullType type) {
		switch (type) {
			case HullType.Little:
			case HullType.Needle:
			case HullType.Gnome:
			case HullType.Cricket:
			case HullType.Argo:
			case HullType.Falcon: return 1;
			case HullType.Adventurer:
			case HullType.Corvette:
			case HullType.Buffalo:
			case HullType.Legionnaire: return 2;
			case HullType.StarWalker:
			case HullType.Warship:
			case HullType.Asterix:
			case HullType.Prime: return 3;
			case HullType.Titan:
			case HullType.Dreadnaut:
			case HullType.Armageddon: return 4;
			default: return 0;
		}
	}

	public static int getShieldSlots (this HullType type) {
		switch (type) {
			case HullType.Little:
			case HullType.Needle:
			case HullType.Gnome:
			case HullType.Cricket:
			case HullType.Argo:
			case HullType.Falcon:
			case HullType.Adventurer:
			case HullType.Corvette:
			case HullType.Buffalo:
			case HullType.Legionnaire: return 1;
			case HullType.StarWalker:
			case HullType.Warship:
			case HullType.Asterix:
			case HullType.Prime:
			case HullType.Titan:
			case HullType.Dreadnaut: return 2;
			case HullType.Armageddon: return 3;
			default: return 0;
		}
	}

	public static int getArmorSlots (this HullType type) {
		switch (type) {
			case HullType.Little:
			case HullType.Needle:
			case HullType.Gnome: return 1;
			case HullType.Cricket:
			case HullType.Argo:
			case HullType.Falcon: return 2;
			case HullType.Adventurer:
			case HullType.Corvette:
			case HullType.Buffalo:
			case HullType.Legionnaire: return 3;
			case HullType.StarWalker:
			case HullType.Warship:
			case HullType.Asterix:
			case HullType.Prime: return 4;
			case HullType.Titan:
			case HullType.Dreadnaut:
			case HullType.Armageddon: return 5;
			default: return 0;
		}
	}

	public static int getWeaponSlots (this HullType type) {
		switch(type) {
			case HullType.Little:
			case HullType.Needle:
			case HullType.Gnome: 		return 0;
			case HullType.Cricket:
			case HullType.Argo:
			case HullType.Falcon: 		return 1;
			case HullType.Adventurer:
			case HullType.Corvette:
			case HullType.Buffalo:
			case HullType.Legionnaire:	return 2;
			case HullType.StarWalker:
			case HullType.Warship:
			case HullType.Asterix:
			case HullType.Prime: 		return 3;
			case HullType.Titan:
			case HullType.Dreadnaut:	return 4;
			case HullType.Armageddon: 	return 5;
			default: return 0;
		}
	}
	
	public static int getCost (this HullType type) {
		switch(type) {
			case HullType.Little:		return 100;
			case HullType.Needle: 		return 200;
			case HullType.Gnome: 		return 240;
			case HullType.Cricket: 		return 300;
			case HullType.Argo: 		return 600;
			case HullType.Falcon: 		return 800;
			case HullType.Adventurer: 	return 900;
			case HullType.Corvette: 	return 1100;
			case HullType.Buffalo: 		return 1600;
			case HullType.Legionnaire: 	return 1800;
			case HullType.StarWalker: 	return 2100;
			case HullType.Warship: 		return 2400;
			case HullType.Asterix: 		return 2800;
			case HullType.Prime: 		return 4000;
			case HullType.Titan: 		return 5000;
			case HullType.Dreadnaut: 	return 6400;
			case HullType.Armageddon: 	return 7600;
			default: return 0;
		}
	}

	public static int getStorageCapacity (this HullType type) {
		switch(type) {
			case HullType.Little:		return 10;
			case HullType.Needle: 		return 14;
			case HullType.Gnome: 		return 18;
			case HullType.Cricket: 		return 22;
			case HullType.Argo: 		return 28;
			case HullType.Falcon: 		return 34;
			case HullType.Adventurer: 	return 40;
			case HullType.Corvette: 	return 48;
			case HullType.Buffalo: 		return 56;
			case HullType.Legionnaire: 	return 64;
			case HullType.StarWalker: 	return 74;
			case HullType.Warship: 		return 86;
			case HullType.Asterix: 		return 100;
			case HullType.Prime: 		return 120;
			case HullType.Titan: 		return 140;
			case HullType.Dreadnaut: 	return 170;
			case HullType.Armageddon: 	return 200;
			default: return 0;
		}
	}

	public static int getHullClass (this HullType type) {
		switch(type) {
			case HullType.Little:
			case HullType.Needle:
			case HullType.Gnome: 		return 0;
			case HullType.Cricket:
			case HullType.Argo:
			case HullType.Falcon: 		return 1;
			case HullType.Adventurer:
			case HullType.Corvette:
			case HullType.Buffalo:
			case HullType.Legionnaire:	return 2;
			case HullType.StarWalker:
			case HullType.Warship:
			case HullType.Asterix:
			case HullType.Prime: 		return 3;
			case HullType.Titan:
			case HullType.Dreadnaut:	return 4;
			case HullType.Armageddon: 	return 5;
			default: return 0;
		}
	}
}