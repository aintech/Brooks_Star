using UnityEngine;
using System.Collections;

public abstract class Slot : MonoBehaviour {

	public Sprite slotBG, activeSlotBG;

	private SpriteRenderer bgRender, iconRender;

	public ItemKind kind { get; protected set; }

	public Item item { get; private set; }

	virtual public void init () {
		bgRender = GetComponent<SpriteRenderer>();
		iconRender = transform.Find("Icon Image").GetComponent<SpriteRenderer>();
	}

	public void setActive (bool asActive) {
		bgRender.sprite = asActive? activeSlotBG: slotBG;
	}

	virtual public void setItem (Item item) {
		item.slot = this;
		this.item = item;
		iconRender.enabled = false;
	}

	virtual public Item takeItem () {
		Item itemRef = item;
		item.slot = null;
		item = null;
		iconRender.enabled = true;
		return itemRef;
	}
}