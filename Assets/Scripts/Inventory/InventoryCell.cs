using UnityEngine;
using System.Collections;

public class InventoryCell : ItemHolder {

	public int index;

//	public Item item { get; private set; }

    private Inventory inventory;

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
//			itemChanged = true;
		}
	}

	public Item takeItem () {
		inventory.getItems ().Remove (index + inventory.getOffset ());
		Item returnItem = item;
		item = null;
		return returnItem;
	}

    public Inventory getInventory () {
        return inventory;
    }

//	public override ItemQuality getQuality () { return item.getItemQuality(); }
//	public override string getName () { return item.getItemName(); }
//	public override bool haveDescribableObject () { return item != null; }
}