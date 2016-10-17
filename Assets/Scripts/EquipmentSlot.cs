using UnityEngine;
using System.Collections;

public class EquipmentSlot : Slot {
	
	override public void init () {
		base.init();
		kind = ItemKind.EQUIPMENT;
	}

	public override void setItem (Item item) {
		switch (slotType) {
			case Type.HAND_WEAPON: Player.equipWeapon((HandWeaponData)item.itemData); break;
			case Type.BODY_ARMOR: Player.equipArmor((BodyArmorData)item.itemData); break;
			default: Debug.Log("Unknown item type: " + item.type()); break;
		}
		base.setItem (item);
	}

	public override Item takeItem () {
		switch (slotType) {
			case Type.HAND_WEAPON: Player.equipWeapon(null); break;
			case Type.BODY_ARMOR: Player.equipArmor(null); break;
			default: Debug.Log("Unknown item type: " + item.type()); break;
		}
		return base.takeItem ();
	}
}