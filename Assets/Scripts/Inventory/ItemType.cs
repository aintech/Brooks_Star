using UnityEngine;
using System.Collections;

public enum ItemType {
	WEAPON, ENGINE, ARMOR, GENERATOR, RADAR, SHIELD, REPAIR_DROID, HARVESTER,
	HAND_WEAPON, BODY_ARMOR,
	MINERAL
}

public enum ItemKind {
	NONE, GOOD, EQUIPMENTS, SHIP_EQUIPMENT
}

public static class TypeDescriptor {
	public static ItemKind getKind (this ItemType type) {
		switch (type) {
			case ItemType.HAND_WEAPON: case ItemType.BODY_ARMOR: return ItemKind.EQUIPMENTS;
			case ItemType.WEAPON: case ItemType.ENGINE: case ItemType.ARMOR: case ItemType.GENERATOR: case ItemType.RADAR: case ItemType.SHIELD:
			case ItemType.REPAIR_DROID: case ItemType.HARVESTER: return ItemKind.SHIP_EQUIPMENT;
			case ItemType.MINERAL: return ItemKind.GOOD;
			default: Debug.Log("Unknown type: " + type); return ItemKind.NONE;
		}
	}
}