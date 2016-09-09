using UnityEngine;
using System.Collections;

public class InventoryCell : MonoBehaviour {

	public int index;

	private InventoryItem item;

	public void setItem (InventoryItem item) {
		this.item = item;
	}

	public InventoryItem getItem () {
		return item;
	}

	public InventoryItem takeItem () {
		Inventory inventory = transform.parent.GetComponent<Inventory> ();
		inventory.getItems ().Remove (index + inventory.getOffset ());
		InventoryItem returnItem = item;
		item = null;
		return returnItem;
	}
}