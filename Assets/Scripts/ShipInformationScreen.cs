using UnityEngine;
using System.Collections;

public class ShipInformationScreen : InventoryContainedScreen {

	private ShipData shipData;

	protected override void checkItemDrop () {
		if (Utils.hit != null) {
			if (Utils.hit.name.Equals("Cell")) {
				InventoryCell cell = Utils.hit.transform.GetComponent<InventoryCell>();
				inventory.addItemToCell(draggedItem, cell);
			} else if (Utils.hit.name.Contains(" Slot")) {
				Messenger.showMessage("Оборудование на корабле можно менять только на планетах и в мастерских");
				draggedItem.returnToParentInventory();
			}
		} else {
			draggedItem.returnToParentInventory();
		}
	}

	public void showScreen (Inventory inventory, ShipData shipData) {
		transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
		this.inventory = inventory;
		this.shipData = shipData;

		inventory.setContainerScreen (this);
		shipData.updateHullInfo ();

		gameObject.SetActive (true);
	}

	public void hideScreen () {
		gameObject.SetActive (false);
	}

	protected override void choseDraggedItemFromSlot (HullSlot slot) {}

	protected override void checkBtnPress (Button btn) {}
}