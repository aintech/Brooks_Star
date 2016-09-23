﻿using UnityEngine;
using System.Collections;

public abstract class Slot : Describable {

	public Type slotType;

	public Sprite activeSlotBG;

	private SpriteRenderer bgRender, iconRender;

	public ItemKind kind { get; protected set; }

	public Item item { get; private set; }

	virtual public void init () {
		bgRender = GetComponent<SpriteRenderer>();
		iconRender = transform.Find("Icon Image").GetComponent<SpriteRenderer>();

		bgRender.sprite = activeSlotBG;
		setActive(false);
	}

	public void setActive (bool asActive) {
		bgRender.enabled = asActive;
	}

	virtual public void setItem (Item newItem) {
		item = newItem;
		item.slot = this;
		item.cell = null;
		item.transform.parent = transform;
		item.transform.localPosition = Vector3.zero;
//		bgRender.enabled = false;
		iconRender.enabled = false;
	}

	virtual public Item takeItem () {
		Item itemRef = item;
		item = null;
//		bgRender.enabled = true;
		iconRender.enabled = true;
		return itemRef;
	}

	public override string getQuality () { return item == null? null: item.getItemQuality().ToString(); }
	public override string getName () { return item == null? null: item.getItemName(); }

	public enum Type {
		NONE,  WEAPON, ENGINE, ARMOR, GENERATOR, RADAR, SHIELD, REPAIR_DROID, HARVESTER, HAND_WEAPON, BODY_ARMOR
	}
}