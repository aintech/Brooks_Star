using UnityEngine;
using System.Collections;

public class ShipInformationScreen : InventoryContainedScreen {

	private StarSystem starSystem;

	private ShipData shipData;

	private Button closeBtn;

	public void init (StarSystem starSystem, ShipData shipData, Inventory inventory) {
		this.starSystem = starSystem;
		this.shipData = shipData;
		this.inventory = inventory;

		inventory.setContainerScreen (this);
		shipData.updateHullInfo ();

		innerInit(inventory, null);

		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		shipData.gameObject.SetActive(true);
		inventory.gameObject.SetActive(true);
		closeBtn.gameObject.SetActive(true);
		transform.Find("Information BG").gameObject.SetActive(true);

		closeScreen();
	}

	protected override void checkItemDrop () {
		if (Utils.hit != null) {
			if (Utils.hit.name.Equals("Cell")) {
				InventoryCell cell = Utils.hit.transform.GetComponent<InventoryCell>();
				inventory.addItemToCell(draggedItem, cell);
			} else if (Utils.hit.name.Contains(" Slot")) {
				Messenger.showMessage("Оборудование на корабле можно менять только на планетах");
				draggedItem.returnToParentInventory();
			}
		} else {
			draggedItem.returnToParentInventory();
		}
	}

	public void showScreen () {
		if (gameObject.activeInHierarchy) { return; }

		inventory.setContainerScreen(this);
		inventory.setInventoryToBegin();

		Camera.main.orthographicSize = 5;
		StarSystem.setGamePause(true);
		shipData.updateShieldValue();
		shipData.updateHealthValue();
		transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
		inventory.transform.parent.position = transform.position;
		foreach (Planet planet in starSystem.getPlanets()) {
			planet.setShipIsNear(false);
		}
		inventory.gameObject.SetActive(true);
		gameObject.SetActive (true);
	}


	public void closeScreen () {
		hideItemInfo();
		inventory.gameObject.SetActive(false);
		gameObject.SetActive(false);
		StarSystem.setGamePause(false);
	}

	protected override void chooseDraggedItemFromSlot (Slot slot) {}

	protected override void checkBtnPress (Button btn) {
		if (btn == closeBtn) {
			closeScreen();
		}
	}
}