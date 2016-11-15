using UnityEngine;
using System.Collections;

public class LootSlot : ItemHolder {
	
	virtual public void setItem (Item item) {
		this.item = item;
		item.transform.parent = transform;
		item.transform.localPosition = Vector3.zero;
	}

	override public Item takeItem () {
		Item itemRef = item;
		item = null;
		return itemRef;
	}
}