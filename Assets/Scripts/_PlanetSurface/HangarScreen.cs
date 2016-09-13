using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HangarScreen : InventoryContainedScreen {
	
	private PlanetSurface planetSurface;

	private ShipData shipData;

	private Vector3 hullViewPosition = new Vector3 (4.95f, 0);

	private Button shipBtn, closeBtn;

	public void init (PlanetSurface planetSurface, ShipData shipData, Inventory inventory, Inventory storage) {
		this.planetSurface = planetSurface;
		this.shipData = shipData;
		this.inventory = inventory;
		this.storage = storage;

		innerInit();
		shipBtn = transform.Find("Ship Button").GetComponent<Button>().init();
		closeBtn = transform.Find("Close Button").GetComponent<Button>().init();

		transform.Find("Hangar BG").gameObject.SetActive(true);

		gameObject.SetActive(false);
	}

	public void showScreen () {
		inventory.setContainerScreen(this);
		storage.setContainerScreen(this);

		inventory.setInventoryToBegin ();
		storage.setInventoryToBegin ();

        inventory.gameObject.SetActive(true);

		storage.setPosition(false);
		shipData.transform.position = hullViewPosition;

		shipData.updateHullInfo ();

		showShipScreen ();

		gameObject.SetActive (true);
	}

	override protected void checkBtnPress (Button btn) {
		if (btn == storageBtn) { showStorageScreen(); }
		else if (btn == shipBtn) { showShipScreen(); }
		else if (btn == closeBtn) { closeScreen(); }
		else { Debug.Log("Unknown button: " + btn.name); }
	}

	override protected void checkItemDrop () {
		if (Utils.hit != null && Utils.hit.name.Equals("Cell")) {
			InventoryCell cell = Utils.hit.transform.GetComponent<InventoryCell>();
			Inventory targetInv = cell.transform.parent.GetComponent<Inventory>();
			if (draggedItem.getCell() == null) {
				shipData.updateHullInfo();
			}
			targetInv.addItemToCell(draggedItem, cell);
		} else if (Utils.hit != null && Utils.hit.name.Contains(" Slot")) {
			HullSlot slot = Utils.hit.transform.GetComponent<HullSlot> ();
			if (slot.getHullSlotType() != getHullToItemType (draggedItem.getItemType ())) {
				if (draggedItem.getCell () == null) {
					if (inventory.gameObject.activeInHierarchy) {
						inventory.addItemToCell (draggedItem, draggedItem.getCell ());
					} else if (storage.gameObject.activeInHierarchy) {
						storage.addItemToCell (draggedItem, draggedItem.getCell ());
					}
					shipData.updateHullInfo();
				} else {
					draggedItem.returnToParentInventory ();
				}
			} else if (slot.getItem () == null) {
				setItemToSlot (slot);
			} else if (slot.getItem() != null) {
				InventoryItem currItem = slot.takeItem ();
				if (inventory.gameObject.activeInHierarchy) {
					inventory.addItemToCell (currItem, draggedItem.getCell ());
				} else if (storage.gameObject.activeInHierarchy) {
					storage.addItemToCell (currItem, draggedItem.getCell ());
				}
				shipData.updateHullInfo ();
				draggedItem.setCell (null);
				setItemToSlot (slot);
			}
		} else if (draggedItem.getCell () == null) {
			if (inventory.gameObject.activeInHierarchy) {
				inventory.addItemToCell (draggedItem, null);
			} else if (storage.gameObject.activeInHierarchy) {
				storage.addItemToCell (draggedItem, null);
			}
			shipData.updateHullInfo ();
		} else {
			draggedItem.returnToParentInventory();
		}
	}

	private void setItemToSlot (HullSlot slot) {
		if (draggedItem.getCell () != null) {
			draggedItem.getCell ().takeItem ();
			draggedItem.getCell().transform.parent.GetComponent<Inventory> ().calculateFreeVolume();
		}
		draggedItem.setCell (null);
		slot.setItem (draggedItem);
		draggedItem.transform.position = slot.transform.position;
		draggedItem.transform.parent = shipData.transform;
		shipData.updateHullInfo ();
	}

	private HullSlot.HullSlotType getHullToItemType (InventoryItem.Type itemType) {
		switch (itemType) {
			case InventoryItem.Type.ARMOR: return HullSlot.HullSlotType.Armor;
			case InventoryItem.Type.ENGINE: return HullSlot.HullSlotType.Engine;
			case InventoryItem.Type.GENERATOR: return HullSlot.HullSlotType.Generator;
			case InventoryItem.Type.HARVESTER: return HullSlot.HullSlotType.Harvester;
			case InventoryItem.Type.RADAR: return HullSlot.HullSlotType.Radar;
			case InventoryItem.Type.REPAIR_DROID: return HullSlot.HullSlotType.RepairDroid;
			case InventoryItem.Type.SHIELD: return HullSlot.HullSlotType.Shield;
			case InventoryItem.Type.WEAPON: return HullSlot.HullSlotType.Weapon;
			default: Debug.Log("Unknown item type: " + itemType); return HullSlot.HullSlotType.Armor;
		}
	}

	override protected void afterItemDrop () {
		if (draggedItem == null) {
			highlightSlot (false, InventoryItem.Type.ARMOR);
		}
	}

	override protected void choseItem (InventoryItem item) {
		base.choseItem(item);
		if (draggedItem != null) {
			highlightSlot (true, item.getItemType ());
		}
	}

	private void highlightSlot (bool hightlight, InventoryItem.Type itemType) {
		if (!hightlight) {
			foreach (HullSlot slot in shipData.getSlots()) {
				slot.setSprite(false);
			}
		} else {
			HullSlot.HullSlotType hullType = getHullToItemType (itemType);
			foreach (HullSlot slot in shipData.getSlots()) {
				if (slot.getHullSlotType() == hullType) {
					slot.setSprite (true);
				}
			}
		}
	}

	private void showShipScreen () {
		storage.gameObject.SetActive(false);
		shipData.gameObject.SetActive (true);
		shipBtn.setActive(false);
		storageBtn.setActive(true);
        if (getChosenItem() != null && getChosenItem().getCell() != null &&
            getChosenItem().getCell().getInventory().getInventoryType() == Inventory.InventoryType.STORAGE)
        {
            hideItemInfo();
        }
	}

	private void showStorageScreen () {
		storage.gameObject.SetActive(true);
		shipData.gameObject.SetActive (false);
		storageBtn.setActive(false);
		shipBtn.setActive(true);
        if (getChosenItem() != null && getChosenItem().getHullSlot() != null) {
            hideItemInfo();
        }
	}

	private void closeScreen () {
		hideItemInfo();
		storage.setPosition(true);
		inventory.gameObject.SetActive (false);
		storage.gameObject.SetActive (false);
		shipData.gameObject.SetActive (false);
        planetSurface.setPlanetBtnsEnabled(true);
		gameObject.SetActive (false);
	}
}
