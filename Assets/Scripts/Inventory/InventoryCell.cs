using UnityEngine;
using System.Collections;

public class InventoryCell : ItemHolder {

	public int index;

	public Inventory inventory { get; private set; }

    public void init(Inventory inventory) {
        this.inventory = inventory;
    }

	public void setItem (Item newItem) {
		item = newItem;
		if (item != null) {
			item.slot = null;
			item.cell = this;
			item.transform.parent = transform;
			item.transform.localPosition = Vector3.zero;
		}
	}

	public Item takeItem () {
		inventory.getItems ().Remove (index + inventory.getOffset ());
		Item returnItem = item;
		item = null;
		return returnItem;
	}
}