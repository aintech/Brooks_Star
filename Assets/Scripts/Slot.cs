using UnityEngine;
using System.Collections;

public abstract class Slot : MonoBehaviour {

	public ItemKind kind { get; protected set; }

	public Item item { get; private set; }

	public abstract void setSprite (bool asActive);

	virtual public void setItem (Item item) {
		item.slot = this;
		this.item = item;
	}

	virtual public Item takeItem () {
		Item itemRef = item;
		item.slot = null;
		item = null;
		return itemRef;
	}
}