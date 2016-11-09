using UnityEngine;
using System.Collections;

public enum ItemType {
	WEAPON, ENGINE, ARMOR, GENERATOR, RADAR, SHIELD, REPAIR_DROID, HARVESTER,
	HAND_WEAPON, BODY_ARMOR,
	GOODS,
	SUPPLY
}

public enum ItemKind {
	EQUIPMENT, SHIP_EQUIPMENT, GOODS, SUPPLY
}

public static class ItemTypeDescriptor {

	private static ItemType[] drops = new ItemType[] { ItemType.WEAPON, ItemType.WEAPON, ItemType.WEAPON, ItemType.ENGINE, ItemType.GENERATOR, ItemType.RADAR, ItemType.SHIELD, ItemType.REPAIR_DROID, ItemType.HARVESTER };

	public static ItemKind kind (this ItemType type) {
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

			case ItemType.SUPPLY:
				return ItemKind.SUPPLY;
				
			default: Debug.Log("Unknown type: " + type); return ItemKind.GOODS;
		}
	}

	public static ItemType[] dropables () {
		return drops;
	}
}