using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryData {

	private Dictionary<int, InventoryItem> items = new Dictionary<int, InventoryItem>();

	private float capacity = 0f;//Объем в ноль означает, что в этот класс данные не были записаны, т.к. объем может быть либо -1, либо положительное значение

	public void setCapacity (float capacity) {
		this.capacity = capacity;
	}

	public float getCapacity () {
		return capacity;
	}

	public Dictionary<int, InventoryItem> getItems () {
		return items;
	}

	public void addToItems (int index, InventoryItem item) {
		items.Add (index, item);
	}
}