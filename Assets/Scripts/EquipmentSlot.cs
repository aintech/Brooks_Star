using UnityEngine;
using System.Collections;

public class EquipmentSlot : Slot {

	public Type equipmentType;

	override public void init () {
		base.init();
		kind = ItemKind.EQUIPMENTS;
	}

	public override void setItem (Item item) {
		switch (equipmentType) {
			case Type.HAND_WEAPON: Player.equipWeapon((HandWeaponData)item.itemData); break;
			case Type.BODY_ARMOR: Player.equipArmor((BodyArmorData)item.itemData); break;
			default: Debug.Log("Unknown item type: " + item.getItemType()); break;
		}
		base.setItem (item);
	}

	public override Item takeItem () {
		switch (equipmentType) {
			case Type.HAND_WEAPON: Player.equipWeapon(null); break;
			case Type.BODY_ARMOR: Player.equipArmor(null); break;
			default: Debug.Log("Unknown item type: " + item.getItemType()); break;
		}
		return base.takeItem ();
	}

	public enum Type {
		NONE, HAND_WEAPON, BODY_ARMOR
	}
}