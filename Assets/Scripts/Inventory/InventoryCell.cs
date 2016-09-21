using UnityEngine;
using System.Collections;

public class InventoryCell : MonoBehaviour {

	public int index;

	private Item item;

    private Inventory inventory;

    public void init(Inventory inventory) {
        this.inventory = inventory;
    }

	public void setItem (Item item) {
		this.item = item;
	}

	public Item getItem () {
		return item;
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
}