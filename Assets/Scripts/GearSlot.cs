using UnityEngine;
using System.Collections;

public class GearSlot : Slot {

	public Type gearType;

	public Sprite[] gearSlotSprites;

	private SpriteRenderer render;

	public void init () {
		render = GetComponent<SpriteRenderer>();
		kind = ItemKind.GEAR;
		setSprite(false);
	}

	override public void setSprite (bool asActive) {
		switch (gearType) {
			case Type.HAND_WEAPON: render.sprite = asActive? gearSlotSprites[1]: gearSlotSprites[0]; break;
			case Type.BODY_ARMOR: render.sprite = asActive? gearSlotSprites[3]: gearSlotSprites[2]; break;
			default: Debug.Log("Unknown gear type: " + gearType); break;
		}
	}

	public override void setItem (Item item) {
		switch (gearType) {
			case Type.HAND_WEAPON: Player.equipWeapon((HandWeaponData)item.itemData); break;
			case Type.BODY_ARMOR: Player.equipArmor((BodyArmorData)item.itemData); break;
			default: Debug.Log("Unknown item type: " + item.getItemType()); break;
		}
		base.setItem (item);
	}

	public override Item takeItem () {
		switch (gearType) {
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