using UnityEngine;
using System.Collections;

public enum ItemType {
	WEAPON, ENGINE, ARMOR, GENERATOR, RADAR, SHIELD, REPAIR_DROID, HARVESTER,
	HAND_WEAPON, BODY_ARMOR,
	GOODS
}

public enum ItemKind {
	EQUIPMENT, SHIP_EQUIPMENT, GOODS
}

public static class TypeDescriptor {
	public static ItemKind getKind (this ItemType type) {
		switch (type) {
			case ItemType.HAND_WEAPON:
			case ItemType.BODY_ARMOR:
				return ItemKind.EQUIPMENT;

			case ItemType.WEAPON:
			case ItemType.ENGINE:
			case ItemType.ARMOR:
			case ItemType.GENERATOR:
			case ItemType.RADAR:
			case ItemType.SHIELD:
			case ItemType.REPAIR_DROID:
			case ItemType.HARVESTER:
				return ItemKind.SHIP_EQUIPMENT;

			case ItemType.GOODS:
				return ItemKind.GOODS;
				
			default: Debug.Log("Unknown type: " + type); return ItemKind.GOODS;
		}
	}
}